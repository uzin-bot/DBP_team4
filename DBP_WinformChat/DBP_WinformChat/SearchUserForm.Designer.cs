using Org.BouncyCastle.Asn1.Crmf;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace leehaeun
{
    partial class SearchUserForm
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
            UserListView = new ListView();
            AddButton = new Button();
            SuspendLayout();
            // 
            // UserListView
            // 
            UserListView.CheckBoxes = true;
            UserListView.FullRowSelect = true;
            UserListView.Location = new Point(12, 12);
            UserListView.Name = "UserListView";
            UserListView.Size = new Size(230, 333);
            UserListView.TabIndex = 4;
            UserListView.UseCompatibleStateImageBehavior = false;
            UserListView.View = View.Details;
            // 
            // AddButton
            // 
            AddButton.Location = new Point(12, 350);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(231, 23);
            AddButton.TabIndex = 5;
            AddButton.Text = "추가";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // SearchUserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(255, 385);
            Controls.Add(AddButton);
            Controls.Add(UserListView);
            Name = "SearchUserForm";
            Text = "SearchUserForm";
            ResumeLayout(false);
        }

        #endregion
        private ListView UserListView;
        private Button AddButton;
    }
}