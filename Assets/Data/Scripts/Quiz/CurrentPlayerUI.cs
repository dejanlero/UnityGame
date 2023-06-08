using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentPlayerUI : MonoBehaviour
{
    public TMP_Text currentPlayerText;
    private int currentPlayer = 0;

    // This function can be called when the player changes
    public void NextPlayerUI()
    {
        currentPlayer = (currentPlayer % 4) + 1;
        currentPlayerText.text = $"Player {currentPlayer}";

        switch (currentPlayer)
        {
            case 1:
                currentPlayerText.color = Color.red;
                break;
            case 2:
                currentPlayerText.color = Color.yellow;
                break;
            case 3:
                currentPlayerText.color = Color.blue;
                break;
            case 4:
                currentPlayerText.color = Color.green;
                break;
        }
    }
}

