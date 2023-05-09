using UnityEngine;
using MyDice.Board;

namespace MyDice.Players
{
    public class PlayerGhost : MonoBehaviour
    {
        #region variable
        protected GameObject prefab;
        public Path path;
        #endregion
        #region Functions
        private void OnDestroy()
        {
            destroyPrefab();
        }
        #endregion
        #region functions
        public void setPath(Path p)
        {
            path = p;
        }
        public Path getPath() { return path; }
        #region prefab
        public void setPrefab(GameObject input)
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
                //updatePosition();
            }
        }
        public void updatePosition(Vector3 position)
        {
            this.transform.position = position;
            if (prefab != null) prefab.transform.position = this.transform.position;
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
        #endregion
    }
}