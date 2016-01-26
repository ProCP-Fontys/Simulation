
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


[Serializable]
public class GridCell
{
    public int Number { get; set; }
    private Point location;
    public Crossing Crossing { get; set; }

    public GridCell(Point location, int number)
    {
        this.location = location;
        this.Number = number;
    }
    public void AddCrossing(Crossing c)
    {
        Crossing = c;
    }
    public void RemoveCrossing()
    {
        Crossing = null;
    }
    public Point ReturnLocation()
    {
        return location;
    }
    public Point TopLeft
    {
        get;
        set;
    }

    public int CrossingID
    {
        get;
        set;
    }

    public int Size
    {
        get;
        set;
    }

    public void IsInCell()
    {

    }

    public void DrawCrossing()
    {

    }

}

