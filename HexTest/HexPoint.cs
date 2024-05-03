using System;
using System.Diagnostics;

[DebuggerDisplay("{Column}, {Row}")]
public class HexPoint
{
    public int Column { get; }
    public int Row { get; }

    public HexPoint(int q, int r)
    {
        Column = q;
        Row = r;
    }

    // Optionally, if Cartesian coordinates are needed:
    public int X => ConvertToX(Column, Row);
    public int Y => ConvertToY(Column, Row);

    private static int ConvertToX(int q, int r)
    {
        return (int)(3.0 / 2 * q);
    }

    private static int ConvertToY(int q, int r)
    {
        return (int)(Math.Sqrt(3) * (r + 0.5 * q));
    }
}
