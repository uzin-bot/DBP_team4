using MySql.Data.MySqlClient;
using namyesol;
using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;

namespace namyesol
{
    public partial class Dept : Form
    {
        string connStr = "server=223.130.151.111;port=3306;database=company;uid=root;pwd=비밀번호;";

        public Dept()
        {
            InitializeComponent();
        }

        private void Dept_Load(object sender, EventArgs e)
        {
            LoadTreeView();
        }

        // 회사 → 부서 → 직원(상태) 표시
        private void LoadTreeView()
        {
            tvdept.Nodes.Clear();
            TreeNode companyNode = new TreeNode("회사");
            tvdept.Nodes.Add(companyNode);

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                //부서 목록
                string sqlDept = "SELECT dept_id, dept_name FROM department";
                MySqlCommand cmdDept = new MySqlCommand(sqlDept, conn);
                MySqlDataReader drDept = cmdDept.ExecuteReader();
                DataTable dtDept = new DataTable();
                dtDept.Load(drDept);

                foreach (DataRow dept in dtDept.Rows)
                {
                    TreeNode deptNode = new TreeNode(dept["dept_name"].ToString());
                    deptNode.Tag = dept["dept_id"];
                    companyNode.Nodes.Add(deptNode);

                    //부서별 직원
                    string sqlEmp = "SELECT emp_id, emp_name, status FROM employee WHERE dept_id = @dept_id";
                    MySqlCommand cmdEmp = new MySqlCommand(sqlEmp, conn);
                    cmdEmp.Parameters.AddWithValue("@dept_id", dept["dept_id"]);
                    MySqlDataReader drEmp = cmdEmp.ExecuteReader();

                    while (drEmp.Read())
                    {
                        string empName = drEmp["emp_name"].ToString();
                        string status = drEmp["status"].ToString();
                        TreeNode empNode = new TreeNode($"{empName} ({status})");
                        empNode.Tag = drEmp["emp_id"];
                        deptNode.Nodes.Add(empNode);
                    }
                    drEmp.Close();
                }
            }

            tvdept.ExpandAll();
        }

        //직원 클릭 시 상세 정보 표시(ID, 이름, 부서)
        private void tvdept_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 2) //직원 노드
            {
                string empId = e.Node.Tag.ToString();

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"
                        SELECT e.emp_id, e.emp_name, e.status, d.dept_name
                        FROM employee e
                        JOIN department d ON e.dept_id = d.dept_id
                        WHERE e.emp_id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", empId);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        txtID.Text = dr["emp_id"].ToString();
                        txtname.Text = dr["emp_name"].ToString();
                        cbdept.Text = dr["dept_name"].ToString();
                    }
                    dr.Close();
                }
            }
        }

        //직원 더블클릭 → 채팅창 열기
        private void tvdept_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 2) //직원 노드
            {
                string targetID = e.Node.Tag.ToString(); // emp_id 가져오기
                // ChatForm chat = new ChatForm(targetID);
                // chat.Show();
            }
        }

        // 즐겨찾기 목록 보기
        private void btnsearch_Click(object sender, EventArgs e)
        {
            lBlist.Items.Clear();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
                    SELECT e.emp_name, e.status
                    FROM employee e
                    JOIN favorite f ON e.emp_id = f.emp_id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lBlist.Items.Add($"{dr["emp_name"]} ({dr["status"]})");
                }
            }
        }

        // 즐겨찾기 추가
        private void btnadd_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("즐겨찾기에 추가할 직원을 선택하세요!");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // 중복 확인
                string checkSql = "SELECT COUNT(*) FROM favorite WHERE emp_id=@emp";
                MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@emp", txtID.Text);
                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (exists > 0)
                {
                    MessageBox.Show("이미 즐겨찾기에 등록된 직원입니다!");
                    return;
                }

                string sql = "INSERT INTO favorite (emp_id) VALUES (@emp)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@emp", txtID.Text);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("즐겨찾기에 추가되었습니다!");
        }

        // 즐겨찾기 삭제
        private void btndelete_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("즐겨찾기에서 삭제할 직원을 선택하세요!");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "DELETE FROM favorite WHERE emp_id=@emp";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@emp", txtID.Text);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("즐겨찾기에서 삭제되었습니다!");
        }

        //채팅하기 버튼
        private void btnChat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtname.Text))
            {
                MessageBox.Show("채팅할 직원을 선택하세요!");
                return;
            }

            //ChatForm chat = new ChatForm(txtname.Text);
            //chat.Show();
        }
    }
}