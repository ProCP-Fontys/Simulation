using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using EntryInformation.Classes;

namespace EntryInformation
{
    public partial class Simulation : Form
    {
        //bool IsSimulationStarted = false;
        bool formationTab = false;

        Crossing crossingTest;

        private Simulator simulator;
        System.Timers.Timer MoveCarsTimer = new System.Timers.Timer();
        //System.Timers.Timer trafficLightTimer = new System.Timers.Timer();
        //Car[] totalCars = new Car[10];
        //Car[] CarsGoingOut = new Car[5];
        //Car[] CarsComingIn = new Car[5];
        //List<Point> StopPointsGoingOut;
        //List<Point> StopPointsComingIn;
        //Neighbours neighbours = new Neighbours();

        public Simulation()
        {
            InitializeComponent();
            simulator = new Simulator(this);

            crossingTest = new CrossingB(0);

            MoveCarsTimer.Elapsed += new ElapsedEventHandler(PaintCarMoving);
            MoveCarsTimer.Interval = 100;

            //StopPointsGoingOut = new List<Point>();
            //StopPointsComingIn = new List<Point>();

            //for (int i = 4; i > -1; i--)
            //{
            //    this.StopPointsGoingOut.Add(new Point(Convert.ToInt16(i + "7"), 112));
            //}

            //for (int i = 186; i > 145; i -= 10)
            //{
            //    this.StopPointsComingIn.Add(new Point(i, 112));
            //}

            //for (int i = 0; i < 10; i++)
            //{
            //    totalCars[i] = new Car(new Point(-3, 112));
            //}



            //cars.Add(new Car(new Point(9, 112)));
            //cars.Add(new Car(new Point(-1, 112)));
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
        private void PaintCarMoving(object sender, ElapsedEventArgs e)
        {
            MoveCarsTimer.Stop();
            Feeder TrafficLightGreen = null;
            int freeSpotLane1Index;
            int freeSpotLane2Index;
            int freeSpotLane3Index = -1;
            int freeSpotLane4Index;

            Feeder feederSpotLane1;
            Feeder feederSpotLane2;
            Feeder feederSpotLane3 = null;
            Feeder feederSpotLane4;

            //calculate random direction now

            foreach (var item in crossingTest.Feeders)
            {
                if (item.TrafficLight.greenLightTimer.Enabled)
                    TrafficLightGreen = item;
                else
                {
                    switch (item.FeederID)
                    {
                        case 1:
                            freeSpotLane1Index = Array.IndexOf(item.CarsComingIn, null);
                            feederSpotLane1 = item;
                            break;
                        case 2:
                            freeSpotLane2Index = Array.IndexOf(item.CarsComingIn, null);
                            feederSpotLane2 = item;
                            break;
                        case 3:
                            freeSpotLane3Index = Array.IndexOf(item.CarsComingIn, null);
                            feederSpotLane3 = item;
                            break;
                        case 4:
                            freeSpotLane4Index = Array.IndexOf(item.CarsComingIn, null);
                            feederSpotLane4 = item;
                            break;
                    }
                }
            }

            if (TrafficLightGreen.CarsGoingOut[4] == null)
            {
                for (int i = 0; i < TrafficLightGreen.TotalCars.Length; i++)
                {
                    if (TrafficLightGreen.TotalCars[i] != null)
                    {
                        TrafficLightGreen.CarsGoingOut[4] = TrafficLightGreen.TotalCars[i];
                        TrafficLightGreen.TotalCars[i] = null;
                        break;
                    }
                }
            }
            PictureBox toDrawOn = (PictureBox)this.Controls.Find("Crossing" + crossingTest.CrossingID, true)[0];

            if (toDrawOn.InvokeRequired)
            {
                // after we've done all the processing, 
                toDrawOn.Invoke(new MethodInvoker(delegate
                {
                    toDrawOn.Refresh();
                }));
            }
            Graphics formGraphics = toDrawOn.CreateGraphics();

            for (int i = 0; i < TrafficLightGreen.CarsGoingOut.Length; i++)
            {
                if (TrafficLightGreen.CarsGoingOut[i] != null)
                {
                    if (TrafficLightGreen.CarsGoingOut[i].X == TrafficLightGreen.StopPointsGoingOut[i].X)
                    {
                        if (i == 0)//if car is at stoplight
                        {
                            Direction chosenDirection = TrafficLightGreen.ReturnDirection();
                            while (chosenDirection != Direction.Straight)//this for now
                            {
                                chosenDirection = TrafficLightGreen.ReturnDirection();
                            }


                            if (freeSpotLane3Index != -1)
                            {
                                formGraphics.FillEllipse(Brushes.Blue, TrafficLightGreen.CarsGoingOut[i].X, TrafficLightGreen.CarsGoingOut[i].Y, 5, 5);
                                feederSpotLane3.CarsComingIn[freeSpotLane3Index] = TrafficLightGreen.CarsGoingOut[i];
                                TrafficLightGreen.CarsGoingOut[i] = null;
                                //CarsGoingOut[i].X++;
                            }
                            else
                                formGraphics.FillEllipse(Brushes.Blue, TrafficLightGreen.CarsGoingOut[i].X, TrafficLightGreen.CarsGoingOut[i].Y, 5, 5);
                        }
                        else
                        {
                            if (TrafficLightGreen.CarsGoingOut[i - 1] == null)
                            {
                                //CarsGoingOut[i].X++;
                                formGraphics.FillEllipse(Brushes.Blue, TrafficLightGreen.CarsGoingOut[i].X, TrafficLightGreen.CarsGoingOut[i].Y, 5, 5);
                                TrafficLightGreen.CarsGoingOut[i - 1] = TrafficLightGreen.CarsGoingOut[i];
                                TrafficLightGreen.CarsGoingOut[i] = null;
                                //insinsi
                            }
                            else
                            {
                                formGraphics.FillEllipse(Brushes.Blue, TrafficLightGreen.CarsGoingOut[i].X, TrafficLightGreen.CarsGoingOut[i].Y, 5, 5);
                            }
                        }
                    }
                    else
                    {
                        formGraphics.FillEllipse(Brushes.Blue, TrafficLightGreen.CarsGoingOut[i].X, TrafficLightGreen.CarsGoingOut[i].Y, 5, 5);
                        TrafficLightGreen.CarsGoingOut[i].X++;
                    }
                }
            }

            for (int i = 0; i < crossingTest.Feeders[2].CarsComingIn.Length; i++)
            {
                if (crossingTest.Feeders[2].CarsComingIn[i] != null)
                {
                    if (crossingTest.Feeders[2].CarsComingIn[i].X == crossingTest.Feeders[2].StopPointsComingIn[i].X)
                    {
                        if (i == 0)//if car is at border of other crossing
                        {
                            if (false)
                            {
                                formGraphics.FillEllipse(Brushes.Blue, crossingTest.Feeders[2].CarsComingIn[i].X, crossingTest.Feeders[2].CarsComingIn[i].Y, 5, 5);
                                crossingTest.Feeders[2].CarsComingIn[4] = crossingTest.Feeders[2].CarsComingIn[i];
                                crossingTest.Feeders[2].CarsComingIn[i] = null;
                                crossingTest.Feeders[2].CarsComingIn[i].X++;
                            }
                            else
                                formGraphics.FillEllipse(Brushes.Blue, crossingTest.Feeders[2].CarsComingIn[i].X, crossingTest.Feeders[2].CarsComingIn[i].Y, 5, 5);
                        }
                        else
                        {
                            if (crossingTest.Feeders[2].CarsComingIn[i - 1] == null)
                            {
                                //CarsGoingOut[i].X++;
                                formGraphics.FillEllipse(Brushes.Blue, crossingTest.Feeders[2].CarsComingIn[i].X, crossingTest.Feeders[2].CarsComingIn[i].Y, 5, 5);
                                crossingTest.Feeders[2].CarsComingIn[i - 1] = crossingTest.Feeders[2].CarsComingIn[i];
                                crossingTest.Feeders[2].CarsComingIn[i] = null;
                                //insinsi
                            }
                            else
                            {
                                formGraphics.FillEllipse(Brushes.Blue, crossingTest.Feeders[2].CarsComingIn[i].X, crossingTest.Feeders[2].CarsComingIn[i].Y, 5, 5);
                            }
                        }
                    }
                    else
                    {
                        formGraphics.FillEllipse(Brushes.Blue, crossingTest.Feeders[2].CarsComingIn[i].X, crossingTest.Feeders[2].CarsComingIn[i].Y, 5, 5);
                        crossingTest.Feeders[2].CarsComingIn[i].X++;
                    }
                }
            }

            foreach (var item in crossingTest.Feeders)
            {
                if (item != TrafficLightGreen)
                {
                    foreach (var item2 in item.CarsGoingOut)
                    {
                        if (item2 != null)
                            formGraphics.FillEllipse(Brushes.Blue, item2.X, item2.Y, 5, 5);
                    }
                }
            }

            MoveCarsTimer.Start();
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
            //label6.Text = "X = " + e.X + " and Y = " + e.Y;
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

            try
            {
                simulator.LinkCrossingsWithNeighbors();

                crossingTest.Feeders[0].TrafficLight.greenLightTimer.Start();

                MoveCarsTimer.Start();

                //if (!IsSimulationStarted)
                //{
                //    gridGroupBox.Enabled = false;
                //    groupBox1.Visible = false;

                //    groupBox2.Location = new Point(3, 36);
                //    groupBox2.Height += 448;
                //    buttonStart.Location = new Point(18, 580);
                //    buttonStart.Text = "Stop";
                //    //groupBox2.Location
                //    groupBox4.Visible = false;
                //    groupBox3.Visible = false;
                //    groupBox5.Visible = false;
                //    IsSimulationStarted = !IsSimulationStarted;
                //}
                //else
                //{

                //    groupBox1.Visible = true; ;
                //    groupBox2.Height -= 448;
                //    groupBox2.Location = new Point(7, 371);
                //    buttonStart.Text = "Start";
                //    buttonStart.Location = new Point(37, 123);
                //    //groupBox2.Location
                //    groupBox3.Visible = true;
                //    groupBox5.Visible = true;
                //    IsSimulationStarted = !IsSimulationStarted;
                //    this.Size = new Size(1050, 741);
                //    groupBox4.Visible = false;
                //    gridGroupBox.Enabled = true;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCreateGrid_Click(object sender, EventArgs e)
        {
            simulator.DrawGrid();
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
    }
}
