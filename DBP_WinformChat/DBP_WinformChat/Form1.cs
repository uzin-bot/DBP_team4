namespace DBP_WinformChat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoginOpen_Click(object sender, EventArgs e)
        {
            var login = new leehaeun.Login();
            login.ShowDialog();
        }

        private void DeptOpen_Click(object sender, EventArgs e)
        {
            var dept = new namyesol.Dept();
            dept.ShowDialog();
        }

        private void ChatOpen_Click(object sender, EventArgs e)
        {
            var chat = new kyg.TempForm();
            chat.ShowDialog();
        }
    }
}
