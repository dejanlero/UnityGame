using UnityEditor;
using UnityEngine;
using MyDice.Board;

namespace MyDice.Editors
{
    public class CircleShape : EditorWindow
    {
        public static ElementNodeCreator target;
        protected bool followRotation = false;
        protected static Quaternion r;
        public static void Open(ref ElementNodeCreator enEditor)
        {
            CircleShape window = GetWindow<CircleShape>("Circle shape");
            target = enEditor;
        }
       
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius: ");
            target.circleStruct.radius = EditorGUILayout.FloatField(target.circleStruct.radius);
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
                target.CircleShape(target.circleStruct);
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