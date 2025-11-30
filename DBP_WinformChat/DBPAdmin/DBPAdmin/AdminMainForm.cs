using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DBPAdmin
{
    public partial class AdminMainForm : Form
    {
        private DBP_WinformChat.DBconnector db;

        public AdminMainForm()
        {
            InitializeComponent();

            db = DBP_WinformChat.DBconnector.GetInstance();

            if (!db.ConnectionTest())
            {
                MessageBox.Show("데이터베이스 연결에 실패했습니다.", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            AttachMenuEvents();
            ShowDashboard();
        }

        // ==================== 메뉴 이벤트 연결 ====================
        private void AttachMenuEvents()
        {
            btnDashboard.Click += MenuButton_Click;
            btnDepartment.Click += MenuButton_Click;
            btnUsers.Click += MenuButton_Click;
            btnChatSearch.Click += MenuButton_Click;
            btnLoginLog.Click += MenuButton_Click;
            btnPermission.Click += MenuButton_Click;
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            SetActiveButton(clickedButton);

            string screenText = clickedButton.Text
                .Replace(" ", "")
                .Replace("📊", "").Replace("🏢", "").Replace("👥", "")
                .Replace("💬", "").Replace("🕐", "").Replace("🔒", "");

            ClearContent();

            switch (screenText)
            {
                case "대시보드": ShowDashboard(); break;
                case "부서관리": ShowDepartmentManage(); break;
                case "사용자관리": ShowUserManage(); break;
                case "대화내용검색": ShowChatSearch(); break;
                case "로그인기록": ShowLoginLog(); break;
                case "권한관리": ShowPermissionManage(); break;
            }
        }

        private void SetActiveButton(Button activeBtn)
        {
            foreach (Control ctrl in pnlLeft.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = UIHelper.Colors.DarkBg;
                    btn.ForeColor = UIHelper.Colors.TextLight;
                }
            }
            activeBtn.BackColor = UIHelper.Colors.Primary;
            activeBtn.ForeColor = Color.White;
        }

        private void ClearContent()
        {
            pnlContent.Controls.Clear();
        }

        // ==================== A. 대시보드 ====================
        private void ShowDashboard()
        {
            var title = UIHelper.CreateTitle("대시보드");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            int cardW = 300, cardH = 120, cardSpacing = 30;
            int startX = UIHelper.CalculateMultiElementStartX(pnlContent.Width, cardW, 3, cardSpacing);
            int cardY = UIHelper.CalculateCenterY(pnlContent.Height, cardH) - 50;

            var card1 = UIHelper.CreateCard(startX, cardY, cardW, cardH);
            card1.Controls.Add(UIHelper.CreateLabel("전체 사용자", 20, 20, 9, UIHelper.Colors.TextSecondary));
            card1.Controls.Add(UIHelper.CreateLabel(GetTotalUserCount() + "명", 20, 50, 28, UIHelper.Colors.TextPrimary, true));
            pnlContent.Controls.Add(card1);

            var card2 = UIHelper.CreateCard(startX + cardW + cardSpacing, cardY, cardW, cardH);
            card2.Controls.Add(UIHelper.CreateLabel("전체 부서", 20, 20, 9, UIHelper.Colors.TextSecondary));
            card2.Controls.Add(UIHelper.CreateLabel(GetTotalDepartmentCount() + "개", 20, 50, 28, UIHelper.Colors.TextPrimary, true));
            pnlContent.Controls.Add(card2);

            var card3 = UIHelper.CreateCard(startX + (cardW + cardSpacing) * 2, cardY, cardW, cardH);
            card3.Controls.Add(UIHelper.CreateLabel("오늘 접속자", 20, 20, 9, UIHelper.Colors.TextSecondary));
            card3.Controls.Add(UIHelper.CreateLabel(GetTodayLoginCount() + "명", 20, 50, 28, UIHelper.Colors.TextPrimary, true));
            pnlContent.Controls.Add(card3);
        }

        private int GetTotalUserCount()
        {
            try
            {
                var dt = db.Query("SELECT COUNT(*) FROM User WHERE Role = 'user'");
                return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
            }
            catch { return 0; }
        }

        private int GetTotalDepartmentCount()
        {
            try
            {
                var dt = db.Query("SELECT COUNT(*) FROM Department");
                return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
            }
            catch { return 0; }
        }

        private int GetTodayLoginCount()
        {
            try
            {
                var dt = db.Query(@"SELECT COUNT(DISTINCT UserId) FROM UserLog 
                                   WHERE DATE(CreatedAt) = CURDATE() AND ActionType = 'LOGIN'");
                return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
            }
            catch { return 0; }
        }

        // ==================== B. 부서관리 ====================
        private void ShowDepartmentManage()
        {
            var title = UIHelper.CreateTitle("부서 관리 (계층 구조)");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            var btnAdd = UIHelper.CreateBlueButton("+ 부서 추가", pnlContent.Width - 140, 15, 120, 40);
            btnAdd.Click += (s, e) => ShowDepartmentEditPanel(null);
            pnlContent.Controls.Add(btnAdd);

            int cardWidth = 970;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX, 70, cardWidth, 60);
            var txtSearch = UIHelper.CreateTextBox(15, 18, 840, 25, "txtDeptSearch", "부서명 검색...");
            var btnSearch = UIHelper.CreateBlueButton("검색", 870, 13, 80, 35);
            btnSearch.Click += (s, e) => LoadDepartmentData(txtSearch.Text);
            searchCard.Controls.Add(txtSearch);
            searchCard.Controls.Add(btnSearch);
            pnlContent.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX, 145, cardWidth, 450, "dgvDepartment");
            dgv.Columns.Add("DeptId", "ID");
            dgv.Columns["DeptId"].Visible = false;
            dgv.Columns.Add("DeptName", "부서명");
            dgv.Columns.Add("ParentName", "상위부서");
            dgv.Columns.Add("UserCount", "인원수");
            dgv.Columns["UserCount"].Width = 80;

            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "수정",
                UseColumnTextForButtonValue = true,
                Width = 100,
                Name = "Edit"
            });

            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "삭제",
                UseColumnTextForButtonValue = true,
                Width = 100,
                Name = "Delete"
            });

            dgv.CellClick += DgvDepartment_CellClick;
            pnlContent.Controls.Add(dgv);

            LoadDepartmentData("");
        }

        private void LoadDepartmentData(string searchKeyword)
        {
            var dgv = pnlContent.Controls.Find("dgvDepartment", true).FirstOrDefault() as DataGridView;
            if (dgv == null) return;

            dgv.Rows.Clear();

            string query = @"
                SELECT d.DeptId, d.DeptName, d.ParentDeptId,
                       IFNULL(p.DeptName, '-') AS ParentName,
                       (SELECT COUNT(*) FROM User WHERE DeptId = d.DeptId) AS UserCount
                FROM Department d
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE d.DeptName LIKE @search
                ORDER BY d.ParentDeptId, d.DeptName";

            try
            {
                var dt = db.Query(query, new MySqlParameter("@search", $"%{searchKeyword}%"));
                foreach (DataRow row in dt.Rows)
                {
                    dgv.Rows.Add(row["DeptId"], row["DeptName"], row["ParentName"], row["UserCount"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"부서 목록 로드 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvDepartment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dgv = sender as DataGridView;
            int deptId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["DeptId"].Value);

            if (dgv.Columns[e.ColumnIndex].Name == "Edit")
            {
                ShowDepartmentEditPanel(deptId);
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (MessageBox.Show("이 부서를 삭제하시겠습니까?\n(소속 사용자나 하위 부서가 있는 경우 삭제할 수 없습니다)",
                    "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteDepartment(deptId);
                }
            }
        }

        private void ShowDepartmentEditPanel(int? deptId)
        {
            pnlContent.Visible = false;

            var editPanel = new Panel
            {
                Name = "pnlDeptEdit",
                Location = pnlContent.Location,
                Size = pnlContent.Size,
                BackColor = pnlContent.BackColor,
                Padding = new Padding(15),
                AutoScroll = true
            };
            this.Controls.Add(editPanel);
            editPanel.BringToFront();

            var title = UIHelper.CreateTitle(deptId.HasValue ? "부서 수정" : "부서 추가");
            title.Location = new Point(15, 15);
            editPanel.Controls.Add(title);

            int cardWidth = 570, cardHeight = 300;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);
            int cardY = UIHelper.CalculateCenterY(pnlContent.Height, cardHeight) - 30;

            var inputCard = UIHelper.CreateCard(cardX, cardY, cardWidth, cardHeight);

            int yPos = 30;

            inputCard.Controls.Add(UIHelper.CreateLabel("부서명", 30, yPos, 10, Color.Black, true));
            var txtName = UIHelper.CreateTextBox(30, yPos + 25, 510, 30, "txtDeptName");
            inputCard.Controls.Add(txtName);
            yPos += 80;

            inputCard.Controls.Add(UIHelper.CreateLabel("상위 부서 (선택 안 하면 최상위)", 30, yPos, 10, Color.Black, true));
            var cboParent = UIHelper.CreateComboBox(30, yPos + 25, 510, 30, "cboParentDept");
            LoadParentDepartmentCombo(cboParent, deptId);
            inputCard.Controls.Add(cboParent);
            yPos += 80;

            var btnSave = UIHelper.CreateBlueButton("저장", 30, yPos, 250, 40);
            btnSave.Click += (s, e) => SaveDepartment(deptId, editPanel);
            inputCard.Controls.Add(btnSave);

            var btnCancel = UIHelper.CreateBlueButton("취소", 290, yPos, 220, 40);
            btnCancel.BackColor = Color.Gray;
            btnCancel.Click += (s, e) => CloseDepartmentEditPanel(editPanel);
            inputCard.Controls.Add(btnCancel);

            editPanel.Controls.Add(inputCard);

            if (deptId.HasValue)
            {
                LoadDepartmentForEdit(deptId.Value, txtName, cboParent);
            }
        }

        private void LoadParentDepartmentCombo(ComboBox cbo, int? excludeDeptId)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new ComboBoxItem { Text = "없음 (최상위 부서)", Value = "" });

            try
            {
                string query = "SELECT DeptId, DeptName FROM Department";
                if (excludeDeptId.HasValue)
                {
                    query += " WHERE DeptId != @excludeId";
                }
                query += " ORDER BY DeptName";

                var dt = excludeDeptId.HasValue
                    ? db.Query(query, new MySqlParameter("@excludeId", excludeDeptId.Value))
                    : db.Query(query);

                foreach (DataRow row in dt.Rows)
                {
                    cbo.Items.Add(new ComboBoxItem
                    {
                        Text = row["DeptName"].ToString(),
                        Value = row["DeptId"].ToString()
                    });
                }

                cbo.DisplayMember = "Text";
                cbo.ValueMember = "Value";
                cbo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"상위 부서 로드 실패: {ex.Message}", "오류");
            }
        }

        private void LoadDepartmentForEdit(int deptId, TextBox txtName, ComboBox cboParent)
        {
            try
            {
                var dt = db.Query("SELECT DeptName, ParentDeptId FROM Department WHERE DeptId = @id",
                    new MySqlParameter("@id", deptId));

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    txtName.Text = row["DeptName"].ToString();

                    if (row["ParentDeptId"] != DBNull.Value)
                    {
                        string parentId = row["ParentDeptId"].ToString();
                        for (int i = 0; i < cboParent.Items.Count; i++)
                        {
                            if (cboParent.Items[i] is ComboBoxItem item && item.Value == parentId)
                            {
                                cboParent.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"부서 정보 로드 실패: {ex.Message}", "오류");
            }
        }

        private void SaveDepartment(int? deptId, Panel editPanel)
        {
            var txtName = editPanel.Controls.Find("txtDeptName", true).FirstOrDefault() as TextBox;
            var cboParent = editPanel.Controls.Find("cboParentDept", true).FirstOrDefault() as ComboBox;

            if (string.IsNullOrWhiteSpace(txtName?.Text))
            {
                MessageBox.Show("부서명을 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? parentId = null;
            if (cboParent.SelectedItem is ComboBoxItem parentItem && !string.IsNullOrEmpty(parentItem.Value))
            {
                parentId = Convert.ToInt32(parentItem.Value);
            }

            try
            {
                if (deptId.HasValue)
                {
                    db.NonQuery("UPDATE Department SET DeptName = @name, ParentDeptId = @parentId WHERE DeptId = @id",
                        new MySqlParameter("@name", txtName.Text),
                        new MySqlParameter("@parentId", parentId.HasValue ? (object)parentId.Value : DBNull.Value),
                        new MySqlParameter("@id", deptId.Value));
                    MessageBox.Show("부서가 수정되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    db.NonQuery("INSERT INTO Department (DeptName, ParentDeptId) VALUES (@name, @parentId)",
                        new MySqlParameter("@name", txtName.Text),
                        new MySqlParameter("@parentId", parentId.HasValue ? (object)parentId.Value : DBNull.Value));
                    MessageBox.Show("부서가 추가되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CloseDepartmentEditPanel(editPanel);
                ShowDepartmentManage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"저장 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseDepartmentEditPanel(Panel editPanel)
        {
            this.Controls.Remove(editPanel);
            editPanel.Dispose();
            pnlContent.Visible = true;
        }

        private void DeleteDepartment(int deptId)
        {
            try
            {
                var dtChild = db.Query("SELECT COUNT(*) FROM Department WHERE ParentDeptId = @id",
                    new MySqlParameter("@id", deptId));
                if (dtChild.Rows.Count > 0 && Convert.ToInt32(dtChild.Rows[0][0]) > 0)
                {
                    MessageBox.Show("하위 부서가 존재하여 삭제할 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dtUser = db.Query("SELECT COUNT(*) FROM User WHERE DeptId = @id",
                    new MySqlParameter("@id", deptId));
                if (dtUser.Rows.Count > 0 && Convert.ToInt32(dtUser.Rows[0][0]) > 0)
                {
                    MessageBox.Show("소속 사용자가 존재하여 삭제할 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                db.NonQuery("DELETE FROM Department WHERE DeptId = @id", new MySqlParameter("@id", deptId));
                LoadDepartmentData("");
                MessageBox.Show("부서가 삭제되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"부서 삭제 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== C. 사용자 관리 ====================
        private void ShowUserManage()
        {
            var title = UIHelper.CreateTitle("사용자 관리");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            int cardWidth = 970;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX, 70, cardWidth, 60);
            var txtSearch = UIHelper.CreateTextBox(15, 18, 500, 25, "txtUserSearch", "이름 또는 ID 검색...");
            var cboDept = UIHelper.CreateComboBox(530, 18, 200, 25, "cboDeptFilter");
            LoadDepartmentComboForFilter(cboDept);

            var btnSearch = UIHelper.CreateBlueButton("검색", 750, 13, 80, 35);
            btnSearch.Click += (s, e) =>
            {
                string deptId = (cboDept.SelectedItem as ComboBoxItem)?.Value;
                LoadUserData(txtSearch.Text, deptId);
            };

            searchCard.Controls.Add(txtSearch);
            searchCard.Controls.Add(cboDept);
            searchCard.Controls.Add(btnSearch);
            pnlContent.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX, 145, cardWidth, 450, "dgvUsers");
            dgv.Columns.Add("UserId", "ID");
            dgv.Columns["UserId"].Visible = false;
            dgv.Columns.Add("Name", "이름");
            dgv.Columns["Name"].Width = 120;
            dgv.Columns.Add("LoginId", "로그인ID");
            dgv.Columns["LoginId"].Width = 150;
            dgv.Columns.Add("Nickname", "별명");
            dgv.Columns["Nickname"].Width = 120;
            dgv.Columns.Add("DeptName", "현재 부서");
            dgv.Columns.Add("DeptId", "DeptId");
            dgv.Columns["DeptId"].Visible = false;

            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "부서 변경",
                UseColumnTextForButtonValue = true,
                Width = 100,
                Name = "ChangeDept"
            });

            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "삭제",
                UseColumnTextForButtonValue = true,
                Width = 80,
                Name = "Delete"
            });

            dgv.CellClick += DgvUsers_CellClick;
            pnlContent.Controls.Add(dgv);

            LoadUserData("", null);
        }

        private void LoadDepartmentComboForFilter(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new ComboBoxItem { Text = "전체 부서", Value = "0" });

            try
            {
                var dt = db.Query("SELECT DeptId, DeptName FROM Department ORDER BY DeptName");
                foreach (DataRow row in dt.Rows)
                {
                    cbo.Items.Add(new ComboBoxItem
                    {
                        Text = row["DeptName"].ToString(),
                        Value = row["DeptId"].ToString()
                    });
                }

                cbo.DisplayMember = "Text";
                cbo.ValueMember = "Value";
                cbo.SelectedIndex = 0;
            }
            catch { }
        }

        private void LoadUserData(string searchKeyword, string deptId)
        {
            var dgv = pnlContent.Controls.Find("dgvUsers", true).FirstOrDefault() as DataGridView;
            if (dgv == null) return;

            dgv.Rows.Clear();

            string query = @"
                SELECT u.UserId, u.Name, u.LoginId, u.Nickname, u.DeptId,
                       IFNULL(d.DeptName, '미배정') AS DeptName
                FROM User u
                LEFT JOIN Department d ON u.DeptId = d.DeptId
                WHERE u.Role = 'user' AND (u.Name LIKE @search OR u.LoginId LIKE @search)";

            var parameters = new List<MySqlParameter> { new MySqlParameter("@search", $"%{searchKeyword}%") };

            if (!string.IsNullOrEmpty(deptId) && deptId != "0")
            {
                query += " AND u.DeptId = @deptId";
                parameters.Add(new MySqlParameter("@deptId", Convert.ToInt32(deptId)));
            }

            query += " ORDER BY u.Name";

            try
            {
                var dt = db.Query(query, parameters.ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    dgv.Rows.Add(
                        row["UserId"],
                        row["Name"],
                        row["LoginId"],
                        row["Nickname"] == DBNull.Value ? "" : row["Nickname"],
                        row["DeptName"],
                        row["DeptId"] == DBNull.Value ? null : row["DeptId"]
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사용자 목록 로드 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dgv = sender as DataGridView;
            int userId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["UserId"].Value);
            string userName = dgv.Rows[e.RowIndex].Cells["Name"].Value.ToString();

            if (dgv.Columns[e.ColumnIndex].Name == "ChangeDept")
            {
                ShowUserDepartmentChangePanel(userId, userName);
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (MessageBox.Show($"사용자 '{userName}'을(를) 삭제하시겠습니까?", "삭제 확인",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteUser(userId);
                }
            }
        }

        private void ShowUserDepartmentChangePanel(int userId, string userName)
        {
            pnlContent.Visible = false;

            var editPanel = new Panel
            {
                Name = "pnlUserDeptChange",
                Location = pnlContent.Location,
                Size = pnlContent.Size,
                BackColor = pnlContent.BackColor,
                Padding = new Padding(15),
                AutoScroll = true
            };
            this.Controls.Add(editPanel);
            editPanel.BringToFront();

            var title = UIHelper.CreateTitle($"사용자 부서 변경 - {userName}");
            title.Location = new Point(15, 15);
            editPanel.Controls.Add(title);

            int cardWidth = 500, cardHeight = 280;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);
            int cardY = UIHelper.CalculateCenterY(pnlContent.Height, cardHeight) - 30;

            var inputCard = UIHelper.CreateCard(cardX, cardY, cardWidth, cardHeight);

            inputCard.Controls.Add(UIHelper.CreateLabel("사용자", 30, 30, 10, Color.Black, true));
            inputCard.Controls.Add(UIHelper.CreateLabel(userName, 30, 55, 11, UIHelper.Colors.Primary));

            inputCard.Controls.Add(UIHelper.CreateLabel("변경할 부서", 30, 100, 10, Color.Black, true));
            var cboDept = UIHelper.CreateComboBox(30, 125, 440, 30, "cboNewDept");
            LoadAllDepartmentCombo(cboDept);
            inputCard.Controls.Add(cboDept);

            var btnSave = UIHelper.CreateBlueButton("변경", 30, 190, 210, 40);
            btnSave.Click += (s, e) => SaveUserDepartmentChange(userId, cboDept, editPanel);
            inputCard.Controls.Add(btnSave);

            var btnCancel = UIHelper.CreateBlueButton("취소", 250, 190, 220, 40);
            btnCancel.BackColor = Color.Gray;
            btnCancel.Click += (s, e) => CloseUserDepartmentChangePanel(editPanel);
            inputCard.Controls.Add(btnCancel);

            editPanel.Controls.Add(inputCard);

            LoadCurrentUserDepartment(userId, cboDept);
        }

        private void LoadAllDepartmentCombo(ComboBox cbo)
        {
            cbo.Items.Clear();

            try
            {
                var dt = db.Query("SELECT DeptId, DeptName FROM Department ORDER BY DeptName");
                foreach (DataRow row in dt.Rows)
                {
                    cbo.Items.Add(new ComboBoxItem
                    {
                        Text = row["DeptName"].ToString(),
                        Value = row["DeptId"].ToString()
                    });
                }

                cbo.DisplayMember = "Text";
                cbo.ValueMember = "Value";
                if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
            }
            catch { }
        }

        private void LoadCurrentUserDepartment(int userId, ComboBox cboDept)
        {
            try
            {
                var dt = db.Query("SELECT DeptId FROM User WHERE UserId = @id",
                    new MySqlParameter("@id", userId));
                if (dt.Rows.Count > 0 && dt.Rows[0]["DeptId"] != DBNull.Value)
                {
                    string deptId = dt.Rows[0]["DeptId"].ToString();
                    for (int i = 0; i < cboDept.Items.Count; i++)
                    {
                        if (cboDept.Items[i] is ComboBoxItem item && item.Value == deptId)
                        {
                            cboDept.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch { }
        }

        private void SaveUserDepartmentChange(int userId, ComboBox cboDept, Panel editPanel)
        {
            if (!(cboDept.SelectedItem is ComboBoxItem item) || string.IsNullOrEmpty(item.Value))
            {
                MessageBox.Show("올바른 부서를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                db.NonQuery("UPDATE User SET DeptId = @deptId WHERE UserId = @userId",
                    new MySqlParameter("@deptId", Convert.ToInt32(item.Value)),
                    new MySqlParameter("@userId", userId));

                MessageBox.Show("부서가 변경되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CloseUserDepartmentChangePanel(editPanel);
                ShowUserManage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"부서 변경 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseUserDepartmentChangePanel(Panel editPanel)
        {
            this.Controls.Remove(editPanel);
            editPanel.Dispose();
            pnlContent.Visible = true;
        }

        private void DeleteUser(int userId)
        {
            try
            {
                // 관련 데이터 삭제 (외래키 제약조건 순서 고려)
                db.NonQuery("DELETE FROM UserVisibleUser WHERE OwnerUserId = @id OR VisibleUserId = @id",
                    new MySqlParameter("@id", userId));
                db.NonQuery("DELETE FROM UserVisibleDept WHERE OwnerUserId = @id",
                    new MySqlParameter("@id", userId));
                db.NonQuery("DELETE FROM ChatPermission WHERE UserAId = @id OR UserBId = @id",
                    new MySqlParameter("@id", userId));
                db.NonQuery("DELETE FROM RecentChat WHERE UserId = @id OR PartnerUserId = @id",
                    new MySqlParameter("@id", userId));
                db.NonQuery("DELETE FROM ChatMessage WHERE FromUserId = @id OR ToUserId = @id",
                    new MySqlParameter("@id", userId));
                db.NonQuery("DELETE FROM UserLog WHERE UserId = @id",
                    new MySqlParameter("@id", userId));
                db.NonQuery("DELETE FROM UserProfileMap WHERE OwnerUserId = @id OR TargetUserId = @id",
                    new MySqlParameter("@id", userId));

                // Profile 테이블에서 해당 사용자의 프로필 삭제
                db.NonQuery("DELETE FROM Profile WHERE UserId = @id",
                    new MySqlParameter("@id", userId));

                // 사용자 삭제
                db.NonQuery("DELETE FROM User WHERE UserId = @id",
                    new MySqlParameter("@id", userId));

                LoadUserData("", null);
                MessageBox.Show("사용자가 삭제되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사용자 삭제 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== D. 대화내용 검색 ====================
        private void ShowChatSearch()
        {
            var title = UIHelper.CreateTitle("대화내용 검색");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            int cardWidth = 970;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX, 70, cardWidth, 70);

            searchCard.Controls.Add(UIHelper.CreateLabel("발신자", 20, 15, 9, Color.Black, true));
            var cboSender = UIHelper.CreateComboBox(80, 12, 180, 25, "cboSender");
            LoadUserComboForSearch(cboSender);
            searchCard.Controls.Add(cboSender);

            searchCard.Controls.Add(UIHelper.CreateLabel("수신자", 280, 15, 9, Color.Black, true));
            var cboReceiver = UIHelper.CreateComboBox(340, 12, 180, 25, "cboReceiver");
            LoadUserComboForSearch(cboReceiver);
            searchCard.Controls.Add(cboReceiver);

            searchCard.Controls.Add(UIHelper.CreateLabel("내용", 540, 15, 9, Color.Black, true));
            var txtContent = UIHelper.CreateTextBox(590, 12, 250, 25, "txtContent", "메시지 내용 검색...");
            searchCard.Controls.Add(txtContent);

            var btnSearch = UIHelper.CreateBlueButton("검색", 860, 10, 90, 30);
            btnSearch.Click += BtnSearchChat_Click;
            searchCard.Controls.Add(btnSearch);

            pnlContent.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX, 155, cardWidth, 490, "dgvChat");
            dgv.Columns.Add("MessageId", "ID");
            dgv.Columns["MessageId"].Visible = false;
            dgv.Columns.Add("FromUser", "발신자");
            dgv.Columns["FromUser"].Width = 120;
            dgv.Columns.Add("ToUser", "수신자");
            dgv.Columns["ToUser"].Width = 120;
            dgv.Columns.Add("Content", "메시지");
            dgv.Columns["Content"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.Columns.Add("SentAt", "전송시간");
            dgv.Columns["SentAt"].Width = 150;
            dgv.Columns.Add("IsRead", "읽음");
            dgv.Columns["IsRead"].Width = 60;

            pnlContent.Controls.Add(dgv);
        }

        private void LoadUserComboForSearch(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new ComboBoxItem { Text = "전체", Value = "0" });

            try
            {
                var dt = db.Query("SELECT UserId, Name FROM User WHERE Role = 'user' ORDER BY Name");
                foreach (DataRow row in dt.Rows)
                {
                    cbo.Items.Add(new ComboBoxItem
                    {
                        Text = row["Name"].ToString(),
                        Value = row["UserId"].ToString()
                    });
                }

                cbo.DisplayMember = "Text";
                cbo.ValueMember = "Value";
                cbo.SelectedIndex = 0;
            }
            catch { }
        }

        private void BtnSearchChat_Click(object sender, EventArgs e)
        {
            var cboSender = pnlContent.Controls.Find("cboSender", true).FirstOrDefault() as ComboBox;
            var cboReceiver = pnlContent.Controls.Find("cboReceiver", true).FirstOrDefault() as ComboBox;
            var txtContent = pnlContent.Controls.Find("txtContent", true).FirstOrDefault() as TextBox;

            string senderId = (cboSender?.SelectedItem as ComboBoxItem)?.Value;
            string receiverId = (cboReceiver?.SelectedItem as ComboBoxItem)?.Value;
            string content = txtContent?.Text ?? "";

            LoadChatData(senderId, receiverId, content);
        }

        private void LoadChatData(string senderId, string receiverId, string content)
        {
            var dgv = pnlContent.Controls.Find("dgvChat", true).FirstOrDefault() as DataGridView;
            if (dgv == null) return;

            dgv.Rows.Clear();

            string query = @"
                SELECT cm.MessageId, cm.Content, cm.SentAt, cm.IsRead,
                       us.Name AS FromUser, ur.Name AS ToUser
                FROM ChatMessage cm
                INNER JOIN User us ON cm.FromUserId = us.UserId
                INNER JOIN User ur ON cm.ToUserId = ur.UserId
                WHERE 1=1";

            var parameters = new List<MySqlParameter>();

            if (!string.IsNullOrEmpty(senderId) && senderId != "0")
            {
                query += " AND cm.FromUserId = @senderId";
                parameters.Add(new MySqlParameter("@senderId", Convert.ToInt32(senderId)));
            }

            if (!string.IsNullOrEmpty(receiverId) && receiverId != "0")
            {
                query += " AND cm.ToUserId = @receiverId";
                parameters.Add(new MySqlParameter("@receiverId", Convert.ToInt32(receiverId)));
            }

            if (!string.IsNullOrWhiteSpace(content) && content != "메시지 내용 검색...")
            {
                query += " AND cm.Content LIKE @content";
                parameters.Add(new MySqlParameter("@content", $"%{content}%"));
            }

            query += " ORDER BY cm.SentAt DESC LIMIT 500";

            try
            {
                var dt = db.Query(query, parameters.ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    string isRead = Convert.ToBoolean(row["IsRead"]) ? "읽음" : "안읽음";
                    dgv.Rows.Add(
                        row["MessageId"],
                        row["FromUser"],
                        row["ToUser"],
                        row["Content"],
                        Convert.ToDateTime(row["SentAt"]).ToString("yyyy-MM-dd HH:mm:ss"),
                        isRead
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"대화 내역 로드 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== E. 로그인/로그아웃 기록 ====================
        private void ShowLoginLog()
        {
            var title = UIHelper.CreateTitle("로그인/로그아웃 기록");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            int cardWidth = 970;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX, 60, cardWidth, 70);

            searchCard.Controls.Add(UIHelper.CreateLabel("사용자", 20, 15, 9, Color.Black, true));
            var cboUser = UIHelper.CreateComboBox(80, 12, 220, 25, "cboLogUser");
            LoadUserComboForSearch(cboUser);
            searchCard.Controls.Add(cboUser);

            searchCard.Controls.Add(UIHelper.CreateLabel("시작일", 320, 15, 9, Color.Black, true));
            var dtpStart = UIHelper.CreateDateTimePicker(375, 12, 180, 25, "dtpLogStart");
            dtpStart.Value = DateTime.Now.AddMonths(-1);
            searchCard.Controls.Add(dtpStart);

            searchCard.Controls.Add(UIHelper.CreateLabel("종료일", 575, 15, 9, Color.Black, true));
            var dtpEnd = UIHelper.CreateDateTimePicker(630, 12, 180, 25, "dtpLogEnd");
            dtpEnd.Value = DateTime.Now;
            searchCard.Controls.Add(dtpEnd);

            var btnSearch = UIHelper.CreateBlueButton("검색", 830, 10, 120, 30);
            btnSearch.Click += BtnSearchLog_Click;
            searchCard.Controls.Add(btnSearch);
            pnlContent.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX, 145, cardWidth, 500, "dgvLoginLog");
            dgv.Columns.Add("UserName", "사용자");
            dgv.Columns["UserName"].Width = 200;
            dgv.Columns.Add("ActionType", "활동");
            dgv.Columns["ActionType"].Width = 120;
            dgv.Columns.Add("CreatedAt", "시간");
            dgv.Columns["CreatedAt"].Width = 200;
            pnlContent.Controls.Add(dgv);

            LoadLoginLogData(null, DateTime.Now.AddMonths(-1), DateTime.Now);
        }

        private void BtnSearchLog_Click(object sender, EventArgs e)
        {
            var cboUser = pnlContent.Controls.Find("cboLogUser", true).FirstOrDefault() as ComboBox;
            var dtpStart = pnlContent.Controls.Find("dtpLogStart", true).FirstOrDefault() as DateTimePicker;
            var dtpEnd = pnlContent.Controls.Find("dtpLogEnd", true).FirstOrDefault() as DateTimePicker;

            string userId = (cboUser?.SelectedItem as ComboBoxItem)?.Value;
            DateTime startDate = dtpStart?.Value ?? DateTime.Now.AddMonths(-1);
            DateTime endDate = dtpEnd?.Value ?? DateTime.Now;

            LoadLoginLogData(userId, startDate, endDate);
        }

        private void LoadLoginLogData(string userId, DateTime startDate, DateTime endDate)
        {
            var dgv = pnlContent.Controls.Find("dgvLoginLog", true).FirstOrDefault() as DataGridView;
            if (dgv == null) return;

            dgv.Rows.Clear();

            string query = @"
                SELECT u.Name AS UserName, ul.ActionType, ul.CreatedAt
                FROM UserLog ul
                INNER JOIN User u ON ul.UserId = u.UserId
                WHERE DATE(ul.CreatedAt) BETWEEN @start AND @end";

            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@start", startDate.Date),
                new MySqlParameter("@end", endDate.Date.AddDays(1).AddSeconds(-1))
            };

            if (!string.IsNullOrEmpty(userId) && userId != "0")
            {
                query += " AND ul.UserId = @userId";
                parameters.Add(new MySqlParameter("@userId", Convert.ToInt32(userId)));
            }

            query += " ORDER BY ul.CreatedAt DESC LIMIT 500";

            try
            {
                var dt = db.Query(query, parameters.ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    dgv.Rows.Add(
                        row["UserName"],
                        row["ActionType"],
                        Convert.ToDateTime(row["CreatedAt"]).ToString("yyyy-MM-dd HH:mm:ss")
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"로그인 기록 로드 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== F. 권한 관리 ====================
        private void ShowPermissionManage()
        {
            var title = UIHelper.CreateTitle("권한 관리");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            // 탭 버튼
            var btnTab1 = UIHelper.CreateBlueButton("부서별 보기 권한", 15, 60, 200, 40);
            var btnTab2 = UIHelper.CreateBlueButton("사용자별 보기 권한", 225, 60, 200, 40);
            var btnTab3 = UIHelper.CreateBlueButton("대화 차단 관리", 435, 60, 200, 40);

            btnTab1.Click += (s, e) =>
            {
                ShowPermissionTab1();
                btnTab1.BackColor = UIHelper.Colors.Primary;
                btnTab2.BackColor = Color.Gray;
                btnTab3.BackColor = Color.Gray;
            };
            btnTab2.Click += (s, e) =>
            {
                ShowPermissionTab2();
                btnTab1.BackColor = Color.Gray;
                btnTab2.BackColor = UIHelper.Colors.Primary;
                btnTab3.BackColor = Color.Gray;
            };
            btnTab3.Click += (s, e) =>
            {
                ShowPermissionTab3();
                btnTab1.BackColor = Color.Gray;
                btnTab2.BackColor = Color.Gray;
                btnTab3.BackColor = UIHelper.Colors.Primary;
            };

            pnlContent.Controls.Add(btnTab1);
            pnlContent.Controls.Add(btnTab2);
            pnlContent.Controls.Add(btnTab3);

            // 기본 탭1 표시
            ShowPermissionTab1();
            btnTab2.BackColor = Color.Gray;
            btnTab3.BackColor = Color.Gray;
        }

        // Tab1: 부서별 보기 권한 (UserVisibleDept)
        private void ShowPermissionTab1()
        {
            var existing = pnlContent.Controls.Find("pnlPermTab", true).FirstOrDefault();
            if (existing != null) pnlContent.Controls.Remove(existing);

            var tabPanel = new Panel
            {
                Name = "pnlPermTab",
                Location = new Point(15, 120),
                Size = new Size(pnlContent.Width - 30, pnlContent.Height - 140),
                BackColor = Color.Transparent
            };

            int cardWidth = 970;
            int cardX = UIHelper.CalculateCenterX(tabPanel.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX - 15, 10, cardWidth, 60);
            searchCard.Controls.Add(UIHelper.CreateLabel("사용자", 20, 18, 9, Color.Black, true));
            var cboUser = UIHelper.CreateComboBox(80, 15, 200, 25, "cboPermUser");
            LoadUserComboForSearch(cboUser);
            searchCard.Controls.Add(cboUser);

            var btnSearch = UIHelper.CreateBlueButton("조회", 300, 13, 80, 35);
            btnSearch.Click += (s, e) => LoadDeptPermissionData((cboUser.SelectedItem as ComboBoxItem)?.Value);
            searchCard.Controls.Add(btnSearch);

            var btnAdd = UIHelper.CreateBlueButton("+ 권한 추가", 850, 13, 100, 35);
            btnAdd.Click += (s, e) => ShowAddDeptPermission();
            searchCard.Controls.Add(btnAdd);

            tabPanel.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX - 15, 85, cardWidth, 450, "dgvDeptPerm");
            dgv.Columns.Add("PermissionId", "ID");
            dgv.Columns["PermissionId"].Visible = false;
            dgv.Columns.Add("UserName", "사용자");
            dgv.Columns.Add("DeptName", "볼 수 있는 부서");
            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "삭제",
                UseColumnTextForButtonValue = true,
                Width = 100,
                Name = "Delete"
            });

            dgv.CellClick += DgvDeptPerm_CellClick;
            tabPanel.Controls.Add(dgv);

            pnlContent.Controls.Add(tabPanel);
            LoadDeptPermissionData(null);
        }

        private void LoadDeptPermissionData(string userId)
        {
            var dgv = pnlContent.Controls.Find("dgvDeptPerm", true).FirstOrDefault() as DataGridView;
            if (dgv == null) return;

            dgv.Rows.Clear();

            string query = @"
                SELECT uvd.OwnerUserId AS PermissionId, u.Name AS UserName, d.DeptName
                FROM UserVisibleDept uvd
                INNER JOIN User u ON uvd.OwnerUserId = u.UserId
                INNER JOIN Department d ON uvd.DeptId = d.DeptId
                WHERE 1=1";

            var parameters = new List<MySqlParameter>();

            if (!string.IsNullOrEmpty(userId) && userId != "0")
            {
                query += " AND uvd.OwnerUserId = @userId";
                parameters.Add(new MySqlParameter("@userId", Convert.ToInt32(userId)));
            }

            query += " ORDER BY u.Name, d.DeptName";

            try
            {
                var dt = db.Query(query, parameters.ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    dgv.Rows.Add(row["PermissionId"], row["UserName"], row["DeptName"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"부서 권한 로드 실패: {ex.Message}", "오류");
            }
        }

        private void DgvDeptPerm_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dgv = sender as DataGridView;
            if (dgv.Columns[e.ColumnIndex].Name == "Delete")
            {
                // 실제로는 OwnerUserId + DeptId 조합으로 삭제해야 하므로 수정 필요
                MessageBox.Show("부서 권한 삭제 기능은 별도 구현이 필요합니다.");
            }
        }

        private void ShowAddDeptPermission()
        {
            MessageBox.Show("부서 권한 추가 UI를 구현하세요.", "알림");
        }

        // Tab2: 사용자별 보기 권한 (UserVisibleUser)
        private void ShowPermissionTab2()
        {
            var existing = pnlContent.Controls.Find("pnlPermTab", true).FirstOrDefault();
            if (existing != null) pnlContent.Controls.Remove(existing);

            var tabPanel = new Panel
            {
                Name = "pnlPermTab",
                Location = new Point(15, 120),
                Size = new Size(pnlContent.Width - 30, pnlContent.Height - 140),
                BackColor = Color.Transparent
            };

            int cardWidth = 970;
            int cardX = UIHelper.CalculateCenterX(tabPanel.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX - 15, 10, cardWidth, 60);
            searchCard.Controls.Add(UIHelper.CreateLabel("사용자", 20, 18, 9, Color.Black, true));
            var cboUser = UIHelper.CreateComboBox(80, 15, 200, 25, "cboPermUser2");
            LoadUserComboForSearch(cboUser);
            searchCard.Controls.Add(cboUser);

            var btnSearch = UIHelper.CreateBlueButton("조회", 300, 13, 80, 35);
            btnSearch.Click += (s, e) => LoadUserPermissionData((cboUser.SelectedItem as ComboBoxItem)?.Value);
            searchCard.Controls.Add(btnSearch);

            var btnAdd = UIHelper.CreateBlueButton("+ 권한 추가", 850, 13, 100, 35);
            btnAdd.Click += (s, e) => ShowAddUserPermission();
            searchCard.Controls.Add(btnAdd);

            tabPanel.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX - 15, 85, cardWidth, 450, "dgvUserPerm");
            dgv.Columns.Add("OwnerUserId", "OwnerUserId");
            dgv.Columns["OwnerUserId"].Visible = false;
            dgv.Columns.Add("VisibleUserId", "VisibleUserId");
            dgv.Columns["VisibleUserId"].Visible = false;
            dgv.Columns.Add("OwnerName", "사용자");
            dgv.Columns.Add("VisibleName", "볼 수 있는 사용자");
            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "삭제",
                UseColumnTextForButtonValue = true,
                Width = 100,
                Name = "Delete"
            });

            dgv.CellClick += DgvUserPerm_CellClick;
            tabPanel.Controls.Add(dgv);

            pnlContent.Controls.Add(tabPanel);
            LoadUserPermissionData(null);
        }

        private void LoadUserPermissionData(string userId)
        {
            var dgv = pnlContent.Controls.Find("dgvUserPerm", true).FirstOrDefault() as DataGridView;
            if (dgv == null) return;

            dgv.Rows.Clear();

            string query = @"
                SELECT uvu.OwnerUserId, uvu.VisibleUserId,
                       u1.Name AS OwnerName, u2.Name AS VisibleName
                FROM UserVisibleUser uvu
                INNER JOIN User u1 ON uvu.OwnerUserId = u1.UserId
                INNER JOIN User u2 ON uvu.VisibleUserId = u2.UserId
                WHERE 1=1";

            var parameters = new List<MySqlParameter>();

            if (!string.IsNullOrEmpty(userId) && userId != "0")
            {
                query += " AND uvu.OwnerUserId = @userId";
                parameters.Add(new MySqlParameter("@userId", Convert.ToInt32(userId)));
            }

            query += " ORDER BY u1.Name, u2.Name";

            try
            {
                var dt = db.Query(query, parameters.ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    dgv.Rows.Add(
                        row["OwnerUserId"],
                        row["VisibleUserId"],
                        row["OwnerName"],
                        row["VisibleName"]
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사용자 권한 로드 실패: {ex.Message}", "오류");
            }
        }

        private void DgvUserPerm_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dgv = sender as DataGridView;
            if (dgv.Columns[e.ColumnIndex].Name == "Delete")
            {
                int ownerId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["OwnerUserId"].Value);
                int visibleId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["VisibleUserId"].Value);

                try
                {
                    db.NonQuery("DELETE FROM UserVisibleUser WHERE OwnerUserId = @oid AND VisibleUserId = @vid",
                        new MySqlParameter("@oid", ownerId),
                        new MySqlParameter("@vid", visibleId));

                    LoadUserPermissionData(null);
                    MessageBox.Show("권한이 삭제되었습니다.", "성공");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"삭제 실패: {ex.Message}", "오류");
                }
            }
        }

        private void ShowAddUserPermission()
        {
            MessageBox.Show("사용자 권한 추가 UI를 구현하세요.", "알림");
        }

        // Tab3: 대화 차단 관리 (ChatPermission)
        private void ShowPermissionTab3()
        {
            var existing = pnlContent.Controls.Find("pnlPermTab", true).FirstOrDefault();
            if (existing != null) pnlContent.Controls.Remove(existing);

            var tabPanel = new Panel
            {
                Name = "pnlPermTab",
                Location = new Point(15, 120),
                Size = new Size(pnlContent.Width - 30, pnlContent.Height - 140),
                BackColor = Color.Transparent
            };

            int cardWidth = 970;
            int cardX = UIHelper.CalculateCenterX(tabPanel.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX - 15, 10, cardWidth, 60);
            var btnRefresh = UIHelper.CreateBlueButton("새로고침", 20, 13, 100, 35);
            btnRefresh.Click += (s, e) => LoadChatPermissionData();
            searchCard.Controls.Add(btnRefresh);

            var btnAdd = UIHelper.CreateBlueButton("+ 차단 추가", 850, 13, 100, 35);
            btnAdd.Click += (s, e) => ShowAddChatBlock();
            searchCard.Controls.Add(btnAdd);

            tabPanel.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX - 15, 85, cardWidth, 450, "dgvChatPerm");
            dgv.Columns.Add("UserAId", "UserAId");
            dgv.Columns["UserAId"].Visible = false;
            dgv.Columns.Add("UserBId", "UserBId");
            dgv.Columns["UserBId"].Visible = false;
            dgv.Columns.Add("UserAName", "사용자 A");
            dgv.Columns.Add("UserBName", "사용자 B");
            dgv.Columns.Add("IsBlocked", "상태");
            dgv.Columns["IsBlocked"].Width = 100;
            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "해제",
                UseColumnTextForButtonValue = true,
                Width = 100,
                Name = "Unblock"
            });

            dgv.CellClick += DgvChatPerm_CellClick;
            tabPanel.Controls.Add(dgv);

            pnlContent.Controls.Add(tabPanel);
            LoadChatPermissionData();
        }

        private void LoadChatPermissionData()
        {
            var dgv = pnlContent.Controls.Find("dgvChatPerm", true).FirstOrDefault() as DataGridView;
            if (dgv == null) return;

            dgv.Rows.Clear();

            string query = @"
                SELECT cp.UserAId, cp.UserBId, cp.IsBlocked,
                       u1.Name AS UserAName, u2.Name AS UserBName
                FROM ChatPermission cp
                INNER JOIN User u1 ON cp.UserAId = u1.UserId
                INNER JOIN User u2 ON cp.UserBId = u2.UserId
                WHERE cp.IsBlocked = 1
                ORDER BY u1.Name, u2.Name";

            try
            {
                var dt = db.Query(query);
                foreach (DataRow row in dt.Rows)
                {
                    string status = Convert.ToBoolean(row["IsBlocked"]) ? "차단됨" : "허용";
                    dgv.Rows.Add(
                        row["UserAId"],
                        row["UserBId"],
                        row["UserAName"],
                        row["UserBName"],
                        status
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"대화 차단 목록 로드 실패: {ex.Message}", "오류");
            }
        }

        private void DgvChatPerm_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dgv = sender as DataGridView;
            if (dgv.Columns[e.ColumnIndex].Name == "Unblock")
            {
                int userAId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["UserAId"].Value);
                int userBId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["UserBId"].Value);

                try
                {
                    db.NonQuery("DELETE FROM ChatPermission WHERE UserAId = @aid AND UserBId = @bid",
                        new MySqlParameter("@aid", userAId),
                        new MySqlParameter("@bid", userBId));

                    LoadChatPermissionData();
                    MessageBox.Show("차단이 해제되었습니다.", "성공");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"차단 해제 실패: {ex.Message}", "오류");
                }
            }
        }

        private void ShowAddChatBlock()
        {
            MessageBox.Show("대화 차단 추가 UI를 구현하세요.", "알림");
        }
    }
}