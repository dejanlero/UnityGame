#if UNITY_EDITOR
//namespace MyDice.Editors
using UnityEditor;
using MyApp.MyDice;

[CustomEditor(typeof(Board))]
public class DiceManagerEditor : Editor
{
    private Board Target;
    #region Inspector
    public override void OnInspectorGUI()
    {
        if ((Target = (Board)target) == null) return;
        serializedObject.Update();
        Params();
        serializedObject.ApplyModifiedProperties();
        Info();
    }
    private void Params()
    {
        if (!EditorTools.Foldout(ref Target.parametersEnable, "Parameter(s)")) return;
        EditorTools.Box_Open();
        EditorTools.PropertyField(serializedObject, "collisionObjectTags", "Collision object tags");
        EditorTools.PropertyField(serializedObject, "rollingThreshold", "Rolling threshold");
        EditorTools.Line();
        EditorTools.PropertyField(serializedObject, "autoDetection_dices", "Dice(s) auto-detection");
        if (!Target.autoDetection_dices)
        {
            EditorTools.PropertyField(serializedObject, "dice", "Dice(s)");
        }
        EditorTools.Box_Close();
    }
    private void Info()
    {
        if (!EditorTools.Foldout(ref Target.infoEnable, "Infomation")) return;
        EditorTools.Box_Open();
        EditorTools.Info("State", Target.getDiceState().ToString());
        EditorTools.Box_Close();
    }
    #endregion
}
#endif