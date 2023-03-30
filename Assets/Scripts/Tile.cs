using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    
    public Image icon;
   
    private Item _item;

    public Item Item
    {
        get => _item;

        set
        {
            if (_item == value) return;

            _item = value;

            icon.sprite = _item.sprite;
        }
    }

    public TileData Data => new TileData(x, y, _item.id);

    // public Tile Left => x > 0 ? GameField.Instance.Tiles[x - 1, y] : null;
    // public Tile Top => y > 0 ? GameField.Instance.Tiles[x, y - 1] : null;
    // public Tile Right => x < GameField.Instance.Width - 1 ? GameField.Instance.Tiles[x + 1, y] : null;
    // public Tile Bottom => y < GameField.Instance.Height - 1 ? GameField.Instance.Tiles[x, y + 1] : null;
    //
    // public Tile[] Neighbours => new[]
    // {
    //     Left,
    //     Top,
    //     Right,
    //     Bottom
    // };
    //
    // public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    // {
    //     var result = new List<Tile> { this };
    //
    //     if (exclude == null)
    //     {
    //         exclude = new List<Tile> { this };
    //     }
    //     else
    //     {
    //         exclude.Add(this);
    //     }
    //
    //     foreach (var neighbour in Neighbours)
    //     {
    //         if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item) continue;
    //
    //         if (neighbour.x == x - 1 || neighbour.x == x + 1)
    //         {
    //             result.Add(neighbour);
    //             result.AddRange(neighbour.GetConnectedTiles(exclude));
    //         }
    //         
    //         //result.AddRange(neighbour.GetConnectedTiles(exclude));
    //     }
    //
    //     return result;
    // }
}
