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
        int q = 0;
        int r = -_nrOfRings;
        for (int i = 0; i < _directions.Length; i++)
        {
            Point direction = _directions[i];
            CreateHexagonsByDirection(ref q, ref r, direction);
        }
    }

    private void CreateHexagonsByDirection(ref int q, ref int r, Point d)
    {
        // The ringNumber corresponds with the number of tiles in the same direction on that ring.
        for (int i = 0; i < _nrOfRings; i++)
        {
            if (_grid.Count >= _maxNumberOfHexes)
            {
                _finishedGrid = true;
                return;
            }
            _grid.Add(new HexPoint(q, r));
            q += d.X;
            r += d.Y;
        }
    }
}