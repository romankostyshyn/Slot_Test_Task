using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Slots-Item")]
public class Item : ScriptableObject
{
    public int id;
    
    public int value;

    public float horizontalMultiplier;
    public float verticalMultiplier;
    public float diagonalMultiplier;
    
    public Sprite sprite;
}
