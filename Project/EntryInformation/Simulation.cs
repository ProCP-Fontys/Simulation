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
        bool IsSimulationStarted = false;
        bool formationTab = false;
        
        private Simulator simulator;

        public Simulation()
        {
            InitializeComponent();
            simulator = new Simulator(this);
        }

        private void frm_Resize(object sender, EventArgs e)
        {
            
            if (!formationTab)
            {

                groupBox4.Visible = true;
                formationTab = !formationTab; ;
            }
            else
            {
                groupBox4.Visible = false;
                Form.ActiveForm.Width -= 250;

                formationTab = !formationTab; ;
            }
            //Deselect other crossings
            foreach (Control pb in gridPanel.Controls)
            {
                if (pb is PictureBox)
                {
                    ((PictureBox)pb).BorderStyle = BorderStyle.None;
                }

            }
        }

        private void gridPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        public void FormExpand(object sender, EventArgs e)
        {
            formationTab = false;
            this.frm_Resize(sender,e);
            
            ((PictureBox)sender).BorderStyle = BorderStyle.Fixed3D;
            
        }

        private void gridPanel_DragDrop(object sender, DragEventArgs e)
        {
            if(!simulator.AddCrossingInCell(e))
                MessageBox.Show("Space not available");
        }

        private void gridPanel_MouseMove(object sender, MouseEventArgs e)
        {
            label14.Text = "X = " + e.X + " and Y = " + e.Y;
        }

        private void Simulation_MouseMove(object sender, MouseEventArgs e)
        {
            label6.Text = "X = " + e.X + " and Y = " + e.Y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!formationTab)
            {

                groupBox4.Visible = true;
                formationTab = !formationTab; ;
            }
            else
            {
                groupBox4.Visible = false;
                Form.ActiveForm.Width -= 250;

                formationTab = !formationTab; ;
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {

            if (!IsSimulationStarted)
            {
                gridGroupBox.Enabled = false;
                groupBox1.Visible = false;

                groupBox2.Location = new Point(3, 36);
                groupBox2.Height += 448;
                buttonStart.Location = new Point(18, 580);
                buttonStart.Text = "Stop";
                //groupBox2.Location
                groupBox4.Visible = false;
                groupBox3.Visible = false;
                groupBox5.Visible = false;
                IsSimulationStarted = !IsSimulationStarted;
            }
            else
            {

                groupBox1.Visible = true; ;
                groupBox2.Height -= 448;
                groupBox2.Location = new Point(7, 371);
                buttonStart.Text = "Start";
                buttonStart.Location = new Point(37, 123);
                //groupBox2.Location
                groupBox3.Visible = true;
                groupBox5.Visible = true;
                IsSimulationStarted = !IsSimulationStarted;
                this.Size = new Size(1050, 741);
                groupBox4.Visible = false;
                gridGroupBox.Enabled = true;
            }
        }

        private void BtnCreateGrid_Click(object sender, EventArgs e)
        {
            simulator.DrawGrid();
        }

        private void CB_MouseDown(object sender, MouseEventArgs e)
        {
            Image imageInBox = CB.Image;
            imageInBox.Tag = "CrossingB";
            CB.DoDragDrop(imageInBox, DragDropEffects.Copy);
        }

        private void CA_MouseDown(object sender, MouseEventArgs e)
        {
            Image imageInBox = CA.Image;
            imageInBox.Tag = "CrossingA";
            CA.DoDragDrop(imageInBox, DragDropEffects.Copy);
        }
    }
}
