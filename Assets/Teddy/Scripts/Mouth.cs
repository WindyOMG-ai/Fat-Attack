using System;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    public FatTracker fatTracker;

    public static event Action<Food, Mouth> OnFoodEaten;

    private void OnTriggerEnter(Collider other)
    {
        Food food = other.GetComponent<Food>();

        if (food == null)
            return;

        OnFoodEaten?.Invoke(food, this);

        fatTracker.AddCalories(food.calories);

        Destroy(other.gameObject);
    }
}