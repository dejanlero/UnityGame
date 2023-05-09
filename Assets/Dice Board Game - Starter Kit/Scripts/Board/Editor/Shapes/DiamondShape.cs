using UnityEditor;
using UnityEngine;
using MyDice.Board;

namespace MyDice.Editors
{
    public class DiamondShape : EditorWindow
    {
        public static ElementNodeCreator target;
        protected bool followRotation = false;
        protected static Quaternion r;
        public static void Open(ref ElementNodeCreator enEditor)
        {
            DiamondShape window = GetWindow<DiamondShape>("Diamond shape");
            target = enEditor;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("a: ");
            target.diamondStruct.a = EditorGUILayout.FloatField(target.diamondStruct.a);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("b: ");
            target.diamondStruct.b = EditorGUILayout.FloatField(target.diamondStruct.b);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("angle: ");
            target.diamondStruct.angle = EditorGUILayout.FloatField(target.diamondStruct.angle);
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
                target.DiamondShape(target.diamondStruct);
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