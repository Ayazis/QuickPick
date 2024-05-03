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
    int _layer = 1;
    int _maxNumberOfHexes;

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
            _layer++;
        }

    }

    private void LoopThroughDirections()
    {
        int q = 0;
        int r = -_layer;

        for (int i = 0; i < _directions.Length; i++)
        {
            Point direction = _directions[i];
            CreateHexagonByDirection(ref q, ref r, direction);
        }
    }

    private void CreateHexagonByDirection(ref int q, ref int r, Point d)
    {
        for (int i = 0; i < _layer; i++)
        {
            if (_grid.Count >= _maxNumberOfHexes)
                return;

            _grid.Add(new HexPoint(q, r));
            q += d.X;
            r += d.Y;
        }
    }
}