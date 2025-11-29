using Org.BouncyCastle.Asn1.Crmf;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace leehaeun
{
    partial class SearchAddressForm
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
            AddressBox = new TextBox();
            SearchButton = new Button();
            ResultBox = new ListBox();
            SuspendLayout();
            // 
            // AddressBox
            // 
            AddressBox.Location = new Point(45, 44);
            AddressBox.Name = "AddressBox";
            AddressBox.Size = new Size(406, 23);
            AddressBox.TabIndex = 0;
            AddressBox.Text = "주소 입력";
            // 
            // SearchButton
            // 
            SearchButton.Location = new Point(486, 44);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(75, 23);
            SearchButton.TabIndex = 1;
            SearchButton.Text = "검색";
            SearchButton.UseVisualStyleBackColor = true;
            SearchButton.Click += SearchButton_Click;
            // 
            // ResultBox
            // 
            ResultBox.FormattingEnabled = true;
            ResultBox.ItemHeight = 15;
            ResultBox.Location = new Point(45, 88);
            ResultBox.Name = "ResultBox";
            ResultBox.Size = new Size(516, 244);
            ResultBox.TabIndex = 2;
            // 
            // SearchAddress
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(605, 376);
            Controls.Add(ResultBox);
            Controls.Add(SearchButton);
            Controls.Add(AddressBox);
            Name = "SearchAddress";
            Text = "SearchAddress";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox AddressBox;
        private Button SearchButton;
        private ListBox ResultBox;
    }
}