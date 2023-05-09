namespace MyDice.Players
{
    [System.Serializable]
    public class PlayerPositionIndex
    {
        public const int NullValue = -1;
        public int[] positionIndex;
        
        public PlayerPositionIndex(int size = 1)
        {
            int _size = size < 1 ? 1 : size;
            positionIndex = new int[_size];
            Reset();
        }
        public void AddIndex(int index)
        {
            int i = positionIndex.Length;
            while (--i != 0)
            {
                positionIndex[i] = positionIndex[i - 1];
            }
            positionIndex[i] = index;
        }
        public int GetIndex(int index)
        {
            if (index < 0 || index >= positionIndex.Length) return NullValue;
            return positionIndex[index];
        }
        public int getSize()
        {
            return positionIndex.Length;
        }
        public void Reset()
        {
            for (int i = 0; i < positionIndex.Length; i++)
                positionIndex[i] = NullValue;
        }
    }
}