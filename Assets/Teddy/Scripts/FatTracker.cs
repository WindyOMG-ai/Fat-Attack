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

    // Private properties
    private int currentCalories;
    private Vector3 headStartScale;
    private Vector3 bellyStartScale;

    private void Start()
    {
        headStartScale = head.transform.localScale;
        bellyStartScale = belly.transform.localScale;
    }

    public void AddCalories(int cals)
    {
        currentCalories += cals;

        CheckScale();

        if (currentCalories >= caloriesForDeath)
            StartCoroutine(Explode());
    }

    public void CheckScale()
    {
        float progress = Mathf.Clamp01((float)currentCalories / caloriesForDeath);

        float scaleMultiplier = Mathf.Lerp(1f, 2f, progress);

        head.transform.localScale = headStartScale * scaleMultiplier;
        belly.transform.localScale = bellyStartScale * scaleMultiplier;
    }

    private IEnumerator Explode()
    {
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