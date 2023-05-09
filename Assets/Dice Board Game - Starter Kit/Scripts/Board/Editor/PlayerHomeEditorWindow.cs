using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyDice.Board
{
    public class PlayerHomeEditorWindow : EditorWindow
    {
        public static PlayerHome target;
        public static ElementNodeCreator elementNode;
        public static void Open(int index, ref ElementNodeCreator eNode)
        {
            elementNode = eNode;
            target = elementNode.playerHomes[index];
            PlayerHomeEditorWindow window = GetWindow<PlayerHomeEditorWindow>(target.getName());
        }
        private void OnGUI()
        {
            if (target == null) { this.Close(); return; }
            ///Name
            {
                if (!string.IsNullOrEmpty(target.getName()))
                {
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("name: " + target.getName());
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
            }
            /// startIndex
            {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Player start index: ");
                {
                    if (target.startIndex >= elementNode.getPointsCount())
                    {
                        target.startIndex = 0;
                    }
                    var point = EditorGUILayout.IntField(target.startIndex);
                    if (point >= 0 && point < elementNode.getPointsCount())
                    {
                        target.startIndex = point;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            /// targetIndex
            {
                bool haveTargetNode = target.targetIndex >= 0 && target.targetIndex < elementNode.getPointsCount();
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                GUILayout.Label("Include target index: ");
                {
                    var val = EditorGUILayout.Toggle(haveTargetNode);
                    if (val != haveTargetNode)
                    {
                        if (haveTargetNode)
                        {
                            target.targetIndex = -1;
                        }
                        else
                        {
                            target.targetIndex = 0;
                        }
                        haveTargetNode = val;
                    }
                }
                GUILayout.EndHorizontal();
                ///
                if (haveTargetNode)
                {
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Player target index: ");
                    {
                        if (target.targetIndex <- 1 || elementNode.getPointsCount()<= target.targetIndex)
                        {
                            target.targetIndex = -1;
                        }
                        var point = EditorGUILayout.IntField(target.targetIndex);
                        if (point >= -1 && point < elementNode.getPointsCount())
                        {
                            target.targetIndex = point;
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
                else
                {
                    if (target.targetIndex != -1)
                        target.targetIndex = -1;
                }
                GUILayout.EndVertical();
            }
            ///playerMode
            {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Player mode: ");
                    target.playerMode = (PlayerMode)EditorGUILayout.Popup((int)target.playerMode, System.Enum.GetNames(typeof(PlayerMode)));
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            ///Count
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Player(s) count: ");
                    {
                        var count = EditorGUILayout.IntField(target.getCount());
                        if (count != target.getCount())
                        {
                            target.setCount(count);
                            target.updatePositions(elementNode.handlePlayerHomesRadius);
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
                ///playerPrefab
                {
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    var obj = EditorGUILayout.ObjectField("Player prefab:", target.playerPrefab, typeof(GameObject), true);
                    if (obj != target.playerPrefab)
                    {
                        target.updatePrefab((GameObject)obj);
                        target.updatePositions(elementNode.handlePlayerHomesRadius);
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
                ///ghostPrefab
                {
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    var obj = EditorGUILayout.ObjectField("Player ghost prefab:", target.ghostPrefab, typeof(GameObject), true);
                    if (obj != target.ghostPrefab)
                    {
                        target.ghostPrefab = (GameObject)obj;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();
            }
            ///Remove
            {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Remove"))
                {
                    elementNode.removePlayerHome(target);
                    this.Close();
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            ///
            GUILayout.Space(2);
        }
    }
}