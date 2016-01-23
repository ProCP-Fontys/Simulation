using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntryInformation
{
    public partial class First : Form
    {
        public First()
        {
            InitializeComponent();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            string streetName = "Name: "+textBoxStreetName.Text;
            string Time = "Time: " + textBoxTimeFrom.Text + " Till " + textBoxTimeTo.Text;
            string date = "Date: " + dateTimePicker.Value.ToString();
            this.Hide();
            var form2 = new Simulation(streetName,Time,date);
            form2.Show();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
            
        }
    }
}
