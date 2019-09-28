using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon = 0,
    Foot,
    Hand,
    Chest,
    Head
}

[CreateAssetMenu(fileName = "New Item", menuName = "NonStopKnight/New Item")]
public class ItemObject : ScriptableObject
{
    public string ItemName;

    [TextArea(3, 5)]
    public string Description;

    public ItemType ItemType;

    public Datas Datas;

    public bool IsEquiped = false; 
}
