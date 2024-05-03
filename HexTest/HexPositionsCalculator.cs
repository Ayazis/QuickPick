using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Drawing;

public interface IHexPositionsCalculator
{
    List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes);
}

public class HexPositionsCalculator : IHexPositionsCalculator
{
    List<HexPoint> _grid = new List<HexPoint> { new HexPoint(0, 0) }; // Start with the central hexagon at (0, 0)
    Point[] _directions = { new(1, 0), new(0, 1), new(-1, 1), new(-1, 0), new(0, -1), new(1, -1) };
    
    int layer = 1;
    public List<HexPoint> GenerateHexagonalGridFixed(int numberOfHexes)
    {
        while (_grid.Count < numberOfHexes)
        {
            int q = 0, r = -layer;
            foreach (var d in _directions)
            {
                for (int i = 0; i < layer; i++)
                {
                    if (_grid.Count >= numberOfHexes)
                        break;

                    _grid.Add(new HexPoint(q, r));                    
                    q += d.X;
                    r += d.Y;
                }
            }
            layer++;
        }
        return _grid;
    }
}
