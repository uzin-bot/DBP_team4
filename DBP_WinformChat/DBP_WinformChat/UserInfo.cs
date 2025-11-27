using DBP_WinformChat;
using System.Data;

namespace leehaeun
{
    internal class UserInfo
    {

        // 사용자 정보
        public static DataTable User { get; private set; } = null!;

        // 사용자 기본 프로필
        public static DataTable Profile { get; private set; } = null!;

        // 내가 상대에게 보여줄 멀티프로필
        public static DataTable? OutboundProfile { get; private set; } = null;

        // 상대가 나에게 보여줄 멀티프로필
        // public static DataTable? InboundProfile { get; private set; } = null;
        // 프로필 열 때만 받아와도 될듯(더블클릭하면 바로 받아와서 프로필 열리게)

        // DB에서 사용자 및 프로필 정보 불러오기
        public static void GetInfo()
        {
            GetUserInfo();
            GetProfileInfo();
            GetMulProfileMapping();
        }

        // DB에서 사용자 정보 불러오기
        public static void GetUserInfo()
        {
            string query = $"SELECT * FROM User WHERE Userid = '{LoginForm.UserId}';";
            User = DBconnector.GetInstance().Query(query);
            int DeptId = Convert.ToInt32(User.Rows[0]["DeptId"]);
            GetDeptName(DeptId);
        }

        // DB에서 사용자 프로필 정보 불러오기
        public static void GetProfileInfo()
        {
            string query = $"SELECT * FROM Profile WHERE UserId = '{LoginForm.UserId}' ORDER BY ProfileId;";
            Profile = DBconnector.GetInstance().Query(query);
        }

        // DB에서 멀티프로필 매핑 정보 불러오기
        public static void GetMulProfileMapping()
        {
            string oquery = $"SELECT * FROM UserProfileMap WHERE OwnerUserId = {LoginForm.UserId};";
            OutboundProfile = DBconnector.GetInstance().Query(oquery);
            //string iquery = $"SELECT * FROM UserProfileMap WHERE TargetUserId = {LoginForm.UserId};";
            //InboundProfile = DBconnector.GetInstance().Query(iquery);
        }

        // DeptId로 DeptName 찾고 User 테이블에 추가
        private static void GetDeptName(int DeptId)
        {
            string query = $"SELECT * FROM Department WHERE DeptId = {DeptId};";
            DataTable dt = DBconnector.GetInstance().Query(query);
            User.Columns.Add("DeptName", typeof(string));
            User.Rows[0]["DeptName"] = dt.Rows[0]["DeptName"];
        }

        // DataTable 초기화
        public static void ResetDataTable()
        {
            User = null!;
            Profile = null!;
            OutboundProfile = null;
            //InboundProfile = null!;
        }
    }
}