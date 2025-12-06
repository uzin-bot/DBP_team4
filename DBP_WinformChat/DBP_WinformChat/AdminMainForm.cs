using DBP_WinformChat;
using leehaeun;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DBPAdmin
{
    public partial class AdminMainForm : Form
    {
        private DBP_WinformChat.DBconnector db;

        public AdminMainForm()
        {
            InitializeComponent();

            db = DBP_WinformChat.DBconnector.GetInstance();

            if (!TestConnection())
            {
                MessageBox.Show("데이터베이스 연결에 실패했습니다.", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            AttachMenuEvents();
            ShowDashboard();
        }

        private bool TestConnection()
        {
            try
            {
                db.Query("SELECT 1");
                return true;
            }
            catch
            {
                return false;
            }
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
                var dt = db.Query("SELECT COUNT(DISTINCT UserId) FROM UserLog WHERE DATE(CreatedAt) = CURDATE() AND ActionType = 'LOGIN'");
                return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
            }
            catch { return 0; }
        }

        // ==================== B. 부서관리 (2단계 계층 구조) ====================
        private void ShowDepartmentManage()
        {
            var title = UIHelper.CreateTitle("부서 관리 (부서 > 팀 2단계 구조)");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            var btnAdd = UIHelper.CreateBlueButton("+ 부서/팀 추가", pnlContent.Width - 140, 15, 120, 40);
            btnAdd.Click += (s, e) => ShowDepartmentEditPanel(null);
            pnlContent.Controls.Add(btnAdd);

            int cardWidth = 1100;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX, 70, cardWidth, 60);
            var txtSearch = UIHelper.CreateTextBox(15, 18, 840, 25, "txtDeptSearch", "부서/팀명 검색...");
            var btnSearch = UIHelper.CreateBlueButton("검색", 870, 13, 80, 35);
            btnSearch.Click += (s, e) => LoadDepartmentData(txtSearch.Text);
            searchCard.Controls.Add(txtSearch);
            searchCard.Controls.Add(btnSearch);
            pnlContent.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX, 145, cardWidth, 450, "dgvDepartment");
            dgv.Columns.Add("DeptId", "ID");
            dgv.Columns["DeptId"].Visible = false;
            dgv.Columns.Add("Level", "레벨");
            dgv.Columns["Level"].Width = 60;
            dgv.Columns.Add("DeptName", "부서/팀명");
            dgv.Columns["DeptName"].Width = 250;
            dgv.Columns.Add("ParentName", "상위 부서");
            dgv.Columns["ParentName"].Width = 200;
            dgv.Columns.Add("UserCount", "인원수");
            dgv.Columns["UserCount"].Width = 80;
            dgv.Columns.Add("FullPath", "전체 경로");
            dgv.Columns["FullPath"].Width = 300;

            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "수정",
                UseColumnTextForButtonValue = true,
                Width = 70,
                Name = "Edit"
            });

            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "삭제",
                UseColumnTextForButtonValue = true,
                Width = 70,
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

            // 2단계 계층 구조 표시 (부서 → 팀)
            string sql = $@"
                SELECT 
                    d.DeptId, 
                    d.DeptName, 
                    d.ParentDeptId,
                    CASE 
                        WHEN d.ParentDeptId IS NULL THEN 1 
                        ELSE 2 
                    END AS Level,
                    IFNULL(p.DeptName, '-') AS ParentName,
                    (SELECT COUNT(*) FROM User WHERE DeptId = d.DeptId) AS UserCount,
                    CASE 
                        WHEN d.ParentDeptId IS NULL THEN d.DeptName
                        ELSE CONCAT(p.DeptName, ' > ', d.DeptName)
                    END AS FullPath
                FROM Department d
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE d.DeptName LIKE '%{searchKeyword}%'
                ORDER BY IFNULL(p.DeptId, d.DeptId), d.ParentDeptId IS NULL DESC, d.DeptName";

            try
            {
                var dt = db.Query(sql);
                foreach (DataRow row in dt.Rows)
                {
                    int level = Convert.ToInt32(row["Level"]);
                    string levelStr = level == 1 ? "부서" : "팀";

                    dgv.Rows.Add(
                        row["DeptId"],
                        levelStr,
                        row["DeptName"],
                        row["ParentName"],
                        row["UserCount"],
                        row["FullPath"]
                    );
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
                if (MessageBox.Show("이 부서/팀을 삭제하시겠습니까?\n(소속 사용자나 하위 팀이 있는 경우 삭제할 수 없습니다)",
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

            var title = UIHelper.CreateTitle(deptId.HasValue ? "부서/팀 수정" : "부서/팀 추가");
            title.Location = new Point(15, 15);
            editPanel.Controls.Add(title);

            int cardWidth = 570, cardHeight = 350;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);
            int cardY = UIHelper.CalculateCenterY(pnlContent.Height, cardHeight) - 30;

            var inputCard = UIHelper.CreateCard(cardX, cardY, cardWidth, cardHeight);

            int yPos = 30;

            inputCard.Controls.Add(UIHelper.CreateLabel("부서/팀 이름", 30, yPos, 10, Color.Black, true));
            var txtName = UIHelper.CreateTextBox(30, yPos + 25, 510, 30, "txtDeptName");
            inputCard.Controls.Add(txtName);
            yPos += 80;

            inputCard.Controls.Add(UIHelper.CreateLabel("상위 부서 (없으면 최상위 부서로 등록됨)", 30, yPos, 10, Color.Black, true));
            var cboParent = UIHelper.CreateComboBox(30, yPos + 25, 510, 30, "cboParentDept");
            LoadParentDepartmentCombo(cboParent, deptId);
            inputCard.Controls.Add(cboParent);
            yPos += 65;

            var lblInfo = UIHelper.CreateLabel("💡 상위 부서를 선택하지 않으면 '부서'로 등록됩니다.\n상위 부서를 선택하면 '팀'으로 등록됩니다.",
                30, yPos, 9, Color.Gray);
            inputCard.Controls.Add(lblInfo);
            yPos += 50;

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
            cbo.Items.Add(new ComboBoxItem { Text = "없음 (최상위 부서로 등록)", Value = "" });

            try
            {
                // 최상위 부서만 표시 (2단계 제한)
                string sql = "SELECT DeptId, DeptName FROM Department WHERE ParentDeptId IS NULL";
                if (excludeDeptId.HasValue)
                {
                    sql += $" AND DeptId != {excludeDeptId.Value}";
                }
                sql += " ORDER BY DeptName";

                var dt = db.Query(sql);

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
                var dt = db.Query($"SELECT DeptName, ParentDeptId FROM Department WHERE DeptId = {deptId}");

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
                MessageBox.Show("부서/팀 이름을 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string parentIdValue = "NULL";
            if (cboParent.SelectedItem is ComboBoxItem parentItem && !string.IsNullOrEmpty(parentItem.Value))
            {
                parentIdValue = parentItem.Value;
            }

            try
            {
                string sql;
                if (deptId.HasValue)
                {
                    sql = $"UPDATE Department SET DeptName = '{txtName.Text}', ParentDeptId = {parentIdValue} WHERE DeptId = {deptId.Value}";
                }
                else
                {
                    sql = $"INSERT INTO Department (DeptName, ParentDeptId) VALUES ('{txtName.Text}', {parentIdValue})";
                }

                db.NonQuery(sql);
                MessageBox.Show(deptId.HasValue ? "부서/팀이 수정되었습니다." : "부서/팀이 추가되었습니다.",
                    "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                var dtChild = db.Query($"SELECT COUNT(*) FROM Department WHERE ParentDeptId = {deptId}");
                if (dtChild.Rows.Count > 0 && Convert.ToInt32(dtChild.Rows[0][0]) > 0)
                {
                    MessageBox.Show("하위 팀이 존재하여 삭제할 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dtUser = db.Query($"SELECT COUNT(*) FROM User WHERE DeptId = {deptId}");
                if (dtUser.Rows.Count > 0 && Convert.ToInt32(dtUser.Rows[0][0]) > 0)
                {
                    MessageBox.Show("소속 사용자가 존재하여 삭제할 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                db.NonQuery($"DELETE FROM Department WHERE DeptId = {deptId}");
                LoadDepartmentData("");
                MessageBox.Show("부서/팀이 삭제되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"부서/팀 삭제 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== C. 사용자 관리 (팀 변경 시 부서 자동 변경) ====================
        private void ShowUserManage()
        {
            var title = UIHelper.CreateTitle("사용자 관리");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            int cardWidth = 1100;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX, 70, cardWidth, 60);
            var txtSearch = UIHelper.CreateTextBox(15, 18, 400, 25, "txtUserSearch", "이름 또는 ID 검색...");
            var cboDept = UIHelper.CreateComboBox(430, 18, 200, 25, "cboDeptFilter");
            LoadDepartmentComboForFilter(cboDept);

            var btnSearch = UIHelper.CreateBlueButton("검색", 650, 13, 80, 35);
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
            dgv.Columns.Add("DeptPath", "소속 (부서 > 팀)");
            dgv.Columns["DeptPath"].Width = 250;
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
            cbo.Items.Add(new ComboBoxItem { Text = "전체 부서/팀", Value = "0" });

            try
            {
                // 계층 구조 표시
                string sql = @"
                    SELECT d.DeptId, 
                           CASE 
                               WHEN d.ParentDeptId IS NULL THEN d.DeptName
                               ELSE CONCAT(p.DeptName, ' > ', d.DeptName)
                           END AS FullPath
                    FROM Department d
                    LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                    ORDER BY IFNULL(p.DeptId, d.DeptId), d.ParentDeptId IS NULL DESC, d.DeptName";

                var dt = db.Query(sql);
                foreach (DataRow row in dt.Rows)
                {
                    cbo.Items.Add(new ComboBoxItem
                    {
                        Text = row["FullPath"].ToString(),
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

            string sql = $@"
                SELECT u.UserId, u.Name, u.LoginId, u.Nickname, u.DeptId,
                       CASE 
                           WHEN u.DeptId IS NULL THEN '미배정'
                           WHEN d.ParentDeptId IS NULL THEN d.DeptName
                           ELSE CONCAT(p.DeptName, ' > ', d.DeptName)
                       END AS DeptPath
                FROM User u
                LEFT JOIN Department d ON u.DeptId = d.DeptId
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE u.Role = 'user' AND (u.Name LIKE '%{searchKeyword}%' OR u.LoginId LIKE '%{searchKeyword}%')";

            if (!string.IsNullOrEmpty(deptId) && deptId != "0")
            {
                sql += $" AND u.DeptId = {deptId}";
            }

            sql += " ORDER BY u.Name";

            try
            {
                var dt = db.Query(sql);
                foreach (DataRow row in dt.Rows)
                {
                    dgv.Rows.Add(
                        row["UserId"],
                        row["Name"],
                        row["LoginId"],
                        row["Nickname"] == DBNull.Value ? "" : row["Nickname"],
                        row["DeptPath"],
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

            var title = UIHelper.CreateTitle($"사용자 부서/팀 변경 - {userName}");
            title.Location = new Point(15, 15);
            editPanel.Controls.Add(title);

            int cardWidth = 600, cardHeight = 350;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);
            int cardY = UIHelper.CalculateCenterY(pnlContent.Height, cardHeight) - 30;

            var inputCard = UIHelper.CreateCard(cardX, cardY, cardWidth, cardHeight);

            inputCard.Controls.Add(UIHelper.CreateLabel("사용자", 30, 30, 10, Color.Black, true));
            inputCard.Controls.Add(UIHelper.CreateLabel(userName, 30, 55, 11, UIHelper.Colors.Primary));

            inputCard.Controls.Add(UIHelper.CreateLabel("변경할 부서/팀", 30, 100, 10, Color.Black, true));
            var cboDept = UIHelper.CreateComboBox(30, 125, 540, 30, "cboNewDept");
            LoadAllDepartmentComboWithHierarchy(cboDept);
            inputCard.Controls.Add(cboDept);

            var lblInfo = UIHelper.CreateLabel("💡 팀을 선택하면 상위 부서도 자동으로 설정됩니다.",
                30, 165, 9, Color.Gray);
            inputCard.Controls.Add(lblInfo);

            var btnSave = UIHelper.CreateBlueButton("변경", 30, 220, 260, 40);
            btnSave.Click += (s, e) => SaveUserDepartmentChange(userId, cboDept, editPanel);
            inputCard.Controls.Add(btnSave);

            var btnCancel = UIHelper.CreateBlueButton("취소", 300, 220, 270, 40);
            btnCancel.BackColor = Color.Gray;
            btnCancel.Click += (s, e) => CloseUserDepartmentChangePanel(editPanel);
            inputCard.Controls.Add(btnCancel);

            editPanel.Controls.Add(inputCard);

            LoadCurrentUserDepartment(userId, cboDept);
        }

        private void LoadAllDepartmentComboWithHierarchy(ComboBox cbo)
        {
            cbo.Items.Clear();

            try
            {
                // 계층 구조 표시
                string sql = @"
                    SELECT d.DeptId, 
                           CASE 
                               WHEN d.ParentDeptId IS NULL THEN CONCAT('🏢 ', d.DeptName, ' (부서)')
                               ELSE CONCAT('  └ 👥 ', d.DeptName, ' (팀)')
                           END AS DisplayName,
                           d.ParentDeptId IS NULL AS IsParent
                    FROM Department d
                    ORDER BY IFNULL(d.ParentDeptId, d.DeptId), d.ParentDeptId IS NULL DESC, d.DeptName";

                var dt = db.Query(sql);
                foreach (DataRow row in dt.Rows)
                {
                    cbo.Items.Add(new ComboBoxItem
                    {
                        Text = row["DisplayName"].ToString(),
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
                var dt = db.Query($"SELECT DeptId FROM User WHERE UserId = {userId}");
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
                MessageBox.Show("올바른 부서/팀을 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 팀 변경 시 부서도 자동 변경 (DeptId 하나만 저장하면 됨)
                db.NonQuery($"UPDATE User SET DeptId = {item.Value} WHERE UserId = {userId}");

                MessageBox.Show("부서/팀이 변경되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                // 관련 데이터 삭제
                db.NonQuery($"DELETE FROM UserVisibleUser WHERE OwnerUserId = {userId} OR VisibleUserId = {userId}");
                db.NonQuery($"DELETE FROM UserVisibleDept WHERE OwnerUserId = {userId}");
                db.NonQuery($"DELETE FROM ChatPermission WHERE UserAId = {userId} OR UserBId = {userId}");
                db.NonQuery($"DELETE FROM RecentChat WHERE UserId = {userId} OR PartnerUserId = {userId}");
                db.NonQuery($"DELETE FROM ChatMessage WHERE FromUserId = {userId} OR ToUserId = {userId}");
                db.NonQuery($"DELETE FROM UserLog WHERE UserId = {userId}");
                db.NonQuery($"DELETE FROM UserProfileMap WHERE OwnerUserId = {userId} OR TargetUserId = {userId}");
                db.NonQuery($"DELETE FROM Profile WHERE UserId = {userId}");
                db.NonQuery($"DELETE FROM User WHERE UserId = {userId}");

                LoadUserData("", null);
                MessageBox.Show("사용자가 삭제되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사용자 삭제 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== D. 대화내용 검색 (시간별 검색 추가) ====================
        private void ShowChatSearch()
        {
            var title = UIHelper.CreateTitle("대화내용 검색");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            int cardWidth = 1100;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX, 70, cardWidth, 120);

            // 첫 번째 줄
            searchCard.Controls.Add(UIHelper.CreateLabel("발신자", 20, 15, 9, Color.Black, true));
            var cboSender = UIHelper.CreateComboBox(80, 12, 180, 25, "cboSender");
            LoadUserComboForSearchAll(cboSender);
            searchCard.Controls.Add(cboSender);

            searchCard.Controls.Add(UIHelper.CreateLabel("수신자", 280, 15, 9, Color.Black, true));
            var cboReceiver = UIHelper.CreateComboBox(340, 12, 180, 25, "cboReceiver");
            LoadUserComboForSearchAll(cboReceiver);
            searchCard.Controls.Add(cboReceiver);

            searchCard.Controls.Add(UIHelper.CreateLabel("내용", 540, 15, 9, Color.Black, true));
            var txtContent = UIHelper.CreateTextBox(590, 12, 390, 25, "txtContent", "메시지 내용 검색...");
            searchCard.Controls.Add(txtContent);

            // 두 번째 줄 (시간 검색)
            searchCard.Controls.Add(UIHelper.CreateLabel("시작일", 20, 55, 9, Color.Black, true));
            var dtpStart = UIHelper.CreateDateTimePicker(80, 52, 200, 25, "dtpChatStart");
            dtpStart.Value = DateTime.Now.AddMonths(-1);
            dtpStart.Format = DateTimePickerFormat.Custom;
            dtpStart.CustomFormat = "yyyy-MM-dd HH:mm";
            dtpStart.ShowUpDown = false;
            searchCard.Controls.Add(dtpStart);

            searchCard.Controls.Add(UIHelper.CreateLabel("종료일", 300, 55, 9, Color.Black, true));
            var dtpEnd = UIHelper.CreateDateTimePicker(360, 52, 200, 25, "dtpChatEnd");
            dtpEnd.Value = DateTime.Now;
            dtpEnd.Format = DateTimePickerFormat.Custom;
            dtpEnd.CustomFormat = "yyyy-MM-dd HH:mm";
            dtpEnd.ShowUpDown = false;
            searchCard.Controls.Add(dtpEnd);

            var btnSearch = UIHelper.CreateBlueButton("검색", 980, 45, 100, 40);
            btnSearch.Click += BtnSearchChat_Click;
            searchCard.Controls.Add(btnSearch);

            pnlContent.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX, 205, cardWidth, 440, "dgvChat");
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

        private void LoadUserComboForSearchAll(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new ComboBoxItem { Text = "전체", Value = "0" });

            try
            {
                // Role 조건 없이 모든 사용자 조회
                var dt = db.Query("SELECT UserId, Name, Role FROM User ORDER BY Name");
                foreach (DataRow row in dt.Rows)
                {
                    string roleTag = row["Role"].ToString() == "admin" ? " [관리자]" : "";
                    cbo.Items.Add(new ComboBoxItem
                    {
                        Text = row["Name"].ToString() + roleTag,
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
            var dtpStart = pnlContent.Controls.Find("dtpChatStart", true).FirstOrDefault() as DateTimePicker;
            var dtpEnd = pnlContent.Controls.Find("dtpChatEnd", true).FirstOrDefault() as DateTimePicker;

            string senderId = (cboSender?.SelectedItem as ComboBoxItem)?.Value;
            string receiverId = (cboReceiver?.SelectedItem as ComboBoxItem)?.Value;
            string content = txtContent?.Text ?? "";
            DateTime startDate = dtpStart?.Value ?? DateTime.Now.AddMonths(-1);
            DateTime endDate = dtpEnd?.Value ?? DateTime.Now;

            LoadChatData(senderId, receiverId, content, startDate, endDate);
        }

        // 기존 LoadChatData(...) 내부의 SQL 빌드 부분을 안전하게 바꾼 예시
        // (숫자 ID는 int로 검증, content는 간단히 싱글쿼트 이스케이프)
        private async void LoadChatData(string senderId, string receiverId, string content, DateTime startDate, DateTime endDate)
        {
            var dgv = pnlContent.Controls.Find("dgvChat", true).FirstOrDefault() as DataGridView;
            if (dgv == null) return;

            dgv.Rows.Clear();

            // ID 검증
            string senderCond = "";
            if (!string.IsNullOrEmpty(senderId) && senderId != "0" && int.TryParse(senderId, out var sId))
                senderCond = $" AND cm.FromUserId = {sId}";

            string receiverCond = "";
            if (!string.IsNullOrEmpty(receiverId) && receiverId != "0" && int.TryParse(receiverId, out var rId))
                receiverCond = $" AND cm.ToUserId = {rId}";

            // 간단한 이스케이프 (과제/시연용)
            string contentCond = "";
            if (!string.IsNullOrWhiteSpace(content) && content != "메시지 내용 검색...")
            {
                var safe = content.Replace("'", "''");
                contentCond = $" AND cm.Content LIKE '%{safe}%'";
            }

            string sql = $@"
        SELECT cm.MessageId, cm.Content, cm.SentAt, cm.IsRead,
               us.Name AS FromUser, ur.Name AS ToUser
        FROM ChatMessage cm
        INNER JOIN `User` us ON cm.FromUserId = us.UserId
        INNER JOIN `User` ur ON cm.ToUserId = ur.UserId
        WHERE cm.SentAt BETWEEN '{startDate:yyyy-MM-dd HH:mm:ss}' AND '{endDate:yyyy-MM-dd HH:mm:ss}'"
                + senderCond + receiverCond + contentCond + " ORDER BY cm.SentAt DESC LIMIT 500";

            try
            {
                // UI 스레드 프리즈 방지: DB 호출을 백그라운드에서 실행
                var dt = await System.Threading.Tasks.Task.Run(() => db.Query(sql));

                foreach (DataRow row in dt.Rows)
                {
                    string isRead = Convert.ToInt32(row["IsRead"]) == 1 ? "읽음" : "안읽음";
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

        // ==================== E. 로그인/로그아웃 기록 (모든 사용자 표시) ====================
        private void ShowLoginLog()
        {
            var title = UIHelper.CreateTitle("로그인/로그아웃 기록");
            title.Location = new Point(15, 15);
            pnlContent.Controls.Add(title);

            int cardWidth = 1000;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX, 60, cardWidth, 70);

            searchCard.Controls.Add(UIHelper.CreateLabel("사용자", 20, 15, 9, Color.Black, true));
            var cboUser = UIHelper.CreateComboBox(80, 12, 220, 25, "cboLogUser");
            LoadUserComboForSearchAll(cboUser); // 모든 사용자 조회
            searchCard.Controls.Add(cboUser);

            searchCard.Controls.Add(UIHelper.CreateLabel("시작일", 320, 15, 9, Color.Black, true));
            var dtpStart = UIHelper.CreateDateTimePicker(375, 12, 180, 25, "dtpLogStart");
            dtpStart.Value = DateTime.Now.AddMonths(-1);
            searchCard.Controls.Add(dtpStart);

            searchCard.Controls.Add(UIHelper.CreateLabel("종료일", 575, 15, 9, Color.Black, true));
            var dtpEnd = UIHelper.CreateDateTimePicker(630, 12, 180, 25, "dtpLogEnd");
            dtpEnd.Value = DateTime.Now;
            searchCard.Controls.Add(dtpEnd);

            var btnSearch = UIHelper.CreateBlueButton("검색", 830, 10, 150, 30);
            btnSearch.Click += BtnSearchLog_Click;
            searchCard.Controls.Add(btnSearch);
            pnlContent.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX, 145, cardWidth, 500, "dgvLoginLog");
            dgv.Columns.Add("UserName", "사용자");
            dgv.Columns["UserName"].Width = 200;
            dgv.Columns.Add("Role", "권한");
            dgv.Columns["Role"].Width = 100;
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

            // 날짜 비교를 CreatedAt 전체 범위로 바꾸고, 삭제된 사용자 로그도 보이도록 LEFT JOIN 사용
            string sql = $@"
        SELECT ul.UserId, u.Name AS UserName, u.Role, ul.ActionType, ul.CreatedAt
        FROM UserLog ul
        LEFT JOIN `User` u ON ul.UserId = u.UserId
        WHERE ul.CreatedAt BETWEEN '{startDate:yyyy-MM-dd} 00:00:00' AND '{endDate:yyyy-MM-dd} 23:59:59'";

            if (!string.IsNullOrEmpty(userId) && userId != "0")
            {
                sql += $" AND ul.UserId = {userId}";
            }

            sql += " ORDER BY ul.CreatedAt DESC";

            try
            {
                var dt = db.Query(sql);
                foreach (DataRow row in dt.Rows)
                {
                    string userName = row["UserName"] == DBNull.Value ? $"ID:{row["UserId"]}" : row["UserName"].ToString();
                    string roleDisplay = row["Role"] == DBNull.Value ? "-" : (row["Role"].ToString() == "admin" ? "관리자" : "일반");
                    dgv.Rows.Add(
                        userName,
                        roleDisplay,
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

        // ==================== F. 권한 관리 (완전 구현) ====================
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

        // ==================== Tab1: 부서별 보기 권한 ====================
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

            int cardWidth = 1100;
            int cardX = UIHelper.CalculateCenterX(tabPanel.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX - 15, 10, cardWidth, 60);
            searchCard.Controls.Add(UIHelper.CreateLabel("사용자", 20, 18, 9, Color.Black, true));
            var cboUser = UIHelper.CreateComboBox(80, 15, 250, 25, "cboPermUser");
            LoadUserComboForSearchAll(cboUser);
            searchCard.Controls.Add(cboUser);

            var btnSearch = UIHelper.CreateBlueButton("조회", 350, 13, 80, 35);
            btnSearch.Click += (s, e) => LoadDeptPermissionData((cboUser.SelectedItem as ComboBoxItem)?.Value);
            searchCard.Controls.Add(btnSearch);

            var btnAdd = UIHelper.CreateBlueButton("+ 권한 추가", 980, 13, 100, 35);
            btnAdd.Click += (s, e) => ShowAddDeptPermission();
            searchCard.Controls.Add(btnAdd);

            tabPanel.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX - 15, 85, cardWidth, 450, "dgvDeptPerm");
            dgv.Columns.Add("OwnerUserId", "OwnerUserId");
            dgv.Columns["OwnerUserId"].Visible = false;
            dgv.Columns.Add("DeptId", "DeptId");
            dgv.Columns["DeptId"].Visible = false;
            dgv.Columns.Add("UserName", "사용자");
            dgv.Columns["UserName"].Width = 200;
            dgv.Columns.Add("DeptPath", "볼 수 있는 부서/팀");
            dgv.Columns["DeptPath"].Width = 300;
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

            string sql = $@"
                SELECT uvd.OwnerUserId, uvd.DeptId, u.Name AS UserName,
                       CASE 
                           WHEN d.ParentDeptId IS NULL THEN d.DeptName
                           ELSE CONCAT(p.DeptName, ' > ', d.DeptName)
                       END AS DeptPath
                FROM UserVisibleDept uvd
                INNER JOIN User u ON uvd.OwnerUserId = u.UserId
                INNER JOIN Department d ON uvd.DeptId = d.DeptId
                LEFT JOIN Department p ON d.ParentDeptId = p.DeptId
                WHERE 1=1";

            if (!string.IsNullOrEmpty(userId) && userId != "0")
            {
                sql += $" AND uvd.OwnerUserId = {userId}";
            }

            sql += " ORDER BY u.Name, DeptPath";

            try
            {
                var dt = db.Query(sql);
                foreach (DataRow row in dt.Rows)
                {
                    dgv.Rows.Add(row["OwnerUserId"], row["DeptId"], row["UserName"], row["DeptPath"]);
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
                int ownerUserId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["OwnerUserId"].Value);
                int deptId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["DeptId"].Value);
                string userName = dgv.Rows[e.RowIndex].Cells["UserName"].Value.ToString();
                string deptPath = dgv.Rows[e.RowIndex].Cells["DeptPath"].Value.ToString();

                if (MessageBox.Show($"'{userName}' 사용자의 '{deptPath}' 보기 권한을 삭제하시겠습니까?",
                    "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteDeptPermission(ownerUserId, deptId);
                }
            }
        }

        private void DeleteDeptPermission(int ownerUserId, int deptId)
        {
            try
            {
                db.NonQuery($"DELETE FROM UserVisibleDept WHERE OwnerUserId = {ownerUserId} AND DeptId = {deptId}");
                LoadDeptPermissionData(null);
                MessageBox.Show("부서 보기 권한이 삭제되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"삭제 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowAddDeptPermission()
        {
            pnlContent.Visible = false;

            var editPanel = new Panel
            {
                Name = "pnlAddDeptPerm",
                Location = pnlContent.Location,
                Size = pnlContent.Size,
                BackColor = pnlContent.BackColor,
                Padding = new Padding(15),
                AutoScroll = true
            };
            this.Controls.Add(editPanel);
            editPanel.BringToFront();

            var title = UIHelper.CreateTitle("부서별 보기 권한 추가");
            title.Location = new Point(15, 15);
            editPanel.Controls.Add(title);

            int cardWidth = 600, cardHeight = 300;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);
            int cardY = UIHelper.CalculateCenterY(pnlContent.Height, cardHeight) - 30;

            var inputCard = UIHelper.CreateCard(cardX, cardY, cardWidth, cardHeight);

            int yPos = 30;

            inputCard.Controls.Add(UIHelper.CreateLabel("사용자 선택", 30, yPos, 10, Color.Black, true));
            var cboUser = UIHelper.CreateComboBox(30, yPos + 25, 540, 30, "cboAddUser");
            LoadUserComboForSearchAll(cboUser);
            inputCard.Controls.Add(cboUser);
            yPos += 80;

            inputCard.Controls.Add(UIHelper.CreateLabel("볼 수 있는 부서/팀", 30, yPos, 10, Color.Black, true));
            var cboDept = UIHelper.CreateComboBox(30, yPos + 25, 540, 30, "cboAddDept");
            LoadAllDepartmentComboWithHierarchy(cboDept);
            inputCard.Controls.Add(cboDept);
            yPos += 80;

            var btnSave = UIHelper.CreateBlueButton("추가", 30, yPos, 260, 40);
            btnSave.Click += (s, e) => SaveDeptPermission(cboUser, cboDept, editPanel);
            inputCard.Controls.Add(btnSave);

            var btnCancel = UIHelper.CreateBlueButton("취소", 300, yPos, 270, 40);
            btnCancel.BackColor = Color.Gray;
            btnCancel.Click += (s, e) => CloseAddDeptPermPanel(editPanel);
            inputCard.Controls.Add(btnCancel);

            editPanel.Controls.Add(inputCard);
        }

        private void SaveDeptPermission(ComboBox cboUser, ComboBox cboDept, Panel editPanel)
        {
            if (!(cboUser.SelectedItem is ComboBoxItem userItem) || string.IsNullOrEmpty(userItem.Value) || userItem.Value == "0")
            {
                MessageBox.Show("사용자를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(cboDept.SelectedItem is ComboBoxItem deptItem) || string.IsNullOrEmpty(deptItem.Value))
            {
                MessageBox.Show("부서/팀을 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 중복 체크
                var dtCheck = db.Query($"SELECT COUNT(*) FROM UserVisibleDept WHERE OwnerUserId = {userItem.Value} AND DeptId = {deptItem.Value}");
                if (dtCheck.Rows.Count > 0 && Convert.ToInt32(dtCheck.Rows[0][0]) > 0)
                {
                    MessageBox.Show("이미 동일한 권한이 존재합니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.NonQuery($"INSERT INTO UserVisibleDept (OwnerUserId, DeptId) VALUES ({userItem.Value}, {deptItem.Value})");
                MessageBox.Show("부서 보기 권한이 추가되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CloseAddDeptPermPanel(editPanel);
                ShowPermissionManage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"추가 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseAddDeptPermPanel(Panel editPanel)
        {
            this.Controls.Remove(editPanel);
            editPanel.Dispose();
            pnlContent.Visible = true;
        }

        // ==================== Tab2: 사용자별 보기 권한 ====================
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

            int cardWidth = 1100;
            int cardX = UIHelper.CalculateCenterX(tabPanel.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX - 15, 10, cardWidth, 60);
            searchCard.Controls.Add(UIHelper.CreateLabel("사용자", 20, 18, 9, Color.Black, true));
            var cboUser = UIHelper.CreateComboBox(80, 15, 250, 25, "cboPermUser2");
            LoadUserComboForSearchAll(cboUser);
            searchCard.Controls.Add(cboUser);

            var btnSearch = UIHelper.CreateBlueButton("조회", 350, 13, 80, 35);
            btnSearch.Click += (s, e) => LoadUserPermissionData((cboUser.SelectedItem as ComboBoxItem)?.Value);
            searchCard.Controls.Add(btnSearch);

            var btnAdd = UIHelper.CreateBlueButton("+ 권한 추가", 980, 13, 100, 35);
            btnAdd.Click += (s, e) => ShowAddUserPermission();
            searchCard.Controls.Add(btnAdd);

            tabPanel.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX - 15, 85, cardWidth, 450, "dgvUserPerm");
            dgv.Columns.Add("OwnerUserId", "OwnerUserId");
            dgv.Columns["OwnerUserId"].Visible = false;
            dgv.Columns.Add("VisibleUserId", "VisibleUserId");
            dgv.Columns["VisibleUserId"].Visible = false;
            dgv.Columns.Add("OwnerName", "사용자");
            dgv.Columns["OwnerName"].Width = 200;
            dgv.Columns.Add("VisibleName", "볼 수 있는 사용자");
            dgv.Columns["VisibleName"].Width = 200;
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

            string sql = @"
                SELECT uvu.OwnerUserId, uvu.VisibleUserId,
                       u1.Name AS OwnerName, u2.Name AS VisibleName
                FROM UserVisibleUser uvu
                INNER JOIN User u1 ON uvu.OwnerUserId = u1.UserId
                INNER JOIN User u2 ON uvu.VisibleUserId = u2.UserId
                WHERE 1=1";

            if (!string.IsNullOrEmpty(userId) && userId != "0")
            {
                sql += $" AND uvu.OwnerUserId = {userId}";
            }

            sql += " ORDER BY u1.Name, u2.Name";

            try
            {
                var dt = db.Query(sql);
                foreach (DataRow row in dt.Rows)
                {
                    dgv.Rows.Add(row["OwnerUserId"], row["VisibleUserId"], row["OwnerName"], row["VisibleName"]);
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
                int ownerUserId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["OwnerUserId"].Value);
                int visibleUserId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["VisibleUserId"].Value);
                string ownerName = dgv.Rows[e.RowIndex].Cells["OwnerName"].Value.ToString();
                string visibleName = dgv.Rows[e.RowIndex].Cells["VisibleName"].Value.ToString();

                if (MessageBox.Show($"'{ownerName}'이(가) '{visibleName}'을(를) 볼 수 있는 권한을 삭제하시겠습니까?",
                    "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteUserPermission(ownerUserId, visibleUserId);
                }
            }
        }

        private void DeleteUserPermission(int ownerUserId, int visibleUserId)
        {
            try
            {
                db.NonQuery($"DELETE FROM UserVisibleUser WHERE OwnerUserId = {ownerUserId} AND VisibleUserId = {visibleUserId}");
                LoadUserPermissionData(null);
                MessageBox.Show("사용자 보기 권한이 삭제되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"삭제 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowAddUserPermission()
        {
            pnlContent.Visible = false;

            var editPanel = new Panel
            {
                Name = "pnlAddUserPerm",
                Location = pnlContent.Location,
                Size = pnlContent.Size,
                BackColor = pnlContent.BackColor,
                Padding = new Padding(15),
                AutoScroll = true
            };
            this.Controls.Add(editPanel);
            editPanel.BringToFront();

            var title = UIHelper.CreateTitle("사용자별 보기 권한 추가");
            title.Location = new Point(15, 15);
            editPanel.Controls.Add(title);

            int cardWidth = 600, cardHeight = 300;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);
            int cardY = UIHelper.CalculateCenterY(pnlContent.Height, cardHeight) - 30;

            var inputCard = UIHelper.CreateCard(cardX, cardY, cardWidth, cardHeight);

            int yPos = 30;

            inputCard.Controls.Add(UIHelper.CreateLabel("사용자 선택", 30, yPos, 10, Color.Black, true));
            var cboOwner = UIHelper.CreateComboBox(30, yPos + 25, 540, 30, "cboOwnerUser");
            LoadUserComboForSearchAll(cboOwner);
            inputCard.Controls.Add(cboOwner);
            yPos += 80;

            inputCard.Controls.Add(UIHelper.CreateLabel("볼 수 있는 사용자", 30, yPos, 10, Color.Black, true));
            var cboVisible = UIHelper.CreateComboBox(30, yPos + 25, 540, 30, "cboVisibleUser");
            LoadUserComboForSearchAll(cboVisible);
            inputCard.Controls.Add(cboVisible);
            yPos += 80;

            var btnSave = UIHelper.CreateBlueButton("추가", 30, yPos, 260, 40);
            btnSave.Click += (s, e) => SaveUserPermission(cboOwner, cboVisible, editPanel);
            inputCard.Controls.Add(btnSave);

            var btnCancel = UIHelper.CreateBlueButton("취소", 300, yPos, 270, 40);
            btnCancel.BackColor = Color.Gray;
            btnCancel.Click += (s, e) => CloseAddUserPermPanel(editPanel);
            inputCard.Controls.Add(btnCancel);

            editPanel.Controls.Add(inputCard);
        }

        private void SaveUserPermission(ComboBox cboOwner, ComboBox cboVisible, Panel editPanel)
        {
            if (!(cboOwner.SelectedItem is ComboBoxItem ownerItem) || string.IsNullOrEmpty(ownerItem.Value) || ownerItem.Value == "0")
            {
                MessageBox.Show("사용자를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(cboVisible.SelectedItem is ComboBoxItem visibleItem) || string.IsNullOrEmpty(visibleItem.Value) || visibleItem.Value == "0")
            {
                MessageBox.Show("볼 수 있는 사용자를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ownerItem.Value == visibleItem.Value)
            {
                MessageBox.Show("자기 자신에게는 권한을 부여할 수 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 중복 체크
                var dtCheck = db.Query($"SELECT COUNT(*) FROM UserVisibleUser WHERE OwnerUserId = {ownerItem.Value} AND VisibleUserId = {visibleItem.Value}");
                if (dtCheck.Rows.Count > 0 && Convert.ToInt32(dtCheck.Rows[0][0]) > 0)
                {
                    MessageBox.Show("이미 동일한 권한이 존재합니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.NonQuery($"INSERT INTO UserVisibleUser (OwnerUserId, VisibleUserId) VALUES ({ownerItem.Value}, {visibleItem.Value})");
                MessageBox.Show("사용자 보기 권한이 추가되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CloseAddUserPermPanel(editPanel);
                ShowPermissionManage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"추가 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseAddUserPermPanel(Panel editPanel)
        {
            this.Controls.Remove(editPanel);
            editPanel.Dispose();
            pnlContent.Visible = true;
        }

        // ==================== Tab3: 대화 차단 관리 ====================
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

            int cardWidth = 1100;
            int cardX = UIHelper.CalculateCenterX(tabPanel.Width, cardWidth);

            var searchCard = UIHelper.CreateCard(cardX - 15, 10, cardWidth, 60);
            searchCard.Controls.Add(UIHelper.CreateLabel("사용자", 20, 18, 9, Color.Black, true));
            var cboUser = UIHelper.CreateComboBox(80, 15, 250, 25, "cboPermUser3");
            LoadUserComboForSearchAll(cboUser);
            searchCard.Controls.Add(cboUser);

            var btnSearch = UIHelper.CreateBlueButton("조회", 350, 13, 80, 35);
            btnSearch.Click += (s, e) => LoadChatPermissionData((cboUser.SelectedItem as ComboBoxItem)?.Value);
            searchCard.Controls.Add(btnSearch);

            var btnAdd = UIHelper.CreateBlueButton("+ 차단 추가", 980, 13, 100, 35);
            btnAdd.Click += (s, e) => ShowAddChatBlock();
            searchCard.Controls.Add(btnAdd);

            tabPanel.Controls.Add(searchCard);

            var dgv = UIHelper.CreateDGV(cardX - 15, 85, cardWidth, 450, "dgvChatPerm");
            dgv.Columns.Add("UserAId", "UserAId");
            dgv.Columns["UserAId"].Visible = false;
            dgv.Columns.Add("UserBId", "UserBId");
            dgv.Columns["UserBId"].Visible = false;
            dgv.Columns.Add("UserAName", "사용자 A");
            dgv.Columns["UserAName"].Width = 200;
            dgv.Columns.Add("UserBName", "사용자 B");
            dgv.Columns["UserBName"].Width = 200;
            dgv.Columns.Add("IsBlocked", "상태");
            dgv.Columns["IsBlocked"].Width = 120;
            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "차단 해제",
                UseColumnTextForButtonValue = true,
                Width = 100,
                Name = "Unblock"
            });
            dgv.Columns.Add(new DataGridViewButtonColumn
            {
                Text = "삭제",
                UseColumnTextForButtonValue = true,
                Width = 80,
                Name = "Delete"
            });

            dgv.CellClick += DgvChatPerm_CellClick;
            tabPanel.Controls.Add(dgv);

            pnlContent.Controls.Add(tabPanel);
            LoadChatPermissionData(null);
        }

        private void LoadChatPermissionData(string userId)
        {
            var dgv = pnlContent.Controls.Find("dgvChatPerm", true).FirstOrDefault() as DataGridView;
            if (dgv == null) return;

            dgv.Rows.Clear();

            string sql = @"
                SELECT cp.UserAId, cp.UserBId, cp.IsBlocked,
                       u1.Name AS UserAName, u2.Name AS UserBName
                FROM ChatPermission cp
                INNER JOIN User u1 ON cp.UserAId = u1.UserId
                INNER JOIN User u2 ON cp.UserBId = u2.UserId
                WHERE 1=1";

            if (!string.IsNullOrEmpty(userId) && userId != "0")
            {
                sql += $" AND (cp.UserAId = {userId} OR cp.UserBId = {userId})";
            }

            sql += " ORDER BY u1.Name, u2.Name";

            try
            {
                var dt = db.Query(sql);
                foreach (DataRow row in dt.Rows)
                {
                    string status = Convert.ToBoolean(row["IsBlocked"]) ? "🚫 차단됨" : "✅ 허용";
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
            int userAId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["UserAId"].Value);
            int userBId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["UserBId"].Value);
            string userAName = dgv.Rows[e.RowIndex].Cells["UserAName"].Value.ToString();
            string userBName = dgv.Rows[e.RowIndex].Cells["UserBName"].Value.ToString();

            if (dgv.Columns[e.ColumnIndex].Name == "Unblock")
            {
                if (MessageBox.Show($"'{userAName}'과(와) '{userBName}' 간의 대화를 허용하시겠습니까?",
                    "차단 해제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    UnblockChat(userAId, userBId);
                }
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (MessageBox.Show($"'{userAName}'과(와) '{userBName}' 간의 대화 설정을 삭제하시겠습니까?",
                    "삭제 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteChatPermission(userAId, userBId);
                }
            }
        }

        private void UnblockChat(int userAId, int userBId)
        {
            try
            {
                db.NonQuery($"UPDATE ChatPermission SET IsBlocked = 0 WHERE UserAId = {userAId} AND UserBId = {userBId}");
                LoadChatPermissionData(null);
                MessageBox.Show("대화가 허용되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"차단 해제 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteChatPermission(int userAId, int userBId)
        {
            try
            {
                db.NonQuery($"DELETE FROM ChatPermission WHERE UserAId = {userAId} AND UserBId = {userBId}");
                LoadChatPermissionData(null);
                MessageBox.Show("대화 설정이 삭제되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"삭제 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowAddChatBlock()
        {
            pnlContent.Visible = false;

            var editPanel = new Panel
            {
                Name = "pnlAddChatBlock",
                Location = pnlContent.Location,
                Size = pnlContent.Size,
                BackColor = pnlContent.BackColor,
                Padding = new Padding(15),
                AutoScroll = true
            };
            this.Controls.Add(editPanel);
            editPanel.BringToFront();

            var title = UIHelper.CreateTitle("대화 차단 추가");
            title.Location = new Point(15, 15);
            editPanel.Controls.Add(title);

            int cardWidth = 600, cardHeight = 300;
            int cardX = UIHelper.CalculateCenterX(pnlContent.Width, cardWidth);
            int cardY = UIHelper.CalculateCenterY(pnlContent.Height, cardHeight) - 30;

            var inputCard = UIHelper.CreateCard(cardX, cardY, cardWidth, cardHeight);

            int yPos = 30;

            inputCard.Controls.Add(UIHelper.CreateLabel("사용자 A", 30, yPos, 10, Color.Black, true));
            var cboUserA = UIHelper.CreateComboBox(30, yPos + 25, 540, 30, "cboUserA");
            LoadUserComboForSearchAll(cboUserA);
            inputCard.Controls.Add(cboUserA);
            yPos += 80;

            inputCard.Controls.Add(UIHelper.CreateLabel("사용자 B", 30, yPos, 10, Color.Black, true));
            var cboUserB = UIHelper.CreateComboBox(30, yPos + 25, 540, 30, "cboUserB");
            LoadUserComboForSearchAll(cboUserB);
            inputCard.Controls.Add(cboUserB);
            yPos += 80;

            var btnSave = UIHelper.CreateBlueButton("차단 추가", 30, yPos, 260, 40);
            btnSave.Click += (s, e) => SaveChatBlock(cboUserA, cboUserB, editPanel);
            inputCard.Controls.Add(btnSave);

            var btnCancel = UIHelper.CreateBlueButton("취소", 300, yPos, 270, 40);
            btnCancel.BackColor = Color.Gray;
            btnCancel.Click += (s, e) => CloseAddChatBlockPanel(editPanel);
            inputCard.Controls.Add(btnCancel);

            editPanel.Controls.Add(inputCard);
        }

        private void SaveChatBlock(ComboBox cboUserA, ComboBox cboUserB, Panel editPanel)
        {
            if (!(cboUserA.SelectedItem is ComboBoxItem userAItem) || string.IsNullOrEmpty(userAItem.Value) || userAItem.Value == "0")
            {
                MessageBox.Show("사용자 A를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(cboUserB.SelectedItem is ComboBoxItem userBItem) || string.IsNullOrEmpty(userBItem.Value) || userBItem.Value == "0")
            {
                MessageBox.Show("사용자 B를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (userAItem.Value == userBItem.Value)
            {
                MessageBox.Show("동일한 사용자를 선택할 수 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 중복 체크 (양방향)
                var dtCheck = db.Query($@"SELECT COUNT(*) FROM ChatPermission 
                    WHERE (UserAId = {userAItem.Value} AND UserBId = {userBItem.Value})
                       OR (UserAId = {userBItem.Value} AND UserBId = {userAItem.Value})");

                if (dtCheck.Rows.Count > 0 && Convert.ToInt32(dtCheck.Rows[0][0]) > 0)
                {
                    MessageBox.Show("이미 동일한 대화 설정이 존재합니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.NonQuery($"INSERT INTO ChatPermission (UserAId, UserBId, IsBlocked) VALUES ({userAItem.Value}, {userBItem.Value}, 1)");
                MessageBox.Show("대화가 차단되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CloseAddChatBlockPanel(editPanel);
                ShowPermissionManage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"차단 추가 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseAddChatBlockPanel(Panel editPanel)
        {
            this.Controls.Remove(editPanel);
            editPanel.Dispose();
            pnlContent.Visible = true;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // 로그아웃 플래그 설정: LoginForm이 로그아웃으로 복귀할지 알 수 있도록 합니다.
                leehaeun.LoginForm.Logout = true;

                var loginForm = Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.GetType().Name == "LoginForm");
                if (loginForm != null)
                {
                    loginForm.Show();
                    loginForm.BringToFront();
                }
                else
                {
                    var lf = new leehaeun.LoginForm();
                    lf.Show();
                }

                // 관리자 폼 닫기
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"로그아웃 처리 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // ==================== ComboBoxItem 헬퍼 클래스 ====================
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}