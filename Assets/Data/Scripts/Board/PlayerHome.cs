using System.Collections.Generic;
using UnityEngine;
using MyDice.Helpers;
using MyDice.Players;

namespace MyDice.Board
{
    [System.Serializable]
    public class PlayerHome
    {
        #region vairbale
        public int index = 0;
        public int startIndex;
        public int targetIndex = -1;
        public GameObject playerPrefab;
        public GameObject ghostPrefab;
        public Vector3 center;
        public List<GameObject> players = new List<GameObject>();
        public Transform parent;
        public PlayerMode playerMode = PlayerMode.Human;
        #region private
        [HideInInspector] public int candidateIndex = -1;
        [HideInInspector] public List<int> candidateIndexes;
        #endregion
        #endregion
        #region constructor
        public PlayerHome(Transform parent)
        {
            this.parent = parent; setCount(1);
        }
        #endregion
        #region functions
        #region logic
        public void Reset()
        {
            candidateIndex = -1;
        }
        #endregion
        public string getName()
        {
            return "Home<" + index + "," + startIndex + ">";
        }
        public void setCenter(Vector3 position)
        {
            center = position;
        }
        public void updatePositions(float radius)
        {
            if (players.Count == 1)
            {
                players[0].transform.position = center;
                players[0].GetComponent<Player>().GoTo_Immediately(center);
                return;
            }
            CircleMaker c = new CircleMaker();
            var vectors = c.CreateCircle(radius, center, players.Count);
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == null)
                {
                    players.Insert(i, createPlayer());
                }
                players[i].transform.position = vectors[i];
                players[i].GetComponent<Player>().GoTo_Immediately(vectors[i]);
            }
        }
        #region count
        public void setCount(int c)
        {
            if (c < 1) return;
            if (c < players.Count)
            {
                for (int i = players.Count - 1; i >= c; i--)
                {
                    destroyPlayer(i);
                }
            }
            else
            {
                for (int i = players.Count; i < c; i++)
                {
                    players.Add(createPlayer());
                }
            }
        }
        public int getCount()
        {
            return players.Count;
        }
        #endregion
        #region players
        #region Candidate
        public void chooseCandidateIndexByAI(int[] diceValue, ref List<GameObject> nodes, ElementNodeCreator.RoutingMode routingMode, bool addUniqueIndex)
        {
            candidateIndexes = new List<int>();
            int bestBenefit = nodes.Count;
            for (int i = 0; i < players.Count; i++)
            {
                Player p = getPlayer(i);
                p.diceValues = diceValue;
                p.CalculatePositionIndex(ref nodes, routingMode, addUniqueIndex);
                if (p.pathManager.UpdateBenefits(startIndex, targetIndex, ref nodes))
                {
                    if (p.currentPositionIndex == targetIndex) continue;
                    Path path = p.pathManager.getBestBenefitPath();
                    if (path == null) continue;
                    if (path.Benefit == bestBenefit)
                    {
                        candidateIndexes.Add(i);
                    }
                    else if (path.Benefit < bestBenefit)
                    {
                        candidateIndexes.Clear();
                        candidateIndexes.Add(i);
                        bestBenefit = path.Benefit;
                    }
                }
            }
            candidateIndex = -1;
            if (candidateIndexes.Count > 0)
            {
                if (candidateIndexes.Count > 1)
                {
                    candidateIndex = candidateIndexes[Random.Range(0, candidateIndexes.Count)];
                }
                else
                {
                    candidateIndex = candidateIndexes[0];
                }
            }
        }
        public void setCandidateIndex(int index)
        {
            candidateIndex = index;
        }
        public int getCandidateIndex()
        {
            return candidateIndex;
        }
        public Player getCandidatePlayer()
        {
            if (players != null && players.Count == 1) return getPlayer(0);
            return getPlayer(candidateIndex);
        }
        #endregion
        public List<Player> getPlayers()
        {
            List<Player> result = new List<Player>();
            if (players != null)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    Player p;
                    if (players[i] == null || (p = players[i].GetComponent<Player>()) == null) continue;
                    result.Add(p);
                }
            }
            return result;
        }
        public Player getPlayer(int index)
        {
            if (index < 0 || index >= players.Count) return null;
            if (players[index] == null) players[index] = createPlayer();
            return players[index].GetComponent<Player>();
        }
        public void initPlayers()
        {
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i].GetComponent<Player>();
                if (player == null)
                {
                    player = players[i].AddComponent<Player>();
                }
                player.currentPositionIndex = startIndex;
                if (player.gostPrefab == null)
                    player.gostPrefab = ghostPrefab;
            }
        }
        public void updatePrefab(GameObject prefab)
        {
            if (playerPrefab == prefab) return;
            playerPrefab = prefab;
            int c = players.Count;
            while (players.Count > 0)
            {
                destroyPlayer(0);
            }
            for (int i = 0; i < c; i++)
            {
                players.Add(createPlayer());
            }
        }
        private void destroyPlayer(int index)
        {
            Object.DestroyImmediate(players[index]);
            players.RemoveAt(index);
        }
        private GameObject createPlayer()
        {
            GameObject newPlayer;
            if (playerPrefab == null) newPlayer = new GameObject();
            else
            {
                newPlayer = Object.Instantiate(playerPrefab);
            }
            Player function = newPlayer.AddComponent<Player>();
            if (parent != null)
            {
                newPlayer.transform.SetParent(parent);
            }
            return newPlayer;
        }
        #endregion
        #region save / load
        #region save
        public void Save(string key)
        {
            if (players != null)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    var node = getPlayer(i);
                    if (node == null) return;
                    node.Save(getKey(key, "player" + i.ToString()));
                }
            }
            PlayerPrefs.Save();
        }
        #endregion
        #region load
        public void Load(string key)
        {
            if (players != null)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    var node = getPlayer(i);
                    if (node == null) return;
                    node.Load(getKey(key, "player" + i.ToString()));
                }
            }
        }
        #endregion
        private string getKey(string key, string variableName)
        {
            return key + "_Player_" + variableName;
        }
        #endregion
        public void onDestroy()
        {
            while (players.Count > 0)
            {
                destroyPlayer(0);
            }
        }
        #endregion
        ~PlayerHome()
        {
            onDestroy();
        }
    }
}