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

        // DB에서 사용자 및 프로필 정보 불러오기
        public static void GetInfo()
        {
            GetUserInfo();
            GetProfileInfo();
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
        }
    }
}