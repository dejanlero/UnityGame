using UnityEngine;

namespace MyDice
{
    public class DiceSide : MonoBehaviour
    {
        #region variables
        #region public
        public int value;
        public bool OnGround { get { return onGround; } }
        #endregion
        #region private
        protected string diceGroundTagName = "Dice Ground";
        private bool onGround;
        #endregion
        #endregion
        #region Functions
        private void OnTriggerStay(Collider other)
        {
            if (other == this) return;
            if (string.Equals(other.tag, diceGroundTagName))
            {
                onGround = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other == this) return;
            if (string.Equals(other.tag, diceGroundTagName))
            {
                onGround = false;
            }
        }
        #endregion
        #region functions
        public void setDiceGroundTagName(string value)
        {
            diceGroundTagName = value;
        }
        #endregion
    }
}