using DBP_WinformChat;
using kyg;
using leehaeun;
using MySqlConnector;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using 남예솔;
using System.Collections.Generic;   // HashSet 사용

namespace DBP_Chat
{
    public partial class Dept : Form
    {
        private int currentUserId;
        private string currentUserName;
        private string currentUserNickname;
        private PermissionManager permissionManager;

        private Color _darkBack = Color.FromArgb(32, 32, 32);
        private Color _darkPanel = Color.FromArgb(45, 45, 45);
        private Color _darkHeader = Color.FromArgb(64, 64, 64);
        private Color _darkButton = Color.FromArgb(80, 100, 90);
        private Color _darkText = Color.White;

        public Dept(int userId, string name, string nickname)
        {
            InitializeComponent();   // 디자이너 생성 코드 호출

            ThemeManager.ThemeChanged += mode => this.OnThemeChanged(mode);

            this.currentUserId = userId;
            this.currentUserName = name;
            this.currentUserNickname = nickname;
            this.permissionManager = new PermissionManager();

            // 폼 Load 이벤트는 여기서 한 번만 연결
            this.Load += this.Dept_Load;

            // TreeView & 버튼 이벤트 – Designer에 안 걸어놨다면 여기서만 등록
            tvdept.NodeMouseDoubleClick += this.tvdept_NodeMouseDoubleClick;
            tvdept.AfterSelect += this.tvdept_AfterSelect;
            tvdept.NodeMouseClick += this.tvdept_NodeMouseClick;

            btnsearch.Click += this.btnsearch_Click;
            btnadd.Click += this.btnadd_Click;
            btndelete.Click += this.btndelete_Click;
            btnChat.Click += this.btnChat_Click;
            btnchatlist.Click += this.btnchatlist_Click;
            lBlist.SelectedIndexChanged += this.lBlist_SelectedIndexChanged;
        }

        private void Dept_Load(object sender, EventArgs e)
        {
            this.LoadTreeView();
            this.LoadFavoriteList();

            try
            {
                var visibleDepts = this.permissionManager.GetVisibleDepartments(this.currentUserId);
                cbDept.Items.Clear();

                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (DataRow dept in visibleDepts.Rows)
                {
                    string deptPath = dept["DeptPath"]?.ToString()?.Trim() ?? string.Empty;
                    string deptName = dept["DeptName"]?.ToString()?.Trim() ?? string.Empty;

                    string topLevelDept =
                        !string.IsNullOrEmpty(deptPath)
                            ? deptPath.Split('>')[0].Trim()
                            : deptName;

                    if (string.IsNullOrEmpty(topLevelDept)) continue;

                    if (seen.Add(topLevelDept))
                        cbDept.Items.Add(topLevelDept);
                }

                if (cbDept.Items.Count > 0)
                    cbDept.SelectedIndex = 0;
            }
            catch
            {
                // 예외는 무시
            }

            this.ApplyLightHelper();

            // 다크 모드 라디오 버튼 설정
            rbDarkMode.AutoCheck = false;
            // Click은 여기서만 등록
            rbDarkMode.Click += this.rbDarkMode_Click;
            // CheckedChanged는 Designer에서 이미 연결했으므로 여기서는 다시 안 건다.
            // rbDarkMode.CheckedChanged += this.rbDarkMode_CheckedChanged;

            if (ThemeManager.CurrentMode == ThemeMode.Dark)
                this.ApplyTheme(true);
            else
                this.ApplyLightHelper();
        }

        private void OnThemeChanged(ThemeMode mode)
        {
            if (mode == ThemeMode.Dark)
            {
                this.ApplyTheme(true);
                rbDarkMode.Checked = true;
            }
            else
            {
                this.ApplyLightHelper();
                rbDarkMode.Checked = false;
            }
        }

        private void rbDarkMode_Click(object sender, EventArgs e)
        {
            rbDarkMode.Checked = !rbDarkMode.Checked;
        }

        private void rbDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbDarkMode.Checked)
            {
                ThemeManager.SetTheme(ThemeMode.Dark);
                this.ApplyTheme(true);
            }
            else
            {
                this.ApplyLightHelper();
            }
        }

        private void btnchatlist_Click(object sender, EventArgs e)
        {
            new chatlist().Show();
        }

        public void RefreshFavorites()
        {
            this.LoadFavoriteList();
        }

        private void LoadTreeView()
        {
            this.tvdept.Nodes.Clear();
            TreeNode companyNode = new TreeNode("회사");
            this.tvdept.Nodes.Add(companyNode);

            DataTable visibleDepts = this.permissionManager.GetVisibleDepartments(this.currentUserId);

            if (visibleDepts == null || visibleDepts.Rows.Count == 0)
            {
                TreeNode noDeptNode = new TreeNode("(볼 수 있는 부서가 없습니다)");
                companyNode.Nodes.Add(noDeptNode);
                this.tvdept.ExpandAll();
                return;
            }

            foreach (DataRow dept in visibleDepts.Rows)
            {
                string deptDisplayName = dept["DeptPath"] != DBNull.Value
                    ? dept["DeptPath"].ToString()
                    : dept["DeptName"].ToString();

                TreeNode deptNode = new TreeNode($"{deptDisplayName} ({dept["UserCount"]}명)");
                deptNode.Tag = dept["DeptId"];
                companyNode.Nodes.Add(deptNode);

                DataTable deptUsers = this.permissionManager.GetUsersByDepartment(Convert.ToInt32(dept["DeptId"]));

                foreach (DataRow user in deptUsers.Rows)
                {
                    int uid = Convert.ToInt32(user["UserId"]);

                    if (uid != this.currentUserId && !this.permissionManager.CanViewUser(this.currentUserId, uid))
                        continue;

                    string uname = user["Name"].ToString();
                    string nick = user["Nickname"].ToString();
                    string loginId = user["LoginId"].ToString();

                    string text = $"({loginId}) {uname} ({nick})";

                    TreeNode userNode = new TreeNode(text);
                    userNode.Tag = uid;

                    if (uid == this.currentUserId)
                    {
                        userNode.Text = $"{text}  - 나";
                        userNode.NodeFont = new Font("맑은 고딕", 10, FontStyle.Bold);
                        userNode.ForeColor = Color.FromArgb(119, 136, 115);
                    }
                    else if (!this.permissionManager.CanChat(this.currentUserId, uid))
                    {
                        userNode.Text = $"{text} 🚫";
                    }

                    deptNode.Nodes.Add(userNode);
                }
            }

            this.tvdept.ExpandAll();
        }

        private void tvdept_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }

        private void tvdept_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level != 2) return;

            int targetUserId = Convert.ToInt32(e.Node.Tag);

            var f = new leehaeun.UserInfoForm(targetUserId);
            f.Show();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            string id = this.txtID.Text.Trim();
            string name = this.txtname.Text.Trim();
            string dept = this.cbDept.SelectedItem?.ToString()?.Trim() ?? string.Empty;

            SearchResultForm s = new SearchResultForm(id, name, dept, this.currentUserId, this);
            s.Show();
        }

        private void LoadFavoriteList()
        {
            this.lBlist.Items.Clear();

            string sql = $@"
                SELECT u.UserId, u.LoginId, u.Name, p.Nickname
                FROM Favorite f
                JOIN User u ON f.FavoriteUserId = u.UserId
                JOIN Profile p ON u.UserId = p.UserId AND p.IsDefault = 1
                WHERE f.UserId = {this.currentUserId}";

            DataTable dt = DBconnector.GetInstance().Query(sql);

            foreach (DataRow row in dt.Rows)
            {
                int userId = Convert.ToInt32(row["UserId"]);

                if (!this.permissionManager.CanViewUser(this.currentUserId, userId))
                    continue;

                string displayText = $"{row["UserId"]} - {row["Name"]} ({row["Nickname"]})";

                if (!this.permissionManager.CanChat(this.currentUserId, userId))
                    displayText += " 🚫";

                this.lBlist.Items.Add(displayText);
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            if (this.tvdept.SelectedNode == null || this.tvdept.SelectedNode.Level != 2)
            {
                MessageBox.Show("직원을 선택하세요!");
                return;
            }

            int targetUserId = Convert.ToInt32(this.tvdept.SelectedNode.Tag);

            if (!this.permissionManager.CanViewUser(this.currentUserId, targetUserId))
            {
                MessageBox.Show("해당 사용자를 볼 수 있는 권한이 없습니다.", "권한 없음",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string checkSql = $@"
                SELECT COUNT(*) 
                FROM Favorite 
                WHERE UserId = {this.currentUserId} AND FavoriteUserId = {targetUserId}";

            DataTable dt = DBconnector.GetInstance().Query(checkSql);

            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                MessageBox.Show("이미 즐겨찾기에 등록되어 있습니다!");
                return;
            }

            string sql =
                $"INSERT INTO Favorite (UserId, FavoriteUserId) VALUES ({this.currentUserId}, {targetUserId})";

            DBconnector.GetInstance().NonQuery(sql);

            MessageBox.Show("즐겨찾기에 추가되었습니다!");
            this.LoadFavoriteList();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (this.lBlist.SelectedItem == null)
            {
                MessageBox.Show("삭제할 대상을 선택하세요!");
                return;
            }

            string userIdText = this.lBlist.SelectedItem.ToString().Split('-')[0].Trim();
            int targetUserId = Convert.ToInt32(userIdText);

            string sql =
                $"DELETE FROM Favorite WHERE UserId = {this.currentUserId} AND FavoriteUserId = {targetUserId}";

            DBconnector.GetInstance().NonQuery(sql);

            MessageBox.Show("삭제되었습니다!");
            this.LoadFavoriteList();
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            int targetUserId = -1;

            if (this.lBlist.SelectedItem != null)
            {
                string userIdText = this.lBlist.SelectedItem.ToString().Split('-')[0].Trim();
                targetUserId = Convert.ToInt32(userIdText);
            }
            else if (this.tvdept.SelectedNode != null && this.tvdept.SelectedNode.Level == 2)
            {
                targetUserId = Convert.ToInt32(this.tvdept.SelectedNode.Tag);
            }
            else
            {
                MessageBox.Show("대화할 직원을 선택하세요!");
                return;
            }

            var result = this.permissionManager.CanSendMessage(this.currentUserId, targetUserId);
            if (!result.CanSend)
            {
                MessageBox.Show(result.Reason, "채팅 불가",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            new ChatForm(this.currentUserId, targetUserId).Show();
        }

        private void tvdept_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.lBlist.ClearSelected();
        }

        private void lBlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tvdept.SelectedNode = null;
        }

        private void logout_button_Click(object sender, EventArgs e)
        {
            LoginForm.Logout = true;
            this.Close();
        }

        private void change_profile_button_Click(object sender, EventArgs e)
        {
            EditInfoForm editForm = new EditInfoForm();
            editForm.ShowDialog();
        }

        private void ApplyTheme(bool isDark)
        {
            if (!isDark)
            {
                this.ApplyLightHelper();
                return;
            }

            Color back = this._darkBack;
            Color panel = this._darkPanel;
            Color header = this._darkHeader;
            Color button = this._darkButton;
            Color text = this._darkText;

            this.BackColor = back;

            this.headerPanel.BackColor = header;
            this.headerLabel.ForeColor = Color.White;
            this.headerLabel.BackColor = Color.Transparent;

            this.tvdept.BackColor = panel;
            this.tvdept.ForeColor = text;

            this.gpinfo.BackColor = panel;
            this.gpinfo.ForeColor = text;

            this.panel1.BackColor = panel;
            this.panel1.ForeColor = text;

            this.label1.ForeColor = text; this.label1.BackColor = Color.Transparent;
            this.label2.ForeColor = text; this.label2.BackColor = Color.Transparent;
            this.label3.ForeColor = text; this.label3.BackColor = Color.Transparent;
            this.label4.ForeColor = text; this.label4.BackColor = Color.Transparent;
            this.label5.ForeColor = text; this.label5.BackColor = Color.Transparent;

            this.txtID.BackColor = Color.FromArgb(30, 30, 30);
            this.txtID.ForeColor = text;
            this.txtname.BackColor = Color.FromArgb(30, 30, 30);
            this.txtname.ForeColor = text;

            this.cbDept.BackColor = Color.FromArgb(30, 30, 30);
            this.cbDept.ForeColor = text;
            this.cbDept.FlatStyle = FlatStyle.Flat;

            this.lBlist.BackColor = Color.FromArgb(30, 30, 30);
            this.lBlist.ForeColor = text;

            this.StyleButton(this.btnsearch, button);
            this.StyleButton(this.btnadd, button);
            this.StyleButton(this.btndelete, button);
            this.StyleButton(this.btnChat, button);
            this.StyleButton(this.btnchatlist, button);
            this.StyleButton(this.change_profile_button, button);
            this.StyleButton(this.logout_button, button);

            this.rbDarkMode.ForeColor = text;
        }

        private void StyleButton(Button b, Color back)
        {
            b.BackColor = back;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
        }

        private void ApplyLightHelper()
        {
            DeptUIHelper.Apply(this);
            this.ResetLabelBackgrounds(this);
            headerLabel.BackColor = Color.Transparent;

            this.cbDept.BackColor = SystemColors.Window;
            this.cbDept.ForeColor = SystemColors.ControlText;
            this.cbDept.FlatStyle = FlatStyle.Standard;

            this.rbDarkMode.ForeColor = SystemColors.ControlText;
        }

        private void ResetLabelBackgrounds(Control root)
        {
            foreach (Control child in root.Controls)
            {
                if (child is Label lbl)
                {
                    lbl.BackColor = Color.Transparent;
                    lbl.ForeColor = SystemColors.ControlText;
                }
                this.ResetLabelBackgrounds(child);
            }
        }
    }
}