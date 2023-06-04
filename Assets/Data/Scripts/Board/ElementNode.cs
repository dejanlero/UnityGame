using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyDice.Board
{
    public class ElementNode : MonoBehaviour
    {
        #region enum
        public enum HostingStatus : int
        {
            Enable,
            EnableAndDontForward,
            Disable
        }
        #endregion
        #region variable
        public int index = -1;
        public GameObject prefab;
        public Vector3 point;
        public UnityEvent onPlayerStayEvent;
        public HostingStatus hostingStatus = HostingStatus.Enable;
        #region HideInInspector
        [HideInInspector] public long ID;
        [HideInInspector] public int redirectIndex = -1;
        [HideInInspector] public ElementNodeType elementNodeType = ElementNodeType.None;
        [HideInInspector] public List<int> connections = new List<int>();
        //[HideInInspector]
        public List<int> incomingConnections = new List<int>();
        #endregion
        #endregion
        #region Functions
        public void OnDestroy()
        {
            destroyPrefab();
        }
        #endregion
        #region functions
        #region ElementNodeType
        public void setElementNodeType(ElementNodeType type)
        {
            if (type != ElementNodeType.RedirectPoint)
            {
                redirectIndex = -1;
            }
            elementNodeType = type;
        }
        public ElementNodeType getElementNodeType()
        {
            return elementNodeType;
        }
        #endregion
        #region HostingStatus
        public void setHostingStatus(HostingStatus status)
        {
            hostingStatus = status;
        }
        #endregion
        #region position
        public void setPosition(Vector3 position)
        {
            point = position;
            updatePosition();
        }
        private void updatePosition()
        {
            if (prefab != null) prefab.transform.position = point;
        }
        #endregion
        #region rotation
        public void setRotation(Quaternion r)
        {
            if (prefab != null)
                prefab.transform.rotation = new Quaternion(r.x, r.y, r.z, r.w);
        }
        #endregion
        #region prefab
        public void UpdatePrefab(GameObject input)
        {
            if (input == null)
            {
                destroyPrefab();
                return;
            }
            if (input != prefab)
            {
                destroyPrefab();
                instantiatePrefab(input);
                updatePosition();
            }
        }
        protected void instantiatePrefab(GameObject input)
        {
            prefab = Object.Instantiate(input);
            prefab.transform.SetParent(this.gameObject.transform);
        }
        protected void destroyPrefab()
        {
            if (prefab != null)
                Object.DestroyImmediate(prefab.gameObject);
        }

        #endregion
        #region redirectIndex
        public void setRedirectIndex(int index)
        {
            if (index < 0)
            {
                elementNodeType = ElementNodeType.None;
                redirectIndex = -1;
                return;
            }
            redirectIndex = index;
            elementNodeType = ElementNodeType.RedirectPoint;
        }
        public int getRedirectIndex()
        {
            return redirectIndex;
        }
        #endregion
        #region connections
        public int getConnectionsSize()
        {
            return connections == null ? -1 : connections.Count;
        }
        public bool ConnectionExist(int connectionIndex)
        {
            return connections.IndexOf(connectionIndex) > -1;
        }
        public void AddConnection(int connectionIndex)
        {
            if (connectionIndex != this.index && !ConnectionExist(connectionIndex))
            {
                connections.Add(connectionIndex);
            }
        }
        public void AddConnection(List<int> connections)
        {
            if (connections == null) return;
            foreach (int k in connections)
            {
                AddConnection(k);
            }
        }
        public void RemoveConnectionByIndex(int connectionIndex)
        {
            connections.RemoveAt(connectionIndex);
        }
        public void RemoveConnectionByValue(int val)
        {
            connections.Remove(val);
        }
        public void ConnectionsReset()
        {
            connections = new List<int>();
        }
        public void DecreaseConnectionValues(int pivot)
        {
            for (int i = 0; i < getConnectionsSize(); i++)
            {
                if (connections[i] > pivot) connections[i]--;
            }
        }
        #endregion
        #region incoming connections
        public bool ChangeIncomingConnectionIndex(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex || incomingConnections == null) return false;
            int index = incomingConnections.IndexOf(fromIndex);
            if (index < 0) return false;
            incomingConnections[index] = toIndex;
            return true;
        }
        public int getIncomingConnectionsSize()
        {
            return incomingConnections == null ? -1 : incomingConnections.Count;
        }
        public bool IncomingConnectionExist(int connectionIndex)
        {
            return incomingConnections.IndexOf(connectionIndex) > -1;
        }
        public void AddIncomingConnection(int connectionIndex)
        {
            if (connectionIndex != this.index && !IncomingConnectionExist(connectionIndex))
                incomingConnections.Add(connectionIndex);
        }
        public void AddIncomingConnection(List<int> connections)
        {
            if (incomingConnections == null) return;
            foreach (int k in incomingConnections)
            {
                AddIncomingConnection(k);
            }
        }
        public void RemoveIncomingConnectionByIndex(int connectionIndex)
        {
            incomingConnections.RemoveAt(connectionIndex);
        }
        public void RemoveIncomingConnectionByValue(int val)
        {
            incomingConnections.Remove(val);
        }
        public void IncomingConnectionsReset()
        {
            incomingConnections = new List<int>();
        }
        public void DecreaseIncomingConnectionValues(int pivot)
        {
            for (int i = 0; i < getIncomingConnectionsSize(); i++)
            {
                if (incomingConnections[i] > pivot) incomingConnections[i]--;
            }
        }
        #endregion
        #region events
        public void InvokeEvents()
        {
            if (onPlayerStayEvent != null) onPlayerStayEvent.Invoke();
        }
        #endregion
        #endregion
    }
}