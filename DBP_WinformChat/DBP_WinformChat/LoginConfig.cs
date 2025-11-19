using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBP_WinformChat.Properties;

namespace leehaeun
{
    internal class LoginConfig
    {
        public static void saveConfig(CheckBox[] checkBoxes, TextBox[] loginInfo)
        {
            DBP_WinformChat.Properties.Settings.Default.RememberMe = checkBoxes[0].Checked;
            DBP_WinformChat.Properties.Settings.Default.SaveInfo = checkBoxes[1].Checked;
            if (checkBoxes[0].Checked || checkBoxes[1].Checked)
            {
                DBP_WinformChat.Properties.Settings.Default.SaveId = loginInfo[0].Text;
                DBP_WinformChat.Properties.Settings.Default.SavePw = loginInfo[1].Text;
            }
            DBP_WinformChat.Properties.Settings.Default.Save();
        }

        public static bool loadConfig(CheckBox[] checkBoxes, TextBox[] loginInfo)
        {
            checkBoxes[0].Checked = DBP_WinformChat.Properties.Settings.Default.RememberMe;
            checkBoxes[1].Checked = DBP_WinformChat.Properties.Settings.Default.SaveInfo;
            if (checkBoxes[0].Checked || checkBoxes[1].Checked)
            {
                loginInfo[0].Text = DBP_WinformChat.Properties.Settings.Default.SaveId;
                loginInfo[1].Text = DBP_WinformChat.Properties.Settings.Default.SavePw;
            }
            if (checkBoxes[0].Checked)
            {
                return true;
            }
            return false;
        }
    }
}
