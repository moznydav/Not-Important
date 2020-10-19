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

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (allTiles[x + y * width] != null) {
                    map[x, y] = true;
                }
            }
        }
    }

    public List<Vector3> GetPath(Vector3 from, Vector3 to)
    {
        var tileFrom = tilemap.WorldToCell(from);
        var tileTo = tilemap.WorldToCell(to);

        var start = Tuple.Create<int, int>(tileFrom.x, tileFrom.y); // TODO: use struct?
        var end = Tuple.Create<int, int>(tileTo.x, tileTo.y);

        var open = new List<Tuple<int, int>>() { start };
        var score = new int[width * height];
        var parents = new Tuple<int, int>[width * height];
        var inOpen = new HashSet<Tuple<int, int>>() { start };

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                score[x + y * width] = Int16.MaxValue;
            }
        }

        score[start.Item1 + start.Item2 * width] = 0;

        while (open.Count != 0)
        {
            var current = open[0];
            open.RemoveAt(open.Count - 1); // TODO: Use priority queue
            inOpen.Remove(current);

            int x = current.Item1;
            int y = current.Item2;

            if (x == end.Item1 && y == end.Item2)
            {
                return BuildPath(parents, end);
            }

            foreach (var neighbor in GetNeighbors(current))
            {
                int gscore = score[x + y * width] + 1; // Use better heuristic, diagonals should be 1.6 etc.
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

                open.Sort((a, b) => {
                    int sa = score[a.Item1 + a.Item2 * width];
                    int sb = score[b.Item1 + b.Item2 * width];

                    if (sa == sb)
                    {
                        return 0;
                    }

                    return sa > sb ? 1 : -1;
                });
            }
        }

        return new List<Vector3>();
    }

    private List<Vector3> BuildPath(Tuple<int, int>[] parents, Tuple<int, int> end)
    {
        var path = new List<Vector3>() { tilemap.CellToWorld(new Vector3Int(end.Item1, end.Item2, 0)) };

        while (parents[end.Item1 + end.Item2 * width] != null)
        {
            end = parents[end.Item1 + end.Item2 * width];
            path.Add(tilemap.CellToWorld(new Vector3Int(end.Item1, end.Item2, 0)));
        }

        return path;
    }

    private IEnumerable<Tuple<int, int>> GetNeighbors(Tuple<int, int> point)
    {
        for (int i = Math.Max(0, point.Item1 - 1); i < Math.Min(width, point.Item1 + 1); i++)
        {
            for (int j = Math.Max(0, point.Item2 - 1); j < Math.Min(height, point.Item2 + 1); j++)
            {
                if (i == 0 && j == 0)
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
}
