using DBP_WinformChat;
using System.Data;

namespace leehaeun
{
    internal class UserInfo
    {

        // 사용자 정보
        // DataTable -> DataRow
        public static DataRow User { get; private set; } = null!;

        // 사용자 프로필
        public static DataTable Profile { get; private set; } = null!;

        // 멀티 프로필 테이블 삭제

        // DB에서 사용자 및 프로필 정보 불러오기
        public static void GetInfo()
        {
            GetUserInfo();
            GetProfileInfo();
            // 멀티 프로필 가져오는 메서드 삭제
        }

        // DB에서 사용자 정보 불러오기
        public static void GetUserInfo()
        {
            string query = $"SELECT * FROM User WHERE Userid = '{LoginForm.UserId}';";
            DataTable dt = DBconnector.GetInstance().Query(query);
            dt.Columns.Add("DeptName", typeof(string));
            User = dt.Rows[0];
            int DeptId = Convert.ToInt32(User["DeptId"]);
            GetDeptName(DeptId);
        }

        // DB에서 사용자 프로필 정보 불러오기
        public static void GetProfileInfo()
        {
            string query = $"SELECT * FROM Profile WHERE UserId = '{LoginForm.UserId}' ORDER BY ProfileId;";
            Profile = DBconnector.GetInstance().Query(query);
        }

        // 멀티 프로필 매핑 정보 불러오기 메서드 삭제

        // DeptId로 DeptName 찾고 User 테이블에 추가
        private static void GetDeptName(int DeptId)
        {
            string query = $"SELECT * FROM Department WHERE DeptId = {DeptId};";
            DataTable dt = DBconnector.GetInstance().Query(query);
            User["DeptName"] = dt.Rows[0]["DeptName"];
        }

        // DataTable 초기화
        public static void ResetDataTable()
        {
            User = null!;
            Profile = null!;
            // 멀티 프로필 테이블 초기화 삭제
        }
    }
}