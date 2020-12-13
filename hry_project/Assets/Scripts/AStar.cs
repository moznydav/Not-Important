using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    private bool[,] map;

    private int width;

    private int height;

    private bool updated;

    static readonly double sqrt2 = Math.Sqrt(2);

    private static int[,] offsets = new int[4,2] {
        {-1, 0}, {1, 0}, {0, 1}, {0, -1}
    };

    private static int[,] cornerOffsets = new int[4,2] {
        {-1, -1}, {1, 1}, {1, -1}, {-1, 1}
    };

    void Awake()
    {
        SetupTileMap();
    }

    public void SetWall(int cellX, int cellY, bool isWall)
    {
        // Debug.Log(cellY + " : " + cellX);
        map[cellX, cellY] = isWall;
        updated = true;
    }

    void SetupTileMap() {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        width = bounds.size.x;
        height = bounds.size.y;

        map = new bool[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (allTiles[x + y * width] != null) {
                    map[x, y] = true;
                }
            }
        }
    }

    public bool IsPathClear(Vector3 from, Vector3 to, bool clearMap = false)
    {
        return IsPathClear(WorldToCell(from), WorldToCell(to), clearMap);
    }

    public bool IsPathClear(Tuple<int, int> from, Tuple<int, int> to, bool clearMap = false)
    {
        var line = GetBresenhamLine(from, to);

        int size = line.Count;
        int start = clearMap && size > 0 ? 1 : 0;
        int end = clearMap ? size - 1 : size;

        for (int i = start; i < end; i++)
        {
            var pos = line[i];
            if (map[pos.Item1, pos.Item2])
            {
                return false;
            }
        }

        return true;
    }

    public Vector3 FindFreeTileInRange(Vector3 origin, int range, int maxRange)
    {
        var originTile = WorldToCell(origin);

        for (int i = 0; i <= maxRange - range; i++)
        {
            var tile = FindFreeTileInRange(originTile, range + i);
            if (tile != null)
            {
                return CellToWorld(tile);
            }
        }

        return new Vector3();
    }

    private bool IsInMap(int x, int y)
    {
        return x < width && y >= 0 && y < height && x >= 0;
    }

    private Tuple<int, int> FindFreeTileInRange(Tuple<int, int> origin, int range)
    {
        int x = origin.Item1;
        int y = origin.Item2;

        for (int i = -range; i <= range; i++)
        {
            if (IsInMap(x + range, y + i) && IsPathClear(origin, Tuple.Create(x + range, y + i)))
            {
                return Tuple.Create(x + range, y + i);
            }

            if (IsInMap(x - range, y + i) && IsPathClear(origin, Tuple.Create(x - range, y + i)))
            {
                return Tuple.Create(x - range, y + i);
            }

            if (IsInMap(x + i, y + range) && IsPathClear(origin, Tuple.Create(x + i, y + range)))
            {
                return Tuple.Create(x + i, y + range);
            }

            if (IsInMap(x + i, y - range) && IsPathClear(origin, Tuple.Create(x + i, y - range)))
            {
                return Tuple.Create(x + i, y - range);
            }
        }

        return null;
    }


    public void SetNewTileMap(Tilemap newTilemap) {
        tilemap = newTilemap;
        SetupTileMap();
    }

    public Tuple<int, int> WorldToCell(Vector3 vec)
    {
        var cell = tilemap.WorldToCell(vec) - tilemap.origin;
        return Tuple.Create(cell.x, cell.y);
    }

    public Vector3 CellToWorld(Tuple<int, int> tuple)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(tuple.Item1, tuple.Item2, 0) + tilemap.origin);
    }

    public List<Vector3> GetPath(Vector3 from, Vector3 to)
    {
        var start = WorldToCell(from);
        var end = WorldToCell(to);

        if (IsPathClear(start, end))
        {
            return new List<Vector3>() { from, to };
        }

        int processedNodes = 0;

        var open = new List<Tuple<int, int>>() { start };
        var inOpen = new HashSet<Tuple<int, int>>() { start };
        var score = CreateScoreArray();
        var gscore = CreateScoreArray();

        var parents = new Tuple<int, int>[width * height];

        score[start.Item1 + start.Item2 * width] = Heuristic(start, end);
        gscore[start.Item1 + start.Item2 * width] = 0;

        while (open.Count != 0)
        {
            var current = open[open.Count - 1];
            open.RemoveAt(open.Count - 1); // TODO: Use priority queue
            inOpen.Remove(current);

            processedNodes++;

            int x = current.Item1;
            int y = current.Item2;

            if (x == end.Item1 && y == end.Item2)
            {
                Debug.Log("Processed nodes" + processedNodes);
                return BuildPath(parents, end);
            }

            bool newAdded = false;

            foreach (var neighbor in GetNeighbors(current))
            {
                double cgscore = gscore[x + y * width] + GetWeight(current, neighbor);
                int index = neighbor.Item1 + neighbor.Item2 * width;

                if (cgscore >= gscore[index])
                {
                    continue;
                }

                parents[index] = current;
                gscore[index] = cgscore;
                score[index] = gscore[index] + Heuristic(neighbor, end);

                if (inOpen.Contains(neighbor))
                {
                    continue;
                }

                inOpen.Add(neighbor);
                open.Add(neighbor);
                newAdded = true;
            }

            if (newAdded)
            {
                open.Sort((a, b) => {
                    double sa = score[a.Item1 + a.Item2 * width];
                    double sb = score[b.Item1 + b.Item2 * width];

                    if (sa == sb)
                    {
                        return 0;
                    }

                    return sa > sb ? -1 : 1;
                });
            }
        }

        return new List<Vector3>();
    }

    private List<Vector3> BuildPath(Tuple<int, int>[] parents, Tuple<int, int> end)
    {
        var path = new List<Vector3>();

        while (parents[end.Item1 + end.Item2 * width] != null)
        {
            path.Add(CellToWorld(end));
            end = parents[end.Item1 + end.Item2 * width];
        }

        return path;
    }

    private double GetWeight(Tuple<int, int> from, Tuple<int, int> to)
    {
        if (from.Item1 != to.Item1 && from.Item2 != to.Item2)
        {
            return sqrt2;
        }

        return 1;
    }

    private IEnumerable<Tuple<int, int>> GetNeighbors(Tuple<int, int> point)
    {
        int x = point.Item1;
        int y = point.Item2;

        bool[,] neighbors = new bool[3, 3];

        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int tmpX = x + offsets[i, 0];
            int tmpY = y + offsets[i, 1];

            if (tmpX >= 0 && tmpX < width && tmpY >= 0 && tmpY < height && !map[tmpX, tmpY])
            {
                neighbors[1 + offsets[i, 0], 1 + offsets[i, 1]] = true;
                yield return Tuple.Create(tmpX, tmpY);
            }
        }

        for (int i = 0; i < cornerOffsets.GetLength(0); i++)
        {
            int tmpX = x + cornerOffsets[i, 0];
            int tmpY = y + cornerOffsets[i, 1];

            if (neighbors[1, 1 + cornerOffsets[i, 1]] && neighbors[1 + cornerOffsets[i, 0], 1] && !map[tmpX, tmpY])
            {
                yield return Tuple.Create(tmpX, tmpY);
            }
        }
    }

    private double[] CreateScoreArray()
    {
        var score = new double[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                score[x + y * width] = Double.MaxValue;
            }
        }

        return score;
    }

    private double Heuristic(Tuple<int, int> node, Tuple<int, int> goal)
    {
        var dx = Math.Abs(node.Item1 - goal.Item1);
        var dy = Math.Abs(node.Item2 - goal.Item2);

        return 1.0 * (dx + dy) + (sqrt2 - 2) * Math.Min(dx, dy);
    }

    /* Method taken from UIR course (https://cw.fel.cvut.cz/wiki/courses/b4m36uir/hw/t1c-map) */
    private List<Tuple<int, int>> GetBresenhamLine(Tuple<int, int> start, Tuple<int, int> end)
    {
        var line = new List<Tuple<int, int>>();

        int x0 = start.Item1;
        int y0 = start.Item2;

        int x1 = end.Item1;
        int y1 = end.Item2;

        var dx = Math.Abs(x1 - x0);
        var dy = Math.Abs(y1 - y0);

        var x = x0;
        var y = y0;

        var sx = x0 > x1 ? -1 : 1;
        var sy = y0 > y1 ? -1 : 1;

        if (dx > dy) {
            var err = dx / 2.0;

            while (x != x1) {
                line.Add(Tuple.Create(x, y));
                err -= dy;
                if (err < 0) {
                    y += sy;
                    err += dx;
                }
                x += sx;
            }
        } else {
            var err = dy / 2.0;

            while (y != y1) {
                line.Add(Tuple.Create(x, y));
                err -= dx;
                if (err < 0) {
                    x += sx;
                    err += dy;
                }
                y += sy;
            }
        }

        return line;
    }
}
