using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EntryInformation;
using System.Drawing;
using EntryInformation.Classes;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//When Sensor is Clicked Show some user feedback
[Serializable]
public class Simulator 
{
    private Grid grid;
    [NonSerialized]
    private Simulation simulation;
    [NonSerialized]
    private List<PictureBox> pictureBoxCrossing;
    [NonSerialized]
    private bool started;
    [NonSerialized]
    public List<Car> cars;
    public int totalAmountOfCars;

    public Simulator(Simulation simulation)
    {
        this.simulation = simulation;
        simulation.gridGroupBox.Enabled = false;//disable so you cannot drag crossing until creating the grid
        grid = new Grid();
        cars = new List<Car>();
        pictureBoxCrossing = new List<PictureBox>();
    }

    public void OpenNewFile()
    {

        SaveFileDialog fd = new SaveFileDialog();
        fd.Filter = "Simulation file (*.SMM)|*.SMM|All files (*.*)|*.*";

        if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            try
            {
                simulation.filePath = fd.FileName;
                simulation.Text = "Simulation Magic!   Street:" + simulation.labelStreetName.Text + "." + simulation.filePath;
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(simulation.filePath,
                                         FileMode.Create,
                                         FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, this.grid);
                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: 2read file. Original error: " + ex.Message);
            }
            finally
            {

            }
        }
    }
    public void saveAsGridOutput()
    {
        SaveFileDialog fd = new SaveFileDialog();
        fd.Filter = "Simulation file (*.SMM)|*.SMM|All files (*.*)|*.*";

        if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            try
            {
                simulation.filePath = fd.FileName;
                simulation.Text = "Simulation Magic!   Street:" + simulation.labelStreetName.Text + "." + simulation.filePath;
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(simulation.filePath,
                                             FileMode.Create,
                                             FileAccess.Write, FileShare.None);
                    formatter.Serialize(stream, this.grid);
                    stream.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: 2read file. Original error: " + ex.Message);
            }
            finally
            {
                
            }
        }
    }


    public void saveGridOutput()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(simulation.filePath,
                                 FileMode.Create,
                                 FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, this.grid);

        stream.Close();
    }

    public void openGridInput()
    {
        OpenFileDialog fd = new OpenFileDialog();
       

        if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            try
            {
                simulation.filePath = fd.FileName;
                simulation.Text = "Simulation Magic!   Street:" + simulation.labelStreetName.Text + "." + simulation.filePath;
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(simulation.filePath,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                Grid obj = (Grid)formatter.Deserialize(stream);

                stream.Close();

                this.grid = obj;

                simulation.gridPanel.Refresh();

                simulation.gridPanel.Controls.Clear();
                pictureBoxCrossing = new List<PictureBox>();


                foreach (var item in this.grid.ReturnGridCells())
                {
                    PictureBox picbox = CreatePicBox(item);
                    if (item.Crossing != null)
                    {
                        if (item.Crossing is CrossingA)
                            picbox.Image = simulation.CA.Image;
                        else
                            picbox.Image = simulation.CB.Image;
                        picbox.SizeMode = PictureBoxSizeMode.StretchImage;
                        simulation.gridPanel.Controls.Add(picbox);
                        pictureBoxCrossing.Add(picbox);
                    }
                }
                simulation.groupBoxCrossingControl.Visible = false;
                simulation.gridGroupBox.Enabled = true;
                calculatePanelSize(grid.nrOfRows, grid.nrOfColumns);
                simulation.Width = simulation.gridGroupBox.Width + 230;
                simulation.comboBoxRows.SelectedIndex = (this.grid.nrOfRows - 1);
                simulation.comboBoxColumns.SelectedIndex = (this.grid.nrOfColumns - 1);
                this.DrawGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: 2read file. Original error: " + ex.Message);
            }
            finally
            {

            }
        }



        
    }
    private void ResetPauzeCountDown()
    {
        simulation.PauzeCountDown = grid.nrOfRows * grid.nrOfColumns;
    }

    private void ResetGrid()
    {
        foreach (var item in grid.ReturnGridCells())
        {
            pictureBoxCrossing = new List<PictureBox>();
            simulation.gridPanel.Controls.Clear();
            item.RemoveCrossing();
        }
        simulation.gridGroupBox.Enabled = false;
    }

    public void ReStart()
    {
        ResetPauzeCountDown();
        foreach (var item in grid.ReturnGridCells())
        {
            item.Crossing.Feeders[item.Crossing.FeederIDIndexToRestart].trafficLight.greenLightTimer.Start();
        }
        simulation.MoveCarsTimer.Start();
    }

    public void Stop()
    {
        started = false;
        simulation.buttonStop.Enabled = false;
        simulation.MoveCarsTimer.Stop();
        InvalidateCrossings();
        simulation.buttonStart.Enabled = false;
        simulation.buttonStart.Text = "Start";
        FormAfterStop();
        Feeder feeder;
        foreach (var item in grid.ReturnGridCells())
        {
            feeder = item.Crossing.Feeders.Find(x => x.trafficLight.greenLightTimer.Enabled);
            if (feeder == null)
                feeder = item.Crossing.Feeders.Find(x => x.trafficLight.yellowLightTimer.Enabled);
            if (feeder == null)
            {
                if (item.Crossing is CrossingA)
                    (item.Crossing as CrossingA).SensorTimer.Stop();
            }
            else
            {
                feeder.trafficLight.greenLightTimer.Stop();
                feeder.trafficLight.yellowLightTimer.Stop();
                simulation.listBoxErrors.Items.Add("Simulation was stopped");
            }
        }
        ResetGrid();
    }

    public void Pauze()
    {
        Feeder feeder;
        foreach (var item in grid.ReturnGridCells())
        {
            feeder = item.Crossing.Feeders.Find(x => x.trafficLight.greenLightTimer.Enabled);
            if (feeder == null)
                feeder = item.Crossing.Feeders.Find(x => x.trafficLight.yellowLightTimer.Enabled);
            if (feeder == null)
            {
                (item.Crossing as CrossingA).Pauze = true;
                item.Crossing.FeederIDIndexToRestart = ((item.Crossing as CrossingA).LGI) % 4;
            }
            else
            {
                feeder.trafficLight.Pauze = true;
                item.Crossing.FeederIDIndexToRestart = (feeder.FeederID) % 4;
                simulation.listBoxErrors.Items.Add("Simulation was pauzed");
            }
        }
        simulation.buttonStart.Enabled = false;
    }

    private void SetTotalAmountOfCars()
    {
        foreach (var item in grid.ReturnGridCells())
        {
            foreach (var item2 in item.Crossing.Feeders)
            {
                totalAmountOfCars += item2.TotalCars.Length;
            }
        }
    }

    private void calculatePanelSize(int nrOfRows, int nrOfColumns)
    {
        int height = nrOfRows * 200;
        int width = nrOfColumns * 200;
        simulation.gridPanel.Size = new Size(width, height);
        simulation.gridGroupBox.Size = new Size(width + 15, height + 30);
        simulation.groupBoxCrossingControl.Location= new Point(simulation.gridGroupBox.Width + 235,simulation.groupBoxCrossingControl.Location.Y);
    }
    public void DrawGrid()
    {
        Pen myPen;
        myPen = new Pen(Color.White);
        Graphics formGraphics = simulation.gridPanel.CreateGraphics();

        //drawing cells rows
        foreach (var item in grid.ReturnGridCells())
        {
            formGraphics.DrawLine(myPen, item.ReturnLocation().X, item.ReturnLocation().Y, (item.ReturnLocation().X + 200), item.ReturnLocation().Y);//top line from the left to right
            formGraphics.DrawLine(myPen, item.ReturnLocation().X, item.ReturnLocation().Y, item.ReturnLocation().X, (item.ReturnLocation().Y + 200));//left line from top to bottom
            formGraphics.DrawLine(myPen, item.ReturnLocation().X, (item.ReturnLocation().Y + 199), (item.ReturnLocation().X + 200), item.ReturnLocation().Y + 199);//bottom line from the left to right
            formGraphics.DrawLine(myPen, (item.ReturnLocation().X + 199), item.ReturnLocation().Y, (item.ReturnLocation().X + 199), (item.ReturnLocation().Y + 200));//right line from top to bottom
        }
        myPen.Dispose();
        formGraphics.Dispose();
    }
    public void CreateGrid()//if window is moved beyond edge from screen part of grid is removed cause it does not repaint//to be done
    {
        List<GridCell> gridOldCells = grid.ReturnGridCells();

        grid.nrOfRows = Convert.ToInt16(simulation.comboBoxRows.SelectedItem);
        grid.nrOfColumns = Convert.ToInt16(simulation.comboBoxColumns.SelectedItem);

        List<GridCell> gridNewCells = grid.ReturnGridCells();

        foreach (var item in gridOldCells)
        {
            GridCell found = gridNewCells.Find(x => x.Number == item.Number);
            if (found == null && item.Crossing != null)
            {
                PictureBox founded = pictureBoxCrossing.Find(x => x.Name == item.Number.ToString());
                pictureBoxCrossing.Remove(founded);
                simulation.gridPanel.Controls.Remove(founded);
                item.RemoveCrossing();
            }
        }

        //remember to resize the form depending on rows and columns
        simulation.groupBoxCrossingControl.Visible = false;
        
        simulation.gridGroupBox.Enabled = true;

        calculatePanelSize(grid.nrOfRows, grid.nrOfColumns);
        simulation.Width = simulation.gridGroupBox.Width + 230;

        this.DrawGrid();
    }

    public void PopErrorWindow()
    {
        simulation.groupBoxCrossingControl.Visible = true;
    }

    //delete crossing
    public void DeleteCrossing()
    {
        PictureBox found = pictureBoxCrossing.Find(x => x.BorderStyle == BorderStyle.Fixed3D);
        pictureBoxCrossing.Remove(found);
        simulation.gridPanel.Controls.Remove(found);
        GridCell gc = grid.ReturnGridCells().Find(x => x.Number == Convert.ToInt32(found.Name));
        gc.RemoveCrossing();
        HideCrossingInput();
        this.DrawGrid();
    }

    private GridCell determinePicboxLocation(Point droppedCoordinates)
    {
        List<GridCell> gridCells = grid.ReturnGridCells();

        if (grid.GridCellsOccupied() != 12)//if all cells are occupied
        {
            GridCell GridCellToBeDropped = null;

            List<GridCell> gridCellsUpdated = new List<GridCell>();//possible X coordinate cells candidates //change name to gridcellsUpdated

            foreach (var item in gridCells)
            {
                if ((droppedCoordinates.X >= item.ReturnLocation().X && droppedCoordinates.X <= (item.ReturnLocation().X + 200)) && item.Crossing == null)
                {
                    gridCellsUpdated.Add(item);
                }

            }
            foreach (var item in gridCellsUpdated)
            {
                if (droppedCoordinates.Y >= item.ReturnLocation().Y && droppedCoordinates.Y <= (item.ReturnLocation().Y + 200))
                {
                    GridCellToBeDropped = item;
                    break;
                }
            }

            if (GridCellToBeDropped != null)
                return GridCellToBeDropped;

            gridCellsUpdated = new List<GridCell>();
            GridCell cellOccupied = null;//which cell user dropped crossing is occupied
            List<GridCell> randomCellsToChoose = new List<GridCell>();//Possible cells to choose from

            foreach (var item in gridCells)//check if below you there are empty spots and check if is in cell
            {
                if ((droppedCoordinates.X >= item.ReturnLocation().X && droppedCoordinates.X <= (item.ReturnLocation().X + 200)) && item.Crossing != null)
                {
                    gridCellsUpdated.Add(item);
                }

            }

            foreach (var item in gridCellsUpdated)
            {
                if (droppedCoordinates.Y >= item.ReturnLocation().Y && droppedCoordinates.Y <= (item.ReturnLocation().Y + 200))
                {
                    cellOccupied = item;
                    break;
                }
            }

            foreach (var item in gridCells)//check if there are emtpy spots
            {
                if (((cellOccupied.ReturnLocation().X + 200) == item.ReturnLocation().X && cellOccupied.ReturnLocation().Y == item.ReturnLocation().Y) && item.Crossing == null)
                {
                    randomCellsToChoose.Add(item);
                }
                else if (((cellOccupied.ReturnLocation().X - 200) == item.ReturnLocation().X && cellOccupied.ReturnLocation().Y == item.ReturnLocation().Y) && item.Crossing == null)
                {
                    randomCellsToChoose.Add(item);
                }
                else if (((cellOccupied.ReturnLocation().Y + 200) == item.ReturnLocation().Y && cellOccupied.ReturnLocation().X == item.ReturnLocation().X) && item.Crossing == null)
                {
                    randomCellsToChoose.Add(item);
                }
                else if (((cellOccupied.ReturnLocation().Y - 200) == item.ReturnLocation().Y && cellOccupied.ReturnLocation().X == item.ReturnLocation().X) && item.Crossing == null)
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
                return randomCellsToChoose[randomChoice];
            }
        }

        return null;
    }

    private void LinkCrossingsWithNeighbors()
    {
            List<GridCell> gridCells = grid.ReturnGridCells();
            foreach (var item in gridCells)
            {
                foreach (var item2 in gridCells)
                {
                    if (((item.ReturnLocation().X + 200) == item2.ReturnLocation().X && item.ReturnLocation().Y == item2.ReturnLocation().Y))
                    {
                        if (item.Crossing != null && item2.Crossing != null)
                            item.Crossing.neighbors.Right = item2.Crossing;
                    }
                    else if (((item.ReturnLocation().X - 200) == item2.ReturnLocation().X && item.ReturnLocation().Y == item2.ReturnLocation().Y))
                    {
                        if (item.Crossing != null && item2.Crossing != null)
                            item.Crossing.neighbors.Left = item2.Crossing;
                    }
                    else if (((item.ReturnLocation().Y + 200) == item2.ReturnLocation().Y && item.ReturnLocation().X == item2.ReturnLocation().X))
                    {
                        if (item.Crossing != null && item2.Crossing != null)
                            item.Crossing.neighbors.Bottom = item2.Crossing;
                    }
                    else if (((item.ReturnLocation().Y - 200) == item2.ReturnLocation().Y && item.ReturnLocation().X == item2.ReturnLocation().X))
                    {
                        if (item.Crossing != null && item2.Crossing != null)
                            item.Crossing.neighbors.Top = item2.Crossing;
                    }
                }
            }
    }

    private PictureBox CreatePicBox(GridCell GridCell)
    {
        PictureBox picbox = new PictureBox();
        picbox.Click += FormExpand;//connect click eventhandler to event
        picbox.MouseUp += picbox_MouseUp;
        picbox.MouseMove += picbox_MouseMove;
        picbox.Size = new Size(200, 200);
        picbox.BorderStyle = BorderStyle.None;
        picbox.Name = Convert.ToString(GridCell.Number);
        picbox.Location = new Point(GridCell.ReturnLocation().X, GridCell.ReturnLocation().Y);

        return picbox;
    }

    public bool AddCrossingInCell(DragEventArgs e)
    {
        GridCell OnGridCellDropped = determinePicboxLocation(simulation.gridPanel.PointToClient(new Point(e.X, e.Y)));
        if (OnGridCellDropped != null)
        {
            PictureBox picbox = CreatePicBox(OnGridCellDropped);

            Bitmap image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
            picbox.Image = image;
            picbox.SizeMode = PictureBoxSizeMode.StretchImage;
            simulation.gridPanel.Controls.Add(picbox);
            pictureBoxCrossing.Add(picbox);
            LinkCrossingAndGridCell(OnGridCellDropped, image);
            return true;
        }
        else
            return false;
    }

    void picbox_MouseUp(object sender, MouseEventArgs e)
    {
        if (started)
        {
            int gridCellID = Convert.ToInt16((sender as PictureBox).Name);

            GridCell gridCellNeeded = grid.ReturnGridCells().Find(x => x.Number == gridCellID);

            Crossing crossing = gridCellNeeded.Crossing;

            if (crossing is CrossingA)
            {
                if (!(crossing as CrossingA).SensorTimer.Enabled)
                {
                    if ((e.X >= 135 && e.X <= 175) && (e.Y >= 30 && e.Y <= 66))
                    {
                        Feeder Feeder;
                        if (crossing is CrossingA)
                        {
                            Feeder = crossing.Feeders.Find(x => x.trafficLight.greenLightTimer.Enabled);
                            if (Feeder == null)
                                Feeder = crossing.Feeders.Find(x => x.trafficLight.yellowLightTimer.Enabled);
                            (crossing as CrossingA).LGI = Feeder.FeederID;
                            Feeder.trafficLight.SensorClicked = true;
                            simulation.labelGreenLPed.Text = "Pedestrian sensor is enabeled for\n       crossing"+ crossing.CrossingID+1;
                            simulation.labelGreenLPed.Visible = true; 
                            simulation.listBoxErrors.Items.Add("Sensor was set");
                        }
                    }
                }
            }
        }
    }

    public void InvalidateCrossings()
    {
        foreach (var item in pictureBoxCrossing)
        {
            item.Invalidate();
        }
    }

    private void LinkPaintEventHandlerToCrossing()
    {
        foreach (var item in pictureBoxCrossing)
        {
            item.Paint += toDrawOn_Paint;
        }
    }

    public String CheckIfGridIsFullyCompleted()
    {
        return grid.CheckGridFull();
    }

    private void StartTimerTrafficLight()
    {
        foreach (var item in grid.ReturnGridCells())
        {
            item.Crossing.Feeders[0].trafficLight.greenLightTimer.Start();//start the left lane's green trafficlight of every crossing on grid
        }
    }

    private void toDrawOn_Paint(object sender, PaintEventArgs e)
    {
        if (cars.Count == totalAmountOfCars)//more details later
        {
            try
            {
                simulation.buttonStop_Click(sender, new EventArgs());
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong showing the results");
            }
            return;
        }
        else
        {
            Crossing CurrentCrossing = (grid.ReturnGridCells().Find(x => x.Number == Convert.ToInt16(((PictureBox)sender).Name))).Crossing;
            if (CurrentCrossing is CrossingA)
            {
                foreach (var item in (CurrentCrossing as CrossingA).peopleTL)
                {
                    e.Graphics.FillEllipse(Brushes.Yellow, item.X, item.Y, 5, 5);

                }
                foreach (var item in (CurrentCrossing as CrossingA).peopleTR)
                {
                    e.Graphics.FillEllipse(Brushes.Yellow, item.X, item.Y, 5, 5);
                }
                foreach (var item in (CurrentCrossing as CrossingA).peopleBL)
                {
                    e.Graphics.FillEllipse(Brushes.Yellow, item.X, item.Y, 5, 5);
                }
                foreach (var item in (CurrentCrossing as CrossingA).peopleBR)
                {
                    e.Graphics.FillEllipse(Brushes.Yellow, item.X, item.Y, 5, 5);
                }

                if ((CurrentCrossing as CrossingA).SensorTimer.Enabled)
                {
                    (CurrentCrossing as CrossingA).movePeople();
                }
            }

            Feeder TrafficLightGreenFeeder = null;

            int freeSpotLaneIndex = -1;
            Feeder feederSpotLane = null;
            Direction direction = Direction.None;

            foreach (var item in CurrentCrossing.Feeders)
            {
                if (item.trafficLight.greenLightTimer.Enabled)
                {
                    item.trafficLight.DrawGreenLight(e);
                    TrafficLightGreenFeeder = item;
                    direction = TrafficLightGreenFeeder.ReturnDirection();
                }
                else
                    item.trafficLight.DrawRedLight(e);
            }

            if (TrafficLightGreenFeeder != null)
            {
                switch (TrafficLightGreenFeeder.FeederID)
                {
                    case 1:
                        switch (direction)
                        {
                            case Direction.Straight:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[2].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[2];
                                break;
                            case Direction.Left:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[1].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[1];
                                break;
                            case Direction.Right:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[3].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[3];
                                break;
                        }
                        break;
                    case 2:
                        switch (direction)
                        {
                            case Direction.Straight:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[3].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[3];
                                break;
                            case Direction.Left:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[2].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[2];
                                break;
                            case Direction.Right:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[0].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[0];
                                break;
                        }
                        break;
                    case 3:
                        switch (direction)
                        {
                            case Direction.Straight:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[0].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[0];
                                break;
                            case Direction.Left:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[3].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[3];
                                break;
                            case Direction.Right:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[1].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[1];
                                break;
                        }
                        break;
                    case 4:
                        switch (direction)
                        {
                            case Direction.Straight:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[1].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[1];
                                break;
                            case Direction.Left:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[0].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[0];
                                break;
                            case Direction.Right:
                                freeSpotLaneIndex = Array.IndexOf(CurrentCrossing.Feeders[2].CarsComingIn, null);
                                feederSpotLane = CurrentCrossing.Feeders[2];
                                break;
                        }
                        break;
                }


                if (TrafficLightGreenFeeder.CarsGoingOut[4] == null)
                {
                    for (int i = 0; i < TrafficLightGreenFeeder.TotalCars.Length; i++)
                    {
                        if (TrafficLightGreenFeeder.TotalCars[i] != null)
                        {
                            TrafficLightGreenFeeder.CarsGoingOut[4] = TrafficLightGreenFeeder.TotalCars[i];
                            TrafficLightGreenFeeder.TotalCars[i] = null;
                            break;
                        }
                    }
                }


                for (int i = 0; i < TrafficLightGreenFeeder.CarsGoingOut.Length; i++)
                {
                    if (TrafficLightGreenFeeder.CarsGoingOut[i] != null)
                    {
                        TrafficLightGreenFeeder.CarsGoingOut[i].CalculateRedTimeSpent();
                        int pointToCheck = -1000;
                        int pointToCompare = -1000;

                        switch (TrafficLightGreenFeeder.FeederID)
                        {
                            case 1:
                                pointToCheck = TrafficLightGreenFeeder.CarsGoingOut[i].X;
                                pointToCompare = TrafficLightGreenFeeder.StopPointsGoingOut[i].X;
                                if (pointToCheck == 197)
                                {
                                    TrafficLightGreenFeeder.CarsGoingOut[i].X = -3;
                                    pointToCheck = -3;
                                }
                                break;
                            case 2:
                                pointToCheck = TrafficLightGreenFeeder.CarsGoingOut[i].Y;
                                pointToCompare = TrafficLightGreenFeeder.StopPointsGoingOut[i].Y;
                                if (pointToCheck == 197)
                                {
                                    TrafficLightGreenFeeder.CarsGoingOut[i].Y = -3;
                                    pointToCheck = -3;
                                }
                                break;
                            case 3:
                                pointToCheck = TrafficLightGreenFeeder.CarsGoingOut[i].X;
                                pointToCompare = TrafficLightGreenFeeder.StopPointsGoingOut[i].X;
                                if (pointToCheck == -3)
                                {
                                    TrafficLightGreenFeeder.CarsGoingOut[i].X = 196;
                                    pointToCheck = 196;
                                }
                                break;
                            case 4:
                                pointToCheck = TrafficLightGreenFeeder.CarsGoingOut[i].Y;
                                pointToCompare = TrafficLightGreenFeeder.StopPointsGoingOut[i].Y;
                                if (pointToCheck == -3)
                                {
                                    TrafficLightGreenFeeder.CarsGoingOut[i].Y = 196;
                                    pointToCheck = 196;
                                }
                                break;
                        }

                        if (pointToCheck == pointToCompare)
                        {
                            if (i == 0)//if car is at stoplight
                            {
                                if (freeSpotLaneIndex != -1)
                                {
                                    TrafficLightGreenFeeder.CarsGoingOut[i].Direction = direction;
                                    e.Graphics.FillEllipse(Brushes.Blue, TrafficLightGreenFeeder.CarsGoingOut[i].X, TrafficLightGreenFeeder.CarsGoingOut[i].Y, 5, 5);
                                    switch (TrafficLightGreenFeeder.FeederID)
                                    {
                                        case 1:
                                            TrafficLightGreenFeeder.CarsGoingOut[i].X++;
                                            break;
                                        case 2:
                                            TrafficLightGreenFeeder.CarsGoingOut[i].Y++;
                                            break;
                                        case 3:
                                            TrafficLightGreenFeeder.CarsGoingOut[i].X++;
                                            break;
                                        case 4:
                                            TrafficLightGreenFeeder.CarsGoingOut[i].Y++;
                                            break;
                                    }
                                    feederSpotLane.CarsComingIn[freeSpotLaneIndex] = TrafficLightGreenFeeder.CarsGoingOut[i];
                                    TrafficLightGreenFeeder.CarsGoingOut[i] = null;
                                }
                                else
                                    e.Graphics.FillEllipse(Brushes.Blue, TrafficLightGreenFeeder.CarsGoingOut[i].X, TrafficLightGreenFeeder.CarsGoingOut[i].Y, 5, 5);
                            }
                            else
                            {
                                if (TrafficLightGreenFeeder.CarsGoingOut[i - 1] == null)
                                {
                                    e.Graphics.FillEllipse(Brushes.Blue, TrafficLightGreenFeeder.CarsGoingOut[i].X, TrafficLightGreenFeeder.CarsGoingOut[i].Y, 5, 5);

                                    switch (TrafficLightGreenFeeder.FeederID)
                                    {
                                        case 1:
                                            TrafficLightGreenFeeder.CarsGoingOut[i].X++;
                                            break;
                                        case 2:
                                            TrafficLightGreenFeeder.CarsGoingOut[i].Y++;
                                            break;
                                        case 3:
                                            TrafficLightGreenFeeder.CarsGoingOut[i].X++;
                                            break;
                                        case 4:
                                            TrafficLightGreenFeeder.CarsGoingOut[i].Y++;
                                            break;
                                    }

                                    TrafficLightGreenFeeder.CarsGoingOut[i - 1] = TrafficLightGreenFeeder.CarsGoingOut[i];
                                    TrafficLightGreenFeeder.CarsGoingOut[i] = null;
                                }
                                else
                                {
                                    e.Graphics.FillEllipse(Brushes.Blue, TrafficLightGreenFeeder.CarsGoingOut[i].X, TrafficLightGreenFeeder.CarsGoingOut[i].Y, 5, 5);
                                }
                            }
                        }
                        else
                        {
                            switch (TrafficLightGreenFeeder.FeederID)
                            {
                                case 1:
                                    e.Graphics.FillEllipse(Brushes.Blue, TrafficLightGreenFeeder.CarsGoingOut[i].X, TrafficLightGreenFeeder.CarsGoingOut[i].Y, 5, 5);
                                    TrafficLightGreenFeeder.CarsGoingOut[i].X++;
                                    break;
                                case 2:
                                    e.Graphics.FillEllipse(Brushes.Blue, TrafficLightGreenFeeder.CarsGoingOut[i].X, TrafficLightGreenFeeder.CarsGoingOut[i].Y, 5, 5);
                                    TrafficLightGreenFeeder.CarsGoingOut[i].Y++;
                                    break;
                                case 3:
                                    e.Graphics.FillEllipse(Brushes.Blue, TrafficLightGreenFeeder.CarsGoingOut[i].X, TrafficLightGreenFeeder.CarsGoingOut[i].Y, 5, 5);
                                    TrafficLightGreenFeeder.CarsGoingOut[i].X--;
                                    break;
                                case 4:
                                    e.Graphics.FillEllipse(Brushes.Blue, TrafficLightGreenFeeder.CarsGoingOut[i].X, TrafficLightGreenFeeder.CarsGoingOut[i].Y, 5, 5);
                                    TrafficLightGreenFeeder.CarsGoingOut[i].Y--;
                                    break;
                            }
                        }
                    }
                }
            }

            foreach (var item in CurrentCrossing.Feeders)
            {
                switch (item.FeederID)
                {
                    case 1:
                        for (int i = 0; i < item.CarsComingIn.Length; i++)
                        {
                            if (item.CarsComingIn[i] != null)
                            {
                                switch (item.CarsComingIn[i].Direction)
                                {
                                    case Direction.Left:
                                        if (item.CarsComingIn[i].Y == 82)
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Direction = Direction.Straight;
                                            item.CarsComingIn[i].X--;
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Y--;
                                        }
                                        break;
                                    case Direction.Straight:
                                        if (item.CarsComingIn[i].X == item.StopPointsComingIn[i].X || (item.CarsComingIn[i].X == -2))//change the stopPoints of incoming lane 1 ID
                                        {
                                            if (i == 0)//is at border
                                            {
                                                if (CurrentCrossing.neighbors.Left != null)//
                                                {
                                                    if (CurrentCrossing.neighbors.Left.Feeders[2].CarsGoingOut[4] == null)
                                                    {
                                                        if (item.CarsComingIn[i].X == -2)
                                                        {
                                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                            item.CarsComingIn[i].X--;
                                                            CurrentCrossing.neighbors.Left.Feeders[2].CarsGoingOut[4] = item.CarsComingIn[i];
                                                            item.CarsComingIn[i] = item.CarsComingIn[i + 1];
                                                            item.CarsComingIn[i + 1] = item.CarsComingIn[i + 2];
                                                            item.CarsComingIn[i + 2] = item.CarsComingIn[i + 3];
                                                            item.CarsComingIn[i + 3] = item.CarsComingIn[i + 4];
                                                            item.CarsComingIn[i + 4] = null;
                                                        }
                                                        else
                                                        {
                                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                            item.CarsComingIn[i].X--;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                    }
                                                }
                                                else
                                                {
                                                    if (item.CarsComingIn[i].X == -2)
                                                    {
                                                        cars.Add(item.CarsComingIn[i]);
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                        item.CarsComingIn[i].X--;
                                                        item.CarsComingIn[i] = item.CarsComingIn[i + 1];
                                                        item.CarsComingIn[i + 1] = item.CarsComingIn[i + 2];
                                                        item.CarsComingIn[i + 2] = item.CarsComingIn[i + 3];
                                                        item.CarsComingIn[i + 3] = item.CarsComingIn[i + 4];
                                                        item.CarsComingIn[i + 4] = null;
                                                    }
                                                    else
                                                    {
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                        item.CarsComingIn[i].X--;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.CarsComingIn[i - 1] == null)
                                                {
                                                    e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                    item.CarsComingIn[i - 1] = item.CarsComingIn[i];
                                                    item.CarsComingIn[i] = null;
                                                }
                                                else
                                                    e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            }
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].X--;
                                        }
                                        break;
                                    case Direction.Right:
                                        if (item.CarsComingIn[i].Y == 82)
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Direction = Direction.Straight;
                                            item.CarsComingIn[i].X--;
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Y++;
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case 2:
                        for (int i = 0; i < item.CarsComingIn.Length; i++)
                        {
                            if (item.CarsComingIn[i] != null)
                            {
                                switch (item.CarsComingIn[i].Direction)
                                {
                                    case Direction.Left:
                                        if (item.CarsComingIn[i].X == 112)
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Direction = Direction.Straight;
                                            item.CarsComingIn[i].Y--;
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].X++;
                                        }
                                        break;
                                    case Direction.Straight:
                                        if (item.CarsComingIn[i].Y == item.StopPointsComingIn[i].Y || (item.CarsComingIn[i].Y == -2))
                                        {
                                            if (i == 0)//is at border
                                            {
                                                if (CurrentCrossing.neighbors.Top != null)//
                                                {
                                                    if (CurrentCrossing.neighbors.Top.Feeders[3].CarsGoingOut[4] == null)
                                                    {
                                                        if (item.CarsComingIn[i].Y == -2)
                                                        {
                                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                            item.CarsComingIn[i].Y--;
                                                            CurrentCrossing.neighbors.Top.Feeders[3].CarsGoingOut[4] = item.CarsComingIn[i];
                                                            item.CarsComingIn[i] = item.CarsComingIn[i + 1];
                                                            item.CarsComingIn[i + 1] = item.CarsComingIn[i + 2];
                                                            item.CarsComingIn[i + 2] = item.CarsComingIn[i + 3];
                                                            item.CarsComingIn[i + 3] = item.CarsComingIn[i + 4];
                                                            item.CarsComingIn[i + 4] = null;
                                                        }
                                                        else
                                                        {
                                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                            item.CarsComingIn[i].Y--;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                    }
                                                }
                                                else
                                                {
                                                    if (item.CarsComingIn[i].Y == -2)
                                                    {
                                                        cars.Add(item.CarsComingIn[i]);
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                        item.CarsComingIn[i].Y--;
                                                        item.CarsComingIn[i] = item.CarsComingIn[i + 1];
                                                        item.CarsComingIn[i + 1] = item.CarsComingIn[i + 2];
                                                        item.CarsComingIn[i + 2] = item.CarsComingIn[i + 3];
                                                        item.CarsComingIn[i + 3] = item.CarsComingIn[i + 4];
                                                        item.CarsComingIn[i + 4] = null;
                                                    }
                                                    else
                                                    {
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                        item.CarsComingIn[i].Y--;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.CarsComingIn[i - 1] == null)
                                                {
                                                    e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                    item.CarsComingIn[i - 1] = item.CarsComingIn[i];
                                                    item.CarsComingIn[i] = null;
                                                }
                                                else
                                                    e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            }
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Y--;
                                        }
                                        break;
                                    case Direction.Right:
                                        if (item.CarsComingIn[i].X == 112)
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Direction = Direction.Straight;
                                            item.CarsComingIn[i].Y--;
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].X--;
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case 3:
                        for (int i = 0; i < item.CarsComingIn.Length; i++)
                        {
                            if (item.CarsComingIn[i] != null)
                            {
                                switch (item.CarsComingIn[i].Direction)
                                {
                                    case Direction.Left:
                                        if (item.CarsComingIn[i].Y == 112)
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Direction = Direction.Straight;
                                            item.CarsComingIn[i].X++;
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Y++;
                                        }
                                        break;
                                    case Direction.Straight:
                                        if (item.CarsComingIn[i].X == item.StopPointsComingIn[i].X || (item.CarsComingIn[i].X == 196) || (item.CarsComingIn[i].X == 199))
                                        {
                                            if (i == 0)//is at border
                                            {
                                                if (CurrentCrossing.neighbors.Right != null)//
                                                {
                                                    if (CurrentCrossing.neighbors.Right.Feeders[0].CarsGoingOut[4] == null)
                                                    {
                                                        if (item.CarsComingIn[i].X == 196)
                                                        {
                                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                            item.CarsComingIn[i].X++;
                                                            CurrentCrossing.neighbors.Right.Feeders[0].CarsGoingOut[4] = item.CarsComingIn[i];
                                                            item.CarsComingIn[i] = item.CarsComingIn[i + 1];
                                                            item.CarsComingIn[i + 1] = item.CarsComingIn[i + 2];
                                                            item.CarsComingIn[i + 2] = item.CarsComingIn[i + 3];
                                                            item.CarsComingIn[i + 3] = item.CarsComingIn[i + 4];
                                                            item.CarsComingIn[i + 4] = null;
                                                        }
                                                        else
                                                        {
                                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                            item.CarsComingIn[i].X++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                    }
                                                }
                                                else
                                                {
                                                    if (item.CarsComingIn[i].X == 199)
                                                    {
                                                        cars.Add(item.CarsComingIn[i]);
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                        item.CarsComingIn[i].X++;
                                                        item.CarsComingIn[i] = item.CarsComingIn[i + 1];
                                                        item.CarsComingIn[i + 1] = item.CarsComingIn[i + 2];
                                                        item.CarsComingIn[i + 2] = item.CarsComingIn[i + 3];
                                                        item.CarsComingIn[i + 3] = item.CarsComingIn[i + 4];
                                                        item.CarsComingIn[i + 4] = null;
                                                    }
                                                    else
                                                    {
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                        item.CarsComingIn[i].X++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.CarsComingIn[i - 1] == null)
                                                {
                                                    e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                    item.CarsComingIn[i - 1] = item.CarsComingIn[i];
                                                    item.CarsComingIn[i] = null;
                                                }
                                                else
                                                    e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            }
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].X++;
                                        }
                                        break;
                                    case Direction.Right:
                                        if (item.CarsComingIn[i].Y == 112)
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Direction = Direction.Straight;
                                            item.CarsComingIn[i].X++;
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Y--;
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case 4:
                        for (int i = 0; i < item.CarsComingIn.Length; i++)
                        {
                            if (item.CarsComingIn[i] != null)
                            {
                                switch (item.CarsComingIn[i].Direction)
                                {
                                    case Direction.Left:
                                        if (item.CarsComingIn[i].X == 82)
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Direction = Direction.Straight;
                                            item.CarsComingIn[i].Y++;
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].X--;
                                        }
                                        break;
                                    case Direction.Straight:
                                        if (item.CarsComingIn[i].Y == item.StopPointsComingIn[i].Y || (item.CarsComingIn[i].Y == 196 && i == 0) || (item.CarsComingIn[i].Y == 199 && i == 0))
                                        {
                                            if (i == 0)//is at border
                                            {
                                                if (CurrentCrossing.neighbors.Bottom != null)//
                                                {
                                                    if (CurrentCrossing.neighbors.Bottom.Feeders[1].CarsGoingOut[4] == null)
                                                    {
                                                        if (item.CarsComingIn[i].Y == 196)
                                                        {
                                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                            item.CarsComingIn[i].Y++;
                                                            CurrentCrossing.neighbors.Bottom.Feeders[1].CarsGoingOut[4] = item.CarsComingIn[i];
                                                            item.CarsComingIn[i] = item.CarsComingIn[i + 1];
                                                            item.CarsComingIn[i + 1] = item.CarsComingIn[i + 2];
                                                            item.CarsComingIn[i + 2] = item.CarsComingIn[i + 3];
                                                            item.CarsComingIn[i + 3] = item.CarsComingIn[i + 4];
                                                            item.CarsComingIn[i + 4] = null;
                                                        }
                                                        else
                                                        {
                                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                            item.CarsComingIn[i].Y++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                    }
                                                }
                                                else
                                                {
                                                    if (item.CarsComingIn[i].Y == 199)
                                                    {
                                                        cars.Add(item.CarsComingIn[i]);
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                        item.CarsComingIn[i].Y++;
                                                        item.CarsComingIn[i] = item.CarsComingIn[i + 1];
                                                        item.CarsComingIn[i + 1] = item.CarsComingIn[i + 2];
                                                        item.CarsComingIn[i + 2] = item.CarsComingIn[i + 3];
                                                        item.CarsComingIn[i + 3] = item.CarsComingIn[i + 4];
                                                        item.CarsComingIn[i + 4] = null;
                                                    }
                                                    else
                                                    {
                                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                        item.CarsComingIn[i].Y++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (item.CarsComingIn[i - 1] == null)
                                                {
                                                    e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                                    item.CarsComingIn[i - 1] = item.CarsComingIn[i];
                                                    item.CarsComingIn[i] = null;
                                                }
                                                else
                                                    e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            }
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Y++;
                                        }
                                        break;
                                    case Direction.Right:
                                        if (item.CarsComingIn[i].X == 82)
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].Direction = Direction.Straight;
                                            item.CarsComingIn[i].Y++;
                                        }
                                        else
                                        {
                                            e.Graphics.FillEllipse(Brushes.Blue, item.CarsComingIn[i].X, item.CarsComingIn[i].Y, 5, 5);
                                            item.CarsComingIn[i].X++;
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                }
            }

            //this is for painting the cars waiting for the greenLight
            foreach (var item in CurrentCrossing.Feeders)
            {
                if (item != TrafficLightGreenFeeder)
                {
                    if (item.CarsGoingOut[4] == null)
                    {
                        for (int i = 0; i < item.TotalCars.Length; i++)//this is for putting cars into the lane from the totalcars lane
                        {
                            if (item.TotalCars[i] != null)
                            {
                                item.CarsGoingOut[4] = item.TotalCars[i];
                                item.TotalCars[i] = null;
                                break;
                            }
                        }
                    }


                    for (int i = 0; i < item.CarsGoingOut.Length; i++)
                    {
                        if (item.CarsGoingOut[i] != null)
                        {
                            item.CarsGoingOut[i].SetTimeRed();
                            int pointToCheck = -1000;
                            int pointToCompare = -1000;

                            switch (item.FeederID)
                            {
                                case 1:
                                    pointToCheck = item.CarsGoingOut[i].X;
                                    pointToCompare = item.StopPointsGoingOut[i].X;
                                    if (pointToCheck == 197)
                                    {
                                        item.CarsGoingOut[i].X = -3;
                                        pointToCheck = -3;
                                    }
                                    break;
                                case 2:
                                    pointToCheck = item.CarsGoingOut[i].Y;
                                    pointToCompare = item.StopPointsGoingOut[i].Y;
                                    if (pointToCheck == 197)
                                    {
                                        item.CarsGoingOut[i].Y = -3;
                                        pointToCheck = -3;
                                    }
                                    break;
                                case 3:
                                    pointToCheck = item.CarsGoingOut[i].X;
                                    pointToCompare = item.StopPointsGoingOut[i].X;
                                    if (pointToCheck == -3)
                                    {
                                        item.CarsGoingOut[i].X = 196;
                                        pointToCheck = 196;
                                    }
                                    break;
                                case 4:
                                    pointToCheck = item.CarsGoingOut[i].Y;
                                    pointToCompare = item.StopPointsGoingOut[i].Y;
                                    if (pointToCheck == -3)
                                    {
                                        item.CarsGoingOut[i].Y = 196;
                                        pointToCheck = 196;
                                    }
                                    break;
                            }

                            if (pointToCheck == pointToCompare)
                            {
                                if (i == 0)//if car is at stoplight
                                {
                                    e.Graphics.FillEllipse(Brushes.Blue, item.CarsGoingOut[i].X, item.CarsGoingOut[i].Y, 5, 5);
                                }
                                else
                                {
                                    if (item.CarsGoingOut[i - 1] == null)
                                    {
                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsGoingOut[i].X, item.CarsGoingOut[i].Y, 5, 5);
                                        switch (item.FeederID)
                                        {
                                            case 1:
                                                item.CarsGoingOut[i].X++;
                                                break;
                                            case 2:
                                                item.CarsGoingOut[i].Y++;
                                                break;
                                            case 3:
                                                item.CarsGoingOut[i].X--;
                                                break;
                                            case 4:
                                                item.CarsGoingOut[i].Y--;
                                                break;
                                        }
                                        item.CarsGoingOut[i - 1] = item.CarsGoingOut[i];
                                        item.CarsGoingOut[i] = null;
                                        //insinsi
                                    }
                                    else
                                    {
                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsGoingOut[i].X, item.CarsGoingOut[i].Y, 5, 5);
                                    }
                                }
                            }
                            else
                            {
                                switch (item.FeederID)
                                {
                                    case 1:
                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsGoingOut[i].X, item.CarsGoingOut[i].Y, 5, 5);
                                        item.CarsGoingOut[i].X++;
                                        break;
                                    case 2:
                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsGoingOut[i].X, item.CarsGoingOut[i].Y, 5, 5);
                                        item.CarsGoingOut[i].Y++;
                                        break;
                                    case 3:
                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsGoingOut[i].X, item.CarsGoingOut[i].Y, 5, 5);
                                        item.CarsGoingOut[i].X--;
                                        break;
                                    case 4:
                                        e.Graphics.FillEllipse(Brushes.Blue, item.CarsGoingOut[i].X, item.CarsGoingOut[i].Y, 5, 5);
                                        item.CarsGoingOut[i].Y--;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }//if time left fix the stopping by cars when leaving crossing or when being passed to another crossing

    public void FormAfterStart()//Hide the Unneeded groupBoxes
    {
        simulation.groupBoxCreatGrid.Visible = false;
        simulation.groupBoxToolBoz.Visible = false;
        simulation.groupBoxSimulationControl.Size = new Size(202, 617);
        simulation.groupBoxSimulationControl.Location = new Point(5, 30);
        simulation.Height = 500;
    }

    public void FormAfterStop()//Hide the Unneeded groupBoxes
    {
        simulation.groupBoxCreatGrid.Visible = true;
        simulation.groupBoxToolBoz.Visible = true;
        simulation.groupBoxSimulationControl.Size = new Size(202, 206);
        simulation.groupBoxSimulationControl.Location = new Point(5, 465);
    }

    private void EnableStopButton()
    {
        simulation.buttonStop.Enabled = true;
    }

    public void Start()
    {
        String error = CheckIfGridIsFullyCompleted();
        if (error != "")
            throw new Exception(error);

        HideCrossingInput();
        DeselectAllCrossings();
        LinkPaintEventHandlerToCrossing();
        FormAfterStart();
        SetTotalAmountOfCars();
        StartTimerTrafficLight();
        EnableStopButton();
        ResetPauzeCountDown();
        simulation.MoveCarsTimer.Start();
        started = true;
    }

    private void DisableCarTextBox()
    {
        simulation.textBoxAmountOfCars.Enabled = false ;
        simulation.textBoxAmountOfCars.BackColor = Color.DarkRed;
        simulation.textBoxAmountOfCars.Text = "0";
    }

    private void EnableCarTextBox()
    {
        simulation.textBoxAmountOfCars.Enabled = true;
        simulation.textBoxAmountOfCars.BackColor = Color.Black;
        simulation.textBoxAmountOfCars.Text = "";
    }

    private void EnableInputs()
    {
        simulation.textBoxGreenLight.Enabled = true;
        simulation.textBoxLeftPerc.Enabled = true;
        simulation.textBoxRightPerc.Enabled = true;
        simulation.textBoxStraightPerc.Enabled = true;
        simulation.textBoxStraightPerc.BackColor = Color.Black;
        simulation.textBoxRightPerc.BackColor = Color.Black;
        simulation.textBoxLeftPerc.BackColor = Color.Black;
        simulation.textBoxGreenLight.BackColor = Color.Black;
        simulation.buttonApply.Enabled = true;
        simulation.buttonApplyToAll.Enabled = true;
    }

    private void DisableAllInputs()
    {
        simulation.textBoxAmountOfCars.Enabled = false;
        simulation.textBoxGreenLight.Enabled = false;
        simulation.textBoxLeftPerc.Enabled = false;
        simulation.textBoxRightPerc.Enabled = false;
        simulation.textBoxStraightPerc.Enabled = false;
        simulation.textBoxStraightPerc.BackColor = Color.DarkRed;
        simulation.textBoxRightPerc.BackColor = Color.DarkRed;
        simulation.textBoxLeftPerc.BackColor = Color.DarkRed;
        simulation.textBoxGreenLight.BackColor = Color.DarkRed;
        simulation.textBoxAmountOfCars.BackColor = Color.DarkRed;
        simulation.buttonApply.Enabled = false;
        simulation.buttonApplyToAll.Enabled = false;
    }

    public void ComboBoxLaneChanging()
    {
        if (simulation.comboBoxLane.SelectedIndex == 0)
        {
            this.DisableAllInputs();
            return;
        }
        else
        {
            EnableInputs();
            DisableCarTextBox();
        }

        int gridCellID = -1;
        foreach (var item in pictureBoxCrossing)
        {
            if (item.BorderStyle == BorderStyle.Fixed3D)
                gridCellID = Convert.ToInt16(item.Name);
        }

        if (gridCellID == -1)
        {
            simulation.listBoxErrors.Items.Clear();
            simulation.listBoxErrors.Items.Add("No crossing selected");
        }
        else
        {
            GridCell gridCellNeeded = grid.ReturnGridCells().Find(x => x.Number == gridCellID);

            Crossing crossing = gridCellNeeded.Crossing;

            String Feeder = simulation.comboBoxLane.SelectedItem.ToString();

            switch (Feeder)
            {
                case "Left Lane":
                    if (crossing.neighbors.Left == null)
                    {
                        EnableCarTextBox();
                    }
                    break;
                case "Right Lane":
                    if (crossing.neighbors.Right == null)
                    {
                        EnableCarTextBox();
                    }
                    break;
                case "Top Lane":
                    if (crossing.neighbors.Top == null)
                    {
                        EnableCarTextBox();
                    }
                    break;
                case "Bottom Lane":
                    if (crossing.neighbors.Bottom == null)
                    {
                        EnableCarTextBox();
                    }
                    break;
            }
        }
    }

    public void ConnectInfoFeeder(object sender)
    {
        simulation.listBoxErrors.Items.Clear();
        int gridCellID = -1;

        foreach (var item in pictureBoxCrossing)
        {
            if (item.BorderStyle == BorderStyle.Fixed3D)
                gridCellID = Convert.ToInt16(item.Name);
        }

        if (gridCellID == -1)
            throw new Exception("No crossing selected");

        GridCell gridCellNeeded = grid.ReturnGridCells().Find(x => x.Number == gridCellID);

        Crossing crossing = gridCellNeeded.Crossing;

        try
        {
            bool exceptionOccurs = false;
            if (String.IsNullOrEmpty(simulation.textBoxAmountOfCars.Text) && simulation.textBoxAmountOfCars.BackColor != Color.DarkRed)
            {
                exceptionOccurs = true;
                simulation.listBoxErrors.Items.Add("Cars input is empty");
            }
            if (String.IsNullOrEmpty(simulation.textBoxGreenLight.Text) && simulation.textBoxGreenLight.BackColor != Color.DarkRed)
            {
                exceptionOccurs = true;
                simulation.listBoxErrors.Items.Add("Green light time input is empty");
            }
            if (String.IsNullOrEmpty(simulation.textBoxPedestrians.Text) && simulation.textBoxPedestrians.BackColor != Color.DarkRed)
            {
                exceptionOccurs = true;
                simulation.listBoxErrors.Items.Add("Pedestrians time input is empty");
            }
            if (String.IsNullOrEmpty(simulation.textBoxLeftPerc.Text) && simulation.textBoxLeftPerc.BackColor != Color.DarkRed)
            {
                exceptionOccurs = true;
                simulation.listBoxErrors.Items.Add("Left percentage car flow input is empty");
            }
            if (String.IsNullOrEmpty(simulation.textBoxRightPerc.Text) && simulation.textBoxRightPerc.BackColor != Color.DarkRed)
            {
                exceptionOccurs = true;
                simulation.listBoxErrors.Items.Add("Right percentage car flow input is empty");
            }
            if (String.IsNullOrEmpty(simulation.textBoxStraightPerc.Text) && simulation.textBoxStraightPerc.BackColor != Color.DarkRed)
            {
                exceptionOccurs = true;
                simulation.listBoxErrors.Items.Add("Straight percentage car flow input is empty");
            }
            if (((Convert.ToInt32(simulation.textBoxLeftPerc.Text)) + (Convert.ToInt32(simulation.textBoxStraightPerc.Text)) + (Convert.ToInt32(simulation.textBoxRightPerc.Text))) != 100)
            {
                exceptionOccurs = true;
                simulation.listBoxErrors.Items.Add("The sum of the percentages is not 100");
            }

            if (!exceptionOccurs)
            {
                if ((sender as Button).Text == "Apply")
                {
                    crossing.Feeders[simulation.comboBoxLane.SelectedIndex - 1].AddDetailes(
                        Convert.ToInt16(simulation.textBoxGreenLight.Text),
                        Convert.ToInt16(simulation.textBoxRightPerc.Text),
                        Convert.ToInt16(simulation.textBoxLeftPerc.Text),
                        Convert.ToInt16(simulation.textBoxStraightPerc.Text),
                        Convert.ToInt16(simulation.textBoxAmountOfCars.Text));
                }
                else
                {
                    List<Crossing> neighbors = crossing.neighbors.RetrieveListOfNeighbors();//in order of left, top, right, bottom
                    {
                        for (int i = 0; i < crossing.Feeders.Count; i++)//i is suitable for both list here(left lane with left neighbor, etc)
                        {
                            if (neighbors[i] == null)
                            {
                                crossing.Feeders[i].AddDetailes(
                                    Convert.ToInt16(simulation.textBoxGreenLight.Text),
                                    Convert.ToInt16(simulation.textBoxRightPerc.Text),
                                    Convert.ToInt16(simulation.textBoxLeftPerc.Text),
                                    Convert.ToInt16(simulation.textBoxStraightPerc.Text),
                                    Convert.ToInt16(simulation.textBoxAmountOfCars.Text));
                            }
                            else
                            {
                                crossing.Feeders[i].AddDetailes(
                                    Convert.ToInt16(simulation.textBoxGreenLight.Text),
                                    Convert.ToInt16(simulation.textBoxRightPerc.Text),
                                    Convert.ToInt16(simulation.textBoxLeftPerc.Text),
                                    Convert.ToInt16(simulation.textBoxStraightPerc.Text),
                                    0);
                            }
                        }
                    }
                }
                
                if (crossing is CrossingA)
                {
                    (crossing as CrossingA).SetSensorTime(Convert.ToInt16(simulation.textBoxPedestrians.Text));
                }
                simulation.listBoxErrors.Items.Add("Applied");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }

    private void DeselectAllCrossings()
    {
        foreach (Control pb in simulation.gridPanel.Controls)
        {
            if (pb is PictureBox)
            {
                (pb as PictureBox).BorderStyle = BorderStyle.None;
            }

        }
    }

    private void FormExpand(object sender, EventArgs e)
    {
        if (!started)
        {
            LinkCrossingsWithNeighbors();
            int gridCellID = Convert.ToInt16((sender as PictureBox).Name);

            GridCell gridCellNeeded = grid.ReturnGridCells().Find(x => x.Number == gridCellID);

            Crossing crossing = gridCellNeeded.Crossing;

            if (crossing is CrossingA)
            {
                simulation.label4.Visible = true;
                simulation.textBoxPedestrians.Visible = true;
                simulation.textBoxPedestrians.Text = "";
            }
            else
            {
                simulation.label4.Visible = false;
                simulation.textBoxPedestrians.Visible = false;
                simulation.textBoxPedestrians.Text = "0";
            }

            //Deselect other crossings

            DeselectAllCrossings();


            (sender as PictureBox).BorderStyle = BorderStyle.Fixed3D;//select crossing

            if (!simulation.groupBoxCrossingControl.Visible)//make side input window visible
            {
                simulation.groupBoxCrossingControl.Visible = true;
            }

            simulation.comboBoxLane.SelectedIndex = 0;
        }
    }

    private void HideCrossingInput()
    {
        simulation.groupBoxCrossingControl.Visible = false;
        simulation.Width = simulation.gridGroupBox.Width - 230;
    }

    private void picbox_MouseMove(object sender, MouseEventArgs e)
    {
        simulation.label6.Text = "X = " + e.X + " and Y = " + e.Y;
    }

    private void LinkCrossingAndGridCell(GridCell gridCell, Image image)
    {
        if (image.Tag.ToString() == "CrossingB")
        {
            gridCell.AddCrossing(new CrossingB(gridCell.Number,this.simulation));
        }
        else
        {
            gridCell.AddCrossing(new CrossingA(gridCell.Number,this.simulation));
        }
    }

    public virtual void Save()
    {
        throw new System.NotImplementedException();
    }

}

