using UnityEngine;

public class HowToManageGame : MonoBehaviour
{
    public BoardGameManager GameManager;
    public bool NeedChangeLogicTurn;
    private bool myLogicTurn = false;

    void Start()
    {

    }

    void Update()
    {
        changeLogicTurn();
        if (myLogicTurn)
        {
            // do my Logic and functions
            // ...
            // ...
        }
        else
        {
            //do nothing
        }
    }
    private void changeLogicTurn()
    {
        if (!NeedChangeLogicTurn) return;
        if (myLogicTurn)
        {
            GameManager.RequestIntrrupt = true;
            if (!GameManager.hasInterrupt())
            {
                return;
                //or wait.
                //you can use timer to handle this.
            }
            GameManager.RequestIntrrupt = false;
        }
        else
        {
            GameManager.resetInterrupt();
        }
        myLogicTurn = !myLogicTurn;
        NeedChangeLogicTurn = false;

    }
}
