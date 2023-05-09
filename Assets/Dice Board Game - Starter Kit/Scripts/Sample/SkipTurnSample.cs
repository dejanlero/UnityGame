using MyDice.Players;
using UnityEngine;

namespace MyDice.Samples
{
    public class SkipTurnSample : MonoBehaviour
    {
        #region Parameters
        public BoardGameManager boardGameManager;
        [Min(0)]
        public int SkipPlayerIndex = 0;
        public KeyCode SkipPlayerActionKey = KeyCode.S;
        #endregion
        #region Functions
        private void Start()
        {
            if (boardGameManager == null
                && (boardGameManager = FindObjectOfType<BoardGameManager>()) == null)
            {
                Debug.LogError("no BoardGameManager founded.");
                Extensions.Quit();
            }
            if (boardGameManager.playerHomes == null || boardGameManager.playerHomes.Count < 1)
            {
                Debug.LogError("no Player(s) founded.");
                Extensions.Quit();
            }
            if (SkipPlayerIndex < 0 || boardGameManager.playerHomes.Count <= SkipPlayerIndex)
            {
                Debug.LogError("\"SkipPlayerIndex\" not exist.");
                Extensions.Quit();
            }
        }
        private void Update()
        {
            if (Input.GetKeyUp(SkipPlayerActionKey))
            {
                Debug.Log("Player Index: " + (SkipPlayerIndex + 1) 
                    + "or Player number: "+ SkipPlayerIndex 
                    + " skipping function.");
                SkippingPlayerIndex(SkipPlayerIndex);
            }
        }
        #endregion
        #region functions
        public void SkippingPlayerIndex(int _playerHomeIndex)
        {
            int player1SkipCount = boardGameManager.SkipTurnCount(_playerHomeIndex);
            if (player1SkipCount < 1)
            {
                //skip player Index for 1 round
                boardGameManager.SkipTurn(_playerHomeIndex, 1);
            }
        }
        #endregion
    }
}