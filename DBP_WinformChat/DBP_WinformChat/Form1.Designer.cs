namespace DBP_WinformChat
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LoginOpen = new Button();
            DeptOpen = new Button();
            ChatOpen = new Button();
            SuspendLayout();
            // 
            // LoginOpen
            // 
            LoginOpen.Location = new Point(102, 148);
            LoginOpen.Name = "LoginOpen";
            LoginOpen.Size = new Size(75, 23);
            LoginOpen.TabIndex = 0;
            LoginOpen.Text = "Login";
            LoginOpen.UseVisualStyleBackColor = true;
            LoginOpen.Click += LoginOpen_Click;
            // 
            // DeptOpen
            // 
            DeptOpen.Location = new Point(102, 218);
            DeptOpen.Name = "DeptOpen";
            DeptOpen.Size = new Size(75, 23);
            DeptOpen.TabIndex = 1;
            DeptOpen.Text = "Dept";
            DeptOpen.UseVisualStyleBackColor = true;
            DeptOpen.Click += DeptOpen_Click;
            // 
            // ChatOpen
            // 
            ChatOpen.Location = new Point(102, 288);
            ChatOpen.Name = "ChatOpen";
            ChatOpen.Size = new Size(75, 23);
            ChatOpen.TabIndex = 2;
            ChatOpen.Text = "Chat";
            ChatOpen.UseVisualStyleBackColor = true;
            ChatOpen.Click += ChatOpen_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(282, 450);
            Controls.Add(ChatOpen);
            Controls.Add(DeptOpen);
            Controls.Add(LoginOpen);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button LoginOpen;
        private Button DeptOpen;
        private Button ChatOpen;
    }
}
