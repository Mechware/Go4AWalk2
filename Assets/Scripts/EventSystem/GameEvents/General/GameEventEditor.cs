using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CustomeEvents;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameEvent eventEditor = (GameEvent) target;
        if(GUILayout.Button("Raise Flag"))
        {
            eventEditor.Raise();
        }
    }

}
