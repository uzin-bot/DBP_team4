namespace DBP_finalproject_chatting
{
    partial class ChatForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatForm));
            rtbChatLog = new RichTextBox();
            txtInput = new TextBox();
            btnSend = new Button();
            niChatAlert = new NotifyIcon(components);
            SuspendLayout();
            // 
            // rtbChatLog
            // 
            rtbChatLog.Location = new Point(29, 26);
            rtbChatLog.Name = "rtbChatLog";
            rtbChatLog.Size = new Size(464, 515);
            rtbChatLog.TabIndex = 0;
            rtbChatLog.Text = "";
            // 
            // txtInput
            // 
            txtInput.Location = new Point(29, 565);
            txtInput.Multiline = true;
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(350, 44);
            txtInput.TabIndex = 1;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(399, 565);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 44);
            btnSend.TabIndex = 2;
            btnSend.Text = "전송";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // niChatAlert
            // 
            niChatAlert.Icon = (Icon)resources.GetObject("niChatAlert.Icon");
            niChatAlert.Text = "notifyIcon1";
            niChatAlert.Visible = true;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(525, 638);
            Controls.Add(btnSend);
            Controls.Add(txtInput);
            Controls.Add(rtbChatLog);
            Name = "ChatForm";
            Text = "ChatForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtbChatLog;
        private TextBox txtInput;
        private Button btnSend;
        private NotifyIcon niChatAlert;
    }
}