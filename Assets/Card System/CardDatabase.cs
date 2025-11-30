using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card Database")]
public class CardDatabase : ScriptableObject
{
    public List<CardSY> allCards;   // drag ALL card assets here
}