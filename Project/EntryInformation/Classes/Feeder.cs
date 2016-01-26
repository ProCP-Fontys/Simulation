
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using EntryInformation.Classes;
using EntryInformation;

[Serializable]
public class Feeder
{
    public int FeederID { get; set; }
    private Crossing crossing;// { get; set; }
    public Car[] CarsGoingOut { get; set; }
    public Car[] CarsComingIn { get; set; }
    public Car[] TotalCars; //Total amount of cars specified at the beginning
    private Random random;
    [NonSerialized]
    public TrafficLight trafficLight;
    public List<Point> StopPointsComingIn { get; set; }
    public List<Point> StopPointsGoingOut { get; set; }
    private List<int> RightPercentageList;
    private List<int> LeftPercentageList;
    private List<int> StraightPercentageList;
    [NonSerialized]
    private Simulation simulation;

    public Feeder(int feederID, Crossing crossing, Simulation simulation)
    {
        this.simulation = simulation;
        this.FeederID = feederID;
        this.crossing = crossing;
        random = new Random();

        CarsComingIn = new Car[5];
        CarsGoingOut = new Car[5];

        StopPointsComingIn = new List<Point>();
        StopPointsGoingOut = new List<Point>();

        switch (this.FeederID)
        {
            case 1:
                for (int i = 4; i > -1; i--)
                {
                    this.StopPointsGoingOut.Add(new Point(Convert.ToInt16(i + "7"), 112));
                }

                for (int i = 12; i < 53; i += 10)
                {
                    this.StopPointsComingIn.Add(new Point(i, 82));
                }
                break;
            case 2:
                for (int i = 4; i > -1; i--)
                {
                    this.StopPointsGoingOut.Add(new Point(82, Convert.ToInt16(i + "7")));
                }
                for (int i = 12; i < 53; i += 10)
                {
                    this.StopPointsComingIn.Add(new Point(112, Convert.ToInt16(i)));
                }
                break;
            case 3:
                for (int i = 146; i < 187; i += 10)
                {
                    this.StopPointsGoingOut.Add(new Point(i, 82));
                }
                for (int i = 186; i > 145; i -= 10)
                {
                    this.StopPointsComingIn.Add(new Point(i, 112));
                }
                break;
            case 4:
                for (int i = 146; i < 187; i += 10)
                {
                    this.StopPointsGoingOut.Add(new Point(112, i));
                }

                for (int i = 192; i > 151; i -= 10)
                {
                    this.StopPointsComingIn.Add(new Point(82, i));
                }
                break;
        }
    }

    public Direction ReturnDirection()
    {
        int randomNmr = random.Next(1, 101);

        bool found = StraightPercentageList.Contains(randomNmr);

        if (found)
            return Direction.Straight;

        found = RightPercentageList.Contains(randomNmr);

        if (found)
            return Direction.Right;

        return Direction.Left;
    }

    public void AddDetailes(int greenLight, int rPercentage, int lPercentage, int sPercentage, int carQuantity)
    {
        List<int> Percentages = new List<int>();

        TotalCars = new Car[carQuantity];

        LeftPercentageList = new List<int>();
        RightPercentageList = new List<int>();
        StraightPercentageList = new List<int>();

        for (int i = 1; i < 101; i++)
        {
            Percentages.Add(i);
        }

        while (StraightPercentageList.Count != sPercentage)
        {
            StraightPercentageList.Add(Percentages[0]);
            Percentages.RemoveAt(0);
        }

        while (LeftPercentageList.Count != lPercentage)
        {
            LeftPercentageList.Add(Percentages[0]);
            Percentages.RemoveAt(0);
        }

        while (RightPercentageList.Count != rPercentage)
        {
            RightPercentageList.Add(Percentages[0]);
            Percentages.RemoveAt(0);
        }

        trafficLight = new TrafficLight(this.crossing, greenLight, this.FeederID, simulation);

        switch (this.FeederID)
        {
            case 1:
                for (int i = 0; i < carQuantity; i++)
                {
                    TotalCars[i] = new Car(new Point(-3, 112));
                }
                break;
            case 2:
                for (int i = 0; i < carQuantity; i++)
                {
                    TotalCars[i] = new Car(new Point(82, -3));
                }
                break;
            case 3:
                for (int i = 0; i < carQuantity; i++)
                {
                    TotalCars[i] = new Car(new Point(199, 82));
                }
                break;
            case 4:
                for (int i = 0; i < carQuantity; i++)
                {
                    TotalCars[i] = new Car(new Point(112, 199));
                }
                break;
        }
    }

}

