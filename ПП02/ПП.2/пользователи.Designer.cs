namespace ПП._2
{
    partial class UserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserForm));
            this.btnRegisterUser = new System.Windows.Forms.Button();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.labelRegRole = new System.Windows.Forms.Label();
            this.txtRegEmail = new System.Windows.Forms.TextBox();
            this.labelRegEmail = new System.Windows.Forms.Label();
            this.txtRegPassword = new System.Windows.Forms.TextBox();
            this.labelRegPassword = new System.Windows.Forms.Label();
            this.txtRegLogin = new System.Windows.Forms.TextBox();
            this.labelRegLogin = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRegisterUser
            // 
            this.btnRegisterUser.Location = new System.Drawing.Point(36, 332);
            this.btnRegisterUser.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.btnRegisterUser.Name = "btnRegisterUser";
            this.btnRegisterUser.Size = new System.Drawing.Size(240, 49);
            this.btnRegisterUser.TabIndex = 26;
            this.btnRegisterUser.Text = "Сохранить";
            this.btnRegisterUser.UseVisualStyleBackColor = true;
            this.btnRegisterUser.Click += new System.EventHandler(this.btnRegisterUser_Click);
            // 
            // cmbRole
            // 
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(172, 267);
            this.cmbRole.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(356, 31);
            this.cmbRole.TabIndex = 25;
            // 
            // labelRegRole
            // 
            this.labelRegRole.AutoSize = true;
            this.labelRegRole.Location = new System.Drawing.Point(32, 273);
            this.labelRegRole.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.labelRegRole.Name = "labelRegRole";
            this.labelRegRole.Size = new System.Drawing.Size(60, 23);
            this.labelRegRole.TabIndex = 24;
            this.labelRegRole.Text = "Роль:";
            // 
            // txtRegEmail
            // 
            this.txtRegEmail.Location = new System.Drawing.Point(172, 197);
            this.txtRegEmail.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.txtRegEmail.Name = "txtRegEmail";
            this.txtRegEmail.Size = new System.Drawing.Size(356, 32);
            this.txtRegEmail.TabIndex = 23;
            // 
            // labelRegEmail
            // 
            this.labelRegEmail.AutoSize = true;
            this.labelRegEmail.Location = new System.Drawing.Point(32, 202);
            this.labelRegEmail.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.labelRegEmail.Name = "labelRegEmail";
            this.labelRegEmail.Size = new System.Drawing.Size(67, 23);
            this.labelRegEmail.TabIndex = 22;
            this.labelRegEmail.Text = "Email:";
            // 
            // txtRegPassword
            // 
            this.txtRegPassword.Location = new System.Drawing.Point(172, 125);
            this.txtRegPassword.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.txtRegPassword.Name = "txtRegPassword";
            this.txtRegPassword.Size = new System.Drawing.Size(356, 32);
            this.txtRegPassword.TabIndex = 21;
            // 
            // labelRegPassword
            // 
            this.labelRegPassword.AutoSize = true;
            this.labelRegPassword.Location = new System.Drawing.Point(32, 130);
            this.labelRegPassword.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.labelRegPassword.Name = "labelRegPassword";
            this.labelRegPassword.Size = new System.Drawing.Size(90, 23);
            this.labelRegPassword.TabIndex = 20;
            this.labelRegPassword.Text = "Пароль:";
            // 
            // txtRegLogin
            // 
            this.txtRegLogin.Location = new System.Drawing.Point(172, 60);
            this.txtRegLogin.Margin = new System.Windows.Forms.Padding(7, 5, 7, 5);
            this.txtRegLogin.Name = "txtRegLogin";
            this.txtRegLogin.Size = new System.Drawing.Size(356, 32);
            this.txtRegLogin.TabIndex = 19;
            // 
            // labelRegLogin
            // 
            this.labelRegLogin.AutoSize = true;
            this.labelRegLogin.Location = new System.Drawing.Point(32, 60);
            this.labelRegLogin.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.labelRegLogin.Name = "labelRegLogin";
            this.labelRegLogin.Size = new System.Drawing.Size(72, 23);
            this.labelRegLogin.TabIndex = 18;
            this.labelRegLogin.Text = "Логин:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(288, 332);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(240, 49);
            this.button1.TabIndex = 27;
            this.button1.Text = "Отмена";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 413);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnRegisterUser);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.labelRegRole);
            this.Controls.Add(this.txtRegEmail);
            this.Controls.Add(this.labelRegEmail);
            this.Controls.Add(this.txtRegPassword);
            this.Controls.Add(this.labelRegPassword);
            this.Controls.Add(this.txtRegLogin);
            this.Controls.Add(this.labelRegLogin);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UserForm";
            this.Text = "пользователи";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRegisterUser;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Label labelRegRole;
        private System.Windows.Forms.TextBox txtRegEmail;
        private System.Windows.Forms.Label labelRegEmail;
        private System.Windows.Forms.TextBox txtRegPassword;
        private System.Windows.Forms.Label labelRegPassword;
        private System.Windows.Forms.TextBox txtRegLogin;
        private System.Windows.Forms.Label labelRegLogin;
        private System.Windows.Forms.Button button1;
    }
}