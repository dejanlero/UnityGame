namespace MyDice.Board
{
    [System.Serializable]
    public class Path
    {
        #region variable
        protected int[] index;
        protected int[] diceValues;
        protected int benefit = int.MaxValue;
        #endregion
        #region constructor
        public Path(int size, int[] diceValues)
        {
            this.diceValues = diceValues;
            index_init(size);
        }
        public Path(Path other)
        {
            this.CopyFrom(other);
        }
        #endregion
        #region functions
        #region index
        public void index_init(int size)
        {
            if (size > 0)
            {
                index = new int[size];
                index_reset();
            }
        }
        public void index_reset()
        {
            for (int i = 0; i < index.Length; i++) index[i] = -1;
        }
        public int getIndexSize()
        {
            return index == null ? -1 : index.Length;
        }
        public bool addIndex(int val, bool addUniqueIndex = false)
        {
            for (int i = 0; i < getIndexSize(); i++)
            {
                var currentIndex = getIndex(i);
                if (addUniqueIndex && currentIndex == val) return false;
                if (currentIndex == -1)
                {
                    setIndex(i, val);
                    return true;
                }
            }
            return false;
        }
        public void setIndex(int i, int val)
        {
            if (getIndexSize() > i && i > -1) index[i] = val;
        }
        public int getIndex(int i)
        {
            if (getIndexSize() > i && i > -1)
                return index[i];
            return -1;
        }

        public int getLastValue()
        {
            for (int i = getIndexSize(); i > -1; i--)
            {
                if (getIndex(i) != -1) return getIndex(i);
            }
            return -1;
        }
        #endregion
        #region benefit
        public int Benefit { get { return benefit; } set { benefit = value; } }
        #endregion
        #region dice Values
        public int getDiceValuesSize()
        {
            return diceValues == null ? -1 : diceValues.Length;
        }
        public int[] getDiceValues()
        {
            return diceValues;
        }
        public void diceValues_CopyFrom(int[] other)
        {
            diceValues = new int[other.Length];
            for (int i = 0; i < other.Length; i++)
                diceValues[i] = other[i];
        }
        #endregion
        #region hitIndex
        public int[] getHitIndex()
        {
            int[] hitIndex = new int[diceValues.Length];
            int lastHit = 0;
            for (int i = 0; i < diceValues.Length; i++)
            {
                hitIndex[i] = getIndex(diceValues[i] + lastHit);
                lastHit = diceValues[i];
            }
            return hitIndex;
        }
        #endregion
        #region logic
        public bool isCompleted()
        {
            for (int i = 0; i < getIndexSize(); i++)
                if (getIndex(i) == -1) return false;
            return true;
        }
        public bool EqualsTo(Path other)
        {
            if (other == null) return false;
            if (other.getIndexSize() != getIndexSize()) return false;
            for (int i = 0; i < getIndexSize(); i++)
                if (other.getIndex(i) != getIndex(i)) return false;
            return true;
        }
        public void CopyFrom(Path other)
        {
            if (other == null) return;
            index_init(other.getIndexSize());
            diceValues_CopyFrom(other.getDiceValues());
            for (int i = 0; i < other.getIndexSize(); i++)
                setIndex(i, other.getIndex(i));
        }
        #endregion
        #endregion
        #region distructor
        ~Path() { }
        #endregion
    }
}