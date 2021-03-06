﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

[Serializable]
public class Grid
{
    private List<GridCell> gridCells;
    public int nrOfRows { get; set; }
    public int nrOfColumns { get; set; }

    public Grid()
    {
        gridCells = new List<GridCell>();
        CreateGridCells();
    }

    public int GridCellsOccupied()
    {
        int occupiedNmbr = 0;

        foreach (var item in gridCells)
        {
            if (item.Crossing != null)
                occupiedNmbr++;
        }

        return occupiedNmbr;
    }

    public void CreateGridCells()
    {
        int gridNmr = 0;
        for (int i = 0; i < 600; i += 200)
        {
            for (int i2 = 0; i2 < 800; i2 += 200)
            {
                gridCells.Add(new GridCell(new Point(i2, i), gridNmr));
                gridNmr++;
            }
        }
    }

    public List<GridCell> ReturnGridCells()
    {
        List<GridCell> listToBeReturned = new List<GridCell>();

        listToBeReturned.Add(gridCells[0]);

        if (nrOfRows == 1)
        {
            for (int i = 1; i < nrOfColumns; i++)
            {
                listToBeReturned.Add(gridCells[i]);
            }
        }
        else if (nrOfRows == 2 && nrOfColumns == 4)
        {
            for (int i = 1; i < (nrOfColumns * nrOfRows); i++)
            {
                listToBeReturned.Add(gridCells[i]);
            }
        }
        else if (nrOfRows == 2 && nrOfColumns == 1)
        {
            listToBeReturned.Add(gridCells[4]);
        }
        else if (nrOfRows == 2 && nrOfColumns == 2)
        {
            listToBeReturned.Add(gridCells[1]);
            listToBeReturned.Add(gridCells[4]);
            listToBeReturned.Add(gridCells[5]);
        }
        else if (nrOfRows == 2 && nrOfColumns == 3)
        {
            listToBeReturned.Add(gridCells[1]);
            listToBeReturned.Add(gridCells[2]);
            listToBeReturned.Add(gridCells[4]);
            listToBeReturned.Add(gridCells[5]);
            listToBeReturned.Add(gridCells[6]);
        }
        else if (nrOfRows == 3 && nrOfColumns == 1)
        {
            listToBeReturned.Add(gridCells[4]);
            listToBeReturned.Add(gridCells[5]);
            listToBeReturned.Add(gridCells[8]);
        }
        else if (nrOfRows == 3 && nrOfColumns == 2)
        {
            listToBeReturned.Add(gridCells[1]);
            listToBeReturned.Add(gridCells[4]);
            listToBeReturned.Add(gridCells[5]);
            listToBeReturned.Add(gridCells[8]);
            listToBeReturned.Add(gridCells[9]);
        }
        else if (nrOfRows == 3 && nrOfColumns == 3)
        {
            for (int i = 1; i < 11; i++)
            {
                listToBeReturned.Add(gridCells[i]);
            }
            listToBeReturned.RemoveAt(3);
            listToBeReturned.RemoveAt(6);
        }
        else if (nrOfRows == 3 && nrOfColumns == 4)
        {
            return gridCells;
        }

        return listToBeReturned;
    }

    public String CheckGridFull()
    {
        int count = 0;
        foreach (var item in this.ReturnGridCells())
        {
            if (item.Crossing != null)
            {
                foreach (var item2 in item.Crossing.Feeders)
                {
                    if (item2.trafficLight == null)
                        return "Crossing information need to be filled" + item.Crossing.CrossingID;
                }
                count++;
            }
        }

        if (nrOfColumns * nrOfRows == count)
            return "";
        return "Grid is not full";
    }

    public void DeleteCrossing(object CellNumber)
    {
        throw new System.NotImplementedException();
    }

}

