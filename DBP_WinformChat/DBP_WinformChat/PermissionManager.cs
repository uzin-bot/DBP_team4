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
                // 1. 본인은 항상 볼 수 있음
                if (ownerUserId == targetUserId)
                    return true;

                // 2. UserVisibleUser 테이블 체크 (있으면 안 보이게 설정된 사용자)
                string sql = $"SELECT COUNT(*) FROM UserVisibleUser WHERE OwnerUserId = {ownerUserId} AND VisibleUserId = {targetUserId}";
                var dt = db.Query(sql);
                
                // UserVisibleUser에 있으면 안 보임 (차단된 사용자)
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    return false;

                // 3. 부서 권한 체크 (UserVisibleDept에 있으면 제한됨)
                string deptCheckSql = $@"
                    SELECT u.DeptId, d.ParentDeptId
                    FROM User u
                    LEFT JOIN Department d ON u.DeptId = d.DeptId
                    WHERE u.UserId = {targetUserId}";
                var dtDept = db.Query(deptCheckSql);

                if (dtDept.Rows.Count > 0 && dtDept.Rows[0]["DeptId"] != DBNull.Value)
                {
                    int targetDeptId = Convert.ToInt32(dtDept.Rows[0]["DeptId"]);
                    
                    // 제한된 부서인지 확인
                    string restrictedSql = $@"
                        SELECT COUNT(*) FROM UserVisibleDept 
                        WHERE OwnerUserId = {ownerUserId} AND DeptId = {targetDeptId}";
                    var dtRestricted = db.Query(restrictedSql);
                    
                    if (Convert.ToInt32(dtRestricted.Rows[0][0]) > 0)
                        return false; // 제한된 부서
                    
                    // 상위 부서가 제한되었는지 확인
                    if (dtDept.Rows[0]["ParentDeptId"] != DBNull.Value)
                    {
                        int parentDeptId = Convert.ToInt32(dtDept.Rows[0]["ParentDeptId"]);
                        string parentRestrictedSql = $@"
                            SELECT COUNT(*) FROM UserVisibleDept 
                            WHERE OwnerUserId = {ownerUserId} AND DeptId = {parentDeptId}";
                        var dtParentRestricted = db.Query(parentRestrictedSql);
                        
                        if (Convert.ToInt32(dtParentRestricted.Rows[0][0]) > 0)
                            return false; // 상위 부서가 제한됨
                    }
                }

                return true; // 제한 없음
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
                    u.LastLoginAt
                FROM User u
                LEFT JOIN Department d ON u.DeptId = d.DeptId
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE u.Role = 'user' 
                  AND u.UserId != {ownerUserId}
                  -- 부서 권한 체크: UserVisibleDept에 있으면 제한됨 (안 보임)
                  AND NOT EXISTS (
                    SELECT 1 FROM UserVisibleDept uvd
                    WHERE uvd.OwnerUserId = {ownerUserId}
                      AND (
                        uvd.DeptId = u.DeptId
                        OR uvd.DeptId = d.ParentDeptId
                      )
                  )
                  -- 사용자별 권한 체크: UserVisibleUser에 있으면 안 보임
                  AND NOT EXISTS (
                    SELECT 1 FROM UserVisibleUser 
                    WHERE OwnerUserId = {ownerUserId} AND VisibleUserId = u.UserId
                  )
                ORDER BY u.Name";

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
                    d.DeptName AS DeptName
                FROM User u
                LEFT JOIN Department d ON u.DeptId = d.DeptId
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE u.Role = 'user' 
                  AND u.UserId != {userId}
                  -- 부서 권한 체크: UserVisibleDept에 있으면 제한됨
                  AND NOT EXISTS (
                    SELECT 1 FROM UserVisibleDept uvd
                    WHERE uvd.OwnerUserId = {userId}
                      AND (uvd.DeptId = u.DeptId OR uvd.DeptId = d.ParentDeptId)
                  )
                  -- 사용자별 권한 체크: UserVisibleUser에 있으면 안 보임
                  AND NOT EXISTS (
                    SELECT 1 FROM UserVisibleUser 
                    WHERE OwnerUserId = {userId} AND VisibleUserId = u.UserId
                  )
                  -- 대화 차단 체크
                  AND NOT EXISTS (
                    SELECT 1 FROM ChatPermission
                    WHERE ((UserAId = {userId} AND UserBId = u.UserId) OR (UserAId = u.UserId AND UserBId = {userId}))
                      AND IsBlocked = 1
                  )
                ORDER BY u.Name";

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
                SELECT u.UserId, u.Name, u.Nickname, u.LoginId,
                       p.DeptName AS ParentDeptName,
                       d.DeptName AS DeptName
                FROM User u
                LEFT JOIN Department d ON u.DeptId = d.DeptId
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE u.DeptId = {deptId} AND u.Role = 'user'
                ORDER BY u.Name";

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
        /// 사용자에게 권한이 있는 부서/팀 목록 (제한되지 않은 부서)
        /// </summary>
        public DataTable GetVisibleDepartments(int userId)
        {
            try
            {
                // UserVisibleDept에 없는 부서 = 볼 수 있는 부서
                string sql = $@"
                SELECT d.DeptId, d.DeptName, d.ParentDeptId,
                       p.DeptName AS ParentDeptName,
                       (SELECT COUNT(*) FROM User WHERE DeptId = d.DeptId AND Role = 'user') AS UserCount
                FROM Department d
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE NOT EXISTS (
                    SELECT 1 FROM UserVisibleDept uvd
                    WHERE uvd.OwnerUserId = {userId}
                      AND (
                        uvd.DeptId = d.DeptId
                        OR uvd.DeptId = d.ParentDeptId
                      )
                )
                ORDER BY IFNULL(p.DeptId, d.DeptId), d.ParentDeptId IS NULL DESC, d.DeptName";

                var dt = db.Query(sql);
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
                var safeKeyword = searchKeyword.Replace("'", "''");
                
                string sql = $@"
                SELECT DISTINCT 
                    u.UserId, 
                    u.Name, 
                    u.Nickname, 
                    u.LoginId,
                    p.DeptName AS ParentDeptName,
                    d.DeptName AS DeptName
                FROM User u
                LEFT JOIN Department d ON u.DeptId = d.DeptId
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE u.Role = 'user' 
                  AND u.UserId != {ownerUserId}
                  AND (u.Name LIKE '%{safeKeyword}%' OR u.Nickname LIKE '%{safeKeyword}%' OR u.LoginId LIKE '%{safeKeyword}%')
                  -- 부서 권한 체크
                  AND NOT EXISTS (
                    SELECT 1 FROM UserVisibleDept uvd
                    WHERE uvd.OwnerUserId = {ownerUserId}
                      AND (uvd.DeptId = u.DeptId OR uvd.DeptId = d.ParentDeptId)
                  )
                  -- 사용자별 권한 체크: UserVisibleUser에 있으면 안 보임
                  AND NOT EXISTS (
                    SELECT 1 FROM UserVisibleUser 
                    WHERE OwnerUserId = {ownerUserId} AND VisibleUserId = u.UserId
                  )
                ORDER BY u.Name
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

        // ==================== 9. 사용자 권한 상세 정보 ====================
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

                // 제한된 부서 수
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

        // ==================== 10. 메시지 전송 가능 여부 확인 ====================
        /// <summary>
        /// 메시지 전송 전 종합 권한 체크
        /// </summary>
        public MessageSendResult CanSendMessage(int fromUserId, int toUserId)
        {
            var result = new MessageSendResult();

            try
            {
                // 1. 본인에게는 메시지 불가
                if (fromUserId == toUserId)
                {
                    result.CanSend = false;
                    result.Reason = "자기 자신에게는 메시지를 보낼 수 없습니다.";
                    return result;
                }

                // 2. 상대방이 존재하는지 확인
                string userCheckSql = $"SELECT COUNT(*) FROM User WHERE UserId = {toUserId} AND Role = 'user'";
                var dtUser = db.Query(userCheckSql);
                if (dtUser.Rows.Count == 0 || Convert.ToInt32(dtUser.Rows[0][0]) == 0)
                {
                    result.CanSend = false;
                    result.Reason = "상대방이 존재하지 않습니다.";
                    return result;
                }

                // 3. 볼 수 있는 사용자인지 확인
                if (!CanViewUser(fromUserId, toUserId))
                {
                    result.CanSend = false;
                    result.Reason = "해당 사용자를 볼 수 있는 권한이 없습니다.";
                    return result;
                }

                // 4. 대화 차단 여부 확인
                if (!CanChat(fromUserId, toUserId))
                {
                    result.CanSend = false;
                    result.Reason = "관리자에 의해 대화가 차단되었습니다.";
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