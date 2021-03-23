using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Text playerTurnText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void SetPlayerTurn(TEAM team)
    {
        if (team == TEAM.WHITE)
        {
            playerTurnText.text = "PLAYERS TURN: WHITE";
            playerTurnText.color = Color.white;
        }
        else
        {
            playerTurnText.text = "PLAYERS TURN: BLACK";
            playerTurnText.color = Color.black;
        }
    }
}