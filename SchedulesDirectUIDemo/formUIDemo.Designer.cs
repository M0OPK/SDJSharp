namespace SchedulesDirect.UIDemo
{
    partial class formUIDemo
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
            this.lblLogin = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.rtResult = new System.Windows.Forms.RichTextBox();
            this.btnServices = new System.Windows.Forms.Button();
            this.btnCountries = new System.Windows.Forms.Button();
            this.lbContinents = new System.Windows.Forms.ListBox();
            this.lbCountries = new System.Windows.Forms.ListBox();
            this.btnTransmitters = new System.Windows.Forms.Button();
            this.btnHeadends = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(13, 31);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(64, 13);
            this.lblLogin.TabIndex = 0;
            this.lblLogin.Text = "Login Name";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(13, 59);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password";
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(84, 28);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(188, 20);
            this.txtLogin.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(84, 56);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(188, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(197, 82);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 25);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // rtResult
            // 
            this.rtResult.Location = new System.Drawing.Point(16, 119);
            this.rtResult.Name = "rtResult";
            this.rtResult.Size = new System.Drawing.Size(522, 153);
            this.rtResult.TabIndex = 5;
            this.rtResult.Text = "";
            // 
            // btnServices
            // 
            this.btnServices.Location = new System.Drawing.Point(16, 82);
            this.btnServices.Name = "btnServices";
            this.btnServices.Size = new System.Drawing.Size(76, 25);
            this.btnServices.TabIndex = 6;
            this.btnServices.Text = "Services";
            this.btnServices.UseVisualStyleBackColor = true;
            this.btnServices.Click += new System.EventHandler(this.btnServices_Click);
            // 
            // btnCountries
            // 
            this.btnCountries.Location = new System.Drawing.Point(452, 82);
            this.btnCountries.Name = "btnCountries";
            this.btnCountries.Size = new System.Drawing.Size(86, 25);
            this.btnCountries.TabIndex = 7;
            this.btnCountries.Text = "Countries";
            this.btnCountries.UseVisualStyleBackColor = true;
            this.btnCountries.Click += new System.EventHandler(this.btnCountries_Click);
            // 
            // lbContinents
            // 
            this.lbContinents.FormattingEnabled = true;
            this.lbContinents.Location = new System.Drawing.Point(12, 278);
            this.lbContinents.Name = "lbContinents";
            this.lbContinents.Size = new System.Drawing.Size(260, 108);
            this.lbContinents.TabIndex = 8;
            this.lbContinents.SelectedIndexChanged += new System.EventHandler(this.lbContinents_SelectedIndexChanged);
            // 
            // lbCountries
            // 
            this.lbCountries.FormattingEnabled = true;
            this.lbCountries.Location = new System.Drawing.Point(278, 278);
            this.lbCountries.Name = "lbCountries";
            this.lbCountries.Size = new System.Drawing.Size(260, 108);
            this.lbCountries.TabIndex = 9;
            // 
            // btnTransmitters
            // 
            this.btnTransmitters.Location = new System.Drawing.Point(360, 82);
            this.btnTransmitters.Name = "btnTransmitters";
            this.btnTransmitters.Size = new System.Drawing.Size(86, 25);
            this.btnTransmitters.TabIndex = 10;
            this.btnTransmitters.Text = "Transmitters";
            this.btnTransmitters.UseVisualStyleBackColor = true;
            this.btnTransmitters.Click += new System.EventHandler(this.btnTransmitters_Click);
            // 
            // btnHeadends
            // 
            this.btnHeadends.Location = new System.Drawing.Point(360, 51);
            this.btnHeadends.Name = "btnHeadends";
            this.btnHeadends.Size = new System.Drawing.Size(86, 25);
            this.btnHeadends.TabIndex = 11;
            this.btnHeadends.Text = "Headends";
            this.btnHeadends.UseVisualStyleBackColor = true;
            this.btnHeadends.Click += new System.EventHandler(this.btnHeadends_Click);
            // 
            // formUIDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 411);
            this.Controls.Add(this.btnHeadends);
            this.Controls.Add(this.btnTransmitters);
            this.Controls.Add(this.lbCountries);
            this.Controls.Add(this.lbContinents);
            this.Controls.Add(this.btnCountries);
            this.Controls.Add(this.btnServices);
            this.Controls.Add(this.rtResult);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblLogin);
            this.Name = "formUIDemo";
            this.Text = "SchedulesDirect.org Demo UI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.RichTextBox rtResult;
        private System.Windows.Forms.Button btnServices;
        private System.Windows.Forms.Button btnCountries;
        private System.Windows.Forms.ListBox lbContinents;
        private System.Windows.Forms.ListBox lbCountries;
        private System.Windows.Forms.Button btnTransmitters;
        private System.Windows.Forms.Button btnHeadends;
    }
}

