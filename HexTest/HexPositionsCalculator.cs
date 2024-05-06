using Hexgrid;
using Newtonsoft.Json;
using System.Drawing;
public interface IHexPositionsCalculator
{
    List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes);
}

public class HexPositionsCalculator : IHexPositionsCalculator
{
    const int COLUMN_START = 0;
    Point[] _directions = { new(1, 0), new(0, 1), new(-1, 1), new(-1, 0), new(0, -1), new(1, -1) };
    int _maxNumberOfHexes;
    bool _finishedGrid;

    private Grid _grid = new();


    public List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes)
    {
        _maxNumberOfHexes = 37;

        FinishGrid();

        var json = JsonConvert.SerializeObject(_grid);
        return _grid.HexPoints;
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
            CreateHexagonsByDirection(direction);
        }
        _grid.DirectionIndex = 0; // if done with directions, reset to 0 for next layer.
        _grid.NrOfRings++;
        if (_finishedGrid)
            return;
    }

    private void CreateHexagonsByDirection(Point d)
    {
        // The ringNumber corresponds with the number of tiles in the same direction on that ring.
        // So, say on circle 3, we need to do 3 tiles in the same direction before moving to the next direction.
        for (int i = 0; i < _grid.NrOfRings; i++)
        {
            if (_grid.HexPoints.Count >= _maxNumberOfHexes)
            {
                _finishedGrid = true;
                return;
            }
            var nexHexpoint = new HexPoint(_grid.CurrentColumn, _grid.CurrentRow);
            _grid.HexPoints.Add(nexHexpoint);
            _grid.CurrentColumn += d.X;
            _grid.CurrentRow += d.Y;
        }
    }

    private class Grid
    {
        public List<HexPoint> HexPoints = new List<HexPoint> { new HexPoint(0, 0) }; // Start with the central hexagon at (0, 0)
        public int CurrentColumn = COLUMN_START;
        public int DirectionIndex;
        public int NrOfRings = 1;
        public int CurrentRow;
    }
}