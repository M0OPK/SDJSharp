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
        SDCountries countryList;
        IEnumerable<SDHeadendsResponse> headEnds;
        SDJSON sd;
        int mode;

        public formUIDemo()
        {
            InitializeComponent();
            sd = new SDJSON();
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
                rtResult.Text = string.Format("{0}\r\n{1}", result.systemStatus.FirstOrDefault().status, result.systemStatus.FirstOrDefault().message);
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

            string serviceText = "Services:\r\n";
            foreach (var service in serviceList)
            {
                serviceText += string.Format("{0} - {1}: {2}\r\n", service.type, service.description, service.uri);
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

        private void lbContinents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mode == 1)
                ShowCountries(lbContinents.SelectedItem.ToString());
            else if (mode == 3)
                ShowLineups(lbContinents.SelectedItem.ToString());
        }

        private void ShowLineups(string headendline)
        {
            string id = headendline.Split('\t').FirstOrDefault();
            var lineups = (from headend in headEnds
                           where headend.headend == id
                           select headend.lineups).FirstOrDefault();

            if (lineups == null)
                return;

            lbCountries.Items.Clear();
            foreach (var lineup in lineups)
            {
                lbCountries.Items.Add(string.Format("{0}\t{1}\t{2}", lineup.lineup, lineup.name, lineup.uri));
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
                lbContinents.Items.Add(string.Format("{0}\t{1}", tx.transmitterArea, tx.transmitterID));
            }
            mode = 2;
        }

        private void btnHeadends_Click(object sender, EventArgs e)
        {
            headEnds = sd.GetHeadends("GBR", "RG457NW");
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

                lbContinents.Items.Add(string.Format("{0}\t{1}\t{2}", headEnd.headend, headEnd.location, headEnd.transport));
            }
            mode = 3;
            if (lbContinents.Items.Count > 0)
                lbContinents.SelectedIndex = 0;
        }

        private void reportErrors()
        {
            var exceptions = sd.GetRawErrors();
            string errors = string.Empty;
            foreach (var ex in exceptions)
            {
                errors += ex.Message + "\r\n";
            }

            if (errors != string.Empty)
                MessageBox.Show(this, errors, "SDJSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
