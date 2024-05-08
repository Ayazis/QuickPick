﻿using Hexgrid;
using Newtonsoft.Json;
using System.Drawing;
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
    Point[] _directions = { new(1, 0), new(0, 1), new(-1, 1), new(-1, 0), new(0, -1), new(1, -1) };
    int _maxNumberOfHexes;

    private Grid _grid = new();


    public List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes, bool useNewMethod = false)
    {

        if (useNewMethod)
            NewMethod(numberOfHexes);
        else
            Oldmethod();


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

    void Oldmethod()
    {
        _maxNumberOfHexes = 37;
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

    private class Grid
    {
        public List<HexPoint> HexPoints = new List<HexPoint> { new HexPoint(0, 0) }; // Start with the central hexagon at (0, 0)
        public int CurrentColumn = COLUMN_START;
        public int DirectionIndex;
        public int MovementIndex;
        public int NrOfRings = 1;
        public int CurrentRow;
    }
}
