using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

public class CrossingA : Crossing
{

    public CrossingA(int crossingID) : base(crossingID) { }
    public System.Timers.Timer SensorTimer { get; set; }
    public int LGI { get; set; }
    public List<Point> peopleTL = new List<Point> { new Point(30, 30), new Point(32, 32), new Point(34, 34), new Point(36, 36) };
    public List<Point> peopleTR= new List<Point> { new Point(166, 30), new Point(168, 32), new Point(180, 34), new Point(182, 36) };
    public List<Point> peopleBL = new List<Point> { new Point(30, 160), new Point(32, 162), new Point(34, 164), new Point(36, 166) };
    public List<Point> peopleBR = new List<Point> { new Point(160, 160), new Point(162, 162), new Point(164, 164), new Point(166, 166) };


   
    public void movePeople()
    {
        for (int i = 0; i < peopleTL.Count;i++ )
        {
            int x = peopleTL[i].X;
            int y = peopleTL[i].Y;
            if (x<56 && y<56)
            {
                x++; y++;
            }
            if (x == 56 && y < 140)
                y++;
            if (y>139)
            {
                if (x>30&& y<160 && i==3)
                {
                    y++; x--;
                }
                if (x > 35 && y < 165 && i == 2)
                {
                    y++; x--;
                }
                if (x > 40 && y < 170 && i == 1)
                {
                    y++; x--;
                }
                if (x > 45 && y < 175 && i == 0)
                {
                    y++; x--;
                }
            }
            peopleTL[i] = new Point(x, y);
        }
        //topRight
        for (int i = 0; i < peopleTR.Count; i++)
        {
            int x = peopleTR[i].X;
            int y = peopleTR[i].Y;
            if (x >= 140 && y < 56)
            {
                x--; 
                y++;
            }
            if (x >50 && y == 56)
                x--;
            if (x <= 56)
            {
                if (x > 30 && y > 30 &&i==0)
                {
                    y--; x--;
                }
                if (x > 35 && y > 35&& i==1)
                {
                    y--; x--;
                }
                if (x > 40 && y > 40 && i==2)
                {
                    y--; x--;
                }
                if (x > 45 && y > 45 && i == 3)
                {
                    y--; x--;
                }
            }
            peopleTR[i] = new Point(x, y);
        }
        //BpttomLeft
        for (int i = 0; i < peopleBL.Count; i++)
        {
            int x = peopleBL[i].X;
            int y = peopleBL[i].Y;
            if (y>140 && x < 139)
            {
                //if (x<56)
                {
                    x++; y--;
                }

                
            }
            else if (x < 140 && y == 140)
                x++;
            else if (y > 139 && x > 139)
            {
                if (x > 30 && y < 160 &&i==0)
                {
                    y++; x++;
                }
                if (x > 35 && y < 165 && i == 1)
                {
                    y++; x++;
                }
                if (x > 40 && y < 170 && i == 2)
                {
                    y++; x++;
                }
                if (x > 45 && y < 175 && i == 3)
                {
                    y++; x++;
                }
            }
            peopleBL[i] = new Point(x, y);
        }
        //BottomRight
        for (int i = 0; i < peopleBR.Count; i++)
        {
            int x = peopleBR[i].X;
            int y = peopleBR[i].Y;
            if (x > 140 && y > 140)
            {
                x--; y--;
            }
            if (x == 140 && y > 56)
                y--;
            if (y > 30)
            {
                if (y < 60 && x < 165 && i==0)
                {
                    y--; x++;
                }
                if (y < 65 && x < 170 && i==1)
                {
                    y--; x++;
                }
                if (y < 70 && x < 177 && i == 2)
                {
                    y--; x++;
                }
                if (y < 75 && x < 182 && i == 3)
                {
                    y--; x++;
                }
            }
            peopleBR[i] = new Point(x, y);
        }
    }


    public void SetSensorTime(int sensorTime)//lastGreenTrafficLightIndex
    {
        
        this.SensorTime = sensorTime;
        SensorTimer = new System.Timers.Timer();
        SensorTimer.Interval = sensorTime*1000;
        SensorTimer.Elapsed += SensorTimer_Elapsed;
    }

    void SensorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        SensorTimer.Stop();
        List<Point> tempTL = this.peopleTL;
        List<Point> tempBL = this.peopleBL;
        this.peopleTL = this.peopleTR;
        peopleBL = tempTL;
        peopleTR = peopleBR;
        peopleBR = tempBL;
        //peopleTL = new List<Point> { new Point(30, 30), new Point(32, 32), new Point(34, 34), new Point(36, 36) };
        //peopleTR = new List<Point> { new Point(166, 30), new Point(168, 32), new Point(180, 34), new Point(182, 36) };
        //peopleBL = new List<Point> { new Point(30, 160), new Point(32, 162), new Point(34, 164), new Point(36, 166) };
        //peopleBR = new List<Point> { new Point(160, 160), new Point(162, 162), new Point(164, 164), new Point(166, 166) };
        this.Feeders.Find(x => x.FeederID == (LGI%4) +1).trafficLight.greenLightTimer.Start();
        
    }

    private int SensorTime;

	public virtual object State
	{
		get;
		set;
	}

	public virtual object Timer
	{
		get;
		set;
	}

	public virtual void TimerHandeler()
	{
		throw new System.NotImplementedException();
	}

	public virtual void SetState()
	{
		throw new System.NotImplementedException();
	}

}

