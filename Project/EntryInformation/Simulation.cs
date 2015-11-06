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
    public partial class Simulation : Form
    {
        public Simulation()
        {
            InitializeComponent();
        }

        public void calculatePanelSize(int nrOfRows, int nrOfColumns)
        {
            int height = nrOfRows * 200;
            int width = nrOfColumns * 200;

            this.gridPanel.Size = new Size(width, height);
            this.gridGroupBox.Size = new Size(width + 10, height + 30);
        }

        private void frm_Resize(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GridCreation();
        }

        public void GridCreation()
        {
            calculatePanelSize(Convert.ToInt16(comboBox1.SelectedItem), Convert.ToInt16(comboBox2.SelectedItem));
            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Black);
            System.Drawing.Graphics formGraphics = gridPanel.CreateGraphics();

            int i2 = 200;

            //drawing rows
            for (int i = 0; i < Convert.ToInt16(comboBox1.SelectedItem) ; i++)
            {

                //if(gridPanel.Height > (200*i))
                //formGraphics.DrawLine(myPen, gridPanel.Width, gridPanel.Height + (200 * i), gridPanel.Width, (200 * Convert.ToInt16(comboBox2.SelectedItem)) );
                formGraphics.DrawLine(myPen, 0, i2, gridPanel.Width, i2);
                i2 += 200;
            }

            i2 = 200;
            //drawing columns
            for (int i = 0; i < Convert.ToInt16(comboBox2.SelectedItem) ; i++)
            {

                //if(gridPanel.Height > (200*i))
                //formGraphics.DrawLine(myPen, gridPanel.Width, gridPanel.Height + (200 * i), gridPanel.Width, (200 * Convert.ToInt16(comboBox2.SelectedItem)) );
                formGraphics.DrawLine(myPen, i2, 0, i2, gridPanel.Height);
                i2 += 200;
            }

            //formGraphics.DrawLine(myPen, 0, 125, 660, 125);
            //formGraphics.DrawLine(myPen, 0, 250, 660, 250);
            //formGraphics.DrawLine(myPen, 0, 375, 660, 375);

            //formGraphics.DrawLine(myPen, 220, 0, 220, 520);
            //formGraphics.DrawLine(myPen, 440, 0, 440, 520);
            ////formGraphics.DrawLine(myPen, 0, 375, 660, 375);
            myPen.Dispose();
            formGraphics.Dispose();
        }

        private void gridPanel_Paint(object sender, PaintEventArgs e)
        {
            GridCreation();
        }

        private void Simulation_Resize(object sender, EventArgs e)
        {
            //if (this.ParentForm.WindowState == FormWindowState.Normal &&
             //   this.WindowState == FormWindowState.Maximized)
           // {
             //   this.ParentForm.WindowState = FormWindowState.Maximized;
            //}
        }
    }
}
