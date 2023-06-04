namespace MyDice.Board
{
    using MyDice.Helpers;
    using MyDice.Players;
    using System.Collections.Generic;

    public class ArrangingPiecesInElementNode
    {
        #region parameters
        public float arrangeTheBeadsRadius;
        public List<ElementNode> elementNodes;
        #endregion
        public void addElementNode(ElementNode node)
        {
            if (node == null) return;
            if (elementNodes == null) elementNodes = new List<ElementNode>();
            elementNodes.Add(node);
        }
        #region functions
        public void findAndArrangePlayers(float radius, ref List<PlayerHome> playerHomes)
        {
            if (elementNodes == null || elementNodes.Count < 0 || radius < 0f)
            {
                elementNodes.Clear();
                return;
            }
            foreach (var node in elementNodes)
            {
                arrangePlayersInNode(getPlayersInSameNode(node, ref playerHomes), node, radius);
            }
            elementNodes.Clear();
        }
        private List<Player> getPlayersInSameNode(ElementNode node, ref List<PlayerHome> playerHomes)
        {
            if (node == null) return null;
            return getPlayersInSameNode(node.index, ref playerHomes);
        }
        private List<Player> getPlayersInSameNode(int nodeIndex, ref List<PlayerHome> playerHomes)
        {
            List<Player> result = new List<Player>();
            if (playerHomes == null) return result;
            for (int i = 0; i < playerHomes.Count; i++)
            {
                List<Player> _players;
                if (playerHomes[i] == null
                    || (_players = playerHomes[i].getPlayers()) == null) continue;
                foreach (var _player in _players)
                {
                    if (_player == null) continue;
                    if (_player.currentPositionIndex == nodeIndex && _player.TouchCount > 0)
                    {
                        result.Add(_player);
                    }
                }
            }
            return result;
        }
        private void arrangePlayersInNode(List<Player> _players, ElementNode node, float radius)
        {
            if (node == null || _players == null || _players.Count < 1 || radius < 0f) return;
            if (_players.Count == 1)
            {
                _players[0].GoTo_Immediately(node.point);
                return;
            }
            CircleMaker c = new CircleMaker();
            var _points = c.CreateCircle(radius, node.point, _players.Count);
            if (_points.Count == _players.Count)
            {
                for (int i = 0; i < _points.Count; i++)
                {
                    _players[i].GoTo_Immediately(_points[i]);
                }
            }
        }
        #endregion
    }
}
