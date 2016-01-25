using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntryInformation.Classes;
using System.Reflection;

namespace EntryInformation
{
    public partial class Simulation : Form
    {
        private Simulator simulator;
        public Timer MoveCarsTimer;
        private bool StopPressed;

        public Simulation(string StreetName, string Time,string date)
        {
            InitializeComponent();
            simulator = new Simulator(this);
            label1Time.Text = Time;
            labelStreetName.Text = StreetName;
            labelDate.Text = date;
            MoveCarsTimer = new Timer();

            MoveCarsTimer.Tick += new EventHandler(PaintCarMoving);
            MoveCarsTimer.Interval = 1;//change speed of the cars moving

            //setting the selected indexes at 0 in the beginning
            comboBoxColumns.SelectedIndex = 0;
            comboBoxRows.SelectedIndex = 0;
        }

        /// <summary>this explanation only for one crossing
        /// first check which trafficLight is green so which lane is allowed to move cars
        /// then check the other 3 lanes their incomingCars lane free spots and check if there is one available
        /// then calculate if there is cars coming in the outgoingCarsLane which has greenlight right now and if there is handle it by putting the incoming cars from totalcars array into the outgoingcarslane of the current greenlight lane
        /// As soon as one car in this lane reaches the stoplight stop point it calculate the random direction based on the percentage and based on this direction it will be assign to the first available spot if there is one and the 0 index of arrayOutgoingcars is gonna be null
        /// when moving the cars check the direction also so u know how to move( straight or something else)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PaintCarMoving(object sender, EventArgs e)
        {
            MoveCarsTimer.Stop();

            simulator.InvalidateCrossings();

            MoveCarsTimer.Start();  
        }

        private void gridPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void gridPanel_DragDrop(object sender, DragEventArgs e)
        {
            if(!simulator.AddCrossingInCell(e))
                MessageBox.Show("Space not available");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void buttonStart_Click(object sender, EventArgs e)//when stop is pressed also show the groupBoxes again
        {
            try
            {
                if (buttonStart.Text == "Start")
                {
                    if (!StopPressed)
                    {
                        simulator.Start();
                    }
                    MoveCarsTimer.Start();
                    buttonStart.Text = "Stop";
                    StopPressed = false;
                }
                else
                {
                    StopPressed = true;
                    MoveCarsTimer.Stop();
                    buttonStart.Text = "Start";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCreateGrid_Click(object sender, EventArgs e)
        {
            simulator.CreateGrid();
            buttonStart.Enabled = true;
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

        private void comboBoxLane_SelectedIndexChanged(object sender, EventArgs e)
        {
            simulator.ComboBoxLaneChanging();//disable or enable textboxinput depending on crossing
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                simulator.ConnectInfoFeeder(sender);
            }
            catch (Exception ex)
            {
                listBoxErrors.Items.Add(ex.Message);
            }
        }

        private void textBoxAmountOfCars_KeyPress(object sender, KeyPressEventArgs e)
        {
            //let you input only integer numbers
            if (!Char.IsDigit(e.KeyChar) && (e.KeyChar != '\b'))
                e.KeyChar = '\0';
        }

        private void buttonApplyToAll_Click(object sender, EventArgs e)
        {
            try
            {
                simulator.ConnectInfoFeeder(sender);
            }
            catch (Exception ex)
            {
                listBoxErrors.Items.Add(ex.Message);
            }
        }

        private void pictureBoxDelete_Click(object sender, EventArgs e)
        {
            listBoxErrors.Items.Clear();
            try
            {
                simulator.DeleteCrossing();
            }
            catch (NullReferenceException)
            {
                simulator.PopErrorWindow();
                listBoxErrors.Items.Add("No crossing selected");
            }
        }
    }
}
