using UnityEditor;
using UnityEngine;
using MyDice.Board;
namespace MyDice.Editors
{
    public class PolygonShape : EditorWindow
    {
        public static ElementNodeCreator target;
        protected bool followRotation = false;
        protected static Quaternion r;
        public static void Open(ref ElementNodeCreator enEditor)
        {
            PolygonShape window = GetWindow<PolygonShape>("N-Gon shape");
            target = enEditor;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("a: ");
            target.polygonStruct.a = EditorGUILayout.FloatField(target.polygonStruct.a);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("edges: ");
            target.polygonStruct.edges = EditorGUILayout.IntField(target.polygonStruct.edges);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("angle: ");
            target.polygonStruct.angle = EditorGUILayout.FloatField(target.polygonStruct.angle);
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
                target.PolygonShape(target.polygonStruct);
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