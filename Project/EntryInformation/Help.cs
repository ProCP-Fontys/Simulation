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
    public partial class Help : Form
    {
        Simulation simulation;
        public Help(Simulation s)
        {
           
            InitializeComponent();
            simulation = s;
        }

        private void Help_FormClosed(object sender, FormClosedEventArgs e)
        {
            simulation.Show();
            
        }
    }
}
