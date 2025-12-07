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
            ResultBox = new ListBox();
            SelectButton = new Button();
            SuspendLayout();
            // 
            // AddressBox
            // 
            AddressBox.Location = new Point(45, 44);
            AddressBox.Name = "AddressBox";
            AddressBox.Size = new Size(515, 23);
            AddressBox.TabIndex = 0;
            AddressBox.Text = "주소 입력";
            // 
            // ResultBox
            // 
            ResultBox.FormattingEnabled = true;
            ResultBox.ItemHeight = 15;
            ResultBox.Location = new Point(45, 88);
            ResultBox.Name = "ResultBox";
            ResultBox.Size = new Size(515, 244);
            ResultBox.TabIndex = 1;
            // 
            // SelectButton
            // 
            SelectButton.Location = new Point(45, 340);
            SelectButton.Name = "SelectButton";
            SelectButton.Size = new Size(515, 40);
            SelectButton.TabIndex = 2;
            SelectButton.Text = "선택";
            SelectButton.UseVisualStyleBackColor = true;
            SelectButton.Click += SelectButton_Click;
            // 
            // SearchAddress
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(605, 400);
            Controls.Add(SelectButton);
            Controls.Add(ResultBox);
            Controls.Add(AddressBox);
            Name = "SearchAddress";
            Text = "SearchAddress";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox AddressBox;
        private ListBox ResultBox;
        private Button SelectButton;
    }
}