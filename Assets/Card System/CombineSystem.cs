using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CombineSystem : ScriptableObject
{
    public Recipe[] recipes;
    // Start is called before the first frame update
    public CardSY TryCombine(CardSY cardA, CardSY cardB)
    {
        if (cardA == null || cardB == null) return null;

        var inputs = new[] { cardA.cardID, cardB.cardID }.OrderBy(x => x).ToArray();
        foreach (var recipe in recipes)
        {
            var required = recipe.inputCardIDs.OrderBy(x => x).ToArray();

            if (inputs.SequenceEqual(required))
            {
                return recipe.resultCard;
            }
        }
        return null;
    }
}
