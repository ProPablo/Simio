using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateMachine), true)]
public class StateMachineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        StateMachine machine = (StateMachine) target;
        if (machine.currentState == null)
        {
            EditorGUILayout.LabelField("Current State", "nothing lol");
        }
        else
        {
            EditorGUILayout.LabelField("Current State", machine.currentState.stateName);
            EditorGUILayout.FloatField(machine.currentState.age);
        }
    }
}

[CustomEditor(typeof(HexGrid), true)]
public class HexGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        var obj = (HexGrid) target;
        if (DrawDefaultInspector())
        {
            if (obj.autoUpdate)
            {
                obj.Init();
            }
        }
        if (GUILayout.Button("Generate"))
        {
            obj.Init();
        }

        if (GUILayout.Button("Clear"))
        {
            obj.ClearGrid();
            
        }
        // if (machine.currentState == null)
        // {
        //     EditorGUILayout.LabelField("Current State", "nothing lol");
        // }
        // else
        // {
        //     EditorGUILayout.LabelField("Current State", machine.currentState.stateName);
        //     EditorGUILayout.FloatField(machine.currentState.age);
        // }
    }
}