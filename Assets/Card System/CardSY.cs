using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { Scene, Item, NPC, Clue, Testimony, Argument }
[CreateAssetMenu(menuName = "Cards/Card")]
public class CardSY : ScriptableObject
{
    public string cardID;
    public string displayName;
    public CardType type;
    public Sprite artwork;
    [TextArea] public string description;
    public string[] keywords;
}

