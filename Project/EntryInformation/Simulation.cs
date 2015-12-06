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
using System.Reflection;

namespace EntryInformation
{
    public partial class Simulation : Form
    {
        //bool IsSimulationStarted = false;
        bool formationTab = false;

        //Crossing crossingTest;
        //List<PictureBox> pictureBoxCrossing;

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

            //crossingTest = new CrossingB(0);

            MoveCarsTimer.Elapsed += new ElapsedEventHandler(PaintCarMoving);
            MoveCarsTimer.Interval = 30;

            //pictureBoxCrossing = new List<PictureBox>();
            

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

            simulator.InvalidateCrossings();

            MoveCarsTimer.Start();
            
            
            


            //for (int i = 0; i < crossingTest.Feeders[2].CarsComingIn.Length; i++)
            //{
            //    if (crossingTest.Feeders[2].CarsComingIn[i] != null)
            //    {
            //        if (crossingTest.Feeders[2].CarsComingIn[i].X == crossingTest.Feeders[2].StopPointsComingIn[i].X)
            //        {
            //            if (i == 0)//if car is at border of other crossing
            //            {
            //                if (false)//crossingTest.neighbors.Right.Feeders[0].CarsGoingOut[4] == null
            //                {
            //                    //crossingTest.neighbors.Right.Feeders[0].CarsGoingOut[4] = crossingTest.Feeders[2].CarsComingIn[i];
            //                    //crossingTest.Feeders[2].CarsComingIn[i] = null;
            //                }
            //                else
            //                    formGraphics.FillEllipse(Brushes.Blue, crossingTest.Feeders[2].CarsComingIn[i].X, crossingTest.Feeders[2].CarsComingIn[i].Y, 5, 5);
            //            }
            //            else
            //            {
            //                if (crossingTest.Feeders[2].CarsComingIn[i - 1] == null)
            //                {
            //                    //CarsGoingOut[i].X++;
            //                    formGraphics.FillEllipse(Brushes.Blue, crossingTest.Feeders[2].CarsComingIn[i].X, crossingTest.Feeders[2].CarsComingIn[i].Y, 5, 5);
            //                    crossingTest.Feeders[2].CarsComingIn[i - 1] = crossingTest.Feeders[2].CarsComingIn[i];
            //                    crossingTest.Feeders[2].CarsComingIn[i] = null;
            //                    //insinsi
            //                }
            //                else
            //                {
            //                    formGraphics.FillEllipse(Brushes.Blue, crossingTest.Feeders[2].CarsComingIn[i].X, crossingTest.Feeders[2].CarsComingIn[i].Y, 5, 5);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            formGraphics.FillEllipse(Brushes.Blue, crossingTest.Feeders[2].CarsComingIn[i].X, crossingTest.Feeders[2].CarsComingIn[i].Y, 5, 5);
            //            crossingTest.Feeders[2].CarsComingIn[i].X++;
            //        }
            //    }
            //}

            //foreach (var item in crossingTest.Feeders)
            //{
            //    if (item != TrafficLightGreenFeeder)
            //    {
            //        foreach (var item2 in item.CarsGoingOut)
            //        {
            //            if (item2 != null)
            //                formGraphics.FillEllipse(Brushes.Blue, item2.X, item2.Y, 5, 5);
            //        }
            //    }
            //}
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
                if (buttonStart.Text == "Start")
                {
                    //crossingTest = new CrossingB(0);
                    simulator.LinkCrossingsWithNeighbors();

                    simulator.LinkPaintEventHandlerToCrossing();

                    simulator.StartTimerTrafficLight();
                    //PictureBox toDrawOn = (PictureBox)this.Controls.Find("Crossing" + crossingTest.CrossingID, true)[0];
                    //PictureBox toDrawOn2 = (PictureBox)this.Controls.Find("Crossing" + 1, true)[0];

                    //toDrawOn.Paint += toDrawOn_Paint;
                    //toDrawOn2.Paint += toDrawOn_Paint;

                    //pictureBoxCrossing.Add(toDrawOn);
                    //pictureBoxCrossing.Add(toDrawOn2);


                    //formGraphics = toDrawOn.CreateGraphics();

                    //GridCell gridCell = null;

                    //gridCell = simulator.grid.ReturnGridCells().Find(x => x.CrossingID == 0);

                    //crossingTest = gridCell.Crossing;

                    //crossingTest.Feeders[3].TrafficLight.greenLightTimer.Start();

                    MoveCarsTimer.Start();
                    buttonStart.Text = "Stop";
                }
                else
                {
                    MoveCarsTimer.Stop();
                    buttonStart.Text = "Start";
                }

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
