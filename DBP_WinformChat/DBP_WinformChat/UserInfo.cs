using DBP_WinformChat;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace leehaeun
{
    internal class UserInfo
    {

        // 사용자 정보
        public static DataTable User { get; private set; } = null!;

        // 사용자 기본 프로필
        public static DataTable Profile { get; private set; } = null!;

        // 사용자 멀티 프로필
        // 프로필에 합칠 수도 있음
        public static DataTable MulProfile { get; private set; } = null!;

        // DB에서 사용자 정보 불러오기
        public static void GetUserInfo()
        {
            string query = $"SELECT * FROM User WHERE Userid = '{LoginForm.UserId}';";
            User = DBconnector.GetInstance().Query(query);
            int DeptId = Convert.ToInt32(User.Rows[0]["DeptId"]);
            GetDeptName(DeptId);
        }

        // DB에서 사용자 기본 프로필 정보 불러오기
        public static void GetProfileInfo()
        {
            string query = $"SELECT * FROM Profile WHERE UserId = '{LoginForm.UserId}' AND IsDefault = 1;";
            Profile = DBconnector.GetInstance().Query(query);
        }

        // DB에서 사용자 멀티 프로필 정보 불러오기
        public static void GetMulProfileInfo()
        {
            string query = $"SELECT * FROM Profile WHERE UserId = '{LoginForm.UserId}' AND IsDefault = 0;";
            MulProfile = DBconnector.GetInstance().Query(query);
        }

        // DeptId로 DeptName 찾고 User 테이블에 추가
        private static void GetDeptName(int DeptId)
        {
            string query = $"SELECT * FROM Department WHERE DeptId = {DeptId};";
            DataTable dt = DBconnector.GetInstance().Query(query);
            User.Columns.Add("DeptName", typeof(string));
            User.Rows[0]["DeptName"] = dt.Rows[0]["DeptName"];
        }

        public static void ResetDataTable()
        {
            User = null!;
            Profile = null!;
            MulProfile = null!;
        }
    }
}