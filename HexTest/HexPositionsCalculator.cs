using System.Drawing;
using System.Windows.Controls;

public interface IHexPositionsCalculator
{
    List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes);
}

public class HexPositionsCalculator : IHexPositionsCalculator
{
    List<HexPoint> _grid = new List<HexPoint> { new HexPoint(0, 0) }; // Start with the central hexagon at (0, 0)
    Point[] _directions = { new(1, 0), new(0, 1), new(-1, 1), new(-1, 0), new(0, -1), new(1, -1) };
    int _nrOfRings = 1;
    int _maxNumberOfHexes;
    bool _finishedGrid;

    public List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes)
    {
        _maxNumberOfHexes = 48;

        FinishGrid();

        return _grid;
    }

    private void FinishGrid()
    {
        for (int i = _grid.Count; i < _maxNumberOfHexes; i++)
        {

            LoopThroughDirections();
            _nrOfRings++;
            if (_finishedGrid)
                return;
        }
    }

    private void LoopThroughDirections()
    {
        int column = 0;
        int row = -_nrOfRings;
        for (int i = 0; i < _directions.Length; i++)
        {
            Point direction = _directions[i];
            CreateHexagonsByDirection(ref column, ref row, direction);
        }
    }

    private void CreateHexagonsByDirection(ref int column, ref int row, Point d)
    {
        // The ringNumber corresponds with the number of tiles in the same direction on that ring.
        for (int i = 0; i < _nrOfRings; i++)
        {
            if (_grid.Count >= _maxNumberOfHexes)
            {
                _finishedGrid = true;
                return;
            }
            _grid.Add(new HexPoint(column, row));
            column += d.X;
            row += d.Y;
        }
    }
}