using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameField : MonoBehaviour
{
    //public static GameField Instance { get; private set; }
    [SerializeField] private Item[] itemTypes;

    [SerializeField] public Row[] rows;

    [SerializeField] private float tweenDuration;

    public Tile[,] Tiles { get; private set; }
    
    private bool _isMatching;

    //public int Width => Tiles.GetLength(0);
    //public int Height => Tiles.GetLength(1);

    public Button shuffleButton;
    
    public event Action<Item, int> OnMatch;
    
    private TileData[,] Matrix
    {
        get
        {
            var width = rows.Max(row => row.tiles.Length);
            var height = rows.Length;

            var data = new TileData[width, height];

            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                data[x, y] = GetTile(x, y).Data;

            return data;
        }
    }

    public void Awake()
    {
        //Instance = this;
    }

    private void Start()
    {
        CreateField();
        shuffleButton.onClick.AddListener(CreateField);
    }

    private Tile[] GetTiles(IList<TileData> tileData)
    {
        var length = tileData.Count;

        var tiles = new Tile[length];

        for (var i = 0; i < length; i++) tiles[i] = GetTile(tileData[i].X, tileData[i].Y);

        return tiles;
    }

    private async Task<bool> TryMatchAsync()
    {
        var didMatch = false;

        _isMatching = true;

        var match = HelpForMatch.FindBestMatch(Matrix);
        
        while (match != null)
        {
            didMatch = true;

            var tiles = GetTiles(match.Tiles);

            var deflateSequence = DOTween.Sequence();

            foreach (var tile in tiles)
            {
                var originalScale = tile.icon.transform.localScale;
                deflateSequence
                    .Append(tile.icon.transform.DOScale(
                        new Vector3(originalScale.x + 1f, originalScale.y + 1f, originalScale.z + 1f),
                        tweenDuration)).Append(tile.icon.transform.DOScale(originalScale, tweenDuration));
                //deflateSequence.Join(tile.icon.transform.DOScale(new Vector3(originalScale.x + 0.5f, originalScale.y + 0.5f, originalScale.z + 0.5f), tweenDuration).SetEase(Ease.InBack));
            }

            await deflateSequence.Play()
                .AsyncWaitForCompletion();

            //var inflateSequence = DOTween.Sequence();

            // foreach (var tile in tiles)
            // {
            //     tile.Item = itemTypes[Random.Range(0, itemTypes.Length)];
            //
            //     inflateSequence.Join(tile.icon.transform.DOScale(Vector3.one, tweenDuration).SetEase(Ease.OutBack));
            // }

            //await inflateSequence.Play()
                //.AsyncWaitForCompletion();

            OnMatch?.Invoke(Array.Find(itemTypes, tileType => tileType.id == match.TypeId), match.Tiles.Length);

            match = HelpForMatch.FindBestMatch(Matrix);
			
			for (int i = 0; i < match.Tiles.Length; i++)
			{  
				Debug.Log($"{match.Tiles[i].X}, {match.Tiles[i].Y}");
			}
        }

        _isMatching = false;

        return didMatch;
    }

    private async void CreateField()
    {
        //Tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];

        if (_isMatching) return;
        
        for (var y = 0; y < rows.Length; y++)
        {
            for (var x = 0; x < rows.Max(row => row.tiles.Length); x++)
            {
                var tile = GetTile(x, y);

                tile.x = x;
                tile.y = y;

                tile.Item = itemTypes[Random.Range(0, itemTypes.Length)];

                //Tiles[x, y] = tile;
            }
        }
        
        await TryMatchAsync();
    }

    private Tile GetTile(int x, int y) => rows[y].tiles[x];
}
