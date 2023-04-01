using UnityEngine;

public class Match
{
    public readonly int TypeId;

    public readonly int Score;

    public readonly TileData[] Tiles;

    public Match(TileData origin, TileData[] horizontal, TileData[] vertical, TileData[] diagonalLeft, TileData[] diagonalRight)
    {
        TypeId = origin.TypeId;

        if (horizontal.Length >= 2 && vertical.Length >= 2)
        {
            Tiles = new TileData[horizontal.Length + vertical.Length + 1];

            Tiles[0] = origin;

            horizontal.CopyTo(Tiles, 1);

            vertical.CopyTo(Tiles, horizontal.Length + 1);
        }
        else if (horizontal.Length >= 1)
        {
            Tiles = new TileData[horizontal.Length + 1];

            Tiles[0] = origin;

            horizontal.CopyTo(Tiles, 1);
        }
        else if (vertical.Length >= 1)
        {
            Tiles = new TileData[vertical.Length + 1];

            Tiles[0] = origin;

            vertical.CopyTo(Tiles, 1);
        }
        else if (diagonalLeft.Length >= 2)
        {
            Tiles = new TileData[diagonalLeft.Length + 1];

            Tiles[0] = origin;

            diagonalLeft.CopyTo(Tiles, 1);
        }
        else if (diagonalRight.Length >= 2)
        {
            Tiles = new TileData[diagonalRight.Length + 1];

            Tiles[0] = origin;

            diagonalRight.CopyTo(Tiles, 1);
        }
        
        else Tiles = null;

        Score = Tiles?.Length ?? -1;
    }
}