using System.Collections.Generic;
using UnityEngine;

namespace MyDice
{
    public class DiceManager : MonoBehaviour
    {
        #region variables
        #region public
        public int[] dicesValues { get; protected set; }
        public List<string> collisionObjectTags = new List<string>() { "Dice Ground Wall" };
        public float rollingThreshold = 10f;
        //dices
        public bool autoDetection_dices = true;
        public Dice[] dice;
        #endregion
        #region HideInInspector
        #region editor
#if UNITY_EDITOR
        [HideInInspector] public bool infoEnable = true;
        [HideInInspector] public bool parametersEnable = true;
#endif
        #endregion
        #endregion
        #region protected
        [HideInInspector] public DiceState dicesState = DiceState.Null;
        #endregion
        #region private
        [HideInInspector] public bool flag = false;
        [HideInInspector] public bool isFresh_dicesValues;
        [HideInInspector] public float rollingThresholdCounter = 0f;
        #endregion
        #endregion
        #region getter setter
        public DiceState getDiceState()
        {
            return dicesState;
        }
        public void setFlag(bool val)
        {
            flag = val;
        }
        public bool getFlag() { return flag; }
        private bool isFlaged()
        {
            if (flag)
            {
                flag = false;
                return true;
            }
            return false;
        }
        #endregion
        #region Functions
        private void Start()
        {
            init();
        }
        private void Update()
        {
            checkDicesState();
            if (dicesState == DiceState.Rolling)
            {
                manageRollingThreshold();
            }
            else if (dicesState == DiceState.Ready)
            {
                if (isFlaged())
                    RoolDices();
            }
            else if (dicesState == DiceState.Finish)
            {
                if (!isFresh_dicesValues)
                {
                    getDicesValues();
                }
            }
        }
        #endregion
        #region functions
        #region init
        public void init()
        {
            findDices();
            initDices();
        }
        #endregion
        #region dices
        private void findDices()
        {
            if (autoDetection_dices)
            {
                dice = FindObjectsOfType<Dice>();
            }
            if (dice == null || dice.Length < 1)
            {
                Debug.LogError("No dice found.");
                Extensions.Quit();
            }
        }
        private void initDices()
        {
            string diceGroundTagName = this.gameObject.tag;
            if (collisionObjectTags == null) collisionObjectTags = new List<string>();
            {
                List<Dice> dices = new List<Dice>();
                for (int i = 0; i < dice.Length; i++)
                {
                    if (dice[i] == null) continue;
                    dices.Add(dice[i]);
                }
                dice = dices.ToArray();
            }
            for (int i = 0; i < dice.Length; i++)
            {
                string tagName = dice[i].tag;
                if (string.IsNullOrEmpty(tagName) || collisionObjectTags.Exists(e => e.Equals(tagName))) continue;
                collisionObjectTags.Add(tagName);
            }
            for (int i = 0; i < dice.Length; i++)
            {
                dice[i].diceGroundTagName = diceGroundTagName;
                dice[i].collisionObjectTags = this.collisionObjectTags;
            }
        }
        private void checkDicesState()
        {
            if (dice.Length < 1) return;
            for (int i = 1; i < dice.Length; i++)
            {
                if (dice[i].diceState != dice[0].diceState)
                {
                    dicesState = DiceState.Null;
                    return;
                }
            }
            if (dicesState != dice[0].diceState) isFresh_dicesValues = false;
            dicesState = dice[0].diceState;
        }
        public void RoolDices()
        {
            for (int i = 0; i < dice.Length; i++)
                dice[i].RollDice();
        }
        public void ResetDices()
        {
            for (int i = 0; i < dice.Length; i++)
                dice[i].ResetDice();
        }
        public int[] getDicesValues()
        {
            if (dicesState != DiceState.Finish) return null;
            isFresh_dicesValues = true;
            int[] result = new int[dice.Length];
            for (int i = 0; i < dice.Length; i++)
                result[i] = dice[i].Value;
            return dicesValues = result;
        }
        #region rolling over flow
        private void manageRollingThreshold()
        {
            rollingThresholdCounter += Time.fixedDeltaTime;
            if (rollingThresholdCounter > rollingThreshold)
            {
                rollingThresholdCounter = 0f;
                Debug.Log("Warning: Rolling is over flow.");
                onRollingOverFlow();
            }
        }
        public virtual void onRollingOverFlow()
        {

        }
        #endregion
        #endregion
        #endregion
    }
}