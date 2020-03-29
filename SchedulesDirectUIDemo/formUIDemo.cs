using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchedulesDirect;

namespace SchedulesDirect.UIDemo
{
    public partial class formUIDemo : Form
    {
        private SDCountries countryList;
        private IEnumerable<SDHeadendsResponse> headEnds;
        private readonly SDJson sd;
        private int mode;

        public formUIDemo()
        {
            InitializeComponent();
            sd = new SDJson();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var tokenResponse = sd.Login(txtLogin.Text, txtPassword.Text);

            if (tokenResponse == null)
            {
                reportErrors();
                return;
            }

            var result = sd.GetStatus();
            if (result != null)
                rtResult.Text =
                    $"{result.systemStatus.FirstOrDefault().status}\r\n{result.systemStatus.FirstOrDefault().message}";
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            rtResult.Clear();

            var serviceList = sd.GetAvailable();

            if (serviceList == null)
            {
                reportErrors();
                return;
            }

            var serviceText = "Services:\r\n";
            foreach (var service in serviceList)
            {
                serviceText += $"{service.type} - {service.description}: {service.uri}\r\n";
            }
            rtResult.Text = serviceText;
        }

        private void btnCountries_Click(object sender, EventArgs e)
        {
            countryList = sd.GetCountries();

            foreach (var continent in countryList.continents)
            {
                lbContinents.Items.Add(continent.continentname);
            }

            if (lbContinents.Items.Count > 0)
                lbContinents.SelectedIndex = 0;

            mode = 1;
        }

        private void btnTransmitters_Click(object sender, EventArgs e)
        {
            var txList = sd.GetTransmitters("GB");

            if (txList == null)
            {
                reportErrors();
                return;
            }

            lbContinents.Items.Clear();
            lbCountries.Items.Clear();

            foreach (var tx in txList)
            {
                lbContinents.Items.Add($"{tx.transmitterArea}\t{tx.transmitterID}");
            }
            mode = 2;
        }

        private void btnHeadends_Click(object sender, EventArgs e)
        {
            headEnds = sd.GetHeadends("USA", "10001");
            if (headEnds == null)
            {
                reportErrors();
                return;
            }

            lbContinents.Items.Clear();
            lbCountries.Items.Clear();
            foreach (var headEnd in headEnds)
            {
                if (headEnd == null)
                    continue;

                lbContinents.Items.Add($"{headEnd.headend}\t{headEnd.location}\t{headEnd.transport}");
            }
            mode = 3;
            if (lbContinents.Items.Count > 0)
                lbContinents.SelectedIndex = 0;
        }

        private void btnAddLineup_Click(object sender, EventArgs e)
        {
            var result = sd.AddLineup("USA-DITV501-DEFAULT");
            //var result = sd.AddLineup("GBR-ABC-AB123S");
            //if (mode == 3)
            //{
            //    var headEnd = headEnds.Where(head => head.headend )

        }

        private void btnListLineups_Click(object sender, EventArgs e)
        {
            var result = sd.GetLineups();
        }

        private void btnGetLineup_Click(object sender, EventArgs e)
        {
            var results = sd.GetLineup("USA-DITV501-DEFAULT", true);
        }

        private void btnGetSchedule_Click(object sender, EventArgs e)
        {
            var reqs = new List<SDScheduleRequest>();
            var req = new SDScheduleRequest("45399", 
                new DateTime[] { DateTime.Parse("2016-11-29"), DateTime.Parse("2016-11-30") }.AsEnumerable());
            reqs.Add(req);
            var req2 = new SDScheduleRequest("82547", 
                new DateTime[] { DateTime.Parse("2016-11-29"), DateTime.Parse("2016-11-30") }.AsEnumerable());
            reqs.Add(req2);
            var result = sd.GetSchedules(reqs.AsEnumerable());
        }

        private void btnGetProgram_Click(object sender, EventArgs e)
        {
            string[] programs = { "EP008403901236", "SP003484480000" };

            var result = sd.GetProgrammes(programs);
            var results = sd.GetDescriptions(programs);
        }

        private void btnGetMD5_Click(object sender, EventArgs e)
        {
            var reqs = new List<SDMD5Request>();
            var req = new SDMD5Request("16689", 
                new DateTime[] { DateTime.Parse("2016-11-29"), DateTime.Parse("2016-11-30") }.AsEnumerable() );
            reqs.Add(req);
            var result = sd.GetMD5(reqs.AsEnumerable());
        }

        private void lbContinents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mode == 1)
                ShowCountries(lbContinents.SelectedItem.ToString());
            else if (mode == 3)
                ShowLineups(lbContinents.SelectedItem.ToString());
        }

        private void ShowLineups(string headendline)
        {
            var id = headendline.Split('\t').FirstOrDefault();
            var lineups = (from headend in headEnds
                           where headend.headend == id
                           select headend.lineups).FirstOrDefault();

            if (lineups == null)
                return;

            lbCountries.Items.Clear();
            foreach (var lineup in lineups)
            {
                lbCountries.Items.Add($"{lineup.lineup}\t{lineup.name}\t{lineup.uri}");
            }
        }

        private void ShowCountries(string continent)
        {
            lbCountries.Items.Clear();
            if (countryList == null)
                return;

            foreach (var country in countryList.continents.Where(cont => cont.continentname == continent).FirstOrDefault().countries)
            {
                lbCountries.Items.Add(country.fullName);
            }
        }

        private void reportErrors()
        {
            var exceptions = sd.GetRawErrors();
            var errors = string.Empty;
            foreach (var ex in exceptions)
            {
                errors += ex.message + "\r\n";
            }
            sd.ClearErrors();

            if (errors != string.Empty)
                MessageBox.Show(this, errors, "SDJSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnDeleteMsg_Click(object sender, EventArgs e)
        {
            var result = sd.DeleteMessage("101");
        }

        private void btnGetLive_Click(object sender, EventArgs e)
        {
            var result = sd.GetStillRunning("SP003484480000");
        }

        private void btnGetLogo_Click(object sender, EventArgs e)
        {
            var result = sd.GetProgramMetadata(new string[] { "EP000014577789", "EP000000351331", "EP008403901236", "SP003484480000" });
        }
    }
}
