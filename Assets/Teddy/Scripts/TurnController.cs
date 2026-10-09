using System;
using TMPro;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public int TurnCount { get; private set; } = 1;

    [Header("UI")]
    public TMP_Text turnCounter;

    [Header("Cameras")]
    public Camera player1Camera;
    public Camera player2Camera;

    private GameObject player1;
    private GameObject player2;

    private Collider player1MouthCol;
    private Collider player2MouthCol;

    public GameObject ControllingPlayer;

    public event Action<int> OnTurnChanged;
    public event Action<GameObject> OnPlayerChanged;

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        if (player1Camera == null)
            player1Camera = player1.GetComponentInChildren<Camera>();

        if (player2Camera == null)
            player2Camera = player2.GetComponentInChildren<Camera>();

        player1MouthCol = GetTrigger(player1);
        player2MouthCol = GetTrigger(player2);

        ControllingPlayer = player1;

        player1MouthCol.enabled = false;
        player2MouthCol.enabled = true;

        player1.GetComponent<FoodTosser>().enabled = true;
        player2.GetComponent<FoodTosser>().enabled = false;

        player1Camera.enabled = true;
        player2Camera.enabled = false;

        turnCounter.text = $"Turn: {TurnCount}";

        OnPlayerChanged?.Invoke(ControllingPlayer);
        OnTurnChanged?.Invoke(TurnCount);

        Debug.Log($"Starting Player: {ControllingPlayer.name}");
    }

    public void PassTurn()
    {
        foreach (Food food in FindObjectsByType<Food>())
            Destroy(food.gameObject);

        TurnCount++;
        turnCounter.text = $"Turn: {TurnCount}";

        OnTurnChanged?.Invoke(TurnCount);

        if (ControllingPlayer == player1)
        {
            Debug.Log("Player1 -> Player2");

            player1MouthCol.enabled = true;
            player2MouthCol.enabled = false;

            player1.GetComponent<FoodTosser>().enabled = false;
            player2.GetComponent<FoodTosser>().enabled = true;

            player1Camera.enabled = false;
            player2Camera.enabled = true;

            ControllingPlayer = player2;
        }
        else
        {
            Debug.Log("Player2 -> Player1");

            player2MouthCol.enabled = true;
            player1MouthCol.enabled = false;

            player2.GetComponent<FoodTosser>().enabled = false;
            player1.GetComponent<FoodTosser>().enabled = true;

            player2Camera.enabled = false;
            player1Camera.enabled = true;

            ControllingPlayer = player1;
        }

        OnPlayerChanged?.Invoke(ControllingPlayer);

        Debug.Log($"Turn: {TurnCount}");
        Debug.Log($"Controller: {ControllingPlayer.name}");
    }

    private Collider GetTrigger(GameObject player)
    {
        foreach (Collider col in player.GetComponentsInChildren<Collider>())
        {
            if (col.isTrigger)
                return col;
        }

        return null;
    }
}