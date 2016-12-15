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
            this.gbChannelIDMap = new System.Windows.Forms.GroupBox();
            this.btnCustomGrid = new System.Windows.Forms.Button();
            this.txtStationID = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtAffiliate = new System.Windows.Forms.TextBox();
            this.rdCustom = new System.Windows.Forms.RadioButton();
            this.txtCallsign = new System.Windows.Forms.TextBox();
            this.txtCustom = new System.Windows.Forms.TextBox();
            this.rdStation = new System.Windows.Forms.RadioButton();
            this.rdCallsign = new System.Windows.Forms.RadioButton();
            this.rdName = new System.Windows.Forms.RadioButton();
            this.rdAffiliate = new System.Windows.Forms.RadioButton();
            this.gbAddedChannels = new System.Windows.Forms.GroupBox();
            this.lvAddedChans = new System.Windows.Forms.ListView();
            this.chAddedID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAddedName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbAvailableChannels = new System.Windows.Forms.GroupBox();
            this.lvAvailableChans = new System.Windows.Forms.ListView();
            this.chAvailID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAvailName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddAllChans = new System.Windows.Forms.Button();
            this.btnRemoveAllChans = new System.Windows.Forms.Button();
            this.btnRemoveChan = new System.Windows.Forms.Button();
            this.btnAddChan = new System.Windows.Forms.Button();
            this.lblLineup = new System.Windows.Forms.Label();
            this.cbLineup = new System.Windows.Forms.ComboBox();
            this.tpGeneral1 = new System.Windows.Forms.TabPage();
            this.gbXMLTVFile = new System.Windows.Forms.GroupBox();
            this.btnBrowseXmlTVFile = new System.Windows.Forms.Button();
            this.txtOutputXmlTVFile = new System.Windows.Forms.TextBox();
            this.lblOutputXmlTVFile = new System.Windows.Forms.Label();
            this.gbRetrievalOptions = new System.Windows.Forms.GroupBox();
            this.lbDateRangeInfo = new System.Windows.Forms.Label();
            this.lblProgrammeItems = new System.Windows.Forms.Label();
            this.txtScheduleItems = new System.Windows.Forms.TextBox();
            this.txtProgrammeItems = new System.Windows.Forms.TextBox();
            this.tkbProgrammePeriod = new System.Windows.Forms.TrackBar();
            this.tkbProgrammeItems = new System.Windows.Forms.TrackBar();
            this.tkbScheduleItems = new System.Windows.Forms.TrackBar();
            this.lblScheduleItems = new System.Windows.Forms.Label();
            this.ckIncludeYesterday = new System.Windows.Forms.CheckBox();
            this.txtProgrammePeriod = new System.Windows.Forms.TextBox();
            this.lbProgrammePeriod = new System.Windows.Forms.Label();
            this.gbXmlTVAttr = new System.Windows.Forms.GroupBox();
            this.cbDisplayNameMode = new System.Windows.Forms.ComboBox();
            this.lblChannelDisplayName = new System.Windows.Forms.Label();
            this.ckProgrammeID = new System.Windows.Forms.CheckBox();
            this.ckShowType = new System.Windows.Forms.CheckBox();
            this.ckStationAfiliate = new System.Windows.Forms.CheckBox();
            this.ckStationCallsign = new System.Windows.Forms.CheckBox();
            this.ckStationName = new System.Windows.Forms.CheckBox();
            this.ckStationID = new System.Windows.Forms.CheckBox();
            this.ckLogicalChannelNo = new System.Windows.Forms.CheckBox();
            this.ckMD5 = new System.Windows.Forms.CheckBox();
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
            this.gbChannelIDMap.SuspendLayout();
            this.gbAddedChannels.SuspendLayout();
            this.gbAvailableChannels.SuspendLayout();
            this.tpGeneral1.SuspendLayout();
            this.gbXMLTVFile.SuspendLayout();
            this.gbRetrievalOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkbProgrammePeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tkbProgrammeItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tkbScheduleItems)).BeginInit();
            this.gbXmlTVAttr.SuspendLayout();
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
            this.lvStations.FullRowSelect = true;
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
            this.tpXMLTV1.Text = "XMLTV Mapping";
            this.tpXMLTV1.Enter += new System.EventHandler(this.tpXMLTV1_Enter);
            // 
            // gbStationMap
            // 
            this.gbStationMap.Controls.Add(this.gbChannelIDMap);
            this.gbStationMap.Controls.Add(this.gbAddedChannels);
            this.gbStationMap.Controls.Add(this.gbAvailableChannels);
            this.gbStationMap.Controls.Add(this.btnAddAllChans);
            this.gbStationMap.Controls.Add(this.btnRemoveAllChans);
            this.gbStationMap.Controls.Add(this.btnRemoveChan);
            this.gbStationMap.Controls.Add(this.btnAddChan);
            this.gbStationMap.Controls.Add(this.lblLineup);
            this.gbStationMap.Controls.Add(this.cbLineup);
            this.gbStationMap.Location = new System.Drawing.Point(8, 7);
            this.gbStationMap.Name = "gbStationMap";
            this.gbStationMap.Size = new System.Drawing.Size(858, 462);
            this.gbStationMap.TabIndex = 0;
            this.gbStationMap.TabStop = false;
            this.gbStationMap.Text = "Station mapping";
            // 
            // gbChannelIDMap
            // 
            this.gbChannelIDMap.Controls.Add(this.btnCustomGrid);
            this.gbChannelIDMap.Controls.Add(this.txtStationID);
            this.gbChannelIDMap.Controls.Add(this.txtName);
            this.gbChannelIDMap.Controls.Add(this.txtAffiliate);
            this.gbChannelIDMap.Controls.Add(this.rdCustom);
            this.gbChannelIDMap.Controls.Add(this.txtCallsign);
            this.gbChannelIDMap.Controls.Add(this.txtCustom);
            this.gbChannelIDMap.Controls.Add(this.rdStation);
            this.gbChannelIDMap.Controls.Add(this.rdCallsign);
            this.gbChannelIDMap.Controls.Add(this.rdName);
            this.gbChannelIDMap.Controls.Add(this.rdAffiliate);
            this.gbChannelIDMap.Location = new System.Drawing.Point(615, 46);
            this.gbChannelIDMap.Name = "gbChannelIDMap";
            this.gbChannelIDMap.Size = new System.Drawing.Size(237, 167);
            this.gbChannelIDMap.TabIndex = 28;
            this.gbChannelIDMap.TabStop = false;
            this.gbChannelIDMap.Text = "XMLTV Channel ID Map";
            // 
            // btnCustomGrid
            // 
            this.btnCustomGrid.Location = new System.Drawing.Point(3, 134);
            this.btnCustomGrid.Name = "btnCustomGrid";
            this.btnCustomGrid.Size = new System.Drawing.Size(75, 23);
            this.btnCustomGrid.TabIndex = 25;
            this.btnCustomGrid.Text = "Custom Grid";
            this.btnCustomGrid.UseVisualStyleBackColor = true;
            this.btnCustomGrid.Click += new System.EventHandler(this.btnCustomGrid_Click);
            // 
            // txtStationID
            // 
            this.txtStationID.Enabled = false;
            this.txtStationID.Location = new System.Drawing.Point(84, 18);
            this.txtStationID.Name = "txtStationID";
            this.txtStationID.Size = new System.Drawing.Size(147, 20);
            this.txtStationID.TabIndex = 11;
            // 
            // txtName
            // 
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(84, 41);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(147, 20);
            this.txtName.TabIndex = 13;
            // 
            // txtAffiliate
            // 
            this.txtAffiliate.Enabled = false;
            this.txtAffiliate.Location = new System.Drawing.Point(84, 64);
            this.txtAffiliate.Name = "txtAffiliate";
            this.txtAffiliate.Size = new System.Drawing.Size(147, 20);
            this.txtAffiliate.TabIndex = 15;
            // 
            // rdCustom
            // 
            this.rdCustom.AutoSize = true;
            this.rdCustom.Location = new System.Drawing.Point(6, 111);
            this.rdCustom.Name = "rdCustom";
            this.rdCustom.Size = new System.Drawing.Size(60, 17);
            this.rdCustom.TabIndex = 23;
            this.rdCustom.TabStop = true;
            this.rdCustom.Text = "Custom";
            this.rdCustom.UseVisualStyleBackColor = true;
            this.rdCustom.CheckedChanged += new System.EventHandler(this.rdCustom_CheckedChanged);
            // 
            // txtCallsign
            // 
            this.txtCallsign.Enabled = false;
            this.txtCallsign.Location = new System.Drawing.Point(84, 87);
            this.txtCallsign.Name = "txtCallsign";
            this.txtCallsign.Size = new System.Drawing.Size(147, 20);
            this.txtCallsign.TabIndex = 17;
            // 
            // txtCustom
            // 
            this.txtCustom.Enabled = false;
            this.txtCustom.Location = new System.Drawing.Point(84, 110);
            this.txtCustom.Name = "txtCustom";
            this.txtCustom.Size = new System.Drawing.Size(147, 20);
            this.txtCustom.TabIndex = 22;
            this.txtCustom.Validated += new System.EventHandler(this.txtCustom_Validated);
            // 
            // rdStation
            // 
            this.rdStation.AutoSize = true;
            this.rdStation.Location = new System.Drawing.Point(6, 19);
            this.rdStation.Name = "rdStation";
            this.rdStation.Size = new System.Drawing.Size(72, 17);
            this.rdStation.TabIndex = 18;
            this.rdStation.TabStop = true;
            this.rdStation.Text = "Station ID";
            this.rdStation.UseVisualStyleBackColor = true;
            this.rdStation.CheckedChanged += new System.EventHandler(this.rdStation_CheckedChanged);
            // 
            // rdCallsign
            // 
            this.rdCallsign.AutoSize = true;
            this.rdCallsign.Location = new System.Drawing.Point(6, 88);
            this.rdCallsign.Name = "rdCallsign";
            this.rdCallsign.Size = new System.Drawing.Size(61, 17);
            this.rdCallsign.TabIndex = 21;
            this.rdCallsign.TabStop = true;
            this.rdCallsign.Text = "Callsign";
            this.rdCallsign.UseVisualStyleBackColor = true;
            this.rdCallsign.CheckedChanged += new System.EventHandler(this.rdCallsign_CheckedChanged);
            // 
            // rdName
            // 
            this.rdName.AutoSize = true;
            this.rdName.Location = new System.Drawing.Point(6, 42);
            this.rdName.Name = "rdName";
            this.rdName.Size = new System.Drawing.Size(53, 17);
            this.rdName.TabIndex = 19;
            this.rdName.TabStop = true;
            this.rdName.Text = "Name";
            this.rdName.UseVisualStyleBackColor = true;
            this.rdName.CheckedChanged += new System.EventHandler(this.rdName_CheckedChanged);
            // 
            // rdAffiliate
            // 
            this.rdAffiliate.AutoSize = true;
            this.rdAffiliate.Location = new System.Drawing.Point(6, 65);
            this.rdAffiliate.Name = "rdAffiliate";
            this.rdAffiliate.Size = new System.Drawing.Size(59, 17);
            this.rdAffiliate.TabIndex = 20;
            this.rdAffiliate.TabStop = true;
            this.rdAffiliate.Text = "Affiliate";
            this.rdAffiliate.UseVisualStyleBackColor = true;
            this.rdAffiliate.CheckedChanged += new System.EventHandler(this.rdAffiliate_CheckedChanged);
            // 
            // gbAddedChannels
            // 
            this.gbAddedChannels.Controls.Add(this.lvAddedChans);
            this.gbAddedChannels.Location = new System.Drawing.Point(339, 46);
            this.gbAddedChannels.Name = "gbAddedChannels";
            this.gbAddedChannels.Size = new System.Drawing.Size(270, 404);
            this.gbAddedChannels.TabIndex = 27;
            this.gbAddedChannels.TabStop = false;
            this.gbAddedChannels.Text = "Added Channels";
            // 
            // lvAddedChans
            // 
            this.lvAddedChans.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAddedID,
            this.chAddedName});
            this.lvAddedChans.FullRowSelect = true;
            this.lvAddedChans.Location = new System.Drawing.Point(6, 19);
            this.lvAddedChans.Name = "lvAddedChans";
            this.lvAddedChans.Size = new System.Drawing.Size(258, 379);
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
            this.chAddedName.Width = 188;
            // 
            // gbAvailableChannels
            // 
            this.gbAvailableChannels.Controls.Add(this.lvAvailableChans);
            this.gbAvailableChannels.Location = new System.Drawing.Point(6, 46);
            this.gbAvailableChannels.Name = "gbAvailableChannels";
            this.gbAvailableChannels.Size = new System.Drawing.Size(270, 404);
            this.gbAvailableChannels.TabIndex = 26;
            this.gbAvailableChannels.TabStop = false;
            this.gbAvailableChannels.Text = "Available Channels";
            // 
            // lvAvailableChans
            // 
            this.lvAvailableChans.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAvailID,
            this.chAvailName});
            this.lvAvailableChans.FullRowSelect = true;
            this.lvAvailableChans.Location = new System.Drawing.Point(6, 19);
            this.lvAvailableChans.Name = "lvAvailableChans";
            this.lvAvailableChans.Size = new System.Drawing.Size(258, 379);
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
            this.chAvailName.Width = 184;
            // 
            // btnAddAllChans
            // 
            this.btnAddAllChans.Location = new System.Drawing.Point(282, 186);
            this.btnAddAllChans.Name = "btnAddAllChans";
            this.btnAddAllChans.Size = new System.Drawing.Size(51, 27);
            this.btnAddAllChans.TabIndex = 9;
            this.btnAddAllChans.Text = ">>";
            this.btnAddAllChans.UseVisualStyleBackColor = true;
            this.btnAddAllChans.Click += new System.EventHandler(this.btnAddAllChans_Click);
            // 
            // btnRemoveAllChans
            // 
            this.btnRemoveAllChans.Location = new System.Drawing.Point(282, 285);
            this.btnRemoveAllChans.Name = "btnRemoveAllChans";
            this.btnRemoveAllChans.Size = new System.Drawing.Size(51, 27);
            this.btnRemoveAllChans.TabIndex = 7;
            this.btnRemoveAllChans.Text = "<<";
            this.btnRemoveAllChans.UseVisualStyleBackColor = true;
            this.btnRemoveAllChans.Click += new System.EventHandler(this.btnRemoveAllChans_Click);
            // 
            // btnRemoveChan
            // 
            this.btnRemoveChan.Location = new System.Drawing.Point(282, 252);
            this.btnRemoveChan.Name = "btnRemoveChan";
            this.btnRemoveChan.Size = new System.Drawing.Size(51, 27);
            this.btnRemoveChan.TabIndex = 6;
            this.btnRemoveChan.Text = "<";
            this.btnRemoveChan.UseVisualStyleBackColor = true;
            this.btnRemoveChan.Click += new System.EventHandler(this.btnRemoveChan_Click);
            // 
            // btnAddChan
            // 
            this.btnAddChan.Location = new System.Drawing.Point(282, 219);
            this.btnAddChan.Name = "btnAddChan";
            this.btnAddChan.Size = new System.Drawing.Size(51, 27);
            this.btnAddChan.TabIndex = 5;
            this.btnAddChan.Text = ">";
            this.btnAddChan.UseVisualStyleBackColor = true;
            this.btnAddChan.Click += new System.EventHandler(this.btnAddChan_Click);
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
            this.cbLineup.Size = new System.Drawing.Size(558, 21);
            this.cbLineup.TabIndex = 0;
            this.cbLineup.SelectedIndexChanged += new System.EventHandler(this.cbLineup_SelectedIndexChanged);
            // 
            // tpGeneral1
            // 
            this.tpGeneral1.BackColor = System.Drawing.SystemColors.Control;
            this.tpGeneral1.Controls.Add(this.gbXMLTVFile);
            this.tpGeneral1.Controls.Add(this.gbRetrievalOptions);
            this.tpGeneral1.Controls.Add(this.gbXmlTVAttr);
            this.tpGeneral1.Controls.Add(this.gbPersistentCache);
            this.tpGeneral1.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral1.Name = "tpGeneral1";
            this.tpGeneral1.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral1.Size = new System.Drawing.Size(874, 513);
            this.tpGeneral1.TabIndex = 2;
            this.tpGeneral1.Text = "General";
            this.tpGeneral1.Enter += new System.EventHandler(this.tpGeneral1_Enter);
            // 
            // gbXMLTVFile
            // 
            this.gbXMLTVFile.Controls.Add(this.btnBrowseXmlTVFile);
            this.gbXMLTVFile.Controls.Add(this.txtOutputXmlTVFile);
            this.gbXMLTVFile.Controls.Add(this.lblOutputXmlTVFile);
            this.gbXMLTVFile.Location = new System.Drawing.Point(6, 198);
            this.gbXMLTVFile.Name = "gbXMLTVFile";
            this.gbXMLTVFile.Size = new System.Drawing.Size(860, 41);
            this.gbXMLTVFile.TabIndex = 3;
            this.gbXMLTVFile.TabStop = false;
            this.gbXMLTVFile.Text = "XMLTV File";
            // 
            // btnBrowseXmlTVFile
            // 
            this.btnBrowseXmlTVFile.Location = new System.Drawing.Point(764, 10);
            this.btnBrowseXmlTVFile.Name = "btnBrowseXmlTVFile";
            this.btnBrowseXmlTVFile.Size = new System.Drawing.Size(90, 24);
            this.btnBrowseXmlTVFile.TabIndex = 6;
            this.btnBrowseXmlTVFile.Text = "Browse";
            this.btnBrowseXmlTVFile.UseVisualStyleBackColor = true;
            this.btnBrowseXmlTVFile.Click += new System.EventHandler(this.btnBrowseXmlTVFile_Click);
            // 
            // txtOutputXmlTVFile
            // 
            this.txtOutputXmlTVFile.Location = new System.Drawing.Point(135, 13);
            this.txtOutputXmlTVFile.Name = "txtOutputXmlTVFile";
            this.txtOutputXmlTVFile.Size = new System.Drawing.Size(623, 20);
            this.txtOutputXmlTVFile.TabIndex = 5;
            // 
            // lblOutputXmlTVFile
            // 
            this.lblOutputXmlTVFile.AutoSize = true;
            this.lblOutputXmlTVFile.Location = new System.Drawing.Point(6, 16);
            this.lblOutputXmlTVFile.Name = "lblOutputXmlTVFile";
            this.lblOutputXmlTVFile.Size = new System.Drawing.Size(123, 13);
            this.lblOutputXmlTVFile.TabIndex = 4;
            this.lblOutputXmlTVFile.Text = "Output XMLTV Filename";
            // 
            // gbRetrievalOptions
            // 
            this.gbRetrievalOptions.Controls.Add(this.lbDateRangeInfo);
            this.gbRetrievalOptions.Controls.Add(this.lblProgrammeItems);
            this.gbRetrievalOptions.Controls.Add(this.txtScheduleItems);
            this.gbRetrievalOptions.Controls.Add(this.txtProgrammeItems);
            this.gbRetrievalOptions.Controls.Add(this.tkbProgrammePeriod);
            this.gbRetrievalOptions.Controls.Add(this.tkbProgrammeItems);
            this.gbRetrievalOptions.Controls.Add(this.tkbScheduleItems);
            this.gbRetrievalOptions.Controls.Add(this.lblScheduleItems);
            this.gbRetrievalOptions.Controls.Add(this.ckIncludeYesterday);
            this.gbRetrievalOptions.Controls.Add(this.txtProgrammePeriod);
            this.gbRetrievalOptions.Controls.Add(this.lbProgrammePeriod);
            this.gbRetrievalOptions.Location = new System.Drawing.Point(380, 52);
            this.gbRetrievalOptions.Name = "gbRetrievalOptions";
            this.gbRetrievalOptions.Size = new System.Drawing.Size(486, 140);
            this.gbRetrievalOptions.TabIndex = 2;
            this.gbRetrievalOptions.TabStop = false;
            this.gbRetrievalOptions.Text = "Programme Retrieval Period";
            // 
            // lbDateRangeInfo
            // 
            this.lbDateRangeInfo.AutoSize = true;
            this.lbDateRangeInfo.Location = new System.Drawing.Point(138, 118);
            this.lbDateRangeInfo.Name = "lbDateRangeInfo";
            this.lbDateRangeInfo.Size = new System.Drawing.Size(266, 13);
            this.lbDateRangeInfo.TabIndex = 4;
            this.lbDateRangeInfo.Text = "Example include date range 04/12/2016 - 07/12/2016";
            // 
            // lblProgrammeItems
            // 
            this.lblProgrammeItems.AutoSize = true;
            this.lblProgrammeItems.Location = new System.Drawing.Point(7, 53);
            this.lblProgrammeItems.Name = "lblProgrammeItems";
            this.lblProgrammeItems.Size = new System.Drawing.Size(133, 13);
            this.lblProgrammeItems.TabIndex = 8;
            this.lblProgrammeItems.Text = "Programme Items/Request";
            // 
            // txtScheduleItems
            // 
            this.txtScheduleItems.Location = new System.Drawing.Point(410, 14);
            this.txtScheduleItems.Name = "txtScheduleItems";
            this.txtScheduleItems.Size = new System.Drawing.Size(70, 20);
            this.txtScheduleItems.TabIndex = 2;
            this.txtScheduleItems.Validated += new System.EventHandler(this.txtScheduleItems_Validated);
            // 
            // txtProgrammeItems
            // 
            this.txtProgrammeItems.Location = new System.Drawing.Point(411, 50);
            this.txtProgrammeItems.Name = "txtProgrammeItems";
            this.txtProgrammeItems.Size = new System.Drawing.Size(70, 20);
            this.txtProgrammeItems.TabIndex = 3;
            this.txtProgrammeItems.Validated += new System.EventHandler(this.txtProgrammeItems_Validated);
            // 
            // tkbProgrammePeriod
            // 
            this.tkbProgrammePeriod.Location = new System.Drawing.Point(141, 85);
            this.tkbProgrammePeriod.Maximum = 14;
            this.tkbProgrammePeriod.Name = "tkbProgrammePeriod";
            this.tkbProgrammePeriod.Size = new System.Drawing.Size(263, 45);
            this.tkbProgrammePeriod.TabIndex = 15;
            this.tkbProgrammePeriod.Value = 1;
            this.tkbProgrammePeriod.Scroll += new System.EventHandler(this.tkbProgrammePeriod_Scroll);
            // 
            // tkbProgrammeItems
            // 
            this.tkbProgrammeItems.Location = new System.Drawing.Point(142, 50);
            this.tkbProgrammeItems.Maximum = 5000;
            this.tkbProgrammeItems.Minimum = 1;
            this.tkbProgrammeItems.Name = "tkbProgrammeItems";
            this.tkbProgrammeItems.Size = new System.Drawing.Size(263, 45);
            this.tkbProgrammeItems.TabIndex = 14;
            this.tkbProgrammeItems.Value = 1;
            this.tkbProgrammeItems.Scroll += new System.EventHandler(this.tkbProgrammeItems_Scroll);
            // 
            // tkbScheduleItems
            // 
            this.tkbScheduleItems.Location = new System.Drawing.Point(141, 17);
            this.tkbScheduleItems.Maximum = 5000;
            this.tkbScheduleItems.Minimum = 1;
            this.tkbScheduleItems.Name = "tkbScheduleItems";
            this.tkbScheduleItems.Size = new System.Drawing.Size(263, 45);
            this.tkbScheduleItems.TabIndex = 13;
            this.tkbScheduleItems.Value = 1;
            this.tkbScheduleItems.Scroll += new System.EventHandler(this.tkbScheduleItems_Scroll);
            // 
            // lblScheduleItems
            // 
            this.lblScheduleItems.AutoSize = true;
            this.lblScheduleItems.Location = new System.Drawing.Point(6, 17);
            this.lblScheduleItems.Name = "lblScheduleItems";
            this.lblScheduleItems.Size = new System.Drawing.Size(125, 13);
            this.lblScheduleItems.TabIndex = 5;
            this.lblScheduleItems.Text = "Schedule Items/Request";
            // 
            // ckIncludeYesterday
            // 
            this.ckIncludeYesterday.AutoSize = true;
            this.ckIncludeYesterday.Location = new System.Drawing.Point(6, 117);
            this.ckIncludeYesterday.Name = "ckIncludeYesterday";
            this.ckIncludeYesterday.Size = new System.Drawing.Size(111, 17);
            this.ckIncludeYesterday.TabIndex = 16;
            this.ckIncludeYesterday.Text = "Include Yesterday";
            this.ckIncludeYesterday.UseVisualStyleBackColor = true;
            this.ckIncludeYesterday.CheckedChanged += new System.EventHandler(this.ckIncludeYesterday_CheckedChanged);
            // 
            // txtProgrammePeriod
            // 
            this.txtProgrammePeriod.Location = new System.Drawing.Point(410, 85);
            this.txtProgrammePeriod.Name = "txtProgrammePeriod";
            this.txtProgrammePeriod.Size = new System.Drawing.Size(70, 20);
            this.txtProgrammePeriod.TabIndex = 4;
            this.txtProgrammePeriod.Validated += new System.EventHandler(this.txtProgrammePeriod_Validated);
            // 
            // lbProgrammePeriod
            // 
            this.lbProgrammePeriod.AutoSize = true;
            this.lbProgrammePeriod.Location = new System.Drawing.Point(6, 88);
            this.lbProgrammePeriod.Name = "lbProgrammePeriod";
            this.lbProgrammePeriod.Size = new System.Drawing.Size(115, 13);
            this.lbProgrammePeriod.TabIndex = 1;
            this.lbProgrammePeriod.Text = "Retrieval Period (Days)";
            // 
            // gbXmlTVAttr
            // 
            this.gbXmlTVAttr.Controls.Add(this.cbDisplayNameMode);
            this.gbXmlTVAttr.Controls.Add(this.lblChannelDisplayName);
            this.gbXmlTVAttr.Controls.Add(this.ckProgrammeID);
            this.gbXmlTVAttr.Controls.Add(this.ckShowType);
            this.gbXmlTVAttr.Controls.Add(this.ckStationAfiliate);
            this.gbXmlTVAttr.Controls.Add(this.ckStationCallsign);
            this.gbXmlTVAttr.Controls.Add(this.ckStationName);
            this.gbXmlTVAttr.Controls.Add(this.ckStationID);
            this.gbXmlTVAttr.Controls.Add(this.ckLogicalChannelNo);
            this.gbXmlTVAttr.Controls.Add(this.ckMD5);
            this.gbXmlTVAttr.Location = new System.Drawing.Point(6, 52);
            this.gbXmlTVAttr.Name = "gbXmlTVAttr";
            this.gbXmlTVAttr.Size = new System.Drawing.Size(368, 140);
            this.gbXmlTVAttr.TabIndex = 1;
            this.gbXmlTVAttr.TabStop = false;
            this.gbXmlTVAttr.Text = "Schedules Direct XMLTV Attributes";
            // 
            // cbDisplayNameMode
            // 
            this.cbDisplayNameMode.FormattingEnabled = true;
            this.cbDisplayNameMode.Items.AddRange(new object[] {
            "Same as channel-id.",
            "Station ID",
            "Station Name",
            "Station Afiliate",
            "Station Callsign"});
            this.cbDisplayNameMode.Location = new System.Drawing.Point(157, 111);
            this.cbDisplayNameMode.Name = "cbDisplayNameMode";
            this.cbDisplayNameMode.Size = new System.Drawing.Size(205, 21);
            this.cbDisplayNameMode.TabIndex = 14;
            this.cbDisplayNameMode.SelectedIndexChanged += new System.EventHandler(this.cbDisplayNameMode_SelectedIndexChanged);
            // 
            // lblChannelDisplayName
            // 
            this.lblChannelDisplayName.AutoSize = true;
            this.lblChannelDisplayName.Location = new System.Drawing.Point(6, 114);
            this.lblChannelDisplayName.Name = "lblChannelDisplayName";
            this.lblChannelDisplayName.Size = new System.Drawing.Size(145, 13);
            this.lblChannelDisplayName.TabIndex = 13;
            this.lblChannelDisplayName.Text = "Channel display-name source";
            // 
            // ckProgrammeID
            // 
            this.ckProgrammeID.AutoSize = true;
            this.ckProgrammeID.Checked = true;
            this.ckProgrammeID.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckProgrammeID.Enabled = false;
            this.ckProgrammeID.Location = new System.Drawing.Point(214, 19);
            this.ckProgrammeID.Name = "ckProgrammeID";
            this.ckProgrammeID.Size = new System.Drawing.Size(145, 17);
            this.ckProgrammeID.TabIndex = 9;
            this.ckProgrammeID.Text = "Programme ID (Required)";
            this.ckProgrammeID.UseVisualStyleBackColor = true;
            // 
            // ckShowType
            // 
            this.ckShowType.AutoSize = true;
            this.ckShowType.Location = new System.Drawing.Point(214, 65);
            this.ckShowType.Name = "ckShowType";
            this.ckShowType.Size = new System.Drawing.Size(141, 17);
            this.ckShowType.TabIndex = 11;
            this.ckShowType.Text = "Show Type (programme)";
            this.ckShowType.UseVisualStyleBackColor = true;
            this.ckShowType.CheckedChanged += new System.EventHandler(this.ckShowType_CheckedChanged);
            // 
            // ckStationAfiliate
            // 
            this.ckStationAfiliate.AutoSize = true;
            this.ckStationAfiliate.Location = new System.Drawing.Point(214, 42);
            this.ckStationAfiliate.Name = "ckStationAfiliate";
            this.ckStationAfiliate.Size = new System.Drawing.Size(136, 17);
            this.ckStationAfiliate.TabIndex = 10;
            this.ckStationAfiliate.Text = "Station Affiliate (station)";
            this.ckStationAfiliate.UseVisualStyleBackColor = true;
            this.ckStationAfiliate.CheckedChanged += new System.EventHandler(this.ckStationAfiliate_CheckedChanged);
            // 
            // ckStationCallsign
            // 
            this.ckStationCallsign.AutoSize = true;
            this.ckStationCallsign.Location = new System.Drawing.Point(214, 88);
            this.ckStationCallsign.Name = "ckStationCallsign";
            this.ckStationCallsign.Size = new System.Drawing.Size(138, 17);
            this.ckStationCallsign.TabIndex = 12;
            this.ckStationCallsign.Text = "Station Callsign (station)";
            this.ckStationCallsign.UseVisualStyleBackColor = true;
            this.ckStationCallsign.CheckedChanged += new System.EventHandler(this.ckStationCallsign_CheckedChanged);
            // 
            // ckStationName
            // 
            this.ckStationName.AutoSize = true;
            this.ckStationName.Location = new System.Drawing.Point(6, 88);
            this.ckStationName.Name = "ckStationName";
            this.ckStationName.Size = new System.Drawing.Size(130, 17);
            this.ckStationName.TabIndex = 8;
            this.ckStationName.Text = "Station Name (station)";
            this.ckStationName.UseVisualStyleBackColor = true;
            this.ckStationName.CheckedChanged += new System.EventHandler(this.ckStationName_CheckedChanged);
            // 
            // ckStationID
            // 
            this.ckStationID.AutoSize = true;
            this.ckStationID.Location = new System.Drawing.Point(6, 65);
            this.ckStationID.Name = "ckStationID";
            this.ckStationID.Size = new System.Drawing.Size(113, 17);
            this.ckStationID.TabIndex = 7;
            this.ckStationID.Text = "Station ID (station)";
            this.ckStationID.UseVisualStyleBackColor = true;
            this.ckStationID.CheckedChanged += new System.EventHandler(this.ckStationID_CheckedChanged);
            // 
            // ckLogicalChannelNo
            // 
            this.ckLogicalChannelNo.AutoSize = true;
            this.ckLogicalChannelNo.Location = new System.Drawing.Point(6, 42);
            this.ckLogicalChannelNo.Name = "ckLogicalChannelNo";
            this.ckLogicalChannelNo.Size = new System.Drawing.Size(172, 17);
            this.ckLogicalChannelNo.TabIndex = 6;
            this.ckLogicalChannelNo.Text = "Logical Channel Number (Map)";
            this.ckLogicalChannelNo.UseVisualStyleBackColor = true;
            this.ckLogicalChannelNo.CheckedChanged += new System.EventHandler(this.ckLogicalChannelNo_CheckedChanged);
            // 
            // ckMD5
            // 
            this.ckMD5.AutoSize = true;
            this.ckMD5.Checked = true;
            this.ckMD5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckMD5.Enabled = false;
            this.ckMD5.Location = new System.Drawing.Point(6, 19);
            this.ckMD5.Name = "ckMD5";
            this.ckMD5.Size = new System.Drawing.Size(157, 17);
            this.ckMD5.TabIndex = 5;
            this.ckMD5.Text = "Programme MD5 (Required)";
            this.ckMD5.UseVisualStyleBackColor = true;
            // 
            // gbPersistentCache
            // 
            this.gbPersistentCache.Controls.Add(this.btnBrowseCache);
            this.gbPersistentCache.Controls.Add(this.ckPersistentCache);
            this.gbPersistentCache.Controls.Add(this.txtCacheFilename);
            this.gbPersistentCache.Controls.Add(this.lbCacheFilename);
            this.gbPersistentCache.Location = new System.Drawing.Point(6, 6);
            this.gbPersistentCache.Name = "gbPersistentCache";
            this.gbPersistentCache.Size = new System.Drawing.Size(860, 40);
            this.gbPersistentCache.TabIndex = 0;
            this.gbPersistentCache.TabStop = false;
            this.gbPersistentCache.Text = "Persistent Cache";
            // 
            // btnBrowseCache
            // 
            this.btnBrowseCache.Location = new System.Drawing.Point(652, 10);
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
            this.ckPersistentCache.Location = new System.Drawing.Point(748, 15);
            this.ckPersistentCache.Name = "ckPersistentCache";
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
            this.txtCacheFilename.Size = new System.Drawing.Size(551, 20);
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
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
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
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
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
            this.gbChannelIDMap.ResumeLayout(false);
            this.gbChannelIDMap.PerformLayout();
            this.gbAddedChannels.ResumeLayout(false);
            this.gbAvailableChannels.ResumeLayout(false);
            this.tpGeneral1.ResumeLayout(false);
            this.gbXMLTVFile.ResumeLayout(false);
            this.gbXMLTVFile.PerformLayout();
            this.gbRetrievalOptions.ResumeLayout(false);
            this.gbRetrievalOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tkbProgrammePeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tkbProgrammeItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tkbScheduleItems)).EndInit();
            this.gbXmlTVAttr.ResumeLayout(false);
            this.gbXmlTVAttr.PerformLayout();
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
        private System.Windows.Forms.Button btnCustomGrid;
        private System.Windows.Forms.GroupBox gbAddedChannels;
        private System.Windows.Forms.GroupBox gbAvailableChannels;
        private System.Windows.Forms.GroupBox gbXmlTVAttr;
        private System.Windows.Forms.CheckBox ckProgrammeID;
        private System.Windows.Forms.CheckBox ckShowType;
        private System.Windows.Forms.CheckBox ckStationAfiliate;
        private System.Windows.Forms.CheckBox ckStationCallsign;
        private System.Windows.Forms.CheckBox ckStationName;
        private System.Windows.Forms.CheckBox ckStationID;
        private System.Windows.Forms.CheckBox ckLogicalChannelNo;
        private System.Windows.Forms.CheckBox ckMD5;
        private System.Windows.Forms.GroupBox gbRetrievalOptions;
        private System.Windows.Forms.TextBox txtProgrammePeriod;
        private System.Windows.Forms.Label lbProgrammePeriod;
        private System.Windows.Forms.TrackBar tkbProgrammePeriod;
        private System.Windows.Forms.Label lbDateRangeInfo;
        private System.Windows.Forms.CheckBox ckIncludeYesterday;
        private System.Windows.Forms.GroupBox gbChannelIDMap;
        private System.Windows.Forms.Label lblProgrammeItems;
        private System.Windows.Forms.TextBox txtScheduleItems;
        private System.Windows.Forms.TextBox txtProgrammeItems;
        private System.Windows.Forms.TrackBar tkbProgrammeItems;
        private System.Windows.Forms.TrackBar tkbScheduleItems;
        private System.Windows.Forms.Label lblScheduleItems;
        private System.Windows.Forms.ComboBox cbDisplayNameMode;
        private System.Windows.Forms.Label lblChannelDisplayName;
        private System.Windows.Forms.GroupBox gbXMLTVFile;
        private System.Windows.Forms.Button btnBrowseXmlTVFile;
        private System.Windows.Forms.TextBox txtOutputXmlTVFile;
        private System.Windows.Forms.Label lblOutputXmlTVFile;
    }
}