namespace ПП._2
{
    partial class Registrationcs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Registrationcs));
            this.btnRegisterUser = new System.Windows.Forms.Button();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.labelRegRole = new System.Windows.Forms.Label();
            this.txtRegEmail = new System.Windows.Forms.TextBox();
            this.labelRegEmail = new System.Windows.Forms.Label();
            this.txtRegPassword = new System.Windows.Forms.TextBox();
            this.labelRegPassword = new System.Windows.Forms.Label();
            this.txtRegLogin = new System.Windows.Forms.TextBox();
            this.labelRegLogin = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRegisterUser
            // 
            this.btnRegisterUser.Location = new System.Drawing.Point(186, 310);
            this.btnRegisterUser.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnRegisterUser.Name = "btnRegisterUser";
            this.btnRegisterUser.Size = new System.Drawing.Size(220, 37);
            this.btnRegisterUser.TabIndex = 17;
            this.btnRegisterUser.Text = "Зарегистрировать";
            this.btnRegisterUser.UseVisualStyleBackColor = true;
            this.btnRegisterUser.Click += new System.EventHandler(this.btnRegisterUser_Click);
            // 
            // cmbRole
            // 
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(186, 240);
            this.cmbRole.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(327, 29);
            this.cmbRole.TabIndex = 16;
            // 
            // labelRegRole
            // 
            this.labelRegRole.AutoSize = true;
            this.labelRegRole.Location = new System.Drawing.Point(58, 245);
            this.labelRegRole.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRegRole.Name = "labelRegRole";
            this.labelRegRole.Size = new System.Drawing.Size(56, 21);
            this.labelRegRole.TabIndex = 15;
            this.labelRegRole.Text = "Роль:";
            // 
            // txtRegEmail
            // 
            this.txtRegEmail.Location = new System.Drawing.Point(186, 176);
            this.txtRegEmail.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtRegEmail.Name = "txtRegEmail";
            this.txtRegEmail.Size = new System.Drawing.Size(327, 30);
            this.txtRegEmail.TabIndex = 14;
            // 
            // labelRegEmail
            // 
            this.labelRegEmail.AutoSize = true;
            this.labelRegEmail.Location = new System.Drawing.Point(58, 180);
            this.labelRegEmail.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRegEmail.Name = "labelRegEmail";
            this.labelRegEmail.Size = new System.Drawing.Size(60, 21);
            this.labelRegEmail.TabIndex = 13;
            this.labelRegEmail.Text = "Email:";
            // 
            // txtRegPassword
            // 
            this.txtRegPassword.Location = new System.Drawing.Point(186, 110);
            this.txtRegPassword.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtRegPassword.Name = "txtRegPassword";
            this.txtRegPassword.Size = new System.Drawing.Size(327, 30);
            this.txtRegPassword.TabIndex = 12;
            // 
            // labelRegPassword
            // 
            this.labelRegPassword.AutoSize = true;
            this.labelRegPassword.Location = new System.Drawing.Point(58, 116);
            this.labelRegPassword.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRegPassword.Name = "labelRegPassword";
            this.labelRegPassword.Size = new System.Drawing.Size(81, 21);
            this.labelRegPassword.TabIndex = 11;
            this.labelRegPassword.Text = "Пароль:";
            // 
            // txtRegLogin
            // 
            this.txtRegLogin.Location = new System.Drawing.Point(186, 46);
            this.txtRegLogin.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtRegLogin.Name = "txtRegLogin";
            this.txtRegLogin.Size = new System.Drawing.Size(327, 30);
            this.txtRegLogin.TabIndex = 10;
            // 
            // labelRegLogin
            // 
            this.labelRegLogin.AutoSize = true;
            this.labelRegLogin.Location = new System.Drawing.Point(58, 51);
            this.labelRegLogin.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRegLogin.Name = "labelRegLogin";
            this.labelRegLogin.Size = new System.Drawing.Size(69, 21);
            this.labelRegLogin.TabIndex = 9;
            this.labelRegLogin.Text = "Логин:";
            // 
            // Registrationcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 376);
            this.Controls.Add(this.btnRegisterUser);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.labelRegRole);
            this.Controls.Add(this.txtRegEmail);
            this.Controls.Add(this.labelRegEmail);
            this.Controls.Add(this.txtRegPassword);
            this.Controls.Add(this.labelRegPassword);
            this.Controls.Add(this.txtRegLogin);
            this.Controls.Add(this.labelRegLogin);
            this.Font = new System.Drawing.Font("Century Gothic", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Registrationcs";
            this.Text = "Регистрация";
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
    }
}