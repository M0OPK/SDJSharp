namespace SDGrabSharp.UI
{
    partial class frmOptions
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
            this.tcConfig = new System.Windows.Forms.TabControl();
            this.tpSD1 = new System.Windows.Forms.TabPage();
            this.gbStations = new System.Windows.Forms.GroupBox();
            this.btnStationCacheClear = new System.Windows.Forms.Button();
            this.lvStations = new System.Windows.Forms.ListView();
            this.chStationID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbHeadends = new System.Windows.Forms.GroupBox();
            this.btnHeadendCacheClear = new System.Windows.Forms.Button();
            this.btnLineupAdd = new System.Windows.Forms.Button();
            this.tvHeadends = new System.Windows.Forms.TreeView();
            this.gbAccountLineups = new System.Windows.Forms.GroupBox();
            this.btnDeleteLineup = new System.Windows.Forms.Button();
            this.lvAccountLineups = new System.Windows.Forms.ListView();
            this.chLineup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpLocation = new System.Windows.Forms.GroupBox();
            this.btnLocationCacheClear = new System.Windows.Forms.Button();
            this.btnHeadends = new System.Windows.Forms.Button();
            this.txtPostCode = new System.Windows.Forms.TextBox();
            this.lbPostCode = new System.Windows.Forms.Label();
            this.tvCountries = new System.Windows.Forms.TreeView();
            this.gbCreds = new System.Windows.Forms.GroupBox();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.checkAlwaysAsk = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.lbLogin = new System.Windows.Forms.Label();
            this.tpXMLTV1 = new System.Windows.Forms.TabPage();
            this.gbStationMap = new System.Windows.Forms.GroupBox();
            this.lvAddedChans = new System.Windows.Forms.ListView();
            this.chAddedID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAddedName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rdCustom = new System.Windows.Forms.RadioButton();
            this.txtCustom = new System.Windows.Forms.TextBox();
            this.rdCallsign = new System.Windows.Forms.RadioButton();
            this.rdAffiliate = new System.Windows.Forms.RadioButton();
            this.rdName = new System.Windows.Forms.RadioButton();
            this.rdStation = new System.Windows.Forms.RadioButton();
            this.txtCallsign = new System.Windows.Forms.TextBox();
            this.txtAffiliate = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtStationID = new System.Windows.Forms.TextBox();
            this.btnAddAllChans = new System.Windows.Forms.Button();
            this.btnRemoveAllChans = new System.Windows.Forms.Button();
            this.btnRemoveChan = new System.Windows.Forms.Button();
            this.btnAddChan = new System.Windows.Forms.Button();
            this.lvAvailableChans = new System.Windows.Forms.ListView();
            this.chAvailID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAvailName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblLineup = new System.Windows.Forms.Label();
            this.cbLineup = new System.Windows.Forms.ComboBox();
            this.tpGeneral1 = new System.Windows.Forms.TabPage();
            this.gbPersistentCache = new System.Windows.Forms.GroupBox();
            this.btnBrowseCache = new System.Windows.Forms.Button();
            this.ckPersistentCache = new System.Windows.Forms.CheckBox();
            this.txtCacheFilename = new System.Windows.Forms.TextBox();
            this.lbCacheFilename = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tcConfig.SuspendLayout();
            this.tpSD1.SuspendLayout();
            this.gbStations.SuspendLayout();
            this.gbHeadends.SuspendLayout();
            this.gbAccountLineups.SuspendLayout();
            this.grpLocation.SuspendLayout();
            this.gbCreds.SuspendLayout();
            this.tpXMLTV1.SuspendLayout();
            this.gbStationMap.SuspendLayout();
            this.tpGeneral1.SuspendLayout();
            this.gbPersistentCache.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcConfig
            // 
            this.tcConfig.Controls.Add(this.tpSD1);
            this.tcConfig.Controls.Add(this.tpXMLTV1);
            this.tcConfig.Controls.Add(this.tpGeneral1);
            this.tcConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcConfig.Location = new System.Drawing.Point(0, 0);
            this.tcConfig.Name = "tcConfig";
            this.tcConfig.SelectedIndex = 0;
            this.tcConfig.Size = new System.Drawing.Size(882, 539);
            this.tcConfig.TabIndex = 0;
            // 
            // tpSD1
            // 
            this.tpSD1.BackColor = System.Drawing.SystemColors.Control;
            this.tpSD1.Controls.Add(this.gbStations);
            this.tpSD1.Controls.Add(this.gbHeadends);
            this.tpSD1.Controls.Add(this.gbAccountLineups);
            this.tpSD1.Controls.Add(this.grpLocation);
            this.tpSD1.Controls.Add(this.gbCreds);
            this.tpSD1.Location = new System.Drawing.Point(4, 22);
            this.tpSD1.Name = "tpSD1";
            this.tpSD1.Padding = new System.Windows.Forms.Padding(3);
            this.tpSD1.Size = new System.Drawing.Size(874, 513);
            this.tpSD1.TabIndex = 0;
            this.tpSD1.Text = "Schedules Direct";
            // 
            // gbStations
            // 
            this.gbStations.Controls.Add(this.btnStationCacheClear);
            this.gbStations.Controls.Add(this.lvStations);
            this.gbStations.Location = new System.Drawing.Point(537, 137);
            this.gbStations.Name = "gbStations";
            this.gbStations.Size = new System.Drawing.Size(323, 332);
            this.gbStations.TabIndex = 4;
            this.gbStations.TabStop = false;
            this.gbStations.Text = "Stations";
            // 
            // btnStationCacheClear
            // 
            this.btnStationCacheClear.Enabled = false;
            this.btnStationCacheClear.Location = new System.Drawing.Point(6, 303);
            this.btnStationCacheClear.Name = "btnStationCacheClear";
            this.btnStationCacheClear.Size = new System.Drawing.Size(75, 23);
            this.btnStationCacheClear.TabIndex = 9;
            this.btnStationCacheClear.Text = "Clear Cache";
            this.btnStationCacheClear.UseVisualStyleBackColor = true;
            this.btnStationCacheClear.Click += new System.EventHandler(this.btnStationCacheClear_Click);
            // 
            // lvStations
            // 
            this.lvStations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chStationID,
            this.chName});
            this.lvStations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvStations.Location = new System.Drawing.Point(6, 15);
            this.lvStations.Name = "lvStations";
            this.lvStations.Size = new System.Drawing.Size(310, 282);
            this.lvStations.TabIndex = 0;
            this.lvStations.UseCompatibleStateImageBehavior = false;
            this.lvStations.View = System.Windows.Forms.View.Details;
            // 
            // chStationID
            // 
            this.chStationID.Text = "ID";
            this.chStationID.Width = 73;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 233;
            // 
            // gbHeadends
            // 
            this.gbHeadends.Controls.Add(this.btnHeadendCacheClear);
            this.gbHeadends.Controls.Add(this.btnLineupAdd);
            this.gbHeadends.Controls.Add(this.tvHeadends);
            this.gbHeadends.Location = new System.Drawing.Point(273, 137);
            this.gbHeadends.Name = "gbHeadends";
            this.gbHeadends.Size = new System.Drawing.Size(258, 332);
            this.gbHeadends.TabIndex = 3;
            this.gbHeadends.TabStop = false;
            this.gbHeadends.Text = "Headend/Lineup";
            // 
            // btnHeadendCacheClear
            // 
            this.btnHeadendCacheClear.Enabled = false;
            this.btnHeadendCacheClear.Location = new System.Drawing.Point(6, 303);
            this.btnHeadendCacheClear.Name = "btnHeadendCacheClear";
            this.btnHeadendCacheClear.Size = new System.Drawing.Size(75, 23);
            this.btnHeadendCacheClear.TabIndex = 8;
            this.btnHeadendCacheClear.Text = "Clear Cache";
            this.btnHeadendCacheClear.UseVisualStyleBackColor = true;
            this.btnHeadendCacheClear.Click += new System.EventHandler(this.btnHeadendCacheClear_Click);
            // 
            // btnLineupAdd
            // 
            this.btnLineupAdd.Location = new System.Drawing.Point(177, 303);
            this.btnLineupAdd.Name = "btnLineupAdd";
            this.btnLineupAdd.Size = new System.Drawing.Size(75, 23);
            this.btnLineupAdd.TabIndex = 7;
            this.btnLineupAdd.Text = "Add";
            this.btnLineupAdd.UseVisualStyleBackColor = true;
            this.btnLineupAdd.Click += new System.EventHandler(this.btnLineupAdd_Click);
            // 
            // tvHeadends
            // 
            this.tvHeadends.Location = new System.Drawing.Point(6, 15);
            this.tvHeadends.Name = "tvHeadends";
            this.tvHeadends.Size = new System.Drawing.Size(246, 282);
            this.tvHeadends.TabIndex = 0;
            this.tvHeadends.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvHeadends_AfterSelect);
            // 
            // gbAccountLineups
            // 
            this.gbAccountLineups.Controls.Add(this.btnDeleteLineup);
            this.gbAccountLineups.Controls.Add(this.lvAccountLineups);
            this.gbAccountLineups.Location = new System.Drawing.Point(271, 6);
            this.gbAccountLineups.Name = "gbAccountLineups";
            this.gbAccountLineups.Size = new System.Drawing.Size(595, 125);
            this.gbAccountLineups.TabIndex = 2;
            this.gbAccountLineups.TabStop = false;
            this.gbAccountLineups.Text = "Account Lineups";
            // 
            // btnDeleteLineup
            // 
            this.btnDeleteLineup.Enabled = false;
            this.btnDeleteLineup.Location = new System.Drawing.Point(514, 96);
            this.btnDeleteLineup.Name = "btnDeleteLineup";
            this.btnDeleteLineup.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteLineup.TabIndex = 6;
            this.btnDeleteLineup.Text = "Delete";
            this.btnDeleteLineup.UseVisualStyleBackColor = true;
            this.btnDeleteLineup.Click += new System.EventHandler(this.btnDeleteLineup_Click);
            // 
            // lvAccountLineups
            // 
            this.lvAccountLineups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chLineup,
            this.chModified});
            this.lvAccountLineups.FullRowSelect = true;
            this.lvAccountLineups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvAccountLineups.Location = new System.Drawing.Point(6, 13);
            this.lvAccountLineups.MultiSelect = false;
            this.lvAccountLineups.Name = "lvAccountLineups";
            this.lvAccountLineups.Size = new System.Drawing.Size(502, 106);
            this.lvAccountLineups.TabIndex = 0;
            this.lvAccountLineups.UseCompatibleStateImageBehavior = false;
            this.lvAccountLineups.View = System.Windows.Forms.View.Details;
            this.lvAccountLineups.SelectedIndexChanged += new System.EventHandler(this.lvAccountLineups_SelectedIndexChanged);
            // 
            // chLineup
            // 
            this.chLineup.Text = "Lineup";
            this.chLineup.Width = 414;
            // 
            // chModified
            // 
            this.chModified.Text = "Date Modified";
            this.chModified.Width = 84;
            // 
            // grpLocation
            // 
            this.grpLocation.Controls.Add(this.btnLocationCacheClear);
            this.grpLocation.Controls.Add(this.btnHeadends);
            this.grpLocation.Controls.Add(this.txtPostCode);
            this.grpLocation.Controls.Add(this.lbPostCode);
            this.grpLocation.Controls.Add(this.tvCountries);
            this.grpLocation.Location = new System.Drawing.Point(6, 137);
            this.grpLocation.Name = "grpLocation";
            this.grpLocation.Size = new System.Drawing.Size(259, 332);
            this.grpLocation.TabIndex = 1;
            this.grpLocation.TabStop = false;
            this.grpLocation.Text = "Location Selection";
            // 
            // btnLocationCacheClear
            // 
            this.btnLocationCacheClear.Enabled = false;
            this.btnLocationCacheClear.Location = new System.Drawing.Point(6, 276);
            this.btnLocationCacheClear.Name = "btnLocationCacheClear";
            this.btnLocationCacheClear.Size = new System.Drawing.Size(75, 23);
            this.btnLocationCacheClear.TabIndex = 7;
            this.btnLocationCacheClear.Text = "Clear Cache";
            this.btnLocationCacheClear.UseVisualStyleBackColor = true;
            this.btnLocationCacheClear.Click += new System.EventHandler(this.btnLocationCacheClear_Click);
            // 
            // btnHeadends
            // 
            this.btnHeadends.Enabled = false;
            this.btnHeadends.Location = new System.Drawing.Point(178, 276);
            this.btnHeadends.Name = "btnHeadends";
            this.btnHeadends.Size = new System.Drawing.Size(75, 23);
            this.btnHeadends.TabIndex = 6;
            this.btnHeadends.Text = "Headends";
            this.btnHeadends.UseVisualStyleBackColor = true;
            this.btnHeadends.Click += new System.EventHandler(this.btnHeadends_Click);
            // 
            // txtPostCode
            // 
            this.txtPostCode.Location = new System.Drawing.Point(70, 305);
            this.txtPostCode.Name = "txtPostCode";
            this.txtPostCode.Size = new System.Drawing.Size(183, 20);
            this.txtPostCode.TabIndex = 2;
            this.txtPostCode.TextChanged += new System.EventHandler(this.txtPostCode_TextChanged);
            // 
            // lbPostCode
            // 
            this.lbPostCode.AutoSize = true;
            this.lbPostCode.Location = new System.Drawing.Point(8, 308);
            this.lbPostCode.Name = "lbPostCode";
            this.lbPostCode.Size = new System.Drawing.Size(56, 13);
            this.lbPostCode.TabIndex = 1;
            this.lbPostCode.Text = "Post Code";
            // 
            // tvCountries
            // 
            this.tvCountries.Location = new System.Drawing.Point(6, 15);
            this.tvCountries.Name = "tvCountries";
            this.tvCountries.Size = new System.Drawing.Size(247, 255);
            this.tvCountries.TabIndex = 0;
            this.tvCountries.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCountries_AfterSelect);
            // 
            // gbCreds
            // 
            this.gbCreds.Controls.Add(this.btnLogout);
            this.gbCreds.Controls.Add(this.btnLogin);
            this.gbCreds.Controls.Add(this.checkAlwaysAsk);
            this.gbCreds.Controls.Add(this.txtPassword);
            this.gbCreds.Controls.Add(this.txtLogin);
            this.gbCreds.Controls.Add(this.lbPassword);
            this.gbCreds.Controls.Add(this.lbLogin);
            this.gbCreds.Location = new System.Drawing.Point(6, 6);
            this.gbCreds.Name = "gbCreds";
            this.gbCreds.Size = new System.Drawing.Size(259, 125);
            this.gbCreds.TabIndex = 0;
            this.gbCreds.TabStop = false;
            this.gbCreds.Text = "Credentials";
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(97, 96);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(178, 96);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // checkAlwaysAsk
            // 
            this.checkAlwaysAsk.AutoSize = true;
            this.checkAlwaysAsk.Location = new System.Drawing.Point(11, 100);
            this.checkAlwaysAsk.Name = "checkAlwaysAsk";
            this.checkAlwaysAsk.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkAlwaysAsk.Size = new System.Drawing.Size(80, 17);
            this.checkAlwaysAsk.TabIndex = 4;
            this.checkAlwaysAsk.Text = "Always Ask";
            this.checkAlwaysAsk.UseVisualStyleBackColor = true;
            this.checkAlwaysAsk.CheckedChanged += new System.EventHandler(this.checkAlwaysAsk_CheckedChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(81, 43);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(172, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(81, 17);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(172, 20);
            this.txtLogin.TabIndex = 2;
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(8, 46);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(53, 13);
            this.lbPassword.TabIndex = 1;
            this.lbPassword.Text = "Password";
            // 
            // lbLogin
            // 
            this.lbLogin.AutoSize = true;
            this.lbLogin.Location = new System.Drawing.Point(8, 20);
            this.lbLogin.Name = "lbLogin";
            this.lbLogin.Size = new System.Drawing.Size(33, 13);
            this.lbLogin.TabIndex = 0;
            this.lbLogin.Text = "Login";
            // 
            // tpXMLTV1
            // 
            this.tpXMLTV1.BackColor = System.Drawing.SystemColors.Control;
            this.tpXMLTV1.Controls.Add(this.gbStationMap);
            this.tpXMLTV1.Location = new System.Drawing.Point(4, 22);
            this.tpXMLTV1.Name = "tpXMLTV1";
            this.tpXMLTV1.Padding = new System.Windows.Forms.Padding(3);
            this.tpXMLTV1.Size = new System.Drawing.Size(874, 513);
            this.tpXMLTV1.TabIndex = 1;
            this.tpXMLTV1.Text = "XMLTV";
            this.tpXMLTV1.Enter += new System.EventHandler(this.tpXMLTV1_Enter);
            // 
            // gbStationMap
            // 
            this.gbStationMap.Controls.Add(this.lvAddedChans);
            this.gbStationMap.Controls.Add(this.rdCustom);
            this.gbStationMap.Controls.Add(this.txtCustom);
            this.gbStationMap.Controls.Add(this.rdCallsign);
            this.gbStationMap.Controls.Add(this.rdAffiliate);
            this.gbStationMap.Controls.Add(this.rdName);
            this.gbStationMap.Controls.Add(this.rdStation);
            this.gbStationMap.Controls.Add(this.txtCallsign);
            this.gbStationMap.Controls.Add(this.txtAffiliate);
            this.gbStationMap.Controls.Add(this.txtName);
            this.gbStationMap.Controls.Add(this.txtStationID);
            this.gbStationMap.Controls.Add(this.btnAddAllChans);
            this.gbStationMap.Controls.Add(this.btnRemoveAllChans);
            this.gbStationMap.Controls.Add(this.btnRemoveChan);
            this.gbStationMap.Controls.Add(this.btnAddChan);
            this.gbStationMap.Controls.Add(this.lvAvailableChans);
            this.gbStationMap.Controls.Add(this.lblLineup);
            this.gbStationMap.Controls.Add(this.cbLineup);
            this.gbStationMap.Location = new System.Drawing.Point(8, 7);
            this.gbStationMap.Name = "gbStationMap";
            this.gbStationMap.Size = new System.Drawing.Size(515, 462);
            this.gbStationMap.TabIndex = 0;
            this.gbStationMap.TabStop = false;
            this.gbStationMap.Text = "Station mapping";
            // 
            // lvAddedChans
            // 
            this.lvAddedChans.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAddedID,
            this.chAddedName});
            this.lvAddedChans.FullRowSelect = true;
            this.lvAddedChans.Location = new System.Drawing.Point(286, 44);
            this.lvAddedChans.Name = "lvAddedChans";
            this.lvAddedChans.Size = new System.Drawing.Size(223, 259);
            this.lvAddedChans.TabIndex = 24;
            this.lvAddedChans.UseCompatibleStateImageBehavior = false;
            this.lvAddedChans.View = System.Windows.Forms.View.Details;
            this.lvAddedChans.SelectedIndexChanged += new System.EventHandler(this.lvAddedChans_SelectedIndexChanged);
            // 
            // chAddedID
            // 
            this.chAddedID.Text = "ID";
            // 
            // chAddedName
            // 
            this.chAddedName.Text = "Name";
            this.chAddedName.Width = 153;
            // 
            // rdCustom
            // 
            this.rdCustom.AutoSize = true;
            this.rdCustom.Location = new System.Drawing.Point(6, 414);
            this.rdCustom.Name = "rdCustom";
            this.rdCustom.Size = new System.Drawing.Size(60, 17);
            this.rdCustom.TabIndex = 23;
            this.rdCustom.TabStop = true;
            this.rdCustom.Text = "Custom";
            this.rdCustom.UseVisualStyleBackColor = true;
            this.rdCustom.CheckedChanged += new System.EventHandler(this.rdCustom_CheckedChanged);
            // 
            // txtCustom
            // 
            this.txtCustom.Enabled = false;
            this.txtCustom.Location = new System.Drawing.Point(88, 413);
            this.txtCustom.Name = "txtCustom";
            this.txtCustom.Size = new System.Drawing.Size(141, 20);
            this.txtCustom.TabIndex = 22;
            this.txtCustom.Leave += new System.EventHandler(this.txtCustom_Leave);
            this.txtCustom.Validated += new System.EventHandler(this.txtCustom_Validated);
            // 
            // rdCallsign
            // 
            this.rdCallsign.AutoSize = true;
            this.rdCallsign.Location = new System.Drawing.Point(6, 388);
            this.rdCallsign.Name = "rdCallsign";
            this.rdCallsign.Size = new System.Drawing.Size(61, 17);
            this.rdCallsign.TabIndex = 21;
            this.rdCallsign.TabStop = true;
            this.rdCallsign.Text = "Callsign";
            this.rdCallsign.UseVisualStyleBackColor = true;
            this.rdCallsign.CheckedChanged += new System.EventHandler(this.rdCallsign_CheckedChanged);
            // 
            // rdAffiliate
            // 
            this.rdAffiliate.AutoSize = true;
            this.rdAffiliate.Location = new System.Drawing.Point(6, 362);
            this.rdAffiliate.Name = "rdAffiliate";
            this.rdAffiliate.Size = new System.Drawing.Size(59, 17);
            this.rdAffiliate.TabIndex = 20;
            this.rdAffiliate.TabStop = true;
            this.rdAffiliate.Text = "Affiliate";
            this.rdAffiliate.UseVisualStyleBackColor = true;
            this.rdAffiliate.CheckedChanged += new System.EventHandler(this.rdAffiliate_CheckedChanged);
            // 
            // rdName
            // 
            this.rdName.AutoSize = true;
            this.rdName.Location = new System.Drawing.Point(6, 336);
            this.rdName.Name = "rdName";
            this.rdName.Size = new System.Drawing.Size(53, 17);
            this.rdName.TabIndex = 19;
            this.rdName.TabStop = true;
            this.rdName.Text = "Name";
            this.rdName.UseVisualStyleBackColor = true;
            this.rdName.CheckedChanged += new System.EventHandler(this.rdName_CheckedChanged);
            // 
            // rdStation
            // 
            this.rdStation.AutoSize = true;
            this.rdStation.Location = new System.Drawing.Point(6, 310);
            this.rdStation.Name = "rdStation";
            this.rdStation.Size = new System.Drawing.Size(72, 17);
            this.rdStation.TabIndex = 18;
            this.rdStation.TabStop = true;
            this.rdStation.Text = "Station ID";
            this.rdStation.UseVisualStyleBackColor = true;
            this.rdStation.CheckedChanged += new System.EventHandler(this.rdStation_CheckedChanged);
            // 
            // txtCallsign
            // 
            this.txtCallsign.Enabled = false;
            this.txtCallsign.Location = new System.Drawing.Point(88, 387);
            this.txtCallsign.Name = "txtCallsign";
            this.txtCallsign.Size = new System.Drawing.Size(141, 20);
            this.txtCallsign.TabIndex = 17;
            // 
            // txtAffiliate
            // 
            this.txtAffiliate.Enabled = false;
            this.txtAffiliate.Location = new System.Drawing.Point(88, 361);
            this.txtAffiliate.Name = "txtAffiliate";
            this.txtAffiliate.Size = new System.Drawing.Size(141, 20);
            this.txtAffiliate.TabIndex = 15;
            // 
            // txtName
            // 
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(88, 335);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(141, 20);
            this.txtName.TabIndex = 13;
            // 
            // txtStationID
            // 
            this.txtStationID.Enabled = false;
            this.txtStationID.Location = new System.Drawing.Point(88, 309);
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(141, 20);
            this.txtStationID.TabIndex = 11;
            // 
            // btnAddAllChans
            // 
            this.btnAddAllChans.Location = new System.Drawing.Point(235, 110);
            this.btnAddAllChans.Name = "btnAddAllChans";
            this.btnAddAllChans.Size = new System.Drawing.Size(45, 27);
            this.btnAddAllChans.TabIndex = 9;
            this.btnAddAllChans.Text = ">>";
            this.btnAddAllChans.UseVisualStyleBackColor = true;
            // 
            // btnRemoveAllChans
            // 
            this.btnRemoveAllChans.Location = new System.Drawing.Point(235, 209);
            this.btnRemoveAllChans.Name = "btnRemoveAllChans";
            this.btnRemoveAllChans.Size = new System.Drawing.Size(45, 27);
            this.btnRemoveAllChans.TabIndex = 7;
            this.btnRemoveAllChans.Text = "<<";
            this.btnRemoveAllChans.UseVisualStyleBackColor = true;
            // 
            // btnRemoveChan
            // 
            this.btnRemoveChan.Location = new System.Drawing.Point(235, 176);
            this.btnRemoveChan.Name = "btnRemoveChan";
            this.btnRemoveChan.Size = new System.Drawing.Size(45, 27);
            this.btnRemoveChan.TabIndex = 6;
            this.btnRemoveChan.Text = "<";
            this.btnRemoveChan.UseVisualStyleBackColor = true;
            // 
            // btnAddChan
            // 
            this.btnAddChan.Location = new System.Drawing.Point(235, 143);
            this.btnAddChan.Name = "btnAddChan";
            this.btnAddChan.Size = new System.Drawing.Size(45, 27);
            this.btnAddChan.TabIndex = 5;
            this.btnAddChan.Text = ">";
            this.btnAddChan.UseVisualStyleBackColor = true;
            this.btnAddChan.Click += new System.EventHandler(this.btnAddChan_Click);
            // 
            // lvAvailableChans
            // 
            this.lvAvailableChans.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAvailID,
            this.chAvailName});
            this.lvAvailableChans.FullRowSelect = true;
            this.lvAvailableChans.Location = new System.Drawing.Point(6, 44);
            this.lvAvailableChans.Name = "lvAvailableChans";
            this.lvAvailableChans.Size = new System.Drawing.Size(223, 259);
            this.lvAvailableChans.TabIndex = 4;
            this.lvAvailableChans.UseCompatibleStateImageBehavior = false;
            this.lvAvailableChans.View = System.Windows.Forms.View.Details;
            this.lvAvailableChans.SelectedIndexChanged += new System.EventHandler(this.lvAvailableChans_SelectedIndexChanged);
            // 
            // chAvailID
            // 
            this.chAvailID.Text = "ID";
            // 
            // chAvailName
            // 
            this.chAvailName.Text = "Name";
            this.chAvailName.Width = 153;
            // 
            // lblLineup
            // 
            this.lblLineup.AutoSize = true;
            this.lblLineup.Location = new System.Drawing.Point(6, 22);
            this.lblLineup.Name = "lblLineup";
            this.lblLineup.Size = new System.Drawing.Size(39, 13);
            this.lblLineup.TabIndex = 1;
            this.lblLineup.Text = "Lineup";
            // 
            // cbLineup
            // 
            this.cbLineup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLineup.FormattingEnabled = true;
            this.cbLineup.Location = new System.Drawing.Point(51, 19);
            this.cbLineup.Name = "cbLineup";
            this.cbLineup.Size = new System.Drawing.Size(458, 21);
            this.cbLineup.TabIndex = 0;
            this.cbLineup.SelectedIndexChanged += new System.EventHandler(this.cbLineup_SelectedIndexChanged);
            // 
            // tpGeneral1
            // 
            this.tpGeneral1.BackColor = System.Drawing.SystemColors.Control;
            this.tpGeneral1.Controls.Add(this.gbPersistentCache);
            this.tpGeneral1.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral1.Name = "tpGeneral1";
            this.tpGeneral1.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral1.Size = new System.Drawing.Size(874, 513);
            this.tpGeneral1.TabIndex = 2;
            this.tpGeneral1.Text = "General";
            this.tpGeneral1.Enter += new System.EventHandler(this.tpGeneral1_Enter);
            // 
            // gbPersistentCache
            // 
            this.gbPersistentCache.Controls.Add(this.btnBrowseCache);
            this.gbPersistentCache.Controls.Add(this.ckPersistentCache);
            this.gbPersistentCache.Controls.Add(this.txtCacheFilename);
            this.gbPersistentCache.Controls.Add(this.lbCacheFilename);
            this.gbPersistentCache.Location = new System.Drawing.Point(6, 6);
            this.gbPersistentCache.Name = "gbPersistentCache";
            this.gbPersistentCache.Size = new System.Drawing.Size(377, 69);
            this.gbPersistentCache.TabIndex = 0;
            this.gbPersistentCache.TabStop = false;
            this.gbPersistentCache.Text = "Persistent Cache";
            // 
            // btnBrowseCache
            // 
            this.btnBrowseCache.Location = new System.Drawing.Point(281, 39);
            this.btnBrowseCache.Name = "btnBrowseCache";
            this.btnBrowseCache.Size = new System.Drawing.Size(90, 24);
            this.btnBrowseCache.TabIndex = 3;
            this.btnBrowseCache.Text = "Browse";
            this.btnBrowseCache.UseVisualStyleBackColor = true;
            this.btnBrowseCache.Click += new System.EventHandler(this.btnBrowseCache_Click);
            // 
            // ckPersistentCache
            // 
            this.ckPersistentCache.AutoSize = true;
            this.ckPersistentCache.Location = new System.Drawing.Point(9, 41);
            this.ckPersistentCache.Name = "ckPersistentCache";
            this.ckPersistentCache.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ckPersistentCache.Size = new System.Drawing.Size(106, 17);
            this.ckPersistentCache.TabIndex = 2;
            this.ckPersistentCache.Text = "Persistent Cache";
            this.ckPersistentCache.UseVisualStyleBackColor = true;
            this.ckPersistentCache.CheckedChanged += new System.EventHandler(this.ckPersistentCache_CheckedChanged);
            // 
            // txtCacheFilename
            // 
            this.txtCacheFilename.Location = new System.Drawing.Point(95, 13);
            this.txtCacheFilename.Name = "txtCacheFilename";
            this.txtCacheFilename.Size = new System.Drawing.Size(276, 20);
            this.txtCacheFilename.TabIndex = 1;
            // 
            // lbCacheFilename
            // 
            this.lbCacheFilename.AutoSize = true;
            this.lbCacheFilename.Location = new System.Drawing.Point(6, 16);
            this.lbCacheFilename.Name = "lbCacheFilename";
            this.lbCacheFilename.Size = new System.Drawing.Size(83, 13);
            this.lbCacheFilename.TabIndex = 0;
            this.lbCacheFilename.Text = "Cache Filename";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveAs);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 497);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(882, 42);
            this.panel1.TabIndex = 5;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(612, 7);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAs.TabIndex = 8;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(16, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(714, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(795, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 539);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tcConfig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmOptions";
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmOptions_FormClosing);
            this.tcConfig.ResumeLayout(false);
            this.tpSD1.ResumeLayout(false);
            this.gbStations.ResumeLayout(false);
            this.gbHeadends.ResumeLayout(false);
            this.gbAccountLineups.ResumeLayout(false);
            this.grpLocation.ResumeLayout(false);
            this.grpLocation.PerformLayout();
            this.gbCreds.ResumeLayout(false);
            this.gbCreds.PerformLayout();
            this.tpXMLTV1.ResumeLayout(false);
            this.gbStationMap.ResumeLayout(false);
            this.gbStationMap.PerformLayout();
            this.tpGeneral1.ResumeLayout(false);
            this.gbPersistentCache.ResumeLayout(false);
            this.gbPersistentCache.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcConfig;
        private System.Windows.Forms.TabPage tpSD1;
        private System.Windows.Forms.TabPage tpXMLTV1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbCreds;
        private System.Windows.Forms.CheckBox checkAlwaysAsk;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.Label lbLogin;
        private System.Windows.Forms.GroupBox grpLocation;
        private System.Windows.Forms.TreeView tvCountries;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.GroupBox gbAccountLineups;
        private System.Windows.Forms.Button btnDeleteLineup;
        private System.Windows.Forms.ListView lvAccountLineups;
        private System.Windows.Forms.Button btnHeadends;
        private System.Windows.Forms.TextBox txtPostCode;
        private System.Windows.Forms.Label lbPostCode;
        private System.Windows.Forms.ColumnHeader chLineup;
        private System.Windows.Forms.ColumnHeader chModified;
        private System.Windows.Forms.GroupBox gbStations;
        private System.Windows.Forms.ListView lvStations;
        private System.Windows.Forms.ColumnHeader chStationID;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.GroupBox gbHeadends;
        private System.Windows.Forms.TreeView tvHeadends;
        private System.Windows.Forms.Button btnLineupAdd;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnLocationCacheClear;
        private System.Windows.Forms.Button btnHeadendCacheClear;
        private System.Windows.Forms.TabPage tpGeneral1;
        private System.Windows.Forms.Button btnStationCacheClear;
        private System.Windows.Forms.GroupBox gbStationMap;
        private System.Windows.Forms.Button btnRemoveAllChans;
        private System.Windows.Forms.Button btnRemoveChan;
        private System.Windows.Forms.Button btnAddChan;
        private System.Windows.Forms.ListView lvAvailableChans;
        private System.Windows.Forms.Label lblLineup;
        private System.Windows.Forms.ComboBox cbLineup;
        private System.Windows.Forms.Button btnAddAllChans;
        private System.Windows.Forms.TextBox txtAffiliate;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtStationID;
        private System.Windows.Forms.RadioButton rdCallsign;
        private System.Windows.Forms.RadioButton rdAffiliate;
        private System.Windows.Forms.RadioButton rdName;
        private System.Windows.Forms.RadioButton rdStation;
        private System.Windows.Forms.TextBox txtCallsign;
        private System.Windows.Forms.RadioButton rdCustom;
        private System.Windows.Forms.TextBox txtCustom;
        private System.Windows.Forms.ListView lvAddedChans;
        private System.Windows.Forms.ColumnHeader chAddedID;
        private System.Windows.Forms.ColumnHeader chAddedName;
        private System.Windows.Forms.ColumnHeader chAvailID;
        private System.Windows.Forms.ColumnHeader chAvailName;
        private System.Windows.Forms.GroupBox gbPersistentCache;
        private System.Windows.Forms.Button btnBrowseCache;
        private System.Windows.Forms.CheckBox ckPersistentCache;
        private System.Windows.Forms.TextBox txtCacheFilename;
        private System.Windows.Forms.Label lbCacheFilename;
    }
}