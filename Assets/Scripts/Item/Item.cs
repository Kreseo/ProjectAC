using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Helmet,
        Chest,
        Pants,
        Boots
    }
    public new string name;
    public int ID;
    public Sprite icon;
    public int price;
    public ItemType type;
}
