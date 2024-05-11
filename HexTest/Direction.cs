using System.Drawing;

public struct Direction
{
    public Direction(eDirections name, Point movement)
    {
        Name = name;
        Movement = movement;
    }

    public eDirections Name { get; }

    public Point Movement { get; }

    public override string ToString()
    {
        return Name.ToString();
    }
}

public static class DirectionOrders
{
    public static Direction[] StartRightUp = new[]
{
        new Direction(eDirections.Right, new Point(1, 0)),
        new Direction(eDirections.Up, new Point(0, -1)),
        new Direction(eDirections.Down, new Point(0, 1)),
        new Direction(eDirections.LeftDown, new Point(-1, 1)),
        new Direction(eDirections.Left, new Point(-1, 0)),
        new Direction(eDirections.RightUp, new Point(1, -1))
    };



}

