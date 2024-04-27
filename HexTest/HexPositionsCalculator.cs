using System;
using System.Collections.Generic;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class HexGridCalculator
{
    public List<Point> CreateHexPositions(int edgeLength, int numHexagons)
    {
        int gridSize = CalculateGridSize(edgeLength, numHexagons);
        double vx = Math.Sin(Math.PI / 6), vy = Math.Cos(Math.PI / 6);
        int topLeftBound = edgeLength - 1, bottomRightBound = 3 * edgeLength - 2;
        List<Point> positions = new List<Point>();


        int count = 0;
        for (int y = 0; y < gridSize; ++y)
        {
            for (int x = 0; x < gridSize; ++x)
            {
                if (ShouldNotPlaceHexagonAtThisPosition(edgeLength, y, x))
                    continue;

                positions.Add(new Point
                {
                    X = vx * y + x,
                    Y = vy * y
                });
                count++;

                if (positions.Count >= numHexagons)
                    return positions;
            }
        }
        return positions;
    }

    private int CalculateGridSize(int edgeLength, int numHexagons)
    {
        const int maxGridSize = int.MaxValue / 2; // Limit to prevent integer overflow
        int gridSize = 2 * edgeLength - 1;

        while (true)
        {
            int count = 0;
            for (int y = 0; y < gridSize; ++y)
            {
                for (int x = 0; x < gridSize; ++x)
                {
                    if (ShouldNotPlaceHexagonAtThisPosition(edgeLength, y, x)) continue;
                    count++;
                    if (count >= numHexagons)
                        return gridSize;
                }
            }

            if (gridSize >= maxGridSize)
                return -1; // Maximum grid size reached, cannot accommodate desired number of hexagons

            gridSize++;
        }
    }

    private static bool ShouldNotPlaceHexagonAtThisPosition(int edgeLength, int y, int x)
    {
       // return false;
        double tl = edgeLength - 1, br = 3 * edgeLength - 2;
        return !(x + y < tl || x + y >= br);
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