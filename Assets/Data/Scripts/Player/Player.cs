using System.Collections.Generic;
using UnityEngine;
using MyDice.Board;
using UnityEngine.Events;
using System.Runtime.Serialization.Json;
using System.Text;
using System;

namespace MyDice.Players
{
    public class Player : MonoBehaviour
    {
        #region variable
        #region public
        public const int MaxPlayerPositionIndex = 2;
        public float movementSpeed = 1;
        public float targetDistanceHit = .05f;
        [HideInInspector] public bool isActive = true;
        public GameObject gostPrefab;
        public PlayerMovementType playerMovementType = PlayerMovementType.Direct;
        #region Events
        public UnityEvent onIdleModeEnter;
        public UnityEvent onMovingModeEnter;
        #endregion
        //public int positionIndexSize = 2;
        #region PlayerPositionIndex
        [HideInInspector] public int[] hitIndex;
        public int currentPositionIndex { get { return positionIndex.GetIndex(0); } set { positionIndex.AddIndex(value); } }
        #endregion
        [HideInInspector] public int TouchCount { get; private set; }
        [HideInInspector] public int playerHomeIndex;
        [HideInInspector] public float deltaTime;
        [HideInInspector] public int[] diceValues;
        [HideInInspector] public PathManager pathManager;
        public PlayerState playerState
        {
            get { return _playerState; }
            set
            {
                switch (value)
                {
                    case PlayerState.Idle:
                        if (_playerState != PlayerState.Idle)
                        {
                            if (onIdleModeEnter != null) onIdleModeEnter.Invoke();
                            if (onMovingModeEnter != null) onIdleModeEnter.RemoveAllListeners();
                        }
                        break;
                    case PlayerState.Moving:
                        if (_playerState != PlayerState.Moving)
                        {
                            if (onMovingModeEnter != null) onMovingModeEnter.Invoke();
                            if (onIdleModeEnter != null) onIdleModeEnter.RemoveAllListeners();
                        }
                        break;
                    case PlayerState.ArrangeMoving:
                    case PlayerState.ArrangeComplete:
                        break;
                    default:
                        if (onIdleModeEnter != null) onIdleModeEnter.RemoveAllListeners();
                        if (onMovingModeEnter != null) onIdleModeEnter.RemoveAllListeners();
                        break;
                }
                _playerState = value;
            }
        }
        #endregion
        #region protected
        [HideInInspector] public PlayerPositionIndex positionIndex;// = new PlayerPositionIndex(MaxPlayerPositionIndex);
        [HideInInspector] public PlayerState _playerState = PlayerState.Null;
        #endregion
        #region private
        [HideInInspector] public List<Vector3> targets;
        #endregion
        #endregion
        #region Functions
        private void OnValidate()
        {
            positionIndex = new PlayerPositionIndex(MaxPlayerPositionIndex);
        }
        
        public void HidePlayer()
        {
            isActive = false;
            gameObject.SetActive(false);
            Debug.Log("SAKRIO IGRACA" + currentPositionIndex);
        }

        void Start()
        {
            if (deltaTime == 0)
                deltaTime = Time.fixedDeltaTime;
            //if (positionIndexSize < 1) positionIndexSize = 1;
            //positionIndex = new PlayerPositionIndex(MaxPlayerPositionIndex);
            //positionIndex.AddIndex(startIndex);
            targets = new List<Vector3>();
            playerState = PlayerState.Idle;
        }
        private void FixedUpdate()
        {
            updateMovementPosition();
        }
        #endregion
        #region functions
        #region prefab

        #endregion
        #region values
        public int getTotalValues()
        {
            int sum = 0;
            if (diceValues == null) return sum;
            for (int i = 0; i < diceValues.Length; i++)
                sum += diceValues[i];
            return sum;
        }
        #endregion
        #region movement
        #region GOTO
        [HideInInspector] public delegate void CompletionHandler();

        public void GoTo_CalculatedIndexes(Path p, List<Vector3> nodes, Action completionHandler)
        {
            currentPositionIndex = p.getIndex(p.getIndexSize() - 1);
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < p.getIndexSize(); i++)
            {
                positions.Add(nodes[p.getIndex(i)]);
            }
            GoTo(positions);

            // Call the completion handler
            if (completionHandler != null)
            {
                completionHandler.Invoke();
            }
        }


        public void GoTo(List<Vector3> positions)
        {
            switch (playerMovementType)
            {
                /* case PlayerMovementType.Circle:
                     CircleMaker c = new CircleMaker();
                     for (int i = 0; i < positions.Count - 1; i++)
                     {
                         var list = c.CreateHalfCircle(positions[i], positions[i + 1], 5);
                         if (list == null) continue;
                         for (int j = 0; j < list.Count; j++)
                             targets.Add(list[j]);
                     }
                     break;*/
                default:
                    for (int i = 0; i < positions.Count; i++)
                        targets.Add(positions[i]);
                    break;
            }
            GoTo_Immediately(positions[0]);
            TouchCount_Increase();
            playerState = PlayerState.Moving;
        }
        public void GoTo_Immediately(Vector3 position)
        {
            this.transform.position = position;
        }
        #endregion
        public void TouchCount_Increase()
        {
            this.TouchCount++;
        }
        public bool hasPath()
        {
            return pathManager != null && pathManager.Paths.Count > 0;
        }
        public void CalculatePositionIndex(ref List<GameObject> nodes, ElementNodeCreator.RoutingMode routingMode,bool addUniqueIndex)
        {
            if (nodes == null || diceValues == null && diceValues.Length < 1) return;
            pathManager = new PathManager(currentPositionIndex, diceValues, routingMode);
            pathManager.FindPaths(ref nodes, addUniqueIndex);
        }
        public void CalculatePositionIndex(int size)
        {
            hitIndex = new int[diceValues.Length + 1];
            hitIndex[0] = currentPositionIndex;
            for (int i = 1; i < hitIndex.Length; i++)
            {
                hitIndex[i] = (hitIndex[i - 1] + diceValues[i - 1]) % size;
            }
            positionIndex.AddIndex((currentPositionIndex + getTotalValues()) % (size));
        }
        private void updateMovementPosition()
        {
            if (targets == null || targets.Count < 1) return;
            if (Vector3.Distance(targets[0], this.transform.position) < targetDistanceHit)
            {
                targets.RemoveAt(0);
                if (targets.Count < 1)
                {
                    var r = this.transform.rotation;
                    r.x = r.z = 0f;
                    this.transform.rotation = r;
                    playerState = PlayerState.MovingComplete;
                }
                return;
            }
            Vector3 dir = (targets[0] - this.transform.position);
            if (dir != Vector3.zero)
            {
                this.transform.rotation = Quaternion.LookRotation(dir);
            }
            this.transform.position = Vector3.MoveTowards(this.transform.position, targets[0], (deltaTime * movementSpeed));
        }
        #endregion
        #region save / load
        #region save
        public void Save(string key)
        {
            ///
            var k = getKey(key, "position");
            PlayerPrefs.SetFloat(k + "_x", this.transform.position.x);
            PlayerPrefs.SetFloat(k + "_y", this.transform.position.y);
            PlayerPrefs.SetFloat(k + "_z", this.transform.position.z);
            ///
            k = getKey(key, "rotation");
            PlayerPrefs.SetFloat(k + "_x", this.transform.rotation.x);
            PlayerPrefs.SetFloat(k + "_y", this.transform.rotation.y);
            PlayerPrefs.SetFloat(k + "_z", this.transform.rotation.z);
            PlayerPrefs.SetFloat(k + "_w", this.transform.rotation.w);
            ///
            if (positionIndex != null)
            {
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(PlayerPositionIndex));
                System.IO.MemoryStream msObj = new System.IO.MemoryStream();
                js.WriteObject(msObj, positionIndex);
                msObj.Position = 0;
                System.IO.StreamReader sr = new System.IO.StreamReader(msObj);
                string json = sr.ReadToEnd();
                sr.Close();
                msObj.Close();
                PlayerPrefs.SetString(getKey(key, "positionIndex"), json);
            }
            ///
            PlayerPrefs.Save();
        }
        #endregion
        #region load
        public void Load(string key)
        {
            var k = getKey(key, "position");
            var x = PlayerPrefs.GetFloat(k + "_x");
            var y = PlayerPrefs.GetFloat(k + "_y");
            var z = PlayerPrefs.GetFloat(k + "_z");
            this.transform.position = new Vector3(x, y, z);
            ///
            k = getKey(key, "rotation");
            x = PlayerPrefs.GetFloat(k + "_x");
            y = PlayerPrefs.GetFloat(k + "_y");
            z = PlayerPrefs.GetFloat(k + "_z");
            var w = PlayerPrefs.GetFloat(k + "_w");
            this.transform.rotation = new Quaternion(x, y, z, w);
            ///
            k = getKey(key, "positionIndex");
            if (PlayerPrefs.HasKey(k))
            {
                string json = PlayerPrefs.GetString(k);
                using (var ms = new System.IO.MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(PlayerPositionIndex));
                    positionIndex = (PlayerPositionIndex)deserializer.ReadObject(ms);
                }
            }
        }
        #endregion
        private string getKey(string key, string variableName)
        {
            return key + "_Player_" + variableName;
        }
        #endregion
        #endregion
    }
}