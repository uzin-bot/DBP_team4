using System;
using System.Windows.Forms;

namespace namyesol
{
    public partial class ChatForm : Form
    {
        private string partnerName;

        public ChatForm(string name)
        {
            InitializeComponent();
            partnerName = name;
            Text = $"{partnerName} 님과의 1:1 채팅";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            /*if (string.IsNullOrWhiteSpace(txtMessage.Text)) return;

            lstChat.Items.Add($"나: {txtMessage.Text}");
            txtMessage.Clear();
            */
        }
    }
}