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
            this.btnAddLineup = new System.Windows.Forms.Button();
            this.btnDeleteLineup = new System.Windows.Forms.Button();
            this.btnListLineups = new System.Windows.Forms.Button();
            this.btnGetLineup = new System.Windows.Forms.Button();
            this.btnGetSchedule = new System.Windows.Forms.Button();
            this.btnGetProgram = new System.Windows.Forms.Button();
            this.btnGetMD5 = new System.Windows.Forms.Button();
            this.btnDeleteMsg = new System.Windows.Forms.Button();
            this.btnGetLive = new System.Windows.Forms.Button();
            this.btnGetLogo = new System.Windows.Forms.Button();
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
            this.btnLogin.Location = new System.Drawing.Point(17, 113);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 25);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // rtResult
            // 
            this.rtResult.Location = new System.Drawing.Point(16, 159);
            this.rtResult.Name = "rtResult";
            this.rtResult.Size = new System.Drawing.Size(531, 153);
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
            this.btnCountries.Location = new System.Drawing.Point(462, 82);
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
            this.lbContinents.Location = new System.Drawing.Point(12, 318);
            this.lbContinents.Name = "lbContinents";
            this.lbContinents.Size = new System.Drawing.Size(260, 108);
            this.lbContinents.TabIndex = 8;
            this.lbContinents.SelectedIndexChanged += new System.EventHandler(this.lbContinents_SelectedIndexChanged);
            // 
            // lbCountries
            // 
            this.lbCountries.FormattingEnabled = true;
            this.lbCountries.Location = new System.Drawing.Point(278, 318);
            this.lbCountries.Name = "lbCountries";
            this.lbCountries.Size = new System.Drawing.Size(269, 108);
            this.lbCountries.TabIndex = 9;
            // 
            // btnTransmitters
            // 
            this.btnTransmitters.Location = new System.Drawing.Point(370, 82);
            this.btnTransmitters.Name = "btnTransmitters";
            this.btnTransmitters.Size = new System.Drawing.Size(86, 25);
            this.btnTransmitters.TabIndex = 10;
            this.btnTransmitters.Text = "Transmitters";
            this.btnTransmitters.UseVisualStyleBackColor = true;
            this.btnTransmitters.Click += new System.EventHandler(this.btnTransmitters_Click);
            // 
            // btnHeadends
            // 
            this.btnHeadends.Location = new System.Drawing.Point(370, 51);
            this.btnHeadends.Name = "btnHeadends";
            this.btnHeadends.Size = new System.Drawing.Size(86, 25);
            this.btnHeadends.TabIndex = 11;
            this.btnHeadends.Text = "Headends";
            this.btnHeadends.UseVisualStyleBackColor = true;
            this.btnHeadends.Click += new System.EventHandler(this.btnHeadends_Click);
            // 
            // btnAddLineup
            // 
            this.btnAddLineup.Location = new System.Drawing.Point(462, 51);
            this.btnAddLineup.Name = "btnAddLineup";
            this.btnAddLineup.Size = new System.Drawing.Size(86, 25);
            this.btnAddLineup.TabIndex = 12;
            this.btnAddLineup.Text = "Add Lineup";
            this.btnAddLineup.UseVisualStyleBackColor = true;
            this.btnAddLineup.Click += new System.EventHandler(this.btnAddLineup_Click);
            // 
            // btnDeleteLineup
            // 
            this.btnDeleteLineup.Location = new System.Drawing.Point(462, 20);
            this.btnDeleteLineup.Name = "btnDeleteLineup";
            this.btnDeleteLineup.Size = new System.Drawing.Size(86, 25);
            this.btnDeleteLineup.TabIndex = 13;
            this.btnDeleteLineup.Text = "Delete Lineup";
            this.btnDeleteLineup.UseVisualStyleBackColor = true;
            // 
            // btnListLineups
            // 
            this.btnListLineups.Location = new System.Drawing.Point(370, 20);
            this.btnListLineups.Name = "btnListLineups";
            this.btnListLineups.Size = new System.Drawing.Size(86, 25);
            this.btnListLineups.TabIndex = 14;
            this.btnListLineups.Text = "List Lineup";
            this.btnListLineups.UseVisualStyleBackColor = true;
            this.btnListLineups.Click += new System.EventHandler(this.btnListLineups_Click);
            // 
            // btnGetLineup
            // 
            this.btnGetLineup.Location = new System.Drawing.Point(278, 20);
            this.btnGetLineup.Name = "btnGetLineup";
            this.btnGetLineup.Size = new System.Drawing.Size(86, 25);
            this.btnGetLineup.TabIndex = 15;
            this.btnGetLineup.Text = "Get Lineup";
            this.btnGetLineup.UseVisualStyleBackColor = true;
            this.btnGetLineup.Click += new System.EventHandler(this.btnGetLineup_Click);
            // 
            // btnGetSchedule
            // 
            this.btnGetSchedule.Location = new System.Drawing.Point(278, 51);
            this.btnGetSchedule.Name = "btnGetSchedule";
            this.btnGetSchedule.Size = new System.Drawing.Size(86, 25);
            this.btnGetSchedule.TabIndex = 16;
            this.btnGetSchedule.Text = "Get Schedule";
            this.btnGetSchedule.UseVisualStyleBackColor = true;
            this.btnGetSchedule.Click += new System.EventHandler(this.btnGetSchedule_Click);
            // 
            // btnGetProgram
            // 
            this.btnGetProgram.Location = new System.Drawing.Point(278, 82);
            this.btnGetProgram.Name = "btnGetProgram";
            this.btnGetProgram.Size = new System.Drawing.Size(86, 25);
            this.btnGetProgram.TabIndex = 17;
            this.btnGetProgram.Text = "Get Program";
            this.btnGetProgram.UseVisualStyleBackColor = true;
            this.btnGetProgram.Click += new System.EventHandler(this.btnGetProgram_Click);
            // 
            // btnGetMD5
            // 
            this.btnGetMD5.Location = new System.Drawing.Point(278, 113);
            this.btnGetMD5.Name = "btnGetMD5";
            this.btnGetMD5.Size = new System.Drawing.Size(86, 25);
            this.btnGetMD5.TabIndex = 18;
            this.btnGetMD5.Text = "Get MD5";
            this.btnGetMD5.UseVisualStyleBackColor = true;
            this.btnGetMD5.Click += new System.EventHandler(this.btnGetMD5_Click);
            // 
            // btnDeleteMsg
            // 
            this.btnDeleteMsg.Location = new System.Drawing.Point(370, 113);
            this.btnDeleteMsg.Name = "btnDeleteMsg";
            this.btnDeleteMsg.Size = new System.Drawing.Size(86, 25);
            this.btnDeleteMsg.TabIndex = 19;
            this.btnDeleteMsg.Text = "Delete Message";
            this.btnDeleteMsg.UseVisualStyleBackColor = true;
            this.btnDeleteMsg.Click += new System.EventHandler(this.btnDeleteMsg_Click);
            // 
            // btnGetLive
            // 
            this.btnGetLive.Location = new System.Drawing.Point(461, 113);
            this.btnGetLive.Name = "btnGetLive";
            this.btnGetLive.Size = new System.Drawing.Size(86, 25);
            this.btnGetLive.TabIndex = 20;
            this.btnGetLive.Text = "Get Live Info";
            this.btnGetLive.UseVisualStyleBackColor = true;
            this.btnGetLive.Click += new System.EventHandler(this.btnGetLive_Click);
            // 
            // btnGetLogo
            // 
            this.btnGetLogo.Location = new System.Drawing.Point(186, 113);
            this.btnGetLogo.Name = "btnGetLogo";
            this.btnGetLogo.Size = new System.Drawing.Size(86, 25);
            this.btnGetLogo.TabIndex = 21;
            this.btnGetLogo.Text = "Get Logo info";
            this.btnGetLogo.UseVisualStyleBackColor = true;
            this.btnGetLogo.Click += new System.EventHandler(this.btnGetLogo_Click);
            // 
            // formUIDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 441);
            this.Controls.Add(this.btnGetLogo);
            this.Controls.Add(this.btnGetLive);
            this.Controls.Add(this.btnDeleteMsg);
            this.Controls.Add(this.btnGetMD5);
            this.Controls.Add(this.btnGetProgram);
            this.Controls.Add(this.btnGetSchedule);
            this.Controls.Add(this.btnGetLineup);
            this.Controls.Add(this.btnListLineups);
            this.Controls.Add(this.btnDeleteLineup);
            this.Controls.Add(this.btnAddLineup);
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
        private System.Windows.Forms.Button btnAddLineup;
        private System.Windows.Forms.Button btnDeleteLineup;
        private System.Windows.Forms.Button btnListLineups;
        private System.Windows.Forms.Button btnGetLineup;
        private System.Windows.Forms.Button btnGetSchedule;
        private System.Windows.Forms.Button btnGetProgram;
        private System.Windows.Forms.Button btnGetMD5;
        private System.Windows.Forms.Button btnDeleteMsg;
        private System.Windows.Forms.Button btnGetLive;
        private System.Windows.Forms.Button btnGetLogo;
    }
}

