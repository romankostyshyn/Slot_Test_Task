using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameField : MonoBehaviour
{
    public static GameField Instance;

    [SerializeField] private TMP_Text stackText;
    [SerializeField] private TMP_Text currentBetText;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button maxBetButton;
    [SerializeField] private Item[] itemTypes;

    [SerializeField] private float tweenDuration;
    [SerializeField] private float score;
    [SerializeField] private float currentBet;
    [SerializeField] private float maxBet;
    [SerializeField] private float minStepBet;
    
    [SerializeField] public Row[] rows;

    public Button shuffleButton;

    public Tile[,] Tiles { get; private set; }

    private bool _isMatching;
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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        stackText.SetText($"{score}");
        currentBetText.SetText($"{currentBet}");
        plusButton.onClick.AddListener(IncreaseBet);
        minusButton.onClick.AddListener(ReduceBet);
        maxBetButton.onClick.AddListener(MaxBet);
        //CreateField();
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

        if (match == null)
        {
            score -= currentBet;
            stackText.SetText($"{score}");
        }

        var originalScale = new Vector3();

        while (match != null)
        {
            didMatch = true;

            var tiles = GetTiles(match.Tiles);

            var deflateSequence = DOTween.Sequence();

            foreach (var tile in tiles)
            {
                originalScale = tile.icon.transform.localScale;

                deflateSequence.Join(tile.icon.transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InBack));
            }

            await deflateSequence.Play()
                .AsyncWaitForCompletion();

            Debug.Log($"HM: {match.HorizontalMultiplier}, VM: {match.VerticalMultiplier}, DM: {match.DiagonalMultiplier} ");
            
            score -= match.HorizontalMultiplier + match.VerticalMultiplier + match.DiagonalMultiplier;
            stackText.SetText($"{score}");

            if (score <= minStepBet)
            {
                stackText.SetText($"{score}");
                currentBet = minStepBet;
                currentBetText.SetText($"{currentBet}");
                GameFieldContainer.Instance.WheelState(true);
            }

            var inflateSequence = DOTween.Sequence();

            foreach (var tile in tiles)
            {
                tile.Item = itemTypes[Random.Range(0, itemTypes.Length)];
                inflateSequence.Join(tile.icon.transform.DOScale(originalScale, tweenDuration).SetEase(Ease.OutBack));
            }

            await inflateSequence.Play()
                .AsyncWaitForCompletion();

            OnMatch?.Invoke(Array.Find(itemTypes, tileType => tileType.id == match.TypeId), match.Tiles.Length);

            match = HelpForMatch.FindBestMatch(Matrix);
        }

        _isMatching = false;

        return didMatch;
    }

    private async void CreateField()
    {
        if (score < currentBet)
        {
            currentBet = minStepBet;
            currentBetText.SetText($"{currentBet}");
            GameFieldContainer.Instance.WheelState(true);
            return;
        }

        if (_isMatching) return;

        for (var y = 0; y < rows.Length; y++)
        {
            for (var x = 0; x < rows.Max(row => row.tiles.Length); x++)
            {
                var tile = GetTile(x, y);

                tile.x = x;
                tile.y = y;

                tile.Item = itemTypes[Random.Range(0, itemTypes.Length)];
            }
        }

        await TryMatchAsync();
    }

    private async void CreateTestField()
    {
        if (_isMatching) return;

        for (var y = 0; y < rows.Length; y++)
        {
            for (var x = 0; x < rows.Max(row => row.tiles.Length); x++)
            {
                var tile = GetTile(x, y);

                tile.x = x;
                tile.y = y;

                if (tile.x == 0 && tile.y == 0 || tile.x == 1 && tile.y == 0 || tile.x == 2 && tile.y == 0 ||
                    tile.x == 1 && tile.y == 1 || tile.x == 2 && tile.y == 2)
                {
                    tile.Item = itemTypes[0];
                }

                // if (tile.x == 0 && tile.y == 0 || tile.x == 1 && tile.y == 0 || tile.x == 2 && tile.y == 0 ||
                //     tile.x == 0 && tile.y == 1 || tile.x == 1 && tile.y == 1 || tile.x == 2 && tile.y == 1 ||
                //     tile.x == 0 && tile.y == 2 || tile.x == 1 && tile.y == 2 || tile.x == 2 && tile.y == 2)
                // {
                //     tile.Item = itemTypes[0];
                // }
            }
        }

        await TryMatchAsync();
    }

    public void UpdateScore(int reward)
    {
        score += reward;
        stackText.SetText($"{score}");
    }

    private void ReduceBet()
    {
        if (currentBet <= minStepBet) return;

        if (currentBet - minStepBet < minStepBet)
        {
            currentBet = minStepBet;
            currentBetText.SetText($"{currentBet}");
            return;
        }

        currentBet -= minStepBet;
        currentBetText.SetText($"{currentBet}");
    }

    private void IncreaseBet()
    {
        if (currentBet >= score || currentBet >= maxBet) return;

        if (currentBet + minStepBet > score) return;

        currentBet += minStepBet;
        currentBetText.SetText($"{currentBet}");
    }

    private void MaxBet()
    {
        if (score < maxBet && score < minStepBet) return;

        if (score < maxBet && score > minStepBet)
        {
            currentBet = score;
            currentBetText.SetText($"{currentBet}");
            return;
        }

        currentBet = maxBet;
        currentBetText.SetText($"{currentBet}");
    }

    private Tile GetTile(int x, int y) => rows[y].tiles[x];
}