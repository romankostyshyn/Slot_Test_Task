using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameField : MonoBehaviour
{
    public static GameField Instance { get; private set; }


    public Row[] rows;
    
    private float _tweenDuration = 0.5f;
    
    public Tile[,] Tiles { get; private set; }

    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);
    
    public Button shuffleButton;
    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateField();
        shuffleButton.onClick.AddListener(CreateField);
    }
    
    private async void Match()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Tiles[x, y];

                var connectedTiles = tile.GetConnectedTiles();

                if (connectedTiles.Skip(1).Count() < 3) continue;

                var deflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    deflateSequence.Join(connectedTile.icon.transform.DOScale(6f, _tweenDuration).Play());
                }

                await deflateSequence.Play().AsyncWaitForCompletion();
            }
        }
    }

    private void CreateField()
    {
        Tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var tile = rows[y].tiles[x];

                tile.x = x;
                tile.y = y;
                
                tile.Item = ItemDB.Items[Random.Range(0, ItemDB.Items.Length)];
                
                Tiles[x, y] = tile;
            }
        }
        
        Match();
    }
}
