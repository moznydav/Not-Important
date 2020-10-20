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

    void Start()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        width = bounds.size.x;
        height = bounds.size.y;

        map = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allTiles[x + y * width] != null)
                {
                    map[x, y] = true;
                }
            }
        }
    }

    private Tuple<int, int> WorldToCell(Vector3 vec)
    {
        var cell = tilemap.WorldToCell(vec) - tilemap.origin;
        return Tuple.Create(cell.x, cell.y);
    }

    private Vector3 CellToWorld(Tuple<int, int> tuple)
    {
        return tilemap.CellToWorld(new Vector3Int(tuple.Item1, tuple.Item2, 0)) + tilemap.origin;
    }

    public List<Vector3> GetPath(Vector3 from, Vector3 to)
    {
        var start = WorldToCell(from); // TODO: use struct?
        var end = WorldToCell(to);

        var open = new List<Tuple<int, int>>() { start }; // TODO: Cache `open`, `inOpen`, `score` and `parents` until `from` change
        var inOpen = new HashSet<Tuple<int, int>>() { start };
        var score = CreateScoreArray();
        var parents = new Tuple<int, int>[width * height];

        score[start.Item1 + start.Item2 * width] = 0;

        while (open.Count != 0)
        {
            var current = open[open.Count - 1];
            open.RemoveAt(open.Count - 1); // TODO: Use priority queue
            inOpen.Remove(current);

            int x = current.Item1;
            int y = current.Item2;

            if (x == end.Item1 && y == end.Item2)
            {
                return BuildPath(parents, end);
            }

            bool newAdded = false;

            foreach (var neighbor in GetNeighbors(current))
            {
                int gscore = score[x + y * width] + 1; //TODO: Use better heuristic, diagonals should be 1.6 etc.
                int index = neighbor.Item1 + neighbor.Item2 * width;

                if (gscore >= score[index])
                {
                    continue;
                }

                parents[index] = current;
                score[index] = gscore;

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
                    int sa = score[a.Item1 + a.Item2 * width];
                    int sb = score[b.Item1 + b.Item2 * width];

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

    private IEnumerable<Tuple<int, int>> GetNeighbors(Tuple<int, int> point)
    {
        int jStart = Math.Max(0, point.Item2 - 1);
        int jEnd = Math.Min(height - 1, point.Item2 + 1);

        for (int i = Math.Max(0, point.Item1 - 1); i <= Math.Min(width - 1, point.Item1 + 1); i++)
        {
            for (int j = jStart; j <= jEnd; j++)
            {
                if (map[i, j] || (i == point.Item1 && j == point.Item2))
                {
                    continue;
                }

                yield return Tuple.Create(i, j);
            }
        }
    }

    private List<Vector3> SimplifyPath(List<Vector3> path)
    {
        return path; //TODO: Try to simplify path using Bresenham line or whatever
    }

    private int[] CreateScoreArray()
    {
        var score = new int[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                score[x + y * width] = Int16.MaxValue;
            }
        }

        return score;
    }
}
