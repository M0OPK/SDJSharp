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

        public frmOptions(ref DataCache datacache, ref Config dataconfig)
        {
            InitializeComponent();

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
            doLogin(txtLogin.Text, txtPassword.Text);
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
            doStatus();
            showCountries();
            updateControls(loggedIn);
        }

        private void doStatus()
        {
            lvAccountLineups.Items.Clear();
            var statusResponse = sdJS.GetStatus();

            if (statusResponse.lineups != null)
            {
                foreach (var lineup in statusResponse.lineups)
                {
                    var item = new ListViewItem(new string[] { lineup.lineup, lineup.modified.GetValueOrDefault(DateTime.Parse("1900-01-01")).Date.ToShortDateString() });
                    lvAccountLineups.Items.Add(item);
                }

                updateLineupsStatus(statusResponse.lineups.Count(), statusResponse.account.maxLineups, changesRemain);
            }

        }

        private void showCountries()
        {
            tvCountries.Nodes.Clear();
            var countryData = cache.GetCountryData(ref sdJS);

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
                gbAccountLineups.Text = string.Format("Account Lineups {0} of {1} changes remaining {2}", lineups.ToString(), maxlineups.ToString(), changestoday.ToString());
            else
                gbAccountLineups.Text = string.Format("Account Lineups {0} of {1}", lineups.ToString(), maxlineups.ToString());
        }

        private void lvAccountLineups_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvStations.Items.Clear();
            if (lvAccountLineups.SelectedItems.Count > 0)
            {
                // Key is first column
                string key = lvAccountLineups.SelectedItems[0].SubItems[0].Text;
                var map = cache.GetLineupData(ref sdJS, key);

                if (map != null)
                {
                    foreach (var station in map.stations)
                    {
                        ListViewItem item = new ListViewItem(new string[] { station.stationID, station.name });
                        lvStations.Items.Add(item);
                    }
                }
                btnDeleteLineup.Enabled = true;
            }
            else
                btnDeleteLineup.Enabled = false;

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
            var headendData = cache.GetHeadendData(ref sdJS, country, postcode);
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
                var response = sdJS.DeleteLineup(lvAccountLineups.SelectedItems[0].SubItems[0].Text);
                if (response != null && response.code == 0)
                {
                    changesRemain = int.Parse(response.changesRemaining);
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
            //cache.Save("F:\\SchedulesDirect\\persistentcache.xml");
            config.cacheFilename = txtCacheFilename.Text;
            config.PersistantCache = ckPersistentCache.Checked;
            config.SDUsername = txtLogin.Text;
            if (txtPassword.Text != passwordHashEntry)
                config.SDPasswordHash = sdJS.hashPassword(txtPassword.Text);
            config.TranslationMatrix = localTranslate;
            config.Save("SDGrabSharp.xml");
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
            foreach (var addedItem in localTranslate.Select(item => item.Value))
            {
                var localItem = localTranslate[string.Format("{0},{1}", addedItem.LineupID, addedItem.SDStationID)];
                if (localItem.displayNameHelper == null || localItem.displayNameHelper == string.Empty)
                {
                    var station = cache.GetLineupData(ref sdJS, localItem.LineupID).stations.Where(thisstation => thisstation.stationID == localItem.SDStationID).FirstOrDefault();
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
                    ListViewItem item = new ListViewItem();
                    item.Text = localItem.SDStationID;
                    item.SubItems.Add(localItem.displayNameHelper);
                    item.Tag = localItem.LineupID;
                    lvAddedChans.Items.Add(item);
                }
            }
        }

        private void showAvailableChannels()
        {
            if (cbLineup.SelectedText == null || (string)cbLineup.SelectedItem == string.Empty)
                return;

            var stationInfo = cache.GetLineupData(ref sdJS, (string)cbLineup.SelectedItem);

            lvAvailableChans.Items.Clear();
            foreach (var station in stationInfo.stations)
            {
                // Only add if not in added list
                ListViewItem item = new ListViewItem();
                item.Text = station.stationID;
                if (!localTranslate.ContainsKey(string.Format("{0},{1}", (string)cbLineup.SelectedItem, station.stationID)))
                {
                    item.SubItems.Add(station.name);
                    lvAvailableChans.Items.Add(item);
                }
            }
        }

        private void cbLineup_SelectedIndexChanged(object sender, EventArgs e)
        {
            showAvailableChannels();
            showAddedChannels();
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
            try { localItem = localTranslate[string.Format("{0},{1}", lineupID, stationID)]; } catch { };
            if (localItem == null)
                return;

            localItem.FieldMode = mode;
        }

        private string updateChannelNames(string lineupID, string stationID, bool addedMode = false, string customName = null)
        {
            string newName = string.Empty;
            txtStationID.Text = string.Empty;
            txtName.Text = string.Empty;
            txtAffiliate.Text = string.Empty;
            txtCallsign.Text = string.Empty;
            if (!rdCustom.Checked)
                txtCustom.Text = string.Empty;

            var stationInfo = cache.GetLineupData(ref sdJS, lineupID).stations.Where(station => station.stationID == stationID).FirstOrDefault();

            if (stationInfo != null)
            {
                txtStationID.Text = stationInfo.stationID;
                txtName.Text = stationInfo.name;
                txtAffiliate.Text = stationInfo.affiliate;
                txtCallsign.Text = stationInfo.callsign;
            }

            // Extra for added list
            if (addedMode)
            {
                Config.XmlTVTranslation localItem = null;
                try { localItem = localTranslate[string.Format("{0},{1}", lineupID, stationID)]; } catch { };
                if (localItem == null)
                    return null;

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
                            txtCustom.Text = localItem.CustomTranslate;  
                        //localItem.CustomTranslate = newName;
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

            foreach (ListViewItem chan in lvAvailableChans.SelectedItems)
            {
                lvAvailableChans.Items.Remove(chan);
                chan.Tag = (string)cbLineup.SelectedItem;
                lvAddedChans.Items.Add(chan);
                AddChannelToMatrix((string)cbLineup.SelectedItem, chan);
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
            try { localItem = localTranslate[key]; } catch { };

            // Undelete if so
            if (localItem != null)
                localItem.isDeleted = false;
            else
            {
                // Get SD Data for this station
                var stationData = cache.GetLineupData(ref sdJS, (string)cbLineup.SelectedItem).stations.Where(station => station.stationID == item.Text).FirstOrDefault();
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

            var stationData = cache.GetLineupData(ref sdJS, (string)cbLineup.SelectedItem).stations.Where(station => station.stationID == item.Text).FirstOrDefault();
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
            dialog.Filter = "XML Files (*.xml)|*.xml|All Files|*.*";
            dialog.FileName = initialFile;
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

        private void txtCustom_Leave(object sender, EventArgs e)
        {
        }
    }
}
