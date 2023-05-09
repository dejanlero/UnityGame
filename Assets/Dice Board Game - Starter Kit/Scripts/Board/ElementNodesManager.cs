using System.Collections.Generic;
using UnityEngine;
namespace MyDice.Board
{
    [System.Serializable]
    public class ElementNodesManager
    {
        [HideInInspector] public List<GameObject> nodes;
        public ElementNodesManager(ref List<GameObject> nodes)
        {
            this.nodes = nodes;
            if (this.nodes == null) this.nodes = new List<GameObject>();
        }
        #region functions
        #region nodes
        public int getNodesSize()
        {
            return nodes == null ? -1 : nodes.Count;
        }
        public int getFinalRedirectFrom(int redirectIndex)
        {
            List<int> paths = new List<int>();
            ElementNode node;
            int rIndex = redirectIndex;
            while (true)
            {
                if ((node = getNode(rIndex)) == null)
                {
                    node = nodes[rIndex].AddComponent<ElementNode>();
                    return -1;
                };
                if (paths.IndexOf(rIndex) > -1) return -1;
                paths.Add(rIndex);
                if (node.redirectIndex < 0) break;
                rIndex = node.redirectIndex;
            }
            return rIndex;
        }
        public ElementNode getNode(int index)
        {
            if (index < 0 || index >= nodes.Count) return null;
            ElementNode result = nodes[index].GetComponent<ElementNode>();
            if (result == null)
            {
                result = nodes[index].AddComponent<ElementNode>();
            }
            return result;
        }
        public ElementNode getFinalDestination(ElementNode node)
        {
            if (node == null) return null;
            List<ElementNode> nodes = new List<ElementNode>();
            ElementNode result = node;
            while (true)
            {
                if (result == null || nodes.IndexOf(result) > -1) break;
                nodes.Add(result);
                switch (result.getElementNodeType())
                {
                    case ElementNodeType.None:
                        return result;
                    case ElementNodeType.RedirectPoint:
                        result = getNode(result.redirectIndex);
                        break;
                    case ElementNodeType.ResetPoint:
                        return result;
                }
            }
            return result;
        }
        #region nodes connections
        public void nodesChangeConnections(int index1, int index2, int connectionValue, ref List<GameObject> nodes)
        {
            ElementNode node1, node2;
            if (nodes[index1] == null
                || nodes[index2] == null
                || (node1 = nodes[index1].GetComponent<ElementNode>()) == null
                || (node2 = nodes[index2].GetComponent<ElementNode>()) == null) return;

            node1.AddConnection(index2);
            node2.AddConnection(connectionValue);
            node1.RemoveConnectionByValue(connectionValue);
        }
        public void nodesRemoveConnection(int nodeIndex, int ConnectionValue)
        {
            ElementNode node;
            if (nodes[nodeIndex] == null || (node = getNode(nodeIndex)) == null) return;
            node.RemoveConnectionByValue(ConnectionValue);
            if (nodes[ConnectionValue] == null || (node = getNode(ConnectionValue)) == null) return;
            node.RemoveIncomingConnectionByValue(nodeIndex);
        }
        #endregion
        #endregion
        #region logic
        public void checkToFixProblems()
        {
            ElementNode node;
            for (int i = 0; i < getNodesSize(); i++)
            {
                if ((node = getNode(i)) == null) continue;
                if (node.redirectIndex > -1 && node.redirectIndex < getNodesSize())
                {
                    if (node.getElementNodeType() != ElementNodeType.RedirectPoint)
                    {
                        node.setElementNodeType(ElementNodeType.RedirectPoint);
                    }
                }
                else
                {
                    node.redirectIndex = -1;
                    if (node.getElementNodeType() == ElementNodeType.RedirectPoint)
                    {
                        node.setElementNodeType(ElementNodeType.None);
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}