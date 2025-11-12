namespace DBP_finalproject_chatting
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            tvDepartmentEmployees = new TreeView();
            lvRecentChats = new ListView();
            textBox1 = new TextBox();
            button1 = new Button();
            niChatAlert = new NotifyIcon(components);
            SuspendLayout();
            // 
            // tvDepartmentEmployees
            // 
            tvDepartmentEmployees.Location = new Point(31, 12);
            tvDepartmentEmployees.Name = "tvDepartmentEmployees";
            tvDepartmentEmployees.Size = new Size(402, 466);
            tvDepartmentEmployees.TabIndex = 0;
            // 
            // lvRecentChats
            // 
            lvRecentChats.Location = new Point(466, 12);
            lvRecentChats.Name = "lvRecentChats";
            lvRecentChats.Size = new Size(446, 272);
            lvRecentChats.TabIndex = 1;
            lvRecentChats.UseCompatibleStateImageBehavior = false;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(466, 351);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(255, 27);
            textBox1.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(779, 349);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 3;
            button1.Text = "로그인";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // niChatAlert
            // 
            niChatAlert.Icon = (Icon)resources.GetObject("niChatAlert.Icon");
            niChatAlert.Text = "notifyIcon1";
            niChatAlert.Visible = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(924, 520);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(lvRecentChats);
            Controls.Add(tvDepartmentEmployees);
            Name = "MainForm";
            Text = "MainForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView tvDepartmentEmployees;
        private ListView lvRecentChats;
        private TextBox textBox1;
        private Button button1;
        private NotifyIcon niChatAlert;
    }
}