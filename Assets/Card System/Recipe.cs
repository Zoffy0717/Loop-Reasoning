using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Recipe")]
public class Recipe : ScriptableObject
{
    public string[] inputCardIDs;
    public CardSY resultCard;

}
    