using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchedulesDirect;
using SDGrabSharp.Common;
using SDGrabSharp.UI.Resources;

namespace SDGrabSharp.UI
{
    public partial class frmOptions : Form
    {
        private SDJson sdJS;
        private bool loggedIn = false;
        private int changesRemain;
        private DataCache cache;
        private Config config;
        private static string passwordHashEntry = "********";
        private Dictionary<string, Config.XmlTVTranslation> localTranslate;
        private bool autoControlUpdate;
        private List<ChannelLocationInfo> locationInfo;

        // Complex way to save a bit of time adding a channel back in the proper place
        internal class ChannelLocationInfo
        {
            internal int originalPosition;
            internal string lineUp;
            internal string stationID;
            internal bool isAvailable;

            internal ChannelLocationInfo(int originalposition, string lineup, string stationid, bool available)
            {
                originalPosition = originalposition;
                lineUp = lineup;
                stationID = stationid;
                isAvailable = available;
            }
        }

        public frmOptions(DataCache datacache, Config dataconfig)
        {
            InitializeComponent();
            Localize();

            Cursor.Current = Cursors.WaitCursor;

            // Setup tooltips
            ToolTip optionsTooltip = new ToolTip();
            optionsTooltip.SetToolTip(btnLogin, "Logs into Schedules direct. Required for most functions");

            config = dataconfig;
            cache = datacache;
            localTranslate = config.TranslationMatrix;
            string token = string.Empty;
            loggedIn = false;
            if (cache.tokenData != null && cache.tokenData.token != string.Empty)
            {
                loggedIn = true;
                token = cache.tokenData.token;
            }

            sdJS = new SDJson("SDGrabSharp/1.0", token);
            changesRemain = -1;

            // Set values from config
            setConfigFields();

            if (!loggedIn && txtLogin.Text != string.Empty)
            {
                if (txtPassword.Text == passwordHashEntry)
                    doLogin(txtLogin.Text, config.SDPasswordHash, true);
                else
                    doLogin(txtLogin.Text, txtPassword.Text);
            }
                    

            if (loggedIn)
                onLoggedIn();

            updateControls(loggedIn);
            Cursor.Current = Cursors.Default;
        }

        private void Localize()
        {
            tpSD1.Text = Strings.tpSD1Title;
            gbStations.Text = Strings.gbStations;
            btnStationCacheClear.Text = Strings.btnStationCacheClear;
            chStationID.Text = Strings.chStationID;
            chName.Text = Strings.chName;
            gbHeadends.Text = Strings.gbHeadends;
            btnHeadendCacheClear.Text = Strings.btnHeadendCacheClear;
            btnLineupAdd.Text = Strings.btnLineupAdd;
            gbAccountLineups.Text = Strings.gbAccountLineups;
            btnDeleteLineup.Text = Strings.btnDeleteLineup;
            chLineup.Text = Strings.chLineup;
            chModified.Text = Strings.chModified;
            grpLocation.Text = Strings.grpLocation;
            btnLocationCacheClear.Text = Strings.btnLocationCacheClear;
            btnHeadends.Text = Strings.btnHeadends;
            lbPostCode.Text = Strings.lbPostCode;
            gbCreds.Text = Strings.gbCreds;
            btnLogout.Text = Strings.btnLogout;
            btnLogin.Text = Strings.btnLogin;
            checkAlwaysAsk.Text = Strings.checkAlwaysAsk;
            lbPassword.Text = Strings.lbPassword;
            lbLogin.Text = Strings.lbLogin;
            tpXMLTV1.Text = Strings.tpXMLTV1;
            gbStationMap.Text = Strings.gbStationMap;
            gbChannelIDMap.Text = Strings.gbChannelIDMap;
            btnCustomGrid.Text = Strings.btnCustomGrid;
            rdCustom.Text = Strings.rdCustom;
            rdStation.Text = Strings.rdStation;
            rdCallsign.Text = Strings.rdCallsign;
            rdName.Text = Strings.rdName;
            rdAffiliate.Text = Strings.rdAffiliate;
            gbAddedChannels.Text = Strings.gbAddedChannels;
            chAddedID.Text = Strings.chAddedID;
            chAddedName.Text = Strings.chAddedName;
            gbAvailableChannels.Text = Strings.gbAvailableChannels;
            chAvailID.Text = Strings.chAvailID;
            chAvailName.Text = Strings.chAvailName;
            lblLineup.Text = Strings.lblLineup;
            tpGeneral1.Text = Strings.tpGeneral1;
            gbRetrievalOptions.Text = Strings.gbRetrievalOptions;
            ckIncludeYesterday.Text = Strings.ckIncludeYesterday;
            lbProgrammePeriod.Text = Strings.lbProgrammePeriod;
            gbXmlTVAttr.Text = Strings.gbXmlTVAttr;
            ckProgrammeID.Text = Strings.ckProgrammeID;
            ckShowType.Text = Strings.ckShowType;
            ckStationAfiliate.Text = Strings.ckStationAfiliate;
            ckStationCallsign.Text = Strings.ckStationCallsign;
            ckStationName.Text = Strings.ckStationName;
            ckStationID.Text = Strings.ckStationID;
            ckLogicalChannelNo.Text = Strings.ckLogicalChannelNo;
            ckMD5.Text = Strings.ckMD5;
            gbPersistentCache.Text = Strings.gbPersistentCache;
            btnBrowseCache.Text = Strings.btnBrowseCache;
            ckPersistentCache.Text = Strings.ckPersistentCache;
            lbCacheFilename.Text = Strings.lbCacheFilename;
            btnSaveAs.Text = Strings.btnSaveAs;
            btnCancel.Text = Strings.btnCancel;
            btnSave.Text = Strings.btnSave;
            btnOK.Text = Strings.btnOK;
            this.Text = Strings.frmOptionsTitle;
            lblScheduleItems.Text = Strings.lblScheduleItems;
            lblProgrammeItems.Text = Strings.lblProgrammeItems;
            lblChannelDisplayName.Text = Strings.lblChannelDisplayName;
            gbXMLTVFile.Text = Strings.gbXMLTVFile;
            lblOutputXmlTVFile.Text = Strings.lblOutputXmlTVFile;
            btnBrowseXmlTVFile.Text = Strings.btnBrowseXmlTVFile;
        }

        private void checkAlwaysAsk_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAlwaysAsk.Checked)
            {
                txtLogin.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                txtLogin.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == passwordHashEntry)
                doLogin(txtLogin.Text, config.SDPasswordHash, true);
            else
                doLogin(txtLogin.Text, txtPassword.Text, false);
        }

        private void doLogin(string username, string password, bool isHash = false)
        {
            if (username != string.Empty && password != string.Empty)
            {
                var loginResponse = sdJS.Login(username, password, isHash);
                if (loginResponse != null && loginResponse.code == 0)
                {
                    loggedIn = true;
                    cache.tokenData = loginResponse;
                    onLoggedIn();
                }

                if (sdJS.HasErrors)
                    reportErrors();
            }

        }

        private void setConfigFields()
        {
            if (!config.LoginAlwaysAsk)
            {
                txtLogin.Text = config.SDUsername;
                txtPassword.Text = passwordHashEntry;
            }

            checkAlwaysAsk.Checked = config.LoginAlwaysAsk;

            ckPersistentCache.Checked = config.PersistantCache;
            txtCacheFilename.Text = config.cacheFilename;
        }

        private void updateControls(bool loginState)
        {
            txtLogin.Enabled = !loginState;
            txtPassword.Enabled = !loginState;
            btnLogin.Enabled = !loginState;
            checkAlwaysAsk.Enabled = !loginState;
            btnLocationCacheClear.Enabled = loginState;
            btnHeadendCacheClear.Enabled = loginState;
            btnStationCacheClear.Enabled = loginState;
            btnLogout.Enabled = loginState;
            btnDeleteLineup.Enabled = loginState;
            txtPostCode.Enabled = loginState;
            gbStationMap.Enabled = loginState;

            if (!loginState)
            {
                tvCountries.Nodes.Clear();
                btnHeadends.Enabled = false;
                tvHeadends.Nodes.Clear();
                btnLineupAdd.Enabled = false;
                lvAccountLineups.Items.Clear();
                btnDeleteLineup.Enabled = false;
                lvStations.Items.Clear();
            }
            else
            {
                if (tvCountries.SelectedNode != null && tvCountries.SelectedNode.Parent != null && txtPostCode.Text != string.Empty)
                    btnHeadends.Enabled = true;

                if (lvAccountLineups.SelectedItems.Count > 0)
                    btnDeleteLineup.Enabled = true;

                if (tvHeadends.SelectedNode != null && tvHeadends.SelectedNode.Parent != null)
                    btnLineupAdd.Enabled = true;
            }
        }

        private void onLoggedIn()
        {
            if (!doStatus())
            {
                loggedIn = false;
                updateControls(loggedIn);
                return;
            }

            showCountries();
            updateControls(loggedIn);
        }

        private bool doStatus()
        {
            sdJS.ClearErrors();
            var statusResponse = sdJS.GetStatus();

            // Try to resolve invalid token state
            if (sdJS.HasErrors)
            {
                var forbiddenError = sdJS.GetLastError();
                if (forbiddenError.isException && forbiddenError.message.Contains("(403)"))
                {
                    loggedIn = false;
                    if (config.SDUsername != string.Empty && config.SDPasswordHash != string.Empty)
                    {
                        doLogin(config.SDUsername, config.SDPasswordHash, true);
                        if (loggedIn)
                            statusResponse = sdJS.GetStatus();
                    }
                }
            }

            if (loggedIn)
            {
                if (statusResponse != null && statusResponse.lineups != null)
                {
                    // Remove translations for lineups that don't exist
                    validateTranslate(localTranslate, statusResponse.lineups.Select(line => line.lineup).ToArray());

                    lvAccountLineups.Items.Clear();
                    foreach (var lineup in statusResponse.lineups)
                    {
                        var item = new ListViewItem();
                        item.Text = lineup.lineup;
                        item.SubItems.Add(lineup.modified.GetValueOrDefault().Date.ToShortDateString());
                        lvAccountLineups.Items.Add(item);
                    }

                    updateLineupsStatus(statusResponse.lineups.Count(), statusResponse.account.maxLineups, changesRemain);
                    sdJS.ClearErrors();
                    return true;
                }
            }

            if (sdJS.HasErrors)
                reportErrors();

            return false;
        }

        private void showCountries()
        {
            tvCountries.Nodes.Clear();
            var countryData = cache.GetCountryData(sdJS);

            foreach (var continent in countryData.continents)
            {
                var thisContinent = tvCountries.Nodes.Add(continent.continentname);
                foreach (var country in continent.countries)
                {
                    var thisCountry = thisContinent.Nodes.Add(country.shortName, country.fullName);

                    foreach (var thisPostCode in cache.headendData.Where(line => line.Key.Split(',')[0] == country.shortName).Select(line => line.Key.Split(',')[1]))
                        thisCountry.Nodes.Add(thisPostCode, thisPostCode);
                }
            }
        }

        private void updateLineupsStatus(int lineups, int maxlineups, int changestoday)
        {
            if (changestoday >= 0)
                gbAccountLineups.Text = string.Format(Strings.gbAccountLineupsRemaining, lineups.ToString(), maxlineups.ToString(), changestoday.ToString());
            else
                gbAccountLineups.Text = string.Format(Strings.gbAccountLineupsNormal, lineups.ToString(), maxlineups.ToString());
        }

        private void lvAccountLineups_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            lvStations.SuspendLayout();
            lvStations.Items.Clear();
            if (lvAccountLineups.SelectedItems.Count > 0)
            {
                // Key is first column
                string key = lvAccountLineups.SelectedItems[0].SubItems[0].Text;
                var map = cache.GetLineupData(sdJS, key);

                if (map != null)
                {
                    foreach (var station in map.stations)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = station.stationID;
                        item.SubItems.Add(station.name);
                        lvStations.Items.Add(item);
                    }
                }
                btnDeleteLineup.Enabled = true;
            }
            else
                btnDeleteLineup.Enabled = false;

            lvStations.ResumeLayout(true);
            Cursor.Current = Cursors.Default;
        }

        private void tvCountries_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvCountries.SelectedNode != null && tvCountries.SelectedNode.Parent != null && tvCountries.SelectedNode.Name != "" && txtPostCode.Text != string.Empty)
                btnHeadends.Enabled = true;
            else
                btnHeadends.Enabled = false;

            if (tvCountries.SelectedNode != null && tvCountries.SelectedNode.Parent != null && tvCountries.SelectedNode.Name == tvCountries.SelectedNode.Text)
                showHeadEnds(tvCountries.SelectedNode.Parent.Name, tvCountries.SelectedNode.Text);
        }

        private void txtPostCode_TextChanged(object sender, EventArgs e)
        {
            if (tvCountries.SelectedNode != null && tvCountries.SelectedNode.Parent != null && txtPostCode.Text != string.Empty)
                btnHeadends.Enabled = true;
            else
                btnHeadends.Enabled = false;
        }

        private void btnHeadends_Click(object sender, EventArgs e)
        {
            if (tvCountries.SelectedNode.Name == tvCountries.SelectedNode.Text && tvCountries.SelectedNode.Parent != null)
                tvCountries.SelectedNode = tvCountries.SelectedNode.Parent;

            showHeadEnds(tvCountries.SelectedNode.Name, txtPostCode.Text);

            if (!tvCountries.SelectedNode.Nodes.ContainsKey(txtPostCode.Text))
                tvCountries.SelectedNode.Nodes.Add(txtPostCode.Text, txtPostCode.Text);
        }

        private void showHeadEnds(string country, string postcode)
        {
            var headendData = cache.GetHeadendData(sdJS, country, postcode);
            if (headendData != null)
            {
                tvHeadends.Nodes.Clear();

                foreach (var headend in headendData)
                {
                    var node = tvHeadends.Nodes.Add(string.Format("{0}/{1}", headend.location, headend.headend));

                    foreach (var lineup in headend.lineups)
                        node.Nodes.Add(lineup.lineup, lineup.name);
                }
            }
        }

        private void tvHeadends_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvHeadends.SelectedNode.Parent != null)
                btnLineupAdd.Enabled = true;
            else
                btnLineupAdd.Enabled = false;
        }

        private void btnLineupAdd_Click(object sender, EventArgs e)
        {
            if (tvHeadends.SelectedNode != null)
            {
                var result = sdJS.AddLineup(tvHeadends.SelectedNode.Name);
                if (result != null && result.code == 0)
                    changesRemain = int.Parse(result.changesRemaining);

                doStatus();
            }
        }

        private void btnDeleteLineup_Click(object sender, EventArgs e)
        {
            if (lvAccountLineups.SelectedItems.Count == 1)
            {
                string lineupID = lvAccountLineups.SelectedItems[0].SubItems[0].Text;
                var response = sdJS.DeleteLineup(lineupID);
                if (response != null && response.code == 0)
                {
                    changesRemain = int.Parse(response.changesRemaining);

                    // Delete all translations for this lineup
                    var lineupItems = localTranslate.Where(line => line.Value.LineupID == lineupID).ToArray();
                    foreach (var lineupItem in lineupItems)
                        localTranslate.Remove(lineupItem.Key);
                }

                doStatus();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            loggedIn = false;
            if (cache.tokenData != null)
                cache.tokenData.token = string.Empty;

            updateControls(loggedIn);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig("SDGrabSharp.xml");
        }

        private void SaveConfig(string filename)
        {
            config.cacheFilename = txtCacheFilename.Text;
            config.XmlTVFileName = txtOutputXmlTVFile.Text;
            config.PersistantCache = ckPersistentCache.Checked;
            config.SDUsername = txtLogin.Text;
            if (txtPassword.Text != passwordHashEntry)
                config.SDPasswordHash = sdJS.hashPassword(txtPassword.Text);
            config.TranslationMatrix = localTranslate;
            config.Save(filename);
            MessageBox.Show(this, Strings.ConfigSaved, "SDSharp", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLocationCacheClear_Click(object sender, EventArgs e)
        {
            cache.countryData = null;
            showCountries();
        }

        private void btnHeadendCacheClear_Click(object sender, EventArgs e)
        {
            cache.headendData = new Dictionary<string, IEnumerable<SDHeadendsResponse>>();
            tvHeadends.Nodes.Clear();
            showCountries();
        }

        private void btnStationCacheClear_Click(object sender, EventArgs e)
        {
            cache.stationMapData = new Dictionary<string, SDGetLineupResponse>();
            lvStations.Items.Clear();
        }

        private void tpXMLTV1_Enter(object sender, EventArgs e)
        {
            // Run when user chooses XMLTV tab
            if (!loggedIn)
                return;

            SDStatusResponse status = sdJS.GetStatus();
            cbLineup.Items.Clear();
            if (status.lineups.Count() > 0)
            {
                foreach (var lineup in status.lineups)
                    cbLineup.Items.Add(lineup.lineup);

                cbLineup.SelectedIndex = 0;
            }
            updateControlsXmlTV();
            rdCustom.Enabled = false;
            rdAffiliate.Enabled = false;
            rdCallsign.Enabled = false;
            rdStation.Enabled = false;
            rdName.Enabled = false;
        }

        private void showAddedChannels()
        {
            if (cbLineup.SelectedItem == null)
                return;

            lvAddedChans.Items.Clear();

            // LINQ query to ensure list is as close to original order from SD as possible
            var addedList =
            (
                from localtrans in localTranslate
                join locinfo in locationInfo
                  on new
                  {
                      joinLineup = localtrans.Value.LineupID,
                      joinStation = localtrans.Value.SDStationID
                  }
                  equals new
                  {
                      joinLineup = locinfo.lineUp,
                      joinStation = locinfo.stationID
                  }
                where !localtrans.Value.isDeleted
                  &&  localtrans.Value.LineupID == (string)cbLineup.SelectedItem
                orderby locinfo.originalPosition
                select localtrans.Value
            );

            foreach (var addedItem in addedList)
            {
                var localItem = localTranslate[addedItem.SDStationID];
                if (localItem.displayNameHelper == null || localItem.displayNameHelper == string.Empty)
                {
                    var station = cache.GetLineupData(sdJS, localItem.LineupID).stations.Where(thisstation => thisstation.stationID == localItem.SDStationID).FirstOrDefault();
                    if (station != null)
                    {
                        switch (localItem.FieldMode)
                        {
                            case Config.XmlTVTranslation.TranslateField.StationID:
                                localItem.displayNameHelper = station.stationID;
                                break;
                            case Config.XmlTVTranslation.TranslateField.StationAffiliate:
                                localItem.displayNameHelper = station.affiliate;
                                break;
                            case Config.XmlTVTranslation.TranslateField.StationCallsign:
                                localItem.displayNameHelper = station.callsign;
                                break;
                            case Config.XmlTVTranslation.TranslateField.StationName:
                                localItem.displayNameHelper = station.name;
                                break;
                            case Config.XmlTVTranslation.TranslateField.Custom:
                                localItem.displayNameHelper = localItem.CustomTranslate;
                                break;
                        }
                    }
                }
                ListViewItem item = new ListViewItem();
                item.Text = localItem.SDStationID;
                item.SubItems.Add(localItem.displayNameHelper);
                item.Tag = (string)localItem.LineupID;
                lvAddedChans.Items.Add(item);
            }
        }

        private void showAvailableChannels(string filter = "")
        {
            if (cbLineup.SelectedText == null || (string)cbLineup.SelectedItem == string.Empty)
                return;

            var stationInfo = cache.GetLineupData(sdJS, (string)cbLineup.SelectedItem);
            if (stationInfo == null)
                return;

            locationInfo = new List<ChannelLocationInfo>();

            lvAvailableChans.Items.Clear();
            int originalPosition = 0;
            foreach (var station in stationInfo.stations)
            {
                ChannelLocationInfo thisInfo = new ChannelLocationInfo(originalPosition, (string)cbLineup.SelectedItem, station.stationID, false);

                if (filter == string.Empty || station.name.ToLower().Contains(filter.ToLower()))
                {
                    // Only add if not in added list
                    ListViewItem item = new ListViewItem();
                    item.Text = station.stationID;
                    if (!localTranslate.ContainsKey(station.stationID) || localTranslate[station.stationID].isDeleted)
                    {
                        item.SubItems.Add(station.name);
                        lvAvailableChans.Items.Add(item);
                        thisInfo.isAvailable = true;
                    }
                }
                locationInfo.Add(thisInfo);
                originalPosition++;
            }
        }

        private void cbLineup_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            lvAvailableChans.SuspendLayout();
            lvAddedChans.SuspendLayout();
            showAvailableChannels();
            showAddedChannels();
            lvAvailableChans.ResumeLayout(true);
            lvAddedChans.ResumeLayout(true);
            Cursor.Current = Cursors.Default;
        }

        private void lvAvailableChans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAvailableChans.SelectedItems.Count == 1)
                updateChannelNames((string)cbLineup.SelectedItem, lvAvailableChans.SelectedItems[0].Text);

            updateControlsXmlTV();
        }

        private void setChannelMode(string lineupID, string stationID, Config.XmlTVTranslation.TranslateField mode)
        {
            Config.XmlTVTranslation localItem = null;
            try { localItem = localTranslate[stationID]; } catch { };
            if (localItem == null)
                return;

            localItem.FieldMode = mode;

            var station = cache.GetLineupData(sdJS, localItem.LineupID).stations.
                Where(line => line.stationID == localItem.SDStationID).FirstOrDefault();

            switch (localItem.FieldMode)
            {
                case Config.XmlTVTranslation.TranslateField.StationID:
                    localItem.displayNameHelper = station.stationID;
                    break;
                case Config.XmlTVTranslation.TranslateField.StationAffiliate:
                    localItem.displayNameHelper = station.affiliate;
                    break;
                case Config.XmlTVTranslation.TranslateField.StationCallsign:
                    localItem.displayNameHelper = station.callsign;
                    break;
                case Config.XmlTVTranslation.TranslateField.StationName:
                    localItem.displayNameHelper = station.name;
                    break;
                case Config.XmlTVTranslation.TranslateField.Custom:
                    localItem.displayNameHelper = localItem.CustomTranslate;
                    break;
            }

        }

        private string updateChannelNames(string lineupID, string stationID, bool addedMode = false, string customName = null)
        {
            string newName = string.Empty;
            txtStationID.Text = string.Empty;
            txtName.Text = string.Empty;
            txtAffiliate.Text = string.Empty;
            txtCallsign.Text = string.Empty;

            if (lineupID == null)
                return null;

            var stationInfo = cache.GetLineupData(sdJS, lineupID).stations.Where(station => station.stationID == stationID).FirstOrDefault();

            if (stationInfo != null)
            {
                txtStationID.Text = stationInfo.stationID;
                txtName.Text = stationInfo.name;
                txtAffiliate.Text = stationInfo.affiliate;
                txtCallsign.Text = stationInfo.callsign;
            }

            var mapInfo = cache.GetLineupData(sdJS, lineupID).map.Where(map => map.stationID == stationID).FirstOrDefault();

            if (mapInfo != null)
            {
                txtLogicalChannel.Text = mapInfo.logicalChannelNumber;
                txtChannelNum.Text = mapInfo.channel;
            }

            // Extra for added list
            if (addedMode)
            {
                Config.XmlTVTranslation localItem = null;
                try { localItem = localTranslate[stationID]; } catch { };
                if (localItem == null)
                    return null;

                if (localItem.FieldMode != Config.XmlTVTranslation.TranslateField.Custom)
                    txtCustom.Text = string.Empty;

                rdCustom.Enabled = true;
                rdAffiliate.Enabled = true;
                rdCallsign.Enabled = true;
                rdStation.Enabled = true;
                rdName.Enabled = true;

                autoControlUpdate = true;
                switch (localItem.FieldMode)
                {
                    case Config.XmlTVTranslation.TranslateField.Custom:
                        rdCustom.Checked = true;
                        if (customName != null)
                        {
                            newName = customName;
                            localItem.CustomTranslate = newName;
                        }
                        else
                        {
                            txtCustom.Text = localItem.CustomTranslate;
                            newName = localItem.CustomTranslate;
                        }
                        break;
                    case Config.XmlTVTranslation.TranslateField.StationAffiliate:
                        rdAffiliate.Checked = true;
                        newName = stationInfo.affiliate;
                        break;
                    case Config.XmlTVTranslation.TranslateField.StationCallsign:
                        rdCallsign.Checked = true;
                        newName = stationInfo.callsign;
                        break;
                    case Config.XmlTVTranslation.TranslateField.StationID:
                        rdStation.Checked = true;
                        newName = stationInfo.stationID;
                        break;
                    case Config.XmlTVTranslation.TranslateField.StationName:
                        rdName.Checked = true;
                        newName = stationInfo.name;
                        break;
                }
                autoControlUpdate = false;
            }
            else
            {
                txtCustom.Text = string.Empty;
                autoControlUpdate = true;
                rdCustom.Checked = false;
                rdAffiliate.Checked = false;
                rdCallsign.Checked = false;
                rdStation.Checked = false;
                rdName.Checked = false;
                autoControlUpdate = false;

                rdCustom.Enabled = false;
                rdAffiliate.Enabled = false;
                rdCallsign.Enabled = false;
                rdStation.Enabled = false;
                rdName.Enabled = false;

            }
            return newName;
        }

        private void updateControlsXmlTV()
        {
            if (lvAvailableChans.SelectedItems.Count == 0)
                btnAddChan.Enabled = false;
            else
                btnAddChan.Enabled = true;

            if (lvAddedChans.SelectedItems.Count == 0)
                btnRemoveChan.Enabled = false;
            else
                btnRemoveChan.Enabled = true;

            if (lvAvailableChans.Items.Count == 0)
                btnAddAllChans.Enabled = false;
            else
                btnAddAllChans.Enabled = true;

            if (lvAddedChans.Items.Count == 0)
                btnRemoveAllChans.Enabled = false;
            else
                btnRemoveAllChans.Enabled = true;

            if (lvAddedChans.SelectedItems.Count == 1 && rdCustom.Checked)
                txtCustom.Enabled = true;
            else
                txtCustom.Enabled = false;
        }

        private void btnAddChan_Click(object sender, EventArgs e)
        {
            if (lvAvailableChans.SelectedItems.Count == 0)
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                lvAddedChans.BeginUpdate();
                lvAvailableChans.BeginUpdate();
                foreach (ListViewItem chan in lvAvailableChans.SelectedItems)
                {
                    AddAddedChannel((string)cbLineup.SelectedItem, chan);
                    AddChannelToMatrix((string)cbLineup.SelectedItem, chan);
                }
            }
            finally
            {
                lvAddedChans.EndUpdate();
                lvAvailableChans.EndUpdate();
                Cursor.Current = Cursors.Default;
            }
        }

        private void lvAddedChans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAddedChans.SelectedItems.Count == 1)
                updateChannelNames((string)lvAddedChans.SelectedItems[0].Tag, lvAddedChans.SelectedItems[0].Text, true);

            updateControlsXmlTV();

        }

        private void AddChannelToMatrix(string lineup, ListViewItem item)
        {
            string key = string.Format("{0},{1}", lineup, item.Text);

            // First see if a deleted version exists
            Config.XmlTVTranslation localItem = null;
            try { localItem = localTranslate[item.Text]; } catch { };

            // Undelete if so
            if (localItem != null)
                localItem.isDeleted = false;
            else
            {
                // Get SD Data for this station
                var stationData = cache.GetLineupData(sdJS, (string)cbLineup.SelectedItem).stations.Where(station => station.stationID == item.Text).FirstOrDefault();
                if (stationData == null)
                    return;

                localItem = new Config.XmlTVTranslation();
                localItem.LineupID = (string)cbLineup.SelectedItem;
                localItem.SDStationID = stationData.stationID;
                localItem.isDeleted = false;
                localItem.FieldMode = config.defaultTranslateMode;
                switch(localItem.FieldMode)
                {
                    case Config.XmlTVTranslation.TranslateField.StationID:
                        localItem.displayNameHelper = stationData.stationID;
                        break;
                    case Config.XmlTVTranslation.TranslateField.StationAffiliate:
                        localItem.displayNameHelper = stationData.affiliate;
                        break;
                    case Config.XmlTVTranslation.TranslateField.StationCallsign:
                        localItem.displayNameHelper = stationData.callsign;
                        break;
                    case Config.XmlTVTranslation.TranslateField.StationName:
                        localItem.displayNameHelper = stationData.name;
                        break;
                    case Config.XmlTVTranslation.TranslateField.Custom:
                        localItem.displayNameHelper = txtCustom.Text;
                        break;
                }

                // Rewrite node channel name to match helper
                item.SubItems[1].Text = localItem.displayNameHelper;

                localTranslate.Add(key, localItem);
            }
        }

        private void updateChannelMatrix(ListViewItem item, Config.XmlTVTranslation.TranslateField mode, string customText="")
        {
            if (item == null)
                return;

            string key = item.Text;
            Config.XmlTVTranslation localItem = null;
            try { localItem = localTranslate[key]; } catch { };

            // Undelete if so
            if (localItem != null)
                return;

            localItem.FieldMode = mode;
            if (customText != string.Empty)
                localItem.CustomTranslate = customText;

            var stationData = cache.GetLineupData(sdJS, (string)cbLineup.SelectedItem).stations.Where(station => station.stationID == item.Text).FirstOrDefault();
            switch (localItem.FieldMode)
            {
                case Config.XmlTVTranslation.TranslateField.StationID:
                    localItem.displayNameHelper = stationData.stationID;
                    break;
                case Config.XmlTVTranslation.TranslateField.StationAffiliate:
                    localItem.displayNameHelper = stationData.affiliate;
                    break;
                case Config.XmlTVTranslation.TranslateField.StationCallsign:
                    localItem.displayNameHelper = stationData.callsign;
                    break;
                case Config.XmlTVTranslation.TranslateField.StationName:
                    localItem.displayNameHelper = stationData.name;
                    break;
                case Config.XmlTVTranslation.TranslateField.Custom:
                    localItem.displayNameHelper = txtCustom.Text;
                    break;
            }

            item.SubItems[1].Text = localItem.displayNameHelper;
        }

        private void updateGeneralControls()
        {
            ckPersistentCache.Checked = config.PersistantCache;
            lbCacheFilename.Enabled = config.PersistantCache;
            txtCacheFilename.Enabled = config.PersistantCache;
            btnBrowseCache.Enabled = config.PersistantCache;
            ckLogicalChannelNo.Checked = config.XmlTVLogicalChannelNumber;
            ckShowType.Checked = config.XmlTVShowType;
            ckStationAfiliate.Checked = config.XmlTVStationAfilliate;
            ckStationCallsign.Checked = config.XmlTVStationCallsign;
            ckStationID.Checked = config.XmlTVStationID;
            ckStationName.Checked = config.XmlTVStationName;
            ckIncludeYesterday.Checked = config.ProgrammeRetrieveYesterday;
            tkbProgrammePeriod.Value = config.ProgrammeRetrieveRangeDays;
            txtProgrammePeriod.Text = config.ProgrammeRetrieveRangeDays.ToString();
            tkbScheduleItems.Value = config.ScheduleRetrievalItems;
            txtScheduleItems.Text = config.ScheduleRetrievalItems.ToString();
            tkbProgrammeItems.Value = config.ProgrammeRetrievalItems;
            txtProgrammeItems.Text = config.ProgrammeRetrievalItems.ToString();
            txtOutputXmlTVFile.Text = config.XmlTVFileName;

            if ((int)config.XmlTVDisplayNameMode >= 0 && (int)config.XmlTVDisplayNameMode < cbDisplayNameMode.Items.Count)
                cbDisplayNameMode.SelectedIndex = (int)config.XmlTVDisplayNameMode;

            updateDateRange();
        }

        private void ckPersistentCache_CheckedChanged(object sender, EventArgs e)
        {
            config.PersistantCache = ckPersistentCache.Checked;
            updateGeneralControls();
        }

        private void tpGeneral1_Enter(object sender, EventArgs e)
        {
            updateGeneralControls();
        }

        private void btnBrowseCache_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            string folder = string.Empty;
            if (txtCacheFilename.Text != string.Empty)
                folder = new DirectoryInfo(txtCacheFilename.Text).Name;

            string initialFile = new FileInfo(config.cacheFilename).Name;
            if (initialFile == string.Empty)
                initialFile = "persistentcache.xml";

            dialog.InitialDirectory = folder;
            dialog.Filter = string.Format("{0}|*.xml|{1}|*.*", Strings.XmlFiles, Strings.AllFiles);
            dialog.FileName = initialFile;
            dialog.Title = Strings.SaveCacheDialogCaption;
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult.ToString() == "OK")
                txtCacheFilename.Text = dialog.FileName;
        }

        private void frmOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (config.PersistantCache && config.cacheFilename != string.Empty)
                cache.Save(config.cacheFilename);
        }

        private void rdStation_CheckedChanged(object sender, EventArgs e)
        {
            if (autoControlUpdate || lvAddedChans.SelectedItems.Count == 0)
                return;

            txtCustom.Enabled = false;

            foreach (ListViewItem addedChannel in lvAddedChans.SelectedItems)
            {
                setChannelMode((string)addedChannel.Tag, addedChannel.Text, Config.XmlTVTranslation.TranslateField.StationID);
                string newName = updateChannelNames((string)addedChannel.Tag, addedChannel.Text, true);
                addedChannel.SubItems[1].Text = newName;
            }
        }

        private void rdName_CheckedChanged(object sender, EventArgs e)
        {
            if (autoControlUpdate || lvAddedChans.SelectedItems.Count == 0)
                return;

            txtCustom.Enabled = false;

            foreach (ListViewItem addedChannel in lvAddedChans.SelectedItems)
            {
                setChannelMode((string)addedChannel.Tag, addedChannel.Text, Config.XmlTVTranslation.TranslateField.StationName);
                string newName = updateChannelNames((string)addedChannel.Tag, addedChannel.Text, true);
                addedChannel.SubItems[1].Text = newName;
            }

        }

        private void rdAffiliate_CheckedChanged(object sender, EventArgs e)
        {
            if (autoControlUpdate || lvAddedChans.SelectedItems.Count == 0)
                return;

            txtCustom.Enabled = false;

            foreach (ListViewItem addedChannel in lvAddedChans.SelectedItems)
            {
                setChannelMode((string)addedChannel.Tag, addedChannel.Text, Config.XmlTVTranslation.TranslateField.StationAffiliate);
                string newName = updateChannelNames((string)addedChannel.Tag, addedChannel.Text, true);
                addedChannel.SubItems[1].Text = newName;
            }
        }

        private void rdCallsign_CheckedChanged(object sender, EventArgs e)
        {
            if (autoControlUpdate || lvAddedChans.SelectedItems.Count == 0)
                return;

            txtCustom.Enabled = false;

            foreach (ListViewItem addedChannel in lvAddedChans.SelectedItems)
            {
                setChannelMode((string)addedChannel.Tag, addedChannel.Text, Config.XmlTVTranslation.TranslateField.StationCallsign);
                string newName = updateChannelNames((string)addedChannel.Tag, addedChannel.Text, true);
                addedChannel.SubItems[1].Text = newName;
            }
        }

        private void rdCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (autoControlUpdate || lvAddedChans.SelectedItems.Count == 0)
                return;

            txtCustom.Enabled = true;

            foreach (ListViewItem addedChannel in lvAddedChans.SelectedItems)
            {
                setChannelMode((string)addedChannel.Tag, addedChannel.Text, Config.XmlTVTranslation.TranslateField.Custom);
                string newName = updateChannelNames((string)addedChannel.Tag, addedChannel.Text, true);
                addedChannel.SubItems[1].Text = newName;
            }
        }

        private void txtCustom_Validated(object sender, EventArgs e)
        {
            if (lvAddedChans.SelectedItems.Count != 1)
                return;

            ListViewItem addedItem = lvAddedChans.SelectedItems[0];
            string newName = updateChannelNames((string)addedItem.Tag, addedItem.Text, true, txtCustom.Text);
            addedItem.SubItems[1].Text = newName;
        }

        private void btnCustomGrid_Click(object sender, EventArgs e)
        {
            CustomGridEntry customGrid = new CustomGridEntry(sdJS, config, cache, localTranslate);
            customGrid.ShowDialog();
            showAddedChannels();
        }

        private void btnRemoveChan_Click(object sender, EventArgs e)
        {
            if (lvAddedChans.SelectedItems.Count == 0)
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                lvAvailableChans.BeginUpdate();
                lvAddedChans.BeginUpdate();
                foreach (ListViewItem chan in lvAddedChans.SelectedItems)
                    removeAddedChannel(chan);
            }
            finally
            {
                lvAddedChans.EndUpdate();
                lvAvailableChans.EndUpdate();
            }
        }

        private void removeAddedChannel(ListViewItem chan)
        {
            lvAddedChans.Items.Remove(chan);
            string translateKey = chan.Text;

            // Only add to left list, if selected lineup matches
            if ((string)cbLineup.SelectedItem == (string)chan.Tag)
            {
                var thisLocal = localTranslate.Select(line => line.Value).
                    Where(line => line.LineupID == (string)chan.Tag && line.SDStationID == chan.Text).FirstOrDefault();
                if (thisLocal != null)
                    thisLocal.isDeleted = true;

                // Try to find original location
                var thisOriginalLocation = locationInfo.
                    Where(line => line.lineUp == (string)chan.Tag && line.stationID == chan.Text).FirstOrDefault();

                ListViewItem nextLocation = null;

                if (thisOriginalLocation != null)
                {
                    nextLocation =
                        (from listItem in lvAvailableChans.Items.Cast<ListViewItem>()
                         join locInfo in locationInfo
                            on new
                            {
                                joinLineup = (string)cbLineup.SelectedItem,
                                joinStation = listItem.Text
                            }
                            equals new
                            {
                                joinLineup = locInfo.lineUp,
                                joinStation = locInfo.stationID
                            }
                         where locInfo.originalPosition > thisOriginalLocation.originalPosition
                            && locInfo.isAvailable
                         select listItem
                        ).FirstOrDefault();
                }

                chan.Tag = null;
                var stationInfo = cache.GetLineupData(sdJS, (string)cbLineup.SelectedItem).stations.Where(line => line.stationID == chan.Text).FirstOrDefault();

                if (stationInfo != null)
                    chan.SubItems[1].Text = stationInfo.name;

                // If the nearest remaining neighbour was found, insert before it, else it goes to the end.
                if (nextLocation != null)
                    lvAvailableChans.Items.Insert(nextLocation.Index, chan);
                else
                    lvAvailableChans.Items.Add(chan);

                if (thisOriginalLocation != null)
                    thisOriginalLocation.isAvailable = true;
            }
        }

        private void AddAddedChannel(string lineup, ListViewItem chan)
        {
            lvAvailableChans.Items.Remove(chan);

            string translateKey = chan.Text;

            // Only add to left list, if selected lineup matches
            if ((string)cbLineup.SelectedItem == lineup)
            {
                var thisLocal = localTranslate.Select(line => line.Value).
                    Where(line => line.LineupID == lineup && line.SDStationID == chan.Text).FirstOrDefault();

                if (thisLocal != null)
                    thisLocal.isDeleted = false;

                // Try to find original location
                var thisOriginalLocation = locationInfo.
                    Where(line => line.lineUp == lineup && line.stationID == chan.Text).FirstOrDefault();

                ListViewItem nextLocation = null;
                if (thisOriginalLocation != null)
                {
                    nextLocation =
                        (from listItem in lvAddedChans.Items.Cast<ListViewItem>()
                         join locInfo in locationInfo
                            on new
                            {
                                joinLineup = (string)cbLineup.SelectedItem,
                                joinStation = listItem.Text
                            }
                            equals new
                            {
                                joinLineup = locInfo.lineUp,
                                joinStation = locInfo.stationID
                            }
                         where locInfo.originalPosition > thisOriginalLocation.originalPosition
                            && !locInfo.isAvailable
                         select listItem
                        ).FirstOrDefault();
                }

                chan.Tag = lineup;
                var stationInfo = cache.GetLineupData(sdJS, (string)cbLineup.SelectedItem).stations.Where(line => line.stationID == chan.Text).FirstOrDefault();

                if (stationInfo != null)
                    chan.SubItems[1].Text = stationInfo.name;

                // If the nearest remaining neighbour was found, insert before it, else it goes to the end.
                if (nextLocation != null)
                    lvAddedChans.Items.Insert(nextLocation.Index, chan);
                else
                    lvAddedChans.Items.Add(chan);

                if (thisOriginalLocation != null)
                    thisOriginalLocation.isAvailable = false;
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            string folder = string.Empty;
            if (txtCacheFilename.Text != string.Empty)
                folder = new DirectoryInfo(txtCacheFilename.Text).Name;

            string initialFile = new FileInfo(config.cacheFilename).Name;
            if (initialFile == string.Empty)
                initialFile = "SDGrabSharp.xml";

            dialog.InitialDirectory = folder;
            dialog.Title = Strings.SaveConfigDialogCaption;
            dialog.Filter = string.Format("{0}|*.xml|{1}|*.*", Strings.XmlFiles, Strings.AllFiles);
            dialog.FileName = initialFile;
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult.ToString() == "OK")
                SaveConfig(dialog.FileName);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ckStationCallsign_CheckedChanged(object sender, EventArgs e)
        {
            config.XmlTVStationCallsign = ckStationCallsign.Checked;
        }

        private void ckStationID_CheckedChanged(object sender, EventArgs e)
        {
            config.XmlTVStationID = ckStationID.Checked;
        }

        private void ckStationName_CheckedChanged(object sender, EventArgs e)
        {
            config.XmlTVStationName = ckStationName.Checked;
        }

        private void ckStationAfiliate_CheckedChanged(object sender, EventArgs e)
        {
            config.XmlTVStationAfilliate = ckStationAfiliate.Checked;
        }

        private void ckShowType_CheckedChanged(object sender, EventArgs e)
        {
            config.XmlTVShowType = ckShowType.Checked;
        }

        private void ckLogicalChannelNo_CheckedChanged(object sender, EventArgs e)
        {
            config.XmlTVLogicalChannelNumber = ckLogicalChannelNo.Checked;
        }

        private void tkbProgrammePeriod_Scroll(object sender, EventArgs e)
        {
            txtProgrammePeriod.Text = tkbProgrammePeriod.Value.ToString();
            config.ProgrammeRetrieveRangeDays = tkbProgrammePeriod.Value;
            updateDateRange();
        }

        private void updateDateRange()
        {
            DateTime dateMin = DateTime.Today.Date;
            DateTime dateMax = dateMin.AddDays(tkbProgrammePeriod.Value);

            if (ckIncludeYesterday.Checked)
                dateMin = dateMin.AddDays(-1.0f);

            lbDateRangeInfo.Text = string.Format(Strings.lbDateRangeInfo,
                dateMin.ToShortDateString(), dateMax.ToShortDateString());
        }

        private void txtProgrammePeriod_Validated(object sender, EventArgs e)
        {
            int newValue = 0;
            if (int.TryParse(txtProgrammePeriod.Text, out newValue))
            {
                if (newValue >= tkbProgrammePeriod.Minimum && newValue <= tkbProgrammePeriod.Maximum)
                {
                    tkbProgrammePeriod.Value = newValue;
                    config.ProgrammeRetrieveRangeDays = newValue;
                }
                else
                    txtProgrammePeriod.Text = tkbProgrammePeriod.Value.ToString();
                updateDateRange();
            }
        }

        private void ckIncludeYesterday_CheckedChanged(object sender, EventArgs e)
        {
            config.ProgrammeRetrieveYesterday = ckIncludeYesterday.Checked;
            updateDateRange();
        }

        private void btnAddAllChans_Click(object sender, EventArgs e)
        {
            if (lvAvailableChans.Items.Count == 0)
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                lvAddedChans.BeginUpdate();
                lvAvailableChans.BeginUpdate();
                foreach (ListViewItem chan in lvAvailableChans.Items)
                {
                    AddAddedChannel((string)cbLineup.SelectedItem, chan);
                    AddChannelToMatrix((string)cbLineup.SelectedItem, chan);
                }
            }
            finally
            {
                lvAddedChans.EndUpdate();
                lvAvailableChans.EndUpdate();
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnRemoveAllChans_Click(object sender, EventArgs e)
        {
            if (lvAddedChans.Items.Count == 0)
                return;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                lvAddedChans.BeginUpdate();
                lvAvailableChans.BeginUpdate();
                foreach (ListViewItem chan in lvAddedChans.Items)
                    removeAddedChannel(chan);
            }
            finally
            {
                lvAddedChans.EndUpdate();
                lvAvailableChans.EndUpdate();
                Cursor.Current = Cursors.Default;
            }
        }

        private void tkbProgrammeItems_Scroll(object sender, EventArgs e)
        {
            txtProgrammeItems.Text = tkbProgrammeItems.Value.ToString();
            config.ProgrammeRetrievalItems = tkbProgrammeItems.Value;
        }

        private void tkbScheduleItems_Scroll(object sender, EventArgs e)
        {
            txtScheduleItems.Text = tkbScheduleItems.Value.ToString();
            config.ScheduleRetrievalItems = tkbScheduleItems.Value;
        }

        private void txtScheduleItems_Validated(object sender, EventArgs e)
        {
            int newValue = 0;
            if (int.TryParse(txtScheduleItems.Text, out newValue))
            {
                if (newValue >= tkbScheduleItems.Minimum && newValue <= tkbScheduleItems.Maximum)
                {
                    tkbScheduleItems.Value = newValue;
                    config.ScheduleRetrievalItems = newValue;
                }
                else
                    txtScheduleItems.Text = tkbScheduleItems.Value.ToString();
            }
        }

        private void txtProgrammeItems_Validated(object sender, EventArgs e)
        {
            int newValue = 0;
            if (int.TryParse(txtProgrammeItems.Text, out newValue))
            {
                if (newValue >= tkbProgrammeItems.Minimum && newValue <= tkbProgrammeItems.Maximum)
                {
                    tkbProgrammeItems.Value = newValue;
                    config.ProgrammeRetrievalItems = newValue;
                }
                else
                    txtProgrammeItems.Text = tkbProgrammeItems.Value.ToString();
            }
        }

        private void cbDisplayNameMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.XmlTVDisplayNameMode = (Config.DisplayNameMode)cbDisplayNameMode.SelectedIndex;
        }

        private void btnBrowseXmlTVFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            string folder = string.Empty;
            if (txtOutputXmlTVFile.Text != string.Empty)
                folder = new DirectoryInfo(txtOutputXmlTVFile.Text).Name;

            string initialFile = new FileInfo(config.cacheFilename).Name;
            if (initialFile == string.Empty)
                initialFile = "guide.xml";

            dialog.InitialDirectory = folder;
            dialog.Filter = string.Format("{0}|*.xml|{1}|*.*", Strings.XmlFiles, Strings.AllFiles);
            dialog.FileName = initialFile;
            dialog.Title = Strings.SaveXmlTvFileDialog;
            var dialogResult = dialog.ShowDialog(this);

            if (dialogResult.ToString() == "OK")
                txtOutputXmlTVFile.Text = dialog.FileName;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Reload old config, if cancelling
            if (System.IO.File.Exists("SDGrabSharp.xml"))
                config.Load("SDGrabSharp.xml");

            this.Close();
        }

        private void validateTranslate(Dictionary<string, Config.XmlTVTranslation> translateData, string[] lineupList)
        {
            var removeList = translateData.Where(line => !lineupList.Any(lineup => lineup == line.Value.LineupID)).ToArray();
            foreach (var lineup in removeList)
                translateData.Remove(lineup.Key);
        }

        private void reportErrors()
        {
            var exceptions = sdJS.GetRawErrors();
            string errors = string.Empty;
            foreach (var ex in exceptions)
            {
                errors += ex.message + "\r\n";
            }
            sdJS.ClearErrors();

            if (errors != string.Empty)
                MessageBox.Show(this, errors, "SDJSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void txtChannelFilter_TextChanged(object sender, EventArgs e)
        {
            showAvailableChannels(txtChannelFilter.Text);
        }
    }
}
