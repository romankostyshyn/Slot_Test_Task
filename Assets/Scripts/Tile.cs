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

    public TileData Data => new TileData(x, y, _item.id, _item.horizontalMultiplier, _item.verticalMultiplier, _item.diagonalMultiplier);
}
