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
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnSendFile = new Button();
            btnEmojiSmiley = new Button();
            btnEmojiCrying = new Button();
            btnEmojiHeart = new Button();
            SuspendLayout();
            // 
            // rtbChatLog
            // 
            rtbChatLog.Location = new Point(30, 72);
            rtbChatLog.Name = "rtbChatLog";
            rtbChatLog.Size = new Size(464, 715);
            rtbChatLog.TabIndex = 0;
            rtbChatLog.Text = "";
            // 
            // txtInput
            // 
            txtInput.Location = new Point(30, 801);
            txtInput.Multiline = true;
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(350, 44);
            txtInput.TabIndex = 1;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(401, 801);
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
            // txtSearch
            // 
            txtSearch.Location = new Point(30, 16);
            txtSearch.Margin = new Padding(4, 4, 4, 4);
            txtSearch.Multiline = true;
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(350, 44);
            txtSearch.TabIndex = 3;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(399, 16);
            btnSearch.Margin = new Padding(4, 4, 4, 4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(96, 45);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "검색";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnSendFile
            // 
            btnSendFile.Location = new Point(401, 865);
            btnSendFile.Margin = new Padding(4, 4, 4, 4);
            btnSendFile.Name = "btnSendFile";
            btnSendFile.Size = new Size(94, 32);
            btnSendFile.TabIndex = 5;
            btnSendFile.Text = "파일전송";
            btnSendFile.UseVisualStyleBackColor = true;
            // 
            // btnEmojiSmiley
            // 
            btnEmojiSmiley.Location = new Point(30, 865);
            btnEmojiSmiley.Margin = new Padding(4, 4, 4, 4);
            btnEmojiSmiley.Name = "btnEmojiSmiley";
            btnEmojiSmiley.Size = new Size(96, 31);
            btnEmojiSmiley.TabIndex = 6;
            btnEmojiSmiley.Text = "웃음";
            btnEmojiSmiley.UseVisualStyleBackColor = true;
            // 
            // btnEmojiCrying
            // 
            btnEmojiCrying.Location = new Point(158, 867);
            btnEmojiCrying.Margin = new Padding(4, 4, 4, 4);
            btnEmojiCrying.Name = "btnEmojiCrying";
            btnEmojiCrying.Size = new Size(96, 31);
            btnEmojiCrying.TabIndex = 7;
            btnEmojiCrying.Text = "슬픔";
            btnEmojiCrying.UseVisualStyleBackColor = true;
            // 
            // btnEmojiHeart
            // 
            btnEmojiHeart.Location = new Point(284, 867);
            btnEmojiHeart.Margin = new Padding(4, 4, 4, 4);
            btnEmojiHeart.Name = "btnEmojiHeart";
            btnEmojiHeart.Size = new Size(96, 31);
            btnEmojiHeart.TabIndex = 8;
            btnEmojiHeart.Text = "하트";
            btnEmojiHeart.UseVisualStyleBackColor = true;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(532, 913);
            Controls.Add(btnEmojiHeart);
            Controls.Add(btnEmojiCrying);
            Controls.Add(btnEmojiSmiley);
            Controls.Add(btnSendFile);
            Controls.Add(btnSearch);
            Controls.Add(txtSearch);
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
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnSendFile;
        private Button btnEmojiSmiley;
        private Button btnEmojiCrying;
        private Button btnEmojiHeart;
    }
}