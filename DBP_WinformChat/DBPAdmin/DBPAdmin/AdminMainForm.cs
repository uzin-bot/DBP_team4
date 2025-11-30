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
        private DBconnector db;

        public AdminMainForm()
        {
            InitializeComponent();

            db = DBconnector.GetInstance();

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
                var dt = db.Query("SELECT COUNT(*) FROM User WHERE IsAdmin = 0");
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
                var dt = db.Query(@"SELECT COUNT(DISTINCT UserId) FROM LoginLog 
                                   WHERE DATE(LoginTime) = CURDATE() AND Action = 'LOGIN'");
                return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
            }
            catch { return 0; }
        }

        // ==================== B. 부서관리 ====================
        private void ShowDepartmentManage()
        {
            var title = UIHelper.CreateTitle("부서 관리 (2단계 구조)");
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
            dgv.Columns.Add("Id", "ID");
            dgv.Columns["Id"].Visible = false;
            dgv.Columns.Add("Level", "레벨");
            dgv.Columns["Level"].Width = 60;
            dgv.Columns.Add("ParentName", "상위부서");
            dgv.Columns.Add("Name", "부서명");
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
                SELECT d.DeptId AS Id, d.DeptName AS Name, d.Level, d.ParentId,
                       IFNULL(p.DeptName, '-') AS ParentName,
                       (SELECT COUNT(*) FROM User WHERE DepartmentId = d.DeptId) AS UserCount
                FROM Department d
                LEFT JOIN Department p ON d.ParentId = p.DeptId
                WHERE d.DeptName LIKE @search
                ORDER BY d.Level, d.ParentId, d.DeptName";

            try
            {
                var dt = db.Query(query, new MySqlParameter("@search", $"%{searchKeyword}%"));
                foreach (DataRow row in dt.Rows)
                {
                    dgv.Rows.Add(row["Id"], row["Level"], row["ParentName"], row["Name"], row["UserCount"]);
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
            int deptId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);

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

            int cardWidth = 570, cardHeight = 380;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);
            int cardY = UIHelper.CalculateCenterY(pnlContent.Height, cardHeight) - 30;

            var inputCard = UIHelper.CreateCard(cardX, cardY, cardWidth, cardHeight);

            int yPos = 30;

            inputCard.Controls.Add(UIHelper.CreateLabel("부서명", 30, yPos, 10, Color.Black, true));
            var txtName = UIHelper.CreateTextBox(30, yPos + 25, 510, 30, "txtDeptName");
            inputCard.Controls.Add(txtName);
            yPos += 80;

            inputCard.Controls.Add(UIHelper.CreateLabel("부서 레벨", 30, yPos, 10, Color.Black, true));
            var cboLevel = UIHelper.CreateComboBox(30, yPos + 25, 510, 30, "cboDeptLevel");
            cboLevel.Items.Add(new ComboBoxItem { Text = "1단계 (최상위 부서)", Value = "1" });
            cboLevel.Items.Add(new ComboBoxItem { Text = "2단계 (하위 부서)", Value = "2" });
            cboLevel.DisplayMember = "Text";
            cboLevel.ValueMember = "Value";
            cboLevel.SelectedIndex = 0;
            cboLevel.SelectedIndexChanged += (s, e) =>
            {
                var cboParent = inputCard.Controls.Find("cboDeptParent", true).FirstOrDefault() as ComboBox;
                if (cboParent != null && cboLevel.SelectedItem is ComboBoxItem item)
                {
                    cboParent.Enabled = (item.Value == "2");
                }
            };
            inputCard.Controls.Add(cboLevel);
            yPos += 80;

            inputCard.Controls.Add(UIHelper.CreateLabel("상위 부서 (2단계 선택 시)", 30, yPos, 10, Color.Black, true));
            var cboParent = UIHelper.CreateComboBox(30, yPos + 25, 510, 30, "cboDeptParent");
            cboParent.Enabled = false;
            LoadParentDepartmentCombo(cboParent);
            inputCard.Controls.Add(cboParent);
            yPos += 80;

            var btnSave = UIHelper.CreateBlueButton("저장", 30, yPos, 250, 40);
            btnSave.Click += (s, e) => SaveDepartment(deptId, editPanel);
            inputCard.Controls.Add(btnSave);

            var btnCancel = UIHelper.CreateBlueButton("취소", 290, yPos, 250, 40);
            btnCancel.BackColor = Color.Gray;
            btnCancel.Click += (s, e) => CloseDepartmentEditPanel(editPanel);
            inputCard.Controls.Add(btnCancel);

            editPanel.Controls.Add(inputCard);

            if (deptId.HasValue)
            {
                LoadDepartmentForEdit(deptId.Value, txtName, cboLevel, cboParent);
            }
        }

        private void LoadParentDepartmentCombo(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new ComboBoxItem { Text = "선택 안함", Value = "" });

            try
            {
                var dt = db.Query("SELECT DeptId, DeptName FROM Department WHERE Level = 1 ORDER BY DeptName");
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

        private void LoadDepartmentForEdit(int deptId, TextBox txtName, ComboBox cboLevel, ComboBox cboParent)
        {
            try
            {
                var dt = db.Query("SELECT DeptName, Level, ParentId FROM Department WHERE DeptId = @id",
                    new MySqlParameter("@id", deptId));

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    txtName.Text = row["DeptName"].ToString();

                    int level = Convert.ToInt32(row["Level"]);
                    cboLevel.SelectedIndex = level - 1;

                    if (level == 2 && row["ParentId"] != DBNull.Value)
                    {
                        string parentId = row["ParentId"].ToString();
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
            var cboLevel = editPanel.Controls.Find("cboDeptLevel", true).FirstOrDefault() as ComboBox;
            var cboParent = editPanel.Controls.Find("cboDeptParent", true).FirstOrDefault() as ComboBox;

            if (string.IsNullOrWhiteSpace(txtName?.Text))
            {
                MessageBox.Show("부서명을 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(cboLevel.SelectedItem is ComboBoxItem levelItem))
            {
                MessageBox.Show("부서 레벨을 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int level = Convert.ToInt32(levelItem.Value);
            int? parentId = null;

            if (level == 2)
            {
                if (!(cboParent.SelectedItem is ComboBoxItem parentItem) || string.IsNullOrEmpty(parentItem.Value))
                {
                    MessageBox.Show("2단계 부서는 상위 부서를 선택해야 합니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                parentId = Convert.ToInt32(parentItem.Value);
            }

            try
            {
                if (deptId.HasValue)
                {
                    db.NonQuery("UPDATE Department SET DeptName = @name, Level = @level, ParentId = @parentId WHERE DeptId = @id",
                        new MySqlParameter("@name", txtName.Text),
                        new MySqlParameter("@level", level),
                        new MySqlParameter("@parentId", parentId.HasValue ? (object)parentId.Value : DBNull.Value),
                        new MySqlParameter("@id", deptId.Value));
                    MessageBox.Show("부서가 수정되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    db.NonQuery("INSERT INTO Department (DeptName, Level, ParentId) VALUES (@name, @level, @parentId)",
                        new MySqlParameter("@name", txtName.Text),
                        new MySqlParameter("@level", level),
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
                var dtChild = db.Query("SELECT COUNT(*) FROM Department WHERE ParentId = @id", new MySqlParameter("@id", deptId));
                if (dtChild.Rows.Count > 0 && Convert.ToInt32(dtChild.Rows[0][0]) > 0)
                {
                    MessageBox.Show("하위 부서가 존재하여 삭제할 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dtUser = db.Query("SELECT COUNT(*) FROM User WHERE DepartmentId = @id", new MySqlParameter("@id", deptId));
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
            var txtSearch = UIHelper.CreateTextBox(15, 18, 500, 25, "txtUserSearch", "이름 또는 이메일 검색...");
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
            dgv.Columns.Add("Id", "ID");
            dgv.Columns["Id"].Visible = false;
            dgv.Columns.Add("Name", "이름");
            dgv.Columns["Name"].Width = 150;
            dgv.Columns.Add("Email", "이메일");
            dgv.Columns.Add("DeptName", "현재 부서");
            dgv.Columns.Add("DeptId", "DeptId");
            dgv.Columns["DeptId"].Visible = false;

            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "부서 변경",
                UseColumnTextForButtonValue = true,
                Width = 120,
                Name = "ChangeDept"
            });

            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "삭제",
                UseColumnTextForButtonValue = true,
                Width = 100,
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
                SELECT u.UserId AS Id, u.Name, u.LoginId AS Email, u.DepartmentId,
                       IFNULL(d.DeptName, '미배정') AS DeptName
                FROM User u
                LEFT JOIN Department d ON u.DepartmentId = d.DeptId
                WHERE u.IsAdmin = 0 AND (u.Name LIKE @search OR u.LoginId LIKE @search)";

            var parameters = new List<MySqlParameter> { new MySqlParameter("@search", $"%{searchKeyword}%") };

            if (!string.IsNullOrEmpty(deptId) && deptId != "0")
            {
                query += " AND u.DepartmentId = @deptId";
                parameters.Add(new MySqlParameter("@deptId", Convert.ToInt32(deptId)));
            }

            query += " ORDER BY u.Name";

            try
            {
                var dt = db.Query(query, parameters.ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    dgv.Rows.Add(row["Id"], row["Name"], row["Email"], row["DeptName"],
                        row["DepartmentId"] == DBNull.Value ? null : row["DepartmentId"]);
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
            int userId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
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
                var dt = db.Query("SELECT DeptId, DeptName, Level FROM Department ORDER BY Level, DeptName");
                foreach (DataRow row in dt.Rows)
                {
                    int level = Convert.ToInt32(row["Level"]);
                    string indent = new string(' ', (level - 1) * 2);
                    cbo.Items.Add(new ComboBoxItem
                    {
                        Text = indent + row["DeptName"].ToString(),
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
                var dt = db.Query("SELECT DepartmentId FROM User WHERE UserId = @id", new MySqlParameter("@id", userId));
                if (dt.Rows.Count > 0 && dt.Rows[0]["DepartmentId"] != DBNull.Value)
                {
                    string deptId = dt.Rows[0]["DepartmentId"].ToString();
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
                db.NonQuery("UPDATE User SET DepartmentId = @deptId WHERE UserId = @userId",
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
                db.NonQuery("DELETE FROM ViewPermission WHERE UserId = @id", new MySqlParameter("@id", userId));
                db.NonQuery("DELETE FROM ChatPermission WHERE UserAId = @id OR UserBId = @id", new MySqlParameter("@id", userId));
                db.NonQuery("DELETE FROM LoginLog WHERE UserId = @id", new MySqlParameter("@id", userId));
                db.NonQuery("DELETE FROM ChatMessage WHERE SenderId = @id OR ReceiverId = @id", new MySqlParameter("@id", userId));
                db.NonQuery("DELETE FROM User WHERE UserId = @id", new MySqlParameter("@id", userId));

                LoadUserData("", null);
                MessageBox.Show("사용자가 삭제되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사용자 삭제 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== D. 대화 내용 검색 ====================
        private void ShowChatSearch()
        {
            var title = UIHelper.CreateTitle("전체 대화 내용 검색");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            int cardWidth = 970;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX, 70, cardWidth, 220);

            searchCard.Controls.Add(UIHelper.CreateLabel("사용자 선택", 25, 25, 9, Color.Black, true));
            var cboUser = UIHelper.CreateComboBox(25, 50, 430, 25, "cboChatUser");
            LoadUserComboForSearch(cboUser);
            searchCard.Controls.Add(cboUser);

            searchCard.Controls.Add(UIHelper.CreateLabel("검색 키워드", 490, 25, 9, Color.Black, true));
            var txtKeyword = UIHelper.CreateTextBox(490, 50, 430, 25, "txtChatKeyword");
            searchCard.Controls.Add(txtKeyword);

            searchCard.Controls.Add(UIHelper.CreateLabel("시작일", 25, 100, 9, Color.Black, true));
            var dtpStart = UIHelper.CreateDateTimePicker(25, 125, 430, 25, "dtpChatStart");
            dtpStart.Value = DateTime.Now.AddMonths(-1);
            searchCard.Controls.Add(dtpStart);

            searchCard.Controls.Add(UIHelper.CreateLabel("종료일", 490, 100, 9, Color.Black, true));
            var dtpEnd = UIHelper.CreateDateTimePicker(490, 125, 430, 25, "dtpChatEnd");
            dtpEnd.Value = DateTime.Now;
            searchCard.Controls.Add(dtpEnd);

            var btnSearch = UIHelper.CreateBlueButton("검색", 25, 170, 920, 35);
            btnSearch.Click += BtnSearchChat_Click;
            searchCard.Controls.Add(btnSearch);
            pnlContent.Controls.Add(searchCard);

            var resultCard = UIHelper.CreateCard(cardX, 305, cardWidth, 290);
            resultCard.Name = "pnlChatResult";
            resultCard.AutoScroll = true;

            var lblResult = UIHelper.CreateLabel("검색 조건을 입력하고 검색 버튼을 눌러주세요", 0, 120, 11, UIHelper.Colors.TextSecondary);
            lblResult.Size = new Size(cardWidth, 30);
            lblResult.TextAlign = ContentAlignment.MiddleCenter;
            lblResult.Name = "lblChatResult";
            resultCard.Controls.Add(lblResult);
            pnlContent.Controls.Add(resultCard);
        }

        private void LoadUserComboForSearch(ComboBox cbo, bool includeAll = true)
        {
            cbo.Items.Clear();
            if (includeAll)
            {
                cbo.Items.Add(new ComboBoxItem { Text = "전체", Value = "0" });
            }

            try
            {
                var dt = db.Query("SELECT UserId, Name FROM User WHERE IsAdmin = 0 ORDER BY Name");
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
                if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
            }
            catch { }
        }

        private void BtnSearchChat_Click(object sender, EventArgs e)
        {
            var cboUser = pnlContent.Controls.Find("cboChatUser", true).FirstOrDefault() as ComboBox;
            var txtKeyword = pnlContent.Controls.Find("txtChatKeyword", true).FirstOrDefault() as TextBox;
            var dtpStart = pnlContent.Controls.Find("dtpChatStart", true).FirstOrDefault() as DateTimePicker;
            var dtpEnd = pnlContent.Controls.Find("dtpChatEnd", true).FirstOrDefault() as DateTimePicker;
            var resultPanel = pnlContent.Controls.Find("pnlChatResult", true).FirstOrDefault() as Panel;

            if (cboUser == null || txtKeyword == null || dtpStart == null || dtpEnd == null || resultPanel == null)
                return;

            resultPanel.Controls.Clear();

            string query = @"
                SELECT cm.MessageId AS Id, cm.Message, cm.SendTime,
                       us.Name AS SenderName, ur.Name AS ReceiverName
                FROM ChatMessage cm
                INNER JOIN User us ON cm.SenderId = us.UserId
                INNER JOIN User ur ON cm.ReceiverId = ur.UserId
                WHERE cm.SendTime BETWEEN @startDate AND @endDate";

            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@startDate", dtpStart.Value.Date),
                new MySqlParameter("@endDate", dtpEnd.Value.Date.AddDays(1).AddSeconds(-1))
            };

            if (cboUser.SelectedItem is ComboBoxItem userItem && userItem.Value != "0")
            {
                query += " AND (cm.SenderId = @userId OR cm.ReceiverId = @userId)";
                parameters.Add(new MySqlParameter("@userId", Convert.ToInt32(userItem.Value)));
            }

            if (!string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                query += " AND cm.Message LIKE @keyword";
                parameters.Add(new MySqlParameter("@keyword", $"%{txtKeyword.Text}%"));
            }

            query += " ORDER BY cm.SendTime DESC LIMIT 100";

            try
            {
                var dt = db.Query(query, parameters.ToArray());

                if (dt.Rows.Count == 0)
                {
                    var lblEmpty = UIHelper.CreateLabel("검색 결과가 없습니다.", 0, 120, 11, UIHelper.Colors.TextSecondary);
                    lblEmpty.Size = new Size(970, 30);
                    lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
                    resultPanel.Controls.Add(lblEmpty);
                    return;
                }

                int yPos = 10;
                foreach (DataRow row in dt.Rows)
                {
                    var msgPanel = UIHelper.CreateCard(10, yPos, 930, 80);
                    msgPanel.BackColor = UIHelper.Colors.AccentLight;

                    msgPanel.Controls.Add(UIHelper.CreateLabel($"{row["SenderName"]} → {row["ReceiverName"]}", 10, 10, 9, Color.Black, true));
                    msgPanel.Controls.Add(UIHelper.CreateLabel(Convert.ToDateTime(row["SendTime"]).ToString("yyyy-MM-dd HH:mm:ss"), 10, 30, 8, UIHelper.Colors.TextSecondary));

                    var lblMsg = UIHelper.CreateLabel(row["Message"].ToString(), 10, 50, 9, Color.Black);
                    lblMsg.Size = new Size(900, 20);
                    msgPanel.Controls.Add(lblMsg);

                    resultPanel.Controls.Add(msgPanel);
                    yPos += 90;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"대화 검색 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // ⭐ 검색 영역을 한 줄로 간결하게 구성
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

            // ⭐ DataGridView를 최대한 크게 배치
            var dgv = UIHelper.CreateDGV(cardX, 145, cardWidth, 500, "dgvLoginLog");
            dgv.Columns.Add("UserName", "사용자");
            dgv.Columns["UserName"].Width = 200;
            dgv.Columns.Add("Action", "활동");
            dgv.Columns["Action"].Width = 120;
            dgv.Columns.Add("LogTime", "시간");
            dgv.Columns["LogTime"].Width = 200;
            pnlContent.Controls.Add(dgv);

            LoadLoginLogData(null, DateTime.Now.AddMonths(-1), DateTime.Now);
        }

        private void BtnSearchLog_Click(object sender, EventArgs e)
        {
            var cboUser = pnlContent.Controls.Find("cboLogUser", true).FirstOrDefault() as ComboBox;
            var dtpStart = pnlContent.Controls.Find("dtpLogStart", true).FirstOrDefault() as DateTimePicker;
            var dtpEnd = pnlContent.Controls.Find("dtpLogEnd", true).FirstOrDefault() as DateTimePicker;

            if (cboUser == null || dtpStart == null || dtpEnd == null) return;

            string userId = null;
            if (cboUser.SelectedItem is ComboBoxItem item && item.Value != "0")
            {
                userId = item.Value;
            }

            LoadLoginLogData(userId, dtpStart.Value, dtpEnd.Value);
        }

        private void LoadLoginLogData(string userId, DateTime startDate, DateTime endDate)
        {
            var dgv = pnlContent.Controls.Find("dgvLoginLog", true).FirstOrDefault() as DataGridView;
            if (dgv == null) return;

            dgv.Rows.Clear();

            string query = @"
                SELECT u.Name AS UserName, ll.Action, ll.LoginTime AS LogTime
                FROM LoginLog ll
                INNER JOIN User u ON ll.UserId = u.UserId
                WHERE ll.LoginTime BETWEEN @startDate AND @endDate";

            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@startDate", startDate.Date),
                new MySqlParameter("@endDate", endDate.Date.AddDays(1).AddSeconds(-1))
            };

            if (!string.IsNullOrEmpty(userId))
            {
                query += " AND ll.UserId = @userId";
                parameters.Add(new MySqlParameter("@userId", userId));
            }

            query += " ORDER BY ll.LoginTime DESC LIMIT 1000";

            try
            {
                var dt = db.Query(query, parameters.ToArray());
                foreach (DataRow row in dt.Rows)
                {
                    string action = row["Action"].ToString() == "LOGIN" ? "로그인" : "로그아웃";
                    dgv.Rows.Add(row["UserName"], action, Convert.ToDateTime(row["LogTime"]).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"로그인 기록 로드 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== G. 직원보기 권한 + H. 대화 권한 ====================
        private void ShowPermissionManage()
        {
            var title = UIHelper.CreateTitle("권한 관리");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            int cardWidth = 475, cardHeight = 520, cardSpacing = 20;
            int totalWidth = (cardWidth * 2) + cardSpacing;
            int startX = UIHelper.CalculateCenterX(pnlContent.Width, totalWidth);
            int cardY = 70;

            // 직원 보기 권한 설정
            var card1 = UIHelper.CreateCard(startX, cardY, cardWidth, cardHeight);
            card1.Controls.Add(UIHelper.CreateLabel("직원 보기 권한 설정", 20, 20, 11, Color.Black, true));

            card1.Controls.Add(UIHelper.CreateLabel("사용자 선택", 20, 60, 9, Color.Black, true));
            var cboUser1 = UIHelper.CreateComboBox(20, 85, 425, 25, "cboViewPermUser");
            LoadUserComboForSearch(cboUser1, false);
            cboUser1.SelectedIndexChanged += CboViewPermUser_SelectedIndexChanged;
            card1.Controls.Add(cboUser1);

            var lblDesc = UIHelper.CreateLabel("보이는 부서 범위 (체크된 부서의 직원만 보임)", 20, 135, 9, Color.Black, true);
            lblDesc.Size = new Size(425, 20);
            card1.Controls.Add(lblDesc);

            var pnlDeptChecks = new Panel
            {
                Location = new Point(20, 165),
                Size = new Size(425, 290),
                Name = "pnlViewDeptChecks",
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            card1.Controls.Add(pnlDeptChecks);

            var btnSave1 = UIHelper.CreateBlueButton("저장", 20, 465, 425, 40);
            btnSave1.Click += BtnSaveViewPermission_Click;
            card1.Controls.Add(btnSave1);
            pnlContent.Controls.Add(card1);

            // 대화 허용/차단 관리
            var card2 = UIHelper.CreateCard(startX + cardWidth + cardSpacing, cardY, cardWidth, cardHeight);
            card2.Controls.Add(UIHelper.CreateLabel("대화 허용/차단 관리", 20, 20, 11, Color.Black, true));

            card2.Controls.Add(UIHelper.CreateLabel("사용자 A", 20, 60, 9, Color.Black, true));
            var cboUserA = UIHelper.CreateComboBox(20, 85, 425, 25, "cboChatPermUserA");
            LoadUserComboForSearch(cboUserA, false);
            card2.Controls.Add(cboUserA);

            card2.Controls.Add(UIHelper.CreateLabel("사용자 B", 20, 135, 9, Color.Black, true));
            var cboUserB = UIHelper.CreateComboBox(20, 160, 425, 25, "cboChatPermUserB");
            LoadUserComboForSearch(cboUserB, false);
            card2.Controls.Add(cboUserB);

            card2.Controls.Add(UIHelper.CreateLabel("대화 설정", 20, 210, 9, Color.Black, true));
            var cboSetting = UIHelper.CreateComboBox(20, 235, 425, 25, "cboChatSetting");
            cboSetting.Items.Add(new ComboBoxItem { Text = "허용", Value = "0" });
            cboSetting.Items.Add(new ComboBoxItem { Text = "차단", Value = "1" });
            cboSetting.DisplayMember = "Text";
            cboSetting.ValueMember = "Value";
            cboSetting.SelectedIndex = 0;
            card2.Controls.Add(cboSetting);

            card2.Controls.Add(UIHelper.CreateLabel("현재 차단 목록", 20, 280, 9, Color.Black, true));

            var lstBlocked = new ListBox
            {
                Location = new Point(20, 305),
                Size = new Size(425, 140),
                Name = "lstChatBlocked",
                Font = UIHelper.Fonts.Normal
            };
            card2.Controls.Add(lstBlocked);

            var btnSave2 = UIHelper.CreateBlueButton("적용", 20, 465, 425, 40);
            btnSave2.Click += BtnSaveChatPermission_Click;
            card2.Controls.Add(btnSave2);
            pnlContent.Controls.Add(card2);

            LoadDepartmentCheckBoxes(pnlDeptChecks);
            LoadChatBlockedList();
        }

        private void CboViewPermUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbo = sender as ComboBox;
            if (cbo?.SelectedItem is ComboBoxItem item && !string.IsNullOrEmpty(item.Value) && item.Value != "0")
            {
                LoadUserViewPermissions(Convert.ToInt32(item.Value));
            }
        }

        private void LoadDepartmentCheckBoxes(Panel pnl)
        {
            if (pnl.Controls.Count > 0) return;

            pnl.Controls.Clear();

            try
            {
                var dt = db.Query("SELECT DeptId, DeptName, Level, ParentId FROM Department ORDER BY Level, ParentId, DeptName");
                int yPos = 10;

                foreach (DataRow row in dt.Rows)
                {
                    int level = Convert.ToInt32(row["Level"]);
                    string indent = new string(' ', (level - 1) * 4);

                    var chk = new CheckBox
                    {
                        Text = indent + row["DeptName"].ToString(),
                        Location = new Point(10, yPos),
                        Size = new Size(380, 25),
                        Tag = row["DeptId"],
                        Font = UIHelper.Fonts.Normal
                    };
                    pnl.Controls.Add(chk);
                    yPos += 30;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"부서 목록 로드 실패: {ex.Message}", "오류");
            }
        }

        private void LoadUserViewPermissions(int userId)
        {
            var pnl = pnlContent.Controls.Find("pnlViewDeptChecks", true).FirstOrDefault() as Panel;
            if (pnl == null) return;

            if (pnl.Controls.Count == 0)
            {
                LoadDepartmentCheckBoxes(pnl);
            }

            foreach (Control ctrl in pnl.Controls)
            {
                if (ctrl is CheckBox chk) chk.Checked = false;
            }

            try
            {
                var dt = db.Query("SELECT CanViewDeptId FROM ViewPermission WHERE UserId = @userId",
                    new MySqlParameter("@userId", userId));

                foreach (DataRow row in dt.Rows)
                {
                    int deptId = Convert.ToInt32(row["CanViewDeptId"]);
                    foreach (Control ctrl in pnl.Controls)
                    {
                        if (ctrl is CheckBox chk && chk.Tag != null && Convert.ToInt32(chk.Tag) == deptId)
                        {
                            chk.Checked = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"권한 로드 실패: {ex.Message}", "오류");
            }
        }

        private void BtnSaveViewPermission_Click(object sender, EventArgs e)
        {
            var cboUser = pnlContent.Controls.Find("cboViewPermUser", true).FirstOrDefault() as ComboBox;
            var pnlDept = pnlContent.Controls.Find("pnlViewDeptChecks", true).FirstOrDefault() as Panel;

            if (cboUser?.SelectedItem is ComboBoxItem item && !string.IsNullOrEmpty(item.Value) && pnlDept != null)
            {
                int userId = Convert.ToInt32(item.Value);

                try
                {
                    db.NonQuery("DELETE FROM ViewPermission WHERE UserId = @userId", new MySqlParameter("@userId", userId));

                    foreach (Control ctrl in pnlDept.Controls)
                    {
                        if (ctrl is CheckBox chk && chk.Checked && chk.Tag != null)
                        {
                            int deptId = Convert.ToInt32(chk.Tag);
                            db.NonQuery("INSERT INTO ViewPermission (UserId, CanViewDeptId) VALUES (@userId, @deptId)",
                                new MySqlParameter("@userId", userId),
                                new MySqlParameter("@deptId", deptId));
                        }
                    }

                    MessageBox.Show("권한이 저장되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"권한 저장 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("사용자를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadChatBlockedList()
        {
            var lst = pnlContent.Controls.Find("lstChatBlocked", true).FirstOrDefault() as ListBox;
            if (lst == null) return;

            lst.Items.Clear();

            try
            {
                var dt = db.Query(@"
                    SELECT cp.PermissionId, ua.Name AS UserAName, ub.Name AS UserBName
                    FROM ChatPermission cp
                    INNER JOIN User ua ON cp.UserAId = ua.UserId
                    INNER JOIN User ub ON cp.UserBId = ub.UserId
                    WHERE cp.IsBlocked = 1");

                foreach (DataRow row in dt.Rows)
                {
                    lst.Items.Add($"{row["UserAName"]} ↔ {row["UserBName"]} (ID: {row["PermissionId"]})");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"차단 목록 로드 실패: {ex.Message}", "오류");
            }
        }

        private void BtnSaveChatPermission_Click(object sender, EventArgs e)
        {
            var cboUserA = pnlContent.Controls.Find("cboChatPermUserA", true).FirstOrDefault() as ComboBox;
            var cboUserB = pnlContent.Controls.Find("cboChatPermUserB", true).FirstOrDefault() as ComboBox;
            var cboSetting = pnlContent.Controls.Find("cboChatSetting", true).FirstOrDefault() as ComboBox;

            if (!(cboUserA?.SelectedItem is ComboBoxItem itemA) || string.IsNullOrEmpty(itemA.Value))
            {
                MessageBox.Show("사용자 A를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(cboUserB?.SelectedItem is ComboBoxItem itemB) || string.IsNullOrEmpty(itemB.Value))
            {
                MessageBox.Show("사용자 B를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (itemA.Value == itemB.Value)
            {
                MessageBox.Show("같은 사용자는 선택할 수 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userAId = Convert.ToInt32(itemA.Value);
            int userBId = Convert.ToInt32(itemB.Value);
            int isBlocked = cboSetting?.SelectedItem is ComboBoxItem settingItem ? Convert.ToInt32(settingItem.Value) : 0;

            try
            {
                db.NonQuery(@"
                    INSERT INTO ChatPermission (UserAId, UserBId, IsBlocked) 
                    VALUES (@userA, @userB, @blocked)
                    ON DUPLICATE KEY UPDATE IsBlocked = @blocked",
                    new MySqlParameter("@userA", userAId),
                    new MySqlParameter("@userB", userBId),
                    new MySqlParameter("@blocked", isBlocked));

                MessageBox.Show("대화 권한이 설정되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadChatBlockedList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"권한 설정 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // ==================== ComboBoxItem 클래스 ====================
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}