using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ORGnice
{
    public partial class Dashboard : Form
    {
        private string user;
        private string role;

        public Dashboard(string user, string role)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            string greetingMessage = $"Welcome, {role} {user}!";
            MessageBox.Show(greetingMessage);
            LandingPage.BringToFront();
            UserGreetLbl.Text = $"Welcome {user}!";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Indicator.Height = dashboard_btn.Height;
            Indicator.Top = dashboard_btn.Top;
            LandingPage.Visible = true;

            if (LandingPage.Visible == true)
            {
                LandingPage.BringToFront();
                UserGreetLbl.Text = "Dashboard";
            }
        }


        private void members_btn_Click(object sender, EventArgs e)
        {
            Indicator.Height = Members_btn.Height;
            Indicator.Top = Members_btn.Top;

            members_tab.BringToFront();
            members_tab.Visible = true;
            UserGreetLbl.Text = "Manage Members";

        }

        private void events_btn_Click(object sender, EventArgs e)
        {
            Indicator.Height = Events_btn.Height;
            Indicator.Top = Events_btn.Top;

            event_tab.BringToFront(); 
            event_tab.Visible = true;
            UserGreetLbl.Text = "Manage Events";

        }

        private void money_btn_Click(object sender, EventArgs e)
        {
            Indicator.Height = Finance_btn.Height;
            Indicator.Top = Finance_btn.Top;
            UserGreetLbl.Text = "Manage Finances";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dashboard_btn_MouseHover(object sender, EventArgs e)
        {
            dashboard_btn.BackColor = Color.FromArgb(40, 39, 41);
        }

        private void events_btn_MouseHover(object sender, EventArgs e)
        {

            Events_btn.BackColor = Color.FromArgb(40, 39, 41);

        }

        private void members_btn_MouseHover(object sender, EventArgs e)
        {
            Members_btn.BackColor = Color.FromArgb(40, 39, 41);

        }

        private void Finance_btn_MouseHover(object sender, EventArgs e)
        {
            Finance_btn.BackColor = Color.FromArgb(40, 39, 41);
        }
    }
}