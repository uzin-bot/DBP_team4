using System;
using System.Data;
using System.Collections.Generic;

namespace DBP_WinformChat
{
    /// <summary>
    /// 관리자가 설정한 권한을 실시간으로 체크하는 클래스
    /// </summary>
    public class PermissionManager
    {
        private DBconnector db;

        public PermissionManager()
        {
            db = DBconnector.GetInstance();
        }

        // DeptPath를 SQL CONCAT 대신 C#에서 조합하도록 하는 헬퍼
        private void AddDeptPathColumn(DataTable dt, string parentColName = "ParentDeptName", string deptColName = "DeptName", bool sortByDeptPath = false)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            if (!dt.Columns.Contains("DeptPath"))
                dt.Columns.Add("DeptPath", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                var parent = (dt.Columns.Contains(parentColName) && row[parentColName] != DBNull.Value) ? row[parentColName].ToString() : null;
                var dept = (dt.Columns.Contains(deptColName) && row[deptColName] != DBNull.Value) ? row[deptColName].ToString() : string.Empty;
                row["DeptPath"] = string.IsNullOrEmpty(parent) ? dept : parent + " > " + dept;
            }

            if (sortByDeptPath)
            {
                dt.DefaultView.Sort = "DeptPath";
                var sorted = dt.DefaultView.ToTable();
                dt.Clear();
                foreach (DataRow r in sorted.Rows)
                    dt.ImportRow(r);
            }
        }

        // ==================== 1. 사용자가 다른 사용자를 볼 수 있는지 체크 ====================
        /// <summary>
        /// 사용자 목록에 표시할 수 있는지 확인
        /// </summary>
        public bool CanViewUser(int ownerUserId, int targetUserId)
        {
            try
            {
                // 1. UserVisibleUser 테이블에서 개별 사용자 권한 확인
                string sql1 = $"SELECT COUNT(*) FROM UserVisibleUser WHERE OwnerUserId = {ownerUserId} AND VisibleUserId = {targetUserId}";

                var dt1 = db.Query(sql1);
                if (dt1.Rows.Count > 0 && Convert.ToInt32(dt1.Rows[0][0]) > 0)
                    return true;

                // 2. UserVisibleDept 테이블에서 부서/팀 기반 권한 확인
                string sql2 = $"SELECT COUNT(*) FROM UserVisibleDept uvd INNER JOIN User u ON u.DeptId = uvd.DeptId WHERE uvd.OwnerUserId = {ownerUserId} AND u.UserId = {targetUserId}";

                var dt2 = db.Query(sql2);
                if (dt2.Rows.Count > 0 && Convert.ToInt32(dt2.Rows[0][0]) > 0)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        // ==================== 2. 두 사용자 간 대화가 가능한지 체크 ====================
        /// <summary>
        /// 관리자가 대화를 차단했는지 확인
        /// </summary>
        public bool CanChat(int userAId, int userBId)
        {
            try
            {
                string sql = $"SELECT IsBlocked FROM ChatPermission WHERE (UserAId = {userAId} AND UserBId = {userBId}) OR (UserAId = {userBId} AND UserBId = {userAId})";

                var dt = db.Query(sql);

                if (dt.Rows.Count > 0)
                {
                    var val = dt.Rows[0]["IsBlocked"];
                    int isBlockedInt = Convert.ToInt32(val);
                    return isBlockedInt == 0;
                }

                return true;
            }
            catch
            {
                return true;
            }
        }

        // ==================== 3. 볼 수 있는 사용자 목록 가져오기 ====================
        /// <summary>
        /// 현재 로그인한 사용자가 볼 수 있는 모든 사용자 목록 조회
        /// </summary>
        public DataTable GetVisibleUsers(int ownerUserId)
        {
            try
            {
                string sql = $@"
                    SELECT DISTINCT 
                        u.UserId, 
                        u.Name, 
                        u.Nickname, 
                        u.LoginId, 
                        u.DeptId, 
                        p.DeptName AS ParentDeptName,
                        d.DeptName AS DeptName,
                        u.IsOnline,
                        u.LastLoginAt
                    FROM User u
                    LEFT JOIN Department d ON u.DeptId = d.DeptId
                    LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                    WHERE u.Role = 'user' 
                    AND u.UserId != {ownerUserId}
                    AND (
                        u.UserId IN (
                            SELECT VisibleUserId 
                            FROM UserVisibleUser 
                            WHERE OwnerUserId = {ownerUserId}
                        )
                        OR
                        u.DeptId IN (
                            SELECT DeptId 
                            FROM UserVisibleDept 
                            WHERE OwnerUserId = {ownerUserId}
                        )
                    )
                    ORDER BY u.IsOnline DESC, u.Name";

                var dt = db.Query(sql);
                AddDeptPathColumn(dt, "ParentDeptName", "DeptName");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception($"사용자 목록 조회 실패: {ex.Message}");
            }
        }

        // ==================== 4. 대화 가능한 사용자만 필터링 ====================
        /// <summary>
        /// 볼 수 있는 사용자 중에서 대화 차단되지 않은 사용자만 가져오기
        /// </summary>
        public DataTable GetChatableUsers(int userId)
        {
            try
            {
                string sql = $@"
                    SELECT DISTINCT 
                        u.UserId, 
                        u.Name, 
                        u.Nickname,
                        p.DeptName AS ParentDeptName,
                        d.DeptName AS DeptName,
                        u.IsOnline
                    FROM User u
                    LEFT JOIN Department d ON u.DeptId = d.DeptId
                    LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                    WHERE u.Role = 'user' 
                    AND u.UserId != {userId}
                    AND (
                        u.UserId IN (SELECT VisibleUserId FROM UserVisibleUser WHERE OwnerUserId = {userId})
                        OR u.DeptId IN (SELECT DeptId FROM UserVisibleDept WHERE OwnerUserId = {userId})
                    )
                    AND u.UserId NOT IN (
                        SELECT CASE 
                            WHEN UserAId = {userId} THEN UserBId 
                            ELSE UserAId 
                        END
                        FROM ChatPermission
                        WHERE (UserAId = {userId} OR UserBId = {userId})
                        AND IsBlocked = 1
                    )
                    ORDER BY u.IsOnline DESC, u.Name";

                var dt = db.Query(sql);
                AddDeptPathColumn(dt, "ParentDeptName", "DeptName");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception($"대화 가능 사용자 조회 실패: {ex.Message}");
            }
        }

        // ==================== 5. 특정 부서 사용자 목록 가져오기 ====================
        /// <summary>
        /// 특정 부서/팀의 사용자 목록 조회
        /// </summary>
        public DataTable GetUsersByDepartment(int deptId)
        {
            try
            {
                string sql = $@"
                    SELECT u.UserId, u.Name, u.Nickname, u.LoginId, u.IsOnline,
                           p.DeptName AS ParentDeptName,
                           d.DeptName AS DeptName
                    FROM User u
                    LEFT JOIN Department d ON u.DeptId = d.DeptId
                    LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                    WHERE u.DeptId = {deptId} AND u.Role = 'user'
                    ORDER BY u.IsOnline DESC, u.Name";

                var dt = db.Query(sql);
                AddDeptPathColumn(dt, "ParentDeptName", "DeptName");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception($"부서 사용자 조회 실패: {ex.Message}");
            }
        }

        // ==================== 6. 사용자가 볼 수 있는 부서 목록 ====================
        /// <summary>
        /// 사용자에게 권한이 있는 부서/팀 목록
        /// </summary>
        public DataTable GetVisibleDepartments(int userId)
        {
            try
            {
                string sql = $@"
                    SELECT DISTINCT d.DeptId, d.DeptName,
                           p.DeptName AS ParentDeptName,
                           (SELECT COUNT(*) FROM User WHERE DeptId = d.DeptId AND Role = 'user') AS UserCount
                    FROM Department d
                    LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                    WHERE d.DeptId IN (
                        SELECT DeptId FROM UserVisibleDept WHERE OwnerUserId = {userId}
                    )";

                var dt = db.Query(sql);
                // DeptPath 기준으로 정렬이 필요하므로 C#에서 DeptPath를 만들고 정렬
                AddDeptPathColumn(dt, "ParentDeptName", "DeptName", sortByDeptPath: true);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception($"부서 목록 조회 실패: {ex.Message}");
            }
        }

        // ==================== 7. 차단된 사용자 목록 ====================
        /// <summary>
        /// 현재 사용자가 대화할 수 없는(차단된) 사용자 목록
        /// </summary>
        public DataTable GetBlockedUsers(int userId)
        {
            try
            {
                string sql = $@"
                    SELECT u.UserId, u.Name, u.Nickname,
                           p.DeptName AS ParentDeptName,
                           d.DeptName AS DeptName
                    FROM User u
                    LEFT JOIN Department d ON u.DeptId = d.DeptId
                    LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                    WHERE u.UserId IN (
                        SELECT CASE 
                            WHEN UserAId = {userId} THEN UserBId 
                            ELSE UserAId 
                        END
                        FROM ChatPermission
                        WHERE (UserAId = {userId} OR UserBId = {userId})
                        AND IsBlocked = 1
                    )
                    ORDER BY u.Name";

                var dt = db.Query(sql);
                AddDeptPathColumn(dt, "ParentDeptName", "DeptName");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception($"차단 사용자 조회 실패: {ex.Message}");
            }
        }

        // ==================== 8. 사용자 검색 (권한 기반) ====================
        /// <summary>
        /// 이름으로 사용자 검색 (권한이 있는 사용자만)
        /// </summary>
        public DataTable SearchUsers(int ownerUserId, string searchKeyword)
        {
            try
            {
                string sql = $@"
                    SELECT DISTINCT 
                        u.UserId, 
                        u.Name, 
                        u.Nickname, 
                        u.LoginId,
                        p.DeptName AS ParentDeptName,
                        d.DeptName AS DeptName,
                        u.IsOnline
                    FROM User u
                    LEFT JOIN Department d ON u.DeptId = d.DeptId
                    LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                    WHERE u.Role = 'user' 
                    AND u.UserId != {ownerUserId}
                    AND (u.Name LIKE '%{searchKeyword}%' OR u.Nickname LIKE '%{searchKeyword}%' OR u.LoginId LIKE '%{searchKeyword}%')
                    AND (
                        u.UserId IN (SELECT VisibleUserId FROM UserVisibleUser WHERE OwnerUserId = {ownerUserId})
                        OR u.DeptId IN (SELECT DeptId FROM UserVisibleDept WHERE OwnerUserId = {ownerUserId})
                    )
                    ORDER BY u.IsOnline DESC, u.Name
                    LIMIT 50";

                var dt = db.Query(sql);
                AddDeptPathColumn(dt, "ParentDeptName", "DeptName");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception($"사용자 검색 실패: {ex.Message}");
            }
        }

        // ==================== 9. 온라인 사용자만 가져오기 ====================
        /// <summary>
        /// 권한이 있는 온라인 사용자만 조회
        /// </summary>
        public DataTable GetOnlineVisibleUsers(int userId)
        {
            try
            {
                string sql = $@"
                    SELECT DISTINCT 
                        u.UserId, 
                        u.Name, 
                        u.Nickname,
                        p.DeptName AS ParentDeptName,
                        d.DeptName AS DeptName
                    FROM User u
                    LEFT JOIN Department d ON u.DeptId = d.DeptId
                    LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                    WHERE u.Role = 'user' 
                    AND u.UserId != {userId}
                    AND u.IsOnline = 1
                    AND (
                        u.UserId IN (SELECT VisibleUserId FROM UserVisibleUser WHERE OwnerUserId = {userId})
                        OR u.DeptId IN (SELECT DeptId FROM UserVisibleDept WHERE OwnerUserId = {userId})
                    )
                    AND u.UserId NOT IN (
                        SELECT CASE 
                            WHEN UserAId = {userId} THEN UserBId 
                            ELSE UserAId 
                        END
                        FROM ChatPermission
                        WHERE (UserAId = {userId} OR UserBId = {userId})
                        AND IsBlocked = 1
                    )
                    ORDER BY u.Name";

                var dt = db.Query(sql);
                AddDeptPathColumn(dt, "ParentDeptName", "DeptName");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception($"온라인 사용자 조회 실패: {ex.Message}");
            }
        }

        // ==================== 10. 사용자 권한 상세 정보 ====================
        /// <summary>
        /// 특정 사용자의 권한 상세 정보 조회
        /// </summary>
        public PermissionInfo GetUserPermissionInfo(int userId)
        {
            try
            {
                var info = new PermissionInfo();

                // 볼 수 있는 사용자 수
                string sql1 = $"SELECT COUNT(DISTINCT VisibleUserId) FROM UserVisibleUser WHERE OwnerUserId = {userId}";
                var dt1 = db.Query(sql1);
                info.VisibleUserCount = dt1.Rows.Count > 0 ? Convert.ToInt32(dt1.Rows[0][0]) : 0;

                // 볼 수 있는 부서 수
                string sql2 = $"SELECT COUNT(DISTINCT DeptId) FROM UserVisibleDept WHERE OwnerUserId = {userId}";
                var dt2 = db.Query(sql2);
                info.VisibleDeptCount = dt2.Rows.Count > 0 ? Convert.ToInt32(dt2.Rows[0][0]) : 0;

                // 차단된 사용자 수
                string sql3 = $"SELECT COUNT(*) FROM ChatPermission WHERE (UserAId = {userId} OR UserBId = {userId}) AND IsBlocked = 1";
                var dt3 = db.Query(sql3);
                info.BlockedUserCount = dt3.Rows.Count > 0 ? Convert.ToInt32(dt3.Rows[0][0]) : 0;

                return info;
            }
            catch (Exception ex)
            {
                throw new Exception($"권한 정보 조회 실패: {ex.Message}");
            }
        }

        // ==================== 11. 메시지 전송 가능 여부 확인 ====================
        /// <summary>
        /// 메시지 전송 전 종합 권한 체크
        /// </summary>
        public MessageSendResult CanSendMessage(int fromUserId, int toUserId)
        {
            var result = new MessageSendResult();

            try
            {
                // 1. 볼 수 있는 사용자인지 확인
                if (!CanViewUser(fromUserId, toUserId))
                {
                    result.CanSend = false;
                    result.Reason = "상대방을 볼 수 있는 권한이 없습니다.";
                    return result;
                }

                // 2. 대화 차단 여부 확인
                if (!CanChat(fromUserId, toUserId))
                {
                    result.CanSend = false;
                    result.Reason = "관리자에 의해 대화가 차단되었습니다.";
                    return result;
                }

                // 3. 상대방이 존재하는지 확인
                string sql = $"SELECT COUNT(*) FROM User WHERE UserId = {toUserId} AND Role = 'user'";
                var dt = db.Query(sql);
                if (dt.Rows.Count == 0 || Convert.ToInt32(dt.Rows[0][0]) == 0)
                {
                    result.CanSend = false;
                    result.Reason = "상대방이 존재하지 않습니다.";
                    return result;
                }

                result.CanSend = true;
                result.Reason = "메시지 전송 가능";
                return result;
            }
            catch (Exception ex)
            {
                result.CanSend = false;
                result.Reason = $"권한 확인 실패: {ex.Message}";
                return result;
            }
        }
    }

    // ==================== 권한 정보 클래스 ====================
    public class PermissionInfo
    {
        public int VisibleUserCount { get; set; }
        public int VisibleDeptCount { get; set; }
        public int BlockedUserCount { get; set; }
    }

    // ==================== 메시지 전송 결과 클래스 ====================
    public class MessageSendResult
    {
        public bool CanSend { get; set; }
        public string Reason { get; set; }
    }
}