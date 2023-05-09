﻿using UnityEditor;
using UnityEngine;
using MyDice.Board;
namespace MyDice.Editors
{
    public class RectangleShape : EditorWindow
    {
        public static ElementNodeCreator target;
        protected bool followRotation = false;
        protected static Quaternion r;
        public static void Open(ref ElementNodeCreator enEditor)
        {
            RectangleShape window = GetWindow<RectangleShape>("Rectangle shape");
            target = enEditor;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("a: ");
            target.squareStruct.a = EditorGUILayout.FloatField(target.squareStruct.a);
            GUILayout.Label("b: ");
            target.squareStruct.b = EditorGUILayout.FloatField(target.squareStruct.b);
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
                target.RectangleShape(target.squareStruct);
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