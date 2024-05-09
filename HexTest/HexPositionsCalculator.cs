using Hexgrid;
using Newtonsoft.Json;
using System.Drawing;
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
    Point[] _directions = new Point[] { Right, Down, LeftDown, Left, Up, RightUp };

    static Point Right = new Point(1, 0);
    static Point Down = new Point(0, 1);
    static Point LeftDown = new Point(-1, 1);
    static Point Left = new Point(-1, 0);
    static Point Up = new Point(0, -1);
    static Point RightUp = new Point(1, -1);

    int _maxNumberOfHexes;

    private Grid _grid = new();


    public List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes, bool useNewMethod = false)
    {

        if (useNewMethod)
            NewMethod(numberOfHexes);
        else
            Oldmethod(numberOfHexes);


        var json = JsonConvert.SerializeObject(_grid);
        return _grid.HexPoints;
    }

    private void NewMethod(int numberOfHexes)
    {
        for (int i = 0; i < numberOfHexes; i++)
        {
            _maxNumberOfHexes++;
            FinishGrid();
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
        int totalPrevious = ringNumber == 0 ? 0 : 1 + 3 * (ringNumber - 1) * ringNumber; // Total hexagons up to previous ring
        int positionInRing = hexNumber - totalPrevious;

        //    Point[] directions = new Point[] {
        //    new Point(1, 0),    // Right
        //    new Point(0, 1),    // Down
        //    new Point(-1, 1),   // LeftDown
        //    new Point(-1, 0),   // Left
        //    new Point(0, -1),   // Up
        //    new Point(1, -1)    // RightUp
        //};

        // Check if the hexagon is the first one in a new ring
        if (positionInRing == 1)
        {
            // Optionally, adjust this direction if your grid starts differently
            return _directions[0]; // Typically 'Right' for the first hexagon in a new ring
        }

        // Find which side the hexagon is on within the ring
        int side = (positionInRing - 1) / ringNumber;
        return _directions[side];
    }
    public int CalculateRingNumber(int hexNumber)
    {
        if (hexNumber == 1)
            return 0; // The central hexagon is always in ring 0.

        int n = 1; // Start checking from the first ring.
        int totalHexagons = 1; // Start with the central hexagon.

        // Calculate the total number of hexagons up to and including ring n
        while (true)
        {
            totalHexagons += 6 * n;
            if (totalHexagons >= hexNumber)
                break;
            n++;
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
