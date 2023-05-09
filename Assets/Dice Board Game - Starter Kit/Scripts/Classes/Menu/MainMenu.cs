#if UNITY_EDITOR
namespace MyDice.Menu
{
    using UnityEditor;
    using UnityEngine;
    using MyApp.MyDice;
    public class MainMenu : MonoBehaviour
    {
        #region Game manager
        [MenuItem(Globals.RootName + "/" + Globals.PROJECT_NAME + "/Game manager/Add (or) Select")]
        private static void AddBoardGameManager()
        {
            GameObject obj;
            var item = FindObjectOfType<BoardGameManager>();
            if (item == null)
            {
                obj = new GameObject("Game manager");
                obj.AddComponent<BoardGameManager>();
                Undo.RegisterCreatedObjectUndo(obj, "Create Game manager");
                obj.transform.position = getPosition();
            }
            else
            {
                obj = item.gameObject;
            }
            Selection.objects = new Object[] { obj };
        }
        #endregion
        #region Dice
        [MenuItem(Globals.RootName + "/" + Globals.PROJECT_NAME + "/Dice/Add Dice 4")]
        private static void AddDice4()
        {
            AddDice("prefabs/Dices/Dice_D4");
        }
        [MenuItem(Globals.RootName + "/" + Globals.PROJECT_NAME + "/Dice/Add Dice 6")]
        private static void AddDice6()
        {
            AddDice("prefabs/Dices/Dice_D6");
        }
        [MenuItem(Globals.RootName + "/" + Globals.PROJECT_NAME + "/Dice/Add Dice 8")]
        private static void AddDice8()
        {
            AddDice("prefabs/Dices/Dice_D8");
        }
        [MenuItem(Globals.RootName + "/" + Globals.PROJECT_NAME + "/Dice/Add Dice 10")]
        private static void AddDice10()
        {
            AddDice("prefabs/Dices/Dice_D10");
        }
        private static void AddDice(string prefabAddress)
        {
            GameObject prefab = (GameObject)Resources.Load(prefabAddress, typeof(GameObject));
            GameObject obj = Object.Instantiate(prefab);
            Undo.RegisterCreatedObjectUndo(obj, "Create Dice");
            obj.transform.position = Vector3.zero; Selection.objects = new Object[] { obj };
        }
        #endregion
        #region Board
        [MenuItem(Globals.RootName + "/" + Globals.PROJECT_NAME + "/Board/Add Board 1")]
        private static void AddBoard_1()
        {
            AddBoard("prefabs/Boards/Board_1");
        }
        [MenuItem(Globals.RootName + "/" + Globals.PROJECT_NAME + "/Board/Add Board 2")]
        private static void AddBoard_2()
        {
            AddBoard("prefabs/Boards/Board_2");
        }
        private static void AddBoard(string prefabAddress)
        {
            GameObject prefab = (GameObject)Resources.Load(prefabAddress, typeof(GameObject));
            GameObject obj = Object.Instantiate(prefab);
            Undo.RegisterCreatedObjectUndo(obj, "Create Board");
            obj.transform.position = getPosition();
            Selection.objects = new Object[] { obj };
        }
        #endregion
        #region About us
        [MenuItem(Globals.RootName + "/Publisher Page")]
        public static void PublisherPage()
        {
            Application.OpenURL("https://assetstore.unity.com/publishers/48757");
        }
        #endregion
        #region Support
        [MenuItem(Globals.RootName + "/Support")]
        public static void Support()
        {
            TextWindow window = (TextWindow)EditorWindow.GetWindow(typeof(TextWindow), true, "Support");
            window.Descriptions = new string[] { "If you need any further assistance, please contact us", "unrealisticarts@gmail.com" };
        }
        #endregion
        private static Vector3 getPosition()
        {
            var item = FindObjectOfType<BoardGameManager>();
            if (item == null) return Vector3.zero;
            return item.transform.position;
        }
    }
}
#endif