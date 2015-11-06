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
            this.Hide();
            var form2 = new Simulation();
            form2.Show();
            form2.Closed += (s, args) => this.Close();
            form2.Show();
            
        }
    }
}
