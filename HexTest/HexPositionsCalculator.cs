using System;
using System.Collections.Generic;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class HexPositionsCalculator
{
    [DebuggerDisplay("{Q}, {R}")]
    public class Point
    {
        public int Q { get; }
        public int R { get; }

        public Point(int q, int r)
        {
            Q = q;
            R = r;
        }

        // Optionally, if Cartesian coordinates are needed:
        public int X => ConvertToX(Q, R);
        public int Y => ConvertToY(Q, R);

        private static int ConvertToX(int q, int r)
        {
            return (int)(3.0 / 2 * q);
        }

        private static int ConvertToY(int q, int r)
        {
            return (int)(Math.Sqrt(3) * (r + 0.5 * q));
        }
    }

    public static List<Point> GenerateHexagonalGridFixed(int numberOfHexes)
    {
        List<Point> grid = new List<Point> { new Point(0, 0) }; // Start with the central hexagon at (0, 0)
        int hexCount = 1;
        int layer = 1;

        while (hexCount < numberOfHexes)
        {
            int q = 0, r = -layer;
            (int, int)[] directions = { (1, 0), (0, 1), (-1, 1), (-1, 0), (0, -1), (1, -1) };

            foreach (var (dq, dr) in directions)
            {
                for (int i = 0; i < layer; i++)
                {
                    if (hexCount >= numberOfHexes)
                        break;

                    grid.Add(new Point(q, r));
                    hexCount++;
                    q += dq;
                    r += dr;
                }
            }
            layer++;
        }
        var json = JsonConvert.SerializeObject(grid);
        return grid;
    }
}



// code snippet from working js
/*
function hexGrid(edgeLength)
{
    var len = 2 * edgeLength - 1,
      vx = Math.sin(Math.PI / 6), vy = Math.cos(Math.PI / 6),
        tl = edgeLength - 1, br = 3 * edgeLength - 2,
        positions = [];

    for (var y = 0; y < len; ++y)
    {
        for (var x = 0; x < len; ++x)
        {
            //you may want to remove this condition
            //to understand the code
            if (x + y < tl || x + y >= br) continue;
            positions.push({
            x: vx* y +x,
				y: vy* y

            });
}
	}
	return positions;
}

var edge = 5;
var cells = hexGrid(edge);

//simple math to determine the corner-indices
var corners = [
    0,
  edge - 1,
  1 - edge + (cells.length >> 1),
  cells.length >> 1,
  edge - 1 + (cells.length >> 1),
  cells.length - edge,
  cells.length - 1
];


cells.forEach((pos, i) => {
    //visualize the grid
    var node = document.createElement("div");
    node.className = "dot";
    //scale the values before applying
    node.style.top = pos.y * 25 + "px";
    node.style.left = pos.x * 25 + "px";
    document.body.appendChild(node);

    if (corners.indexOf(i) !== -1)
        node.style.background = "#666";
});
*/