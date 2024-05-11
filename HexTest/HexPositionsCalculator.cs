using Hexgrid;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using System.Text;
public interface IHexPositionsCalculator
{
    List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes);
}
public class HexPositionsCalculator : IHexPositionsCalculator
{
    private List<HexPoint> _hexPoints = new List<HexPoint> { new HexPoint(0, 0) }; // Start with the central hexagon at (0, 0)      
    public HexPositionsCalculator()
    {
        //_directions = DirectionHelper.StartRightUp;
    }


    Point[] _directions = new Point[] { Right, Down, LeftDown, Left, Up, RightUp };

    static Point Right = new Point(1, 0);
    static Point Up = new Point(0, -1);
    static Point Down = new Point(0, 1);
    static Point LeftDown = new Point(-1, 1);
    static Point Left = new Point(-1, 0);
    static Point RightUp = new Point(1, -1);



    public List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes)
    {
        CreateGrid(numberOfHexes);

        var json = JsonConvert.SerializeObject(_hexPoints);
        return _hexPoints;
    }

    private void CreateGrid(int numberOfHexes)
    {
        for (int i = 1; i < numberOfHexes; i++)
        {
            Point newDirection = GetDirectionForNextHexagon(i);
            var prev = _hexPoints.Last();
            var newPoint = new HexPoint(prev.Column + newDirection.X, prev.Row + newDirection.Y);
            _hexPoints.Add(newPoint);
        }
    }

    public Point GetDirectionForNextHexagon(int hexNumber)
    {

        int ringNumber = CalculateRingNumber(hexNumber);
        int totalPrevious = GetTotalHexesUpUntillThisRing(ringNumber);
        int positionInRing = hexNumber - totalPrevious;

        int side = GetSideIndex(ringNumber, hexNumber);

        if (positionInRing == 0)
            return _directions.Last(); // repeat last move to start new ring.

        return _directions[side];
    }

    public int GetSideIndex(int ringNum, int hexNum)
    {
        if (ringNum == 0)
            return -1;  // Central hexagon, no side index.

        // Calculate the starting hex number of the current ring
        int startOfRing = 1 + 3 * ringNum * (ringNum - 1);

        // Determine the offset within the ring
        int offset = hexNum - startOfRing;

        // Calculate side index based on the offset
        int sideIndex = (offset / ringNum) % 6;
        return sideIndex;
    }


    private static int GetTotalHexesUpUntillThisRing(int ringNumber)
    {
        int totalPrevious = 1; // Starting with 1 hexagon in the central position.
        for (int i = 1; i < ringNumber; i++)
        {
            totalPrevious += 6 * i;
        }
        return totalPrevious;
    }

    public int CalculateRingNumber(int hexNumber)
    {
        if (hexNumber == 1) return 1; // The central hexagon is considered to be in Ring 1.

        int n = 1; // Start from ring 1
        int totalHexagons = 0; // Start with the central hexagon, part of Ring 1.

        while (true)
        {
            totalHexagons += 6 * n; // Add the hexagons in the current ring
            if (totalHexagons >= hexNumber)
            {
                break;
            }
            n++; // Increment to the next ring
        }

        return n;
    }




}
public static class PointExtensions
{
    public static Point AddPoint(this Point point1, Point point2)
    {
        return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }
}