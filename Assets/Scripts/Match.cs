using UnityEngine;

public class Match
{
    public readonly int TypeId;

    public readonly int Score;

    public readonly float HorizontalMultiplier;
    public readonly float VerticalMultiplier;
    public readonly float DiagonalMultiplier;

    public readonly TileData[] Tiles;

    public Match(TileData origin, TileData[] horizontal, TileData[] vertical, TileData[] diagonalLeft,
        TileData[] diagonalRight)
    {
        TypeId = origin.TypeId;

        if (horizontal.Length >= 2 && vertical.Length >= 2 && diagonalRight.Length >= 2)
        {
            Tiles = new TileData[horizontal.Length + vertical.Length + diagonalRight.Length];

            Tiles[0] = origin;

            horizontal.CopyTo(Tiles, 1);

            vertical.CopyTo(Tiles, 1);

            diagonalRight.CopyTo(Tiles, horizontal.Length + 1);
        }
        else if (horizontal.Length >= 2 && vertical.Length >= 2)
        {
            Tiles = new TileData[horizontal.Length + vertical.Length + 1];

            Tiles[0] = origin;
            
            HorizontalMultiplier = Tiles[0].HorMultiplier;
            
            VerticalMultiplier = Tiles[0].VertMultiplier;

            horizontal.CopyTo(Tiles, 1);

            vertical.CopyTo(Tiles, horizontal.Length + 1);
        }
        else if (horizontal.Length >= 2 && diagonalLeft.Length >= 2)
        {
            Tiles = new TileData[horizontal.Length + diagonalLeft.Length + 1];

            Tiles[0] = origin;

            HorizontalMultiplier = Tiles[0].HorMultiplier;

            DiagonalMultiplier = Tiles[0].DiagMultiplier;

            horizontal.CopyTo(Tiles, 1);

            diagonalLeft.CopyTo(Tiles, horizontal.Length + 1);
        }
        else if (horizontal.Length >= 2 && diagonalRight.Length >= 2)
        {
            Tiles = new TileData[horizontal.Length + diagonalRight.Length + 1];

            Tiles[0] = origin;

            HorizontalMultiplier = Tiles[0].HorMultiplier;

            DiagonalMultiplier = Tiles[0].DiagMultiplier;

            horizontal.CopyTo(Tiles, 1);

            diagonalRight.CopyTo(Tiles, horizontal.Length + 1);
        }
        else if (vertical.Length >= 2 && diagonalLeft.Length >= 2)
        {
            Tiles = new TileData[vertical.Length + diagonalLeft.Length + 1];

            Tiles[0] = origin;

            VerticalMultiplier = Tiles[0].VertMultiplier;

            DiagonalMultiplier = Tiles[0].DiagMultiplier;

            vertical.CopyTo(Tiles, 1);

            diagonalLeft.CopyTo(Tiles, vertical.Length + 1);
        }
        else if (vertical.Length >= 2 && diagonalRight.Length >= 2)
        {
            Tiles = new TileData[vertical.Length + diagonalRight.Length + 1];

            Tiles[0] = origin;

            VerticalMultiplier = Tiles[0].VertMultiplier;

            DiagonalMultiplier = Tiles[0].DiagMultiplier;

            vertical.CopyTo(Tiles, 1);

            diagonalRight.CopyTo(Tiles, vertical.Length + 1);
        }
        else if (horizontal.Length >= 2)
        {
            Tiles = new TileData[horizontal.Length + 1];

            Tiles[0] = origin;

            HorizontalMultiplier = Tiles[0].HorMultiplier;

            horizontal.CopyTo(Tiles, 1);
        }
        else if (vertical.Length >= 2)
        {
            Tiles = new TileData[vertical.Length + 1];

            Tiles[0] = origin;

            VerticalMultiplier = Tiles[0].VertMultiplier;

            vertical.CopyTo(Tiles, 1);
        }
        else if (diagonalLeft.Length >= 2)
        {
            Tiles = new TileData[diagonalLeft.Length + 1];

            Tiles[0] = origin;

            DiagonalMultiplier = Tiles[0].DiagMultiplier;

            diagonalLeft.CopyTo(Tiles, 1);
        }
        else if (diagonalRight.Length >= 2)
        {
            Tiles = new TileData[diagonalRight.Length + 1];

            Tiles[0] = origin;

            DiagonalMultiplier = Tiles[0].DiagMultiplier;

            diagonalRight.CopyTo(Tiles, 1);
        }

        else Tiles = null;

        Score = Tiles?.Length ?? -1;
    }
}