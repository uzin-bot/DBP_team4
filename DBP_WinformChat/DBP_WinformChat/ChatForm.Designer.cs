using DBPAdmin;

namespace kyg
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
            rtbChatLog.Location = new Point(40, 108);
            rtbChatLog.Margin = new Padding(4, 4, 4, 4);
            rtbChatLog.Name = "rtbChatLog";
            rtbChatLog.Size = new Size(617, 1070);
            rtbChatLog.TabIndex = 0;
            rtbChatLog.Text = "";
            // 
            // txtInput
            // 
            txtInput.Location = new Point(40, 1202);
            txtInput.Margin = new Padding(4, 4, 4, 4);
            txtInput.Multiline = true;
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(465, 64);
            txtInput.TabIndex = 1;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(535, 1202);
            btnSend.Margin = new Padding(4, 4, 4, 4);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(125, 66);
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
            txtSearch.Location = new Point(40, 24);
            txtSearch.Margin = new Padding(5, 6, 5, 6);
            txtSearch.Multiline = true;
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(465, 64);
            txtSearch.TabIndex = 3;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(532, 24);
            btnSearch.Margin = new Padding(5, 6, 5, 6);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(128, 68);
            btnSearch.TabIndex = 4;
            btnSearch.Text = "검색";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnSendFile
            // 
            btnSendFile.Location = new Point(535, 1298);
            btnSendFile.Margin = new Padding(5, 6, 5, 6);
            btnSendFile.Name = "btnSendFile";
            btnSendFile.Size = new Size(125, 48);
            btnSendFile.TabIndex = 5;
            btnSendFile.Text = "파일전송";
            btnSendFile.UseVisualStyleBackColor = true;
            // 
            // btnEmojiSmiley
            // 
            btnEmojiSmiley.Location = new Point(40, 1298);
            btnEmojiSmiley.Margin = new Padding(5, 6, 5, 6);
            btnEmojiSmiley.Name = "btnEmojiSmiley";
            btnEmojiSmiley.Size = new Size(128, 46);
            btnEmojiSmiley.TabIndex = 6;
            btnEmojiSmiley.Text = "웃음";
            btnEmojiSmiley.UseVisualStyleBackColor = true;
            // 
            // btnEmojiCrying
            // 
            btnEmojiCrying.Location = new Point(211, 1300);
            btnEmojiCrying.Margin = new Padding(5, 6, 5, 6);
            btnEmojiCrying.Name = "btnEmojiCrying";
            btnEmojiCrying.Size = new Size(128, 46);
            btnEmojiCrying.TabIndex = 7;
            btnEmojiCrying.Text = "슬픔";
            btnEmojiCrying.UseVisualStyleBackColor = true;
            // 
            // btnEmojiHeart
            // 
            btnEmojiHeart.Location = new Point(379, 1300);
            btnEmojiHeart.Margin = new Padding(5, 6, 5, 6);
            btnEmojiHeart.Name = "btnEmojiHeart";
            btnEmojiHeart.Size = new Size(128, 46);
            btnEmojiHeart.TabIndex = 8;
            btnEmojiHeart.Text = "하트";
            btnEmojiHeart.UseVisualStyleBackColor = true;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(709, 1370);
            Controls.Add(btnEmojiHeart);
            Controls.Add(btnEmojiCrying);
            Controls.Add(btnEmojiSmiley);
            Controls.Add(btnSendFile);
            Controls.Add(btnSearch);
            Controls.Add(txtSearch);
            Controls.Add(btnSend);
            Controls.Add(txtInput);
            Controls.Add(rtbChatLog);
            Margin = new Padding(4, 4, 4, 4);
            Name = "ChatForm";
            Text = "ChatForm";
            ResumeLayout(false);
            PerformLayout();
            // ----------------------------------------------------------------

            // ChatForm.Designer.cs 파일의 InitializeComponent() 메서드 내

            // ----------------------------------------------------------------
            // ChatFormUIHelper를 사용하여 스타일 및 둥근 모서리 적용
            // ----------------------------------------------------------------

            // 폼 배경에 가장 밝은 색(ColorLightest)을 적용합니다.
            ChatFormUIHelper.ApplyFormStyle(this);

            // RichTextBox 배경에 중간 밝은 색(ColorLight)을 적용합니다.
            ChatFormUIHelper.ApplyDisplayStyle(this.rtbChatLog);

            // TextBox 스타일 적용 및 색상 설정
            // ApplyInputStyle 내부에서 둥근 모서리 적용 코드는 제거해야 합니다. (아래 2번 참고)
            ChatFormUIHelper.ApplyInputStyle(this.txtInput);
            ChatFormUIHelper.ApplyInputStyle(this.txtSearch);

            // 버튼 스타일 및 흰색 텍스트 적용 (둥근 모서리 적용 코드는 제거해야 합니다. 아래 2번 참고)
            ChatFormUIHelper.ApplyButtonStyle(this.btnSend);
            ChatFormUIHelper.ApplyButtonStyle(this.btnSearch);
            ChatFormUIHelper.ApplyButtonStyle(this.btnSendFile);
            ChatFormUIHelper.ApplyButtonStyle(this.btnEmojiSmiley);
            ChatFormUIHelper.ApplyButtonStyle(this.btnEmojiCrying);
            ChatFormUIHelper.ApplyButtonStyle(this.btnEmojiHeart);
            // ----------------------------------------------------------------

            /*

            // ChatForm.Designer.cs 파일의 InitializeComponent() 메서드 내

            // ----------------------------------------------------------------
            // ChatFormUIHelper를 사용하여 스타일 및 둥근 모서리 적용
            // ----------------------------------------------------------------

            // 1. 폼 배경에 중간 밝은 색(ColorLight)을 적용합니다.
            
            ChatFormUIHelper.ApplyDisplayStyle(this);

            // 2. RichTextBox 배경에 아주 밝은 색(ColorLightest)을 적용합니다.
            // (ApplyLightestStyle은 Control을 받으므로 RichTextBox에 사용 가능합니다.)
            ChatFormUIHelper.ApplyLightestStyle(this.rtbChatLog);

            // TextBox 스타일 적용 및 둥근 모서리 적용
            ChatFormUIHelper.ApplyInputStyle(this.txtInput);
            ChatFormUIHelper.ApplyInputStyle(this.txtSearch);

            // 버튼 스타일 및 둥근 모서리, 흰색 텍스트 적용
            ChatFormUIHelper.ApplyButtonStyle(this.btnSend);
            ChatFormUIHelper.ApplyButtonStyle(this.btnSearch);
            ChatFormUIHelper.ApplyButtonStyle(this.btnSendFile);
            ChatFormUIHelper.ApplyButtonStyle(this.btnEmojiSmiley);
            ChatFormUIHelper.ApplyButtonStyle(this.btnEmojiCrying);
            ChatFormUIHelper.ApplyButtonStyle(this.btnEmojiHeart);
            // ----------------------------------------------------------------
            */


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