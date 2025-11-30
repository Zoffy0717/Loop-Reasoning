using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Testimony Combine System")]
public class TestimonyCombineSystem : ScriptableObject
{
    public Recipe[] testimonyRecipes;

    // result of combining 3 cards
    public CardSY TryCombineThree(CardSY c1, CardSY c2, CardSY c3)
    {
        if (c1 == null || c2 == null || c3 == null)
            return null;

        // Put all card IDs in sorted array
        var inputs = new[] { c1.cardID, c2.cardID, c3.cardID }
                     .OrderBy(x => x)
                     .ToArray();

        foreach (var recipe in testimonyRecipes)
        {
            // skip 2-card recipes
            if (recipe.inputCardIDs.Length != 3)
                continue;

            var required = recipe.inputCardIDs
                                .OrderBy(x => x)
                                .ToArray();

            if (inputs.SequenceEqual(required))
            {
                Debug.Log("3-card testimony success!");
                return recipe.resultCard;
            }
        }

        Debug.Log("No matching 3-card testimony recipe.");
        return null;
    }

    // used by highlighting
    public bool CanCombineThree(CardSY c1, CardSY c2, CardSY c3)
    {
        if (c1 == null || c2 == null || c3 == null)
            return false;

        var inputs = new[] { c1.cardID, c2.cardID, c3.cardID }
                     .OrderBy(x => x)
                     .ToArray();

        foreach (var recipe in testimonyRecipes)
        {
            if (recipe.inputCardIDs.Length != 3)
                continue;

            var required = recipe.inputCardIDs
                                .OrderBy(x => x)
                                .ToArray();

            if (inputs.SequenceEqual(required))
                return true;
        }

        return false;
    }

    public List<CardSY> GetCombinableWithThree(CardSY slot1, CardSY slot2, List<CardSY> allCards)
    {
        List<CardSY> result = new List<CardSY>();

        foreach (var c in allCards)
        {
            // Skip duplicates (don’t combine a card with itself unless allowed)
            if (c == slot1 || c == slot2)
                continue;

            // If only slot1 is filled → check recipes using slot1 + c + ANY third card
            if (slot1 != null && slot2 == null)
            {
                foreach (var recipe in testimonyRecipes)
                {
                    if (recipe.inputCardIDs.Length != 3)
                        continue;

                    // Check if recipe contains both slot1 + c
                    if (recipe.inputCardIDs.Contains(slot1.cardID) &&
                        recipe.inputCardIDs.Contains(c.cardID))
                    {
                        result.Add(c);
                        break;
                    }
                }
                continue;
            }

            // If slot1 and slot2 both filled → check if c completes a full recipe
            if (slot1 != null && slot2 != null)
            {
                if (CanCombineThree(slot1, slot2, c))
                {
                    result.Add(c);
                }
            }
        }

        return result;
    }
}
