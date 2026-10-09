using System;
using System.Collections;
using UnityEngine;

public class FatTracker : MonoBehaviour
{
    [Header("Properties")]
    public int caloriesForDeath;

    [Header("References")]
    public GameObject head;
    public GameObject belly;
    public ParticleSystem mouthPartSys;
    public ParticleSystem bellyPartSys;

    public int CurrentCalories { get; private set; }

    public event Action<int> OnCaloriesChanged;
    public event Action OnExploded;

    private Vector3 headStartScale;
    private Vector3 bellyStartScale;

    private void Start()
    {
        headStartScale = head.transform.localScale;
        bellyStartScale = belly.transform.localScale;
    }

    public void AddCalories(int cals)
    {
        CurrentCalories += cals;

        OnCaloriesChanged?.Invoke(CurrentCalories);

        CheckScale();

        if (CurrentCalories >= caloriesForDeath)
            StartCoroutine(Explode());
    }

    public void CheckScale()
    {
        float progress = Mathf.Clamp01(CurrentCalories / caloriesForDeath);

        float scaleMultiplier = Mathf.Lerp(1f, 2f, progress);

        head.transform.localScale = headStartScale * scaleMultiplier;
        belly.transform.localScale = bellyStartScale * scaleMultiplier;
    }

    private IEnumerator Explode()
    {
        OnExploded?.Invoke();

        yield return new WaitForSeconds(2);

        mouthPartSys.GetComponent<AudioSource>().Play();
        mouthPartSys.Play();

        yield return new WaitForSeconds(2);

        head.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(0.75f);

        bellyPartSys.GetComponent<AudioSource>().Play();
        bellyPartSys.Play();

        Destroy(belly);

        head.AddComponent<Rigidbody>();

        head.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 750f, -100f));
    }
}