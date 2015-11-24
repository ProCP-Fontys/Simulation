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
        List<Point> pointsOfGridCells;
        List<Point> randomCellsToChoose;
        List<Point> occupiedCells;
        int nrOfGridCellsOccupied = 0;
        private Point cellOccupied;

        public Simulation()
        {
            InitializeComponent();

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

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox2.DoDragDrop(pictureBox2.Image, DragDropEffects.Copy);
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
            //still have to check empty spots and your left and up//make a method to check the empty spots near a gridCell and then choose one of them
        }

        private void gridPanel_DragDrop(object sender, DragEventArgs e)
        {
            Point locationToBeDropped = determinePicboxLocation(this.gridPanel.PointToClient(new Point(e.X, e.Y)));
            if (locationToBeDropped.X != -1)
            {
                PictureBox picbox = new PictureBox();
                picbox.Size = new Size(200, 200);
                picbox.BorderStyle = BorderStyle.FixedSingle;
                picbox.Location = new Point(locationToBeDropped.X, locationToBeDropped.Y);

                Bitmap image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                picbox.Image = image;
                picbox.SizeMode = PictureBoxSizeMode.StretchImage;
                gridPanel.Controls.Add(picbox);
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
    }
}
