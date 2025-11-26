namespace leehaeun
{
    partial class EditUserInfoForm
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            IdBox = new TextBox();
            PwBox = new TextBox();
            NameBox = new TextBox();
            ZipCodeBox = new TextBox();
            AddressBox = new TextBox();
            DeptBox = new TextBox();
            SearchAddressButton = new Button();
            SaveInfoButton = new Button();
            CancelButton = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 20);
            label1.Name = "label1";
            label1.Size = new Size(43, 15);
            label1.TabIndex = 0;
            label1.Text = "아이디";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 64);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 1;
            label2.Text = "비밀번호";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 108);
            label3.Name = "label3";
            label3.Size = new Size(31, 15);
            label3.TabIndex = 2;
            label3.Text = "이름";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 152);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 3;
            label4.Text = "주소";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 224);
            label5.Name = "label5";
            label5.Size = new Size(59, 15);
            label5.TabIndex = 4;
            label5.Text = "소속 부서";
            // 
            // IdBox
            // 
            IdBox.Location = new Point(12, 38);
            IdBox.Name = "IdBox";
            IdBox.ReadOnly = true;
            IdBox.Size = new Size(166, 23);
            IdBox.TabIndex = 5;
            // 
            // PwBox
            // 
            PwBox.Location = new Point(12, 82);
            PwBox.Name = "PwBox";
            PwBox.PasswordChar = '*';
            PwBox.Size = new Size(166, 23);
            PwBox.TabIndex = 6;
            // 
            // NameBox
            // 
            NameBox.Location = new Point(12, 126);
            NameBox.Name = "NameBox";
            NameBox.Size = new Size(166, 23);
            NameBox.TabIndex = 7;
            // 
            // ZipCodeBox
            // 
            ZipCodeBox.Location = new Point(12, 170);
            ZipCodeBox.Name = "ZipCodeBox";
            ZipCodeBox.ReadOnly = true;
            ZipCodeBox.Size = new Size(98, 23);
            ZipCodeBox.TabIndex = 8;
            // 
            // AddressBox
            // 
            AddressBox.Location = new Point(12, 198);
            AddressBox.Name = "AddressBox";
            AddressBox.ReadOnly = true;
            AddressBox.Size = new Size(166, 23);
            AddressBox.TabIndex = 9;
            // 
            // DeptBox
            // 
            DeptBox.Location = new Point(12, 242);
            DeptBox.Name = "DeptBox";
            DeptBox.ReadOnly = true;
            DeptBox.Size = new Size(166, 23);
            DeptBox.TabIndex = 10;
            // 
            // SearchAddressButton
            // 
            SearchAddressButton.Location = new Point(116, 169);
            SearchAddressButton.Name = "SearchAddressButton";
            SearchAddressButton.Size = new Size(62, 23);
            SearchAddressButton.TabIndex = 11;
            SearchAddressButton.Text = "검색";
            SearchAddressButton.UseVisualStyleBackColor = true;
            SearchAddressButton.Click += SearchAddressButton_Click;
            // 
            // SaveInfoButton
            // 
            SaveInfoButton.Location = new Point(12, 280);
            SaveInfoButton.Name = "SaveInfoButton";
            SaveInfoButton.Size = new Size(120, 23);
            SaveInfoButton.TabIndex = 12;
            SaveInfoButton.Text = "변경 내용 저장";
            SaveInfoButton.UseVisualStyleBackColor = true;
            SaveInfoButton.Click += SaveInfoButton_Click;
            // 
            // CancelButton
            // 
            CancelButton.Location = new Point(138, 280);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(40, 23);
            CancelButton.TabIndex = 13;
            CancelButton.Text = "취소";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // UserInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(190, 318);
            Controls.Add(CancelButton);
            Controls.Add(SaveInfoButton);
            Controls.Add(SearchAddressButton);
            Controls.Add(DeptBox);
            Controls.Add(AddressBox);
            Controls.Add(ZipCodeBox);
            Controls.Add(NameBox);
            Controls.Add(PwBox);
            Controls.Add(IdBox);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "UserInfo";
            Text = "UserInfo";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox IdBox;
        private TextBox PwBox;
        private TextBox NameBox;
        private TextBox ZipCodeBox;
        private TextBox AddressBox;
        private TextBox DeptBox;
        private Button SearchAddressButton;
        private Button SaveInfoButton;
        private Button CancelButton;
    }
}