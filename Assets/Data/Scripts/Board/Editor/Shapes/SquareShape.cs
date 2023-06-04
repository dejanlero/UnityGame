using UnityEditor;
using UnityEngine;
using MyDice.Board;
namespace MyDice.Editors
{
    public class SquareShape : EditorWindow
    {
        public static ElementNodeCreator target;
        protected bool followRotation = false;
        protected static Quaternion r;
        public static void Open(ref ElementNodeCreator enEditor)
        {
            SquareShape window = GetWindow<SquareShape>("Square shape");
            target = enEditor;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("a: ");
            target.squareStruct.a = EditorGUILayout.FloatField(target.squareStruct.a);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("angle: ");
            target.squareStruct.angle = EditorGUILayout.FloatField(target.squareStruct.angle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Follow rotation: ");
            followRotation = EditorGUILayout.Toggle(followRotation);
            GUILayout.EndHorizontal();
            if (followRotation)
            {
                GUILayout.BeginHorizontal();
                Vector4 rv = new Vector4(r.x, r.y, r.z, r.w);
                rv = EditorGUILayout.Vector4Field("Rotation: ", rv);
                r = new Quaternion(rv.x, rv.y, rv.z, rv.w);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh"))
            {
                target.SquareShape(target.squareStruct);
                if (followRotation)
                {
                    target.updatePrefabs(r);
                }
                this.Close();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}