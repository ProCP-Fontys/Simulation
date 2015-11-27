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
        List<Point> pointsOfGridCells;
        List<Point> randomCellsToChoose;
        List<Point> occupiedCells;
        int nrOfGridCellsOccupied = 0;
        private Point cellOccupied;
        private Simulator simulator;
        public Panel gridPanel2;
        public Simulation()
        {
            InitializeComponent();
            gridPanel2 = new Panel();
            simulator = new Simulator(this);
            pointsOfGridCells = new List<Point>();
            randomCellsToChoose = new List<Point>();
            occupiedCells = new List<Point>();

            //first row
            pointsOfGridCells.Add(new Point(0, 0));
            pointsOfGridCells.Add(new Point(200, 0));
            pointsOfGridCells.Add(new Point(400, 0));
            pointsOfGridCells.Add(new Point(600, 0));

            //second row
            pointsOfGridCells.Add(new Point(0, 200));
            pointsOfGridCells.Add(new Point(200, 200));
            pointsOfGridCells.Add(new Point(400, 200));
            pointsOfGridCells.Add(new Point(600, 200));

            //third row
            pointsOfGridCells.Add(new Point(0, 400));
            pointsOfGridCells.Add(new Point(200, 400));
            pointsOfGridCells.Add(new Point(400, 400));
            pointsOfGridCells.Add(new Point(600, 400));
            gridGroupBox.Enabled = false;
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

        private void Simulation_Resize(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            CA.DoDragDrop(CA.Image, DragDropEffects.Copy);
        }

        private void gridPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private Point determinePicboxLocation(Point droppedCoordinates)
        {
            if (nrOfGridCellsOccupied != 12)//if all cells are occupied
            {
                Point locationToBeDropped = new Point(-1, -1);

                List<Point> pointsOfGridCellsUpdated = new List<Point>();

                foreach (var item in pointsOfGridCells)//check if below you there are empty spots and check if is in cell
                {
                    if (droppedCoordinates.X >= item.X && droppedCoordinates.X <= (item.X + 200))
                    {
                        locationToBeDropped.X = item.X;
                        pointsOfGridCellsUpdated.Add(item);
                    }

                }
                foreach (var item in pointsOfGridCellsUpdated)
                {
                    if (droppedCoordinates.Y >= item.Y && droppedCoordinates.Y <= (item.Y + 200))
                    {
                        locationToBeDropped.Y = item.Y;
                        nrOfGridCellsOccupied++;
                        occupiedCells.Add(item);
                        pointsOfGridCells.Remove(item);
                        break;
                    }
                }

                if (locationToBeDropped.Y != -1)
                    return locationToBeDropped;

                pointsOfGridCellsUpdated = new List<Point>();

                foreach (var item in occupiedCells)//check if below you there are empty spots and check if is in cell
                {
                    if (droppedCoordinates.X >= item.X && droppedCoordinates.X <= (item.X + 200))
                    {
                        locationToBeDropped.X = item.X;
                        pointsOfGridCellsUpdated.Add(item);
                    }

                }
                foreach (var item in pointsOfGridCellsUpdated)
                {
                    if (droppedCoordinates.Y >= item.Y && droppedCoordinates.Y <= (item.Y + 200))
                    {
                        locationToBeDropped.Y = item.Y;
                        cellOccupied = locationToBeDropped;
                        break;
                    }
                }

                foreach (var item in pointsOfGridCells)//check if there is an empty spot just below
                {
                    if ((cellOccupied.X + 200) == item.X && cellOccupied.Y == item.Y)
                    {
                        randomCellsToChoose.Add(item);
                    }
                    else if ((cellOccupied.X - 200) == item.X && cellOccupied.Y == item.Y)
                    {
                        randomCellsToChoose.Add(item);
                    }
                    else if ((cellOccupied.Y + 200) == item.Y && cellOccupied.X == item.X)
                    {
                        randomCellsToChoose.Add(item);
                    }
                    else if ((cellOccupied.Y - 200) == item.Y && cellOccupied.X == item.X)
                    {
                        randomCellsToChoose.Add(item);
                    }
                }

                if (randomCellsToChoose.Count != 0)
                {
                    Random random = new Random();

                    int randomChoice = random.Next(0, randomCellsToChoose.Count);
                    while (randomCellsToChoose[randomChoice] == null)
                    {
                        randomChoice = random.Next(0, randomCellsToChoose.Count);
                    }

                    nrOfGridCellsOccupied++;
                    pointsOfGridCells.Remove(randomCellsToChoose[randomChoice]);
                    Point PointCellToReturn = randomCellsToChoose[randomChoice];
                    randomCellsToChoose = new List<Point>();
                    occupiedCells.Add(PointCellToReturn);
                    return PointCellToReturn;
                }
                else
                    MessageBox.Show("Space not available");

            }
            return new Point(-1, -1);
            
        }

        private void FormExpand(object sender, EventArgs e)
        {
            formationTab = false;
            this.frm_Resize(sender,e);
            
            ((PictureBox)sender).BorderStyle = BorderStyle.Fixed3D;
            
        }

        private void gridPanel_DragDrop(object sender, DragEventArgs e)
        {
            Point locationToBeDropped = determinePicboxLocation(this.gridPanel.PointToClient(new Point(e.X, e.Y)));
            if (locationToBeDropped.X != -1)
            {
                PictureBox picbox = new PictureBox();
                picbox.Click += new EventHandler(FormExpand);
                picbox.Size = new Size(200, 200);
                picbox.BorderStyle = BorderStyle.None;
                picbox.Location = new Point(locationToBeDropped.X, locationToBeDropped.Y);

                Bitmap image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                picbox.Image = image;
                picbox.SizeMode = PictureBoxSizeMode.StretchImage;
                gridPanel.Controls.Add(picbox);
                Crossing c = new Crossing();
                //simulator.addCrossing(c);
                //this.mainForm = mainForm;
                //return true;
            }
        }

        private void gridPanel_MouseMove(object sender, MouseEventArgs e)
        {
            label14.Text = "X = " + e.X + " and Y = " + e.Y;
        }

        private void Simulation_MouseMove(object sender, MouseEventArgs e)
        {
            label6.Text = "X = " + e.X + " and Y = " + e.Y;
        }

        private void BtnCreateGrid_Click(object sender, EventArgs e)
        {
           
            //remove the picture box
            ////Form.ControlCollection FC = new ControlCollection(this);
            //foreach (Control item in gridGroupBox.Controls)
            //{


            //    item.Visible = false;
                
            //}
            //gridPanel = new Panel();
            //this.calculatePanelSize(Convert.ToInt16(this.comboBoxRows.SelectedItem),Convert.ToInt16(this.comboBoxColumns.SelectedItem));
            ////repeateeeeeeeeeed
            //
            //nrOfGridCellsOccupied=0;
            //occupiedCells=new List<Point>();

            //pointsOfGridCells = new List<Point>();
            //pointsOfGridCells.Add(new Point(0, 0));
            //pointsOfGridCells.Add(new Point(200, 0));
            //pointsOfGridCells.Add(new Point(400, 0));
            //pointsOfGridCells.Add(new Point(600, 0));

            ////second row
            //pointsOfGridCells.Add(new Point(0, 200));
            //pointsOfGridCells.Add(new Point(200, 200));
            //pointsOfGridCells.Add(new Point(400, 200));
            //pointsOfGridCells.Add(new Point(600, 200));

            ////third row
            //pointsOfGridCells.Add(new Point(0, 400));
            //pointsOfGridCells.Add(new Point(200, 400));
            //pointsOfGridCells.Add(new Point(400, 400));
            //pointsOfGridCells.Add(new Point(600, 400));
            pointsOfGridCells = new List<Point>();
            randomCellsToChoose = new List<Point>();
            occupiedCells = new List<Point>();

            //first row
            pointsOfGridCells.Add(new Point(0, 0));
            pointsOfGridCells.Add(new Point(200, 0));
            pointsOfGridCells.Add(new Point(400, 0));
            pointsOfGridCells.Add(new Point(600, 0));

            //second row
            pointsOfGridCells.Add(new Point(0, 200));
            pointsOfGridCells.Add(new Point(200, 200));
            pointsOfGridCells.Add(new Point(400, 200));
            pointsOfGridCells.Add(new Point(600, 200));

            //third row
            pointsOfGridCells.Add(new Point(0, 400));
            pointsOfGridCells.Add(new Point(200, 400));
            pointsOfGridCells.Add(new Point(400, 400));
            pointsOfGridCells.Add(new Point(600, 400));
            gridPanel.Controls.Clear();
            gridGroupBox.Enabled = true;
            simulator.DrawGrid(comboBoxRows, comboBoxColumns, gridPanel, gridGroupBox);
            
            Form.ActiveForm.Width = 235 + gridGroupBox.Width;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            CB.DoDragDrop(CB.Image, DragDropEffects.Copy);
            
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
            gridPanel.Controls.Clear();
            pointsOfGridCells = new List<Point>();
            randomCellsToChoose = new List<Point>();
            occupiedCells = new List<Point>();
            simulator.Grid.ReturnGridCells(3,4);
            //first row
            pointsOfGridCells.Add(new Point(0, 0));
            pointsOfGridCells.Add(new Point(200, 0));
            pointsOfGridCells.Add(new Point(400, 0));
            pointsOfGridCells.Add(new Point(600, 0));

            //second row
            pointsOfGridCells.Add(new Point(0, 200));
            pointsOfGridCells.Add(new Point(200, 200));
            pointsOfGridCells.Add(new Point(400, 200));
            pointsOfGridCells.Add(new Point(600, 200));

            //third row
            pointsOfGridCells.Add(new Point(0, 400));
            pointsOfGridCells.Add(new Point(200, 400));
            pointsOfGridCells.Add(new Point(400, 400));
            pointsOfGridCells.Add(new Point(600, 400));
        }

        private void Simulation_Load(object sender, EventArgs e)
        {

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

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
            

        }

        private void dkhb(object sender, MouseEventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        
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

        private void CB_DragDrop(object sender, DragEventArgs e)
        {
            CB.DoDragDrop(CB.Image, DragDropEffects.Copy);
        }
    }
}
