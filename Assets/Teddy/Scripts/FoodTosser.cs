using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoodTosser : MonoBehaviour
{
    public GameObject foodToToss;
    public InputAction tossAction;

    bool tossed = false;

    private void OnEnable()
    {
        tossAction.Enable();
    }

    private void OnDisable()
    {
        tossAction.Disable();
    }

    void Update()
    {
        if(tossAction.IsPressed() && !tossed)
        {

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                tossed = true;

                GameObject food = Instantiate(foodToToss, transform.position, Quaternion.identity);

                Rigidbody rb = food.GetComponent<Rigidbody>();

                Vector3 direction = (hit.point - transform.position).normalized;

                rb.AddForce(direction * 15f, ForceMode.Impulse);
            }
        }
    }
}