using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpForMatch
{
    public static (TileData[], TileData[]) GetConnections(int originX, int originY, TileData[,] tiles)
    {
        var origin = tiles[originX, originY];

        var width = tiles.GetLength(0);
        var height = tiles.GetLength(1);

        var horizontalConnections = new List<TileData>();
        var verticalConnections = new List<TileData>();

        for (var x = originX - 1; x >= 0; x--)
        {
            var other = tiles[x, originY];

            if (other.TypeId != origin.TypeId) break;

            horizontalConnections.Add(other);
        }

        for (var x = originX + 1; x < width; x++)
        {
            var other = tiles[x, originY];

            if (other.TypeId != origin.TypeId) break;

            horizontalConnections.Add(other);
        }

        for (var y = originY - 1; y >= 0; y--)
        {
            var other = tiles[originX, y];

            if (other.TypeId != origin.TypeId) break;

            verticalConnections.Add(other);
        }

        for (var y = originY + 1; y < height; y++)
        {
            var other = tiles[originX, y];

            if (other.TypeId != origin.TypeId) break;

            verticalConnections.Add(other);
        }

        return (horizontalConnections.ToArray(), verticalConnections.ToArray());
    }

    public static Match FindBestMatch(TileData[,] tiles)
    {
        var bestMatch = default(Match);

        for (var y = 0; y < tiles.GetLength(1); y++)
        {
            for (var x = 0; x < tiles.GetLength(0); x++)
            {
                var tile = tiles[x, y];

                var (h, v) = GetConnections(x, y, tiles);

                var match = new Match(tile, h, v);

                if (match.Score < 0) continue;

                if (bestMatch == null)
                {
                    bestMatch = match;
                }
                else if (match.Score > bestMatch.Score) bestMatch = match;
            }
        }

        return bestMatch;
    }
}
