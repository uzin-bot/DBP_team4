using Org.BouncyCastle.Asn1.Crmf;
using static System.Net.Mime.MediaTypeNames;

namespace leehaeun
{
    partial class SearchUserForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            UserScrollPanel = new Panel();
            AddButton = new Button();
            SuspendLayout();
            // 
            // UserScrollPanel
            // 
            UserScrollPanel.AutoScroll = true;
            UserScrollPanel.Location = new Point(12, 12);
            UserScrollPanel.Name = "UserScrollPanel";
            UserScrollPanel.Size = new Size(440, 290);
            UserScrollPanel.TabIndex = 0;
            // 
            // AddButton
            // 
            AddButton.Location = new Point(12, 315);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(440, 38);
            AddButton.TabIndex = 1;
            AddButton.Text = "추가";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // SearchUserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 450);
            Controls.Add(AddButton);
            Controls.Add(UserScrollPanel);
            Name = "SearchUserForm";
            Text = "SearchUserForm";
            ResumeLayout(false);
        }

        #endregion

        private Panel UserScrollPanel;
        private Button AddButton;
    }
}
