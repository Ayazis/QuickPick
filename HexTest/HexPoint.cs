using System;
using System.Diagnostics;
using System.Drawing;

namespace Hexgrid;
[DebuggerDisplay("{Column}, {Row}")]
public struct HexPoint : IEquatable<HexPoint>
{

    public int Column { get; set; }
    public int Row { get; set; }

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

    bool IEquatable<HexPoint>.Equals(HexPoint other)
    {
        return other.Column == Column && other.Row == Row;
    }
    public override bool Equals(object obj)
    {
        if (obj is HexPoint other)
        {
            return this.Column == other.Column && this.Row == other.Row;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Column, Row);
    }
    public override string ToString()
    {
        return $"{Column}, {Row}";
    }
}



