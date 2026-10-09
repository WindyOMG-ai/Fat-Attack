using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoodTosser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject foodPrefab;

    [Header("Input")]
    [SerializeField] private InputAction tossAction;

    [Header("Throw Settings")]
    [SerializeField] private float maxPower = 30f;
    [SerializeField] private float chargeRate = 15f;
    [SerializeField] private float upwardArc = 0.35f;
    [SerializeField] private float turnDelay = 5f;

    private TurnController turnController;
    private Camera playerCamera;
    private Mouth mouth;

    private float currentPower;
    private bool isThrowing;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        mouth = GetComponentInChildren<Mouth>();
    }

    private void OnEnable()
    {
        tossAction.Enable();
    }

    private void OnDisable()
    {
        tossAction.Disable();
    }

    private void Start()
    {
        turnController = FindAnyObjectByType<TurnController>();

        Debug.Log($"{name} initialized.");
    }

    private void Update()
    {
        if (turnController == null)
            return;

        if (isThrowing)
            return;

        if (turnController.ControllingPlayer != gameObject)
            return;

        HandleAimingAndThrowing();
    }

    private void HandleAimingAndThrowing()
    {
        if (tossAction.IsPressed())
        {
            currentPower += chargeRate * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, 0f, maxPower);

            Debug.Log($"Power: {currentPower:F1}");
        }

        if (tossAction.WasReleasedThisFrame())
        {
            Vector3 throwDirection = GetMouseDirection();

            StartCoroutine(Toss(throwDirection, currentPower));

            currentPower = 0f;
        }
    }

    private Vector3 GetMouseDirection()
    {
        Plane aimPlane = new Plane(Vector3.up, transform.position);

        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (aimPlane.Raycast(ray, out float enter))
        {
            Vector3 mousePoint = ray.GetPoint(enter);

            Vector3 direction = (mousePoint - transform.position).normalized;

            direction += Vector3.up * upwardArc;

            return direction.normalized;
        }

        return transform.forward;
    }

    private IEnumerator Toss(Vector3 direction, float power)
    {
        isThrowing = true;

        Debug.Log("==== TOSS ====");
        Debug.Log($"Power: {power}");
        Debug.Log($"Direction: {direction}");

        GameObject food = Instantiate(foodPrefab, transform.position, Quaternion.identity);

        Rigidbody rb = food.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Food prefab has no Rigidbody!");

            isThrowing = false;
            yield break;
        }

        rb.AddForce(direction * power, ForceMode.Impulse);

        yield return new WaitForSeconds(turnDelay);

        turnController.PassTurn();

        isThrowing = false;
    }
}