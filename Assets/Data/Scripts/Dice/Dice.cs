using System.Collections.Generic;
using UnityEngine;

namespace MyDice
{
    public enum DiceState
    {
        Null, Ready, Rolling, Finish
    }

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Dice : MonoBehaviour
    {
        #region variables
        #region public
        public bool IsGrounded { get { return isGrounded; } }
        public int Value { get { return value; } }

        #region Torque
        [Header("Torque")]
        public float minTorque = 500;
        public float maxTorque = 1000;
        #endregion
        #region Force
        [Header("Force")]
        public float force = 500;
        public Vector3 forceDirection = Vector3.down;
        public Quaternion rotation = Quaternion.identity;
        #endregion
        public string diceGroundTagName = "Dice Ground";
        [SerializeField]
        public List<string> collisionObjectTags = new List<string>() { "Dice", "Dice Ground Wall" };
        public DiceState diceState
        {
            get { return state; }
            protected set
            {
                state = value;
                if (value == DiceState.Ready)
                    render.enabled = false;
                else
                    render.enabled = true;
            }
        }
        #endregion
        #region protected
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public Collider coll;
        [HideInInspector] public DiceSide[] diceSides;
        [HideInInspector] public MeshRenderer render;
        [HideInInspector] public DiceState state;
        [HideInInspector] public bool isGrounded;
        [HideInInspector] public bool thrown;
        [HideInInspector] public Vector3 initPosition;
        [HideInInspector] public int value;
        #endregion
        #endregion
        #region Functions
        private void OnValidate()
        {
            rb = GetComponent<Rigidbody>();
            coll = GetComponent<Collider>();
            render = gameObject.GetComponentInChildren<MeshRenderer>();
        }
        public void Start()
        {
            init();
        }
        private void Update()
        {
            if (diceState == DiceState.Ready)
            {
                // if (Input.GetKeyDown(KeyCode.Space)) RollDice();
            }
            else if (diceState == DiceState.Finish)
            {
                // if (Input.GetKeyDown(KeyCode.Space)) ResetDice();
            }
            else
            {
                if (rb.IsSleeping())
                {
                    if (!isGrounded && thrown)
                    {
                        isGrounded = true;
                        rb.useGravity = false;
                        diceState = findValue() ? DiceState.Finish : DiceState.Null;
                    }
                    else if (isGrounded && value == 0)
                    {
                        roolAgain();
                    }
                }
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            string otherTag = collision.gameObject.tag;
            if (string.IsNullOrEmpty(otherTag)) return;
            if (otherTag.Equals(diceGroundTagName)) return;
            if (collisionObjectTags == null
                || collisionObjectTags.Exists(e => e == otherTag)) return;
            Physics.IgnoreCollision(collision.collider, coll);
        }

        #endregion
        #region functions
        public bool init()
        {
            initPosition = transform.position;
            rb.useGravity = false;
            rb.isKinematic = false;
            diceSides = GetComponentsInChildren<DiceSide>();
            if (diceSides == null || diceSides.Length < 1)
            {
                Debug.Log("No side found.");
                return false;
            }

            for (int i = 0; i < diceSides.Length; i++)
            {
                diceSides[i].setDiceGroundTagName(diceGroundTagName);
            }
            if (maxTorque < minTorque)
            {
                Extensions.Swap<float>(ref minTorque, ref maxTorque);
            }
            diceState = DiceState.Ready;
            return true;
        }
        public void RollDice()
        {
            if (!thrown && !isGrounded)
            {
                forceRollDice();
            }
            else if (thrown && isGrounded)
            {
                ResetDice();
            }
        }
        public void ResetDice()
        {
            value = 0;
            transform.position = initPosition;
            transform.rotation = Random.rotation;
            rb.useGravity = thrown = isGrounded = false;
            diceState = DiceState.Ready;
        }
        private void roolAgain()
        {
            ResetDice();
            forceRollDice();
        }
        private void forceRollDice()
        {
            diceState = DiceState.Rolling;
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque));
            rb.AddForce(forceDirection * force, ForceMode.Force);
        }
        private bool findValue()
        {
            value = 0;
            if (diceSides == null) return false;
            for (int i = 0; i < diceSides.Length; i++)
            {
                if (diceSides[i].OnGround)
                {
                    value = diceSides[i].value;
                    return true;
                }
            }
            return true;
        }

        #endregion
    }
}