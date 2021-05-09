using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Store Item", menuName = "Fitcoin Demo/Store Item")]
public class StoreItem : ScriptableObject
{
    public Sprite icon = null;
    public string itemName = "Item";
    public string description = "This is an item.";
    public int cost = 0;

    public string id = "item";
}
