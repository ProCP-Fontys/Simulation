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

namespace EntryInformation
{
    public partial class Simulation : Form
    {
        bool IsSimulationStarted = false;
        bool formationTab = false;
        
        private Simulator simulator;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        Car[] totalCars = new Car[10];
        Car[] CarsGoingOut = new Car[5];
        List<Point> StopPointsGoingOut;

        public Simulation()
        {
            InitializeComponent();
            simulator = new Simulator(this);
            aTimer.Elapsed += new ElapsedEventHandler(PaintCarMoving);
            aTimer.Interval = 50;

            StopPointsGoingOut = new List<Point>();

            for (int i = 4; i > -1; i--)
            {
                this.StopPointsGoingOut.Add(new Point(Convert.ToInt16(i + "7"), 112));
            }

            for (int i = 0; i < 10; i++)
            {
                totalCars[i] = new Car(new Point(-3, 112));
            }



            //cars.Add(new Car(new Point(9, 112)));
            //cars.Add(new Car(new Point(-1, 112)));
        }
        bool greenLight = true;
        private void PaintCarMoving(object sender, ElapsedEventArgs e)
        {
            aTimer.Stop();
            
            if(CarsGoingOut[4] == null)
            {
                for (int i = 0; i < totalCars.Length; i++)
			    {
                    if (totalCars[i] != null)
                    {
                        CarsGoingOut[4] = totalCars[i];
                        totalCars[i] = null;
                        break;
                    }
                }
            }
            PictureBox toDrawOn = (PictureBox)this.Controls.Find("Crossing0", true)[0];

            if (toDrawOn.InvokeRequired)
            {
                // after we've done all the processing, 
                toDrawOn.Invoke(new MethodInvoker(delegate
                {
                    toDrawOn.Refresh();
                }));
            }
            Graphics formGraphics = toDrawOn.CreateGraphics();

            for (int i = 0; i < CarsGoingOut.Length ; i++)
			{
                if (CarsGoingOut[i] != null)
                {
                    if (CarsGoingOut[i].X == StopPointsGoingOut[i].X)
                    {
                        if (i == 0)//if car is at stoplight
                        {
                            greenLight = false;
                            if (greenLight)//stopLight is green
                            {
                                //greenLight = false;
                                formGraphics.FillEllipse(Brushes.Blue, CarsGoingOut[i].X, CarsGoingOut[i].Y, 5, 5);
                                CarsGoingOut[i].X++;
                            }
                            else
                            {
                                //greenLight = true;
                                formGraphics.FillEllipse(Brushes.Blue, CarsGoingOut[i].X, CarsGoingOut[i].Y, 5, 5);
                                //CarsGoingOut[i].X++;
                            }
                        }
                        else
                        {
                            if (CarsGoingOut[i - 1] == null)
                            {
                                //CarsGoingOut[i].X++;
                                formGraphics.FillEllipse(Brushes.Blue, CarsGoingOut[i].X, CarsGoingOut[i].Y, 5, 5);
                                CarsGoingOut[i - 1] = CarsGoingOut[i];
                                CarsGoingOut[i] = null;
                                //insinsi
                            }
                            else
                            {
                                formGraphics.FillEllipse(Brushes.Blue, CarsGoingOut[i].X, CarsGoingOut[i].Y, 5, 5);
                            }
                        }
                    }
                    else
                    {
                        formGraphics.FillEllipse(Brushes.Blue, CarsGoingOut[i].X, CarsGoingOut[i].Y, 5, 5);
                        CarsGoingOut[i].X++;
                    }
                }
                
                
			} 

            //toDrawOn.Refresh();
            if(greenLight)
                aTimer.Start();
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

                aTimer.Start();

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

        private void gridPanel_Paint(object sender, PaintEventArgs e)
        {
            //' Draw a 200 by 150 pixel green rectangle.
            e.Graphics.DrawRectangle(Pens.Green, 10, 10, 200, 150);
            //' Draw a blue square
            e.Graphics.DrawRectangle(Pens.Blue, 30, 30, 150, 150);
            //' Draw a 150 pixel diameter red circle.
            e.Graphics.DrawEllipse(Pens.Red, 0, 0, 300, 300);
            //' Draw a 250 by 125 pixel yellow oval.
            e.Graphics.DrawEllipse(Pens.Yellow, 20, 20, 250, 125);
        }
    }
}
