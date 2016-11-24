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
        public formUIDemo()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SDJSON sd = new SDJSON();
            var tokenResponse = sd.Login(txtLogin.Text, txtPassword.Text);
            if (tokenResponse != null)
            {
                var result = sd.GetStatus();
                if (result != null)
                    rtResult.Text = string.Format("{0}\r\n{1}", result.systemStatus.FirstOrDefault().status, result.systemStatus.FirstOrDefault().message);
            }
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            rtResult.Clear();
            SDJSON sd = new SDJSON();

            var serviceList = sd.GetAvailable();

            if (serviceList != null)
            {
                string serviceText = "Services:\r\n";
                foreach (var service in serviceList)
                {
                    serviceText += string.Format("{0} - {1}: {2}\r\n", service.type, service.description, service.uri);
                }
                rtResult.Text = serviceText;
            }
        }

        private void btnCountries_Click(object sender, EventArgs e)
        {
            SDJSON sd = new SDJSON();
            countryList = sd.GetCountries();

            foreach (var continent in countryList.continents)
            {
                lbContinents.Items.Add(continent.continentname);
            }

            if (lbContinents.Items.Count > 0)
                lbContinents.SelectedIndex = 0;
        }

        private void lbContinents_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowCountries(lbContinents.SelectedItem.ToString());
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
    }
}
