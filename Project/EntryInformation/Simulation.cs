﻿using System;
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
        private bool PauzePressed;
        private bool stopPressed;
        public int PauzeCountDown { get; set; }

        //menue attribute test
        bool isSaved = false;
        Simulation simulation;
        public String filePath;

        public Simulation(string StreetName, string Time,string date)
        {
            InitializeComponent();
            this.Text = "Simulation Magic!   Street:" + StreetName+"."+ filePath;
            simulator = new Simulator(this);
            label1Time.Text = Time;
            labelStreetName.Text = StreetName;
            labelDate.Text = date;
            MoveCarsTimer = new Timer();

            MoveCarsTimer.Tick += new EventHandler(PaintCarMoving);
            MoveCarsTimer.Interval = 50;//change speed of the cars moving

            //setting the selected indexes at 0 in the beginning
            comboBoxColumns.SelectedIndex = 0;
            comboBoxRows.SelectedIndex = 0;
            //menu
            filePath = "Miki.bin";
            simulation = this;
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
                    //saveToolStripMenuItem_Click(sender,e);
                    timerSImulationOn.Start();
                    if (!stopPressed)
                    {
                        if (!PauzePressed)
                        {
                            simulator.Start();
                        }
                        else
                        {
                            simulator.ReStart();
                        }
                        buttonStart.Text = "Pauze";
                        PauzePressed = false;
                    }
                }
                else
                {
                    timerSImulationOn.Stop();
                    PauzePressed = true;
                    simulator.Pauze();
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
            stopPressed = false;
            PauzePressed = false;
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

        public void buttonStop_Click(object sender, EventArgs e)
        {
            stopPressed = true;
            labelGreenLPed.Visible = false;
            simulator.Stop();
            //openToolStripMenuItem_Click(sender, e);
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form help = new Help(this);
            help.Show();

        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }
        void saveFile()
        {
            simulator.saveGridOutput();


            //    if (outputStream != null)
            //    {
            //        BinaryFormatter bf = new BinaryFormatter();

            //        bf.Serialize(outputStream, simulation.gri);

            //        isSaved = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: 1save file: " + ex.Message);
            //}
            //finally
            //{
            //    outputStream.Close();
            //}
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isSaved)
            {
                DialogResult msgResult = MessageBox.Show("You have unsaved changes open, do you want to save these?", "", MessageBoxButtons.YesNoCancel);

                if (msgResult == System.Windows.Forms.DialogResult.Yes)
                {
                    if (filePath != null)
                    {
                        saveFile();
                        simulator.openGridInput();
                    }
                    else
                    {
                        saveASToolStripMenuItem_Click(sender, e);
                        simulator.openGridInput();
                    }
                }
                else if (msgResult == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else if (msgResult == System.Windows.Forms.DialogResult.No)
                    simulator.openGridInput();
            }

       
        }

        private void saveASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simulator.saveAsGridOutput();
        }

        private void timerSImulationOn_Tick(object sender, EventArgs e)
        {
            labelTotallCars.Text = "Total cars:"+simulator.totalAmountOfCars.ToString();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isSaved)
            {
                DialogResult msgResult = MessageBox.Show("You have unsaved changes open, do you want to save these?", "", MessageBoxButtons.YesNoCancel);

                if (msgResult == System.Windows.Forms.DialogResult.Yes)
                {
                    if (filePath != null)
                    {
                        saveFile();
                        simulator.OpenNewFile();
                    }
                    else
                    {
                        saveASToolStripMenuItem_Click(sender, e);
                        simulator.openGridInput();
                    }
                }
                else if (msgResult == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else if (msgResult == System.Windows.Forms.DialogResult.No)
                    simulator.OpenNewFile();
            }


            
        }

        private void trackBarChangeSpeed_MouseEnter(object sender, EventArgs e)
        {
            simulation.Cursor = Cursors.Hand;
        }

        private void trackBarChangeSpeed_MouseLeave(object sender, EventArgs e)
        {
            simulation.Cursor = Cursors.Cross;
        }

        private void trackBarChangeSpeed_Scroll(object sender, EventArgs e)
        {
            MoveCarsTimer.Interval = trackBarChangeSpeed.Value;
        }
    }
}
