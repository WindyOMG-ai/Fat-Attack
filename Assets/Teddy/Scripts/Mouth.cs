using Unity.ProjectAuditor.Editor.Core;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    public FatTracker fatTracker;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Food food = other.GetComponent<Food>();

        if (food == null)
            return;

        fatTracker.AddCalories(food.calories);
        Destroy(other.gameObject);
    }
}
