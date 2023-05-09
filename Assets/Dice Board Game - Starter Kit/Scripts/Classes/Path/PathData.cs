using System.Collections.Generic;

namespace MyDice.Board
{
    [System.Serializable]
    public class PathData
    {
        #region variable
        protected List<int> nodes;
        protected bool loop;
        #endregion
        #region constructor
        public PathData()
        {
            init();
        }
        public PathData(PathData other)
        {
            init();
            CopyFrom(other);
        }
        #endregion
        #region functions
        #region logic
        private void init()
        {
            nodes = new List<int>();
            this.loop = false;
        }
        #endregion
        #region nodes
        public List<int> getNodes()
        {
            return nodes;
        }
        public int getNodesCount()
        {
            return nodes.Count;
        }
        public bool nodeIsExist(int node)
        {
            return nodes.IndexOf(node) > -1;
        }
        public PathData addNode(int node)
        {
            if (nodeIsExist(node)) loop = true;
            nodes.Insert(nodes.Count, node);
            return this;
        }
        #endregion
        #region logic
        public bool hasLoop()
        {
            return loop;
        }
        public void CopyFrom(PathData other)
        {
            this.loop = other.hasLoop();
            nodes = new List<int>(other.getNodes());
        }
        #endregion
        #endregion
        #region distructor
        ~PathData() { }
        #endregion
    }
}