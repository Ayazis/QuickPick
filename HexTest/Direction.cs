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
    // Use private fields and Properties to ensure proper initialization.
    private static Point right = new Point(1, 0);
    private static Point up = new Point(0, -1);
    private static Point down = new Point(0, 1);
    private static Point leftDown = new Point(-1, 1);
    private static Point left = new Point(-1, 0);
    private static Point rightUp = new Point(1, -1);

    public static Point Right => right;
    public static Point Up => up;
    public static Point Down => down;
    public static Point LeftDown => leftDown;
    public static Point Left => left;
    public static Point RightUp => rightUp;


    public static Direction[] StartRightUp = new[]
   {
        new Direction(eDirections.Right,Right),
        new Direction(eDirections.Down, Down),
        new Direction(eDirections.LeftDown, LeftDown),
        new Direction(eDirections.Left, Left),
        new Direction(eDirections.Up, Up),
        new Direction(eDirections.RightUp, RightUp)
    };

}

