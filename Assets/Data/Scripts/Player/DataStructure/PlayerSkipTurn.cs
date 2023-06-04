using System;

namespace MyDice.Board.DataStructure
{
    public class PlayerSkipTurn
    {
        public const int MIN = 0;
        public int PlayerIndex;
        public int SkipRound;
        #region 
        public PlayerSkipTurn(int playerIndex) : this(playerIndex, MIN + 1)
        {
        }
        public PlayerSkipTurn(int playerIndex, int skipRound)
        {
            this.PlayerIndex = playerIndex;
            this.SkipRound = skipRound;
        }
        #endregion
        #region Logic
        public bool Skipping()
        {
            return (--SkipRound) >= MIN;
        }
        #endregion
        public bool PlayerIndexEquals(PlayerSkipTurn node)
        {
            if (node == null) return false;
            return this.PlayerIndex == node.PlayerIndex;
        }
        public bool MergeIfPlayerIndexEquals(PlayerSkipTurn node)
        {
            if (node == null
                || !PlayerIndexEquals(node)) return false;
            SkipRound += node.SkipRound;
            return true;
        }
        public override string ToString() => $"({PlayerIndex}, {SkipRound})";
    }
}
