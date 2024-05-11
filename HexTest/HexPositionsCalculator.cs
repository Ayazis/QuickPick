using Hexgrid;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Xps;
public interface IHexPositionsCalculator
{
    List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes, bool useNewMethod = false);
}
public class HexPositionsCalculator : IHexPositionsCalculator
{
    public HexPositionsCalculator()
    {

    }
    const int COLUMN_START = 0;
    // old method:
    // Point[] _directions = new Point[] { Right, Down, LeftDown, Left, Up, RightUp };

    // new method
    //Point[] _directions = new Point[] { Up, Right, Down, LeftDown, Left, Up, RightUp };

    // start RightUp
    Point[] _directions = new Point[] { Right, Down, LeftDown, Left, Up, RightUp };

    static Point Right = new Point(1, 0);
    static Point Up = new Point(0, -1);
    static Point Down = new Point(0, 1);
    static Point LeftDown = new Point(-1, 1);
    static Point Left = new Point(-1, 0);
    static Point RightUp = new Point(1, -1);

    int _maxNumberOfHexes;

    private Grid _grid = new();


    public List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes, bool useNewMethod = false)
    {
        //Oldmethod(numberOfHexes);
        NewMethod(numberOfHexes);

        //if (useNewMethod)
        //    NewMethod(numberOfHexes);
        //else
        //    Oldmethod(numberOfHexes);


        var json = JsonConvert.SerializeObject(_grid);
        return _grid.HexPoints;
    }

    private void NewMethod(int numberOfHexes)
    {
        for (int i = 1; i < numberOfHexes; i++)
        {
            Point newDirection = GetDirectionForNextHexagon(i);
            var prev = _grid.HexPoints.Last();
            var newPoint = new HexPoint(prev.Column + newDirection.X, prev.Row + newDirection.Y);
            _grid.HexPoints.Add(newPoint);
        }
    }

    void Oldmethod(int numberOfHexes)
    {
        _maxNumberOfHexes = numberOfHexes;
        FinishGrid();

    }

    private void FinishGrid()
    {
        for (int i = _grid.HexPoints.Count; i < _maxNumberOfHexes; i++)
        {
            ContinueGrid();
        }
    }

    private void ContinueGrid()
    {
        _grid.CurrentRow = -_grid.NrOfRings; // Use negative, this places the start of the new ring above the previous one.
        for (int i = _grid.DirectionIndex; i < _directions.Length; i++) // Use directionIndex to continue with the correct direction.
        {
            Point direction = _directions[i];
            Debug.WriteLine(direction.X + " " + direction.Y);
            CreateHexagonsByDirection(direction, out bool finishedGridState, out bool finishedDirection);
            if (finishedGridState)
            {
                if (finishedDirection)
                {
                    FinishLayer();
                    return;
                }
                return;
            }
            _grid.DirectionIndex++;
            _grid.MovementIndex = 0;
        }
        FinishLayer();
        return;
    }

    private void FinishLayer()
    {
        _grid.DirectionIndex = 0; // if done with directions, reset to 0 for next layer.
        _grid.MovementIndex = 0;
        _grid.NrOfRings++;
    }

    private void CreateHexagonsByDirection(Point d, out bool finishedGrid, out bool finishedDirection)
    {
        // The ringNumber corresponds with the number of tiles in the same direction on that ring.
        // So, say on circle 3, we need to do 3 tiles in the same direction before moving to the next direction.
        // Todo: i might not always be 0. we need to keep track of how many movements in the currentDirection we've had.

        for (int i = _grid.MovementIndex; i < _grid.NrOfRings; i++)
        {
            if (_grid.HexPoints.Count >= _maxNumberOfHexes)
            {
                finishedGrid = true;
                finishedDirection = false;
                return;
            }

            var nexHexpoint = new HexPoint(_grid.CurrentColumn, _grid.CurrentRow);
            _grid.HexPoints.Add(nexHexpoint);
            _grid.CurrentColumn += d.X;
            _grid.CurrentRow += d.Y;
            _grid.MovementIndex++;
        }
        finishedDirection = true;
        finishedGrid = false;
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


            //return ringNumber == 0 ? 0 : 1 + 3 * (ringNumber - 1) * ringNumber;
            // Total hexagons up to previous ring
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


    private class Grid
    {
        public List<HexPoint> HexPoints = new List<HexPoint> { new HexPoint(0, 0) }; // Start with the central hexagon at (0, 0)
        public int CurrentColumn = COLUMN_START;
        public int DirectionIndex;
        public int MovementIndex;
        public int NrOfRings = 1;
        public int CurrentRow;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"HexPoints: {HexPoints.Count}");
            sb.AppendLine($"CurrentColumn: {CurrentColumn}");
            sb.AppendLine($"DirectionIndex: {DirectionIndex}");
            sb.AppendLine($"MovementIndex: {MovementIndex}");
            sb.AppendLine($"NrOfRings: {NrOfRings}");
            sb.AppendLine($"CurrentRow: {CurrentRow}");

            return sb.ToString();

        }
    }

}
public static class PointExtensions
{
    public static Point AddPoint(this Point point1, Point point2)
    {
        return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }
}