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

public static class DirectionHelper
{
    public static Direction[] StartRightUp = new[]
{
        new Direction(eDirections.Right,Right),
        new Direction(eDirections.Down, Down),
        new Direction(eDirections.LeftDown, LeftDown),
        new Direction(eDirections.Left, Left),
        new Direction(eDirections.Up, Up),
        new Direction(eDirections.RightUp, RightUp)
    };

    static Point Right = new Point(1, 0);
    static Point Up = new Point(0, -1);
    static Point Down = new Point(0, 1);
    static Point LeftDown = new Point(-1, 1);
    static Point Left = new Point(-1, 0);
    static Point RightUp = new Point(1, -1);

}

