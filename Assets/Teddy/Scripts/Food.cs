using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int calories;

    public static event Action<Food> OnSpawned;
    public static event Action<Food> OnDestroyed;

    private void OnEnable()
    {
        OnSpawned?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}