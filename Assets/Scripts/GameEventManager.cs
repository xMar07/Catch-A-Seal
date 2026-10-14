using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static event Action<FallingObject> OnItemCaught;
    public static event Action<FallingObject> OnGoodItemMissed; // Added this

    public static void TriggerItemCaught(FallingObject caughtItem)
    {
        OnItemCaught?.Invoke(caughtItem);
    }

    public static void TriggerGoodItemMissed(FallingObject missedItem)
    {
        OnGoodItemMissed?.Invoke(missedItem);
    }
}