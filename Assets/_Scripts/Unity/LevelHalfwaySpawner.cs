using UnityEngine;
using UnityEditor;
using Mono.Cecil;

[CustomEditor(typeof(HighestPointFinder))]
public class LevelHalfwaySpawner : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        HighestPointFinder highestPointFinder = (HighestPointFinder)target;

        if(GUILayout.Button("Create HalfWay Trigger")){
            highestPointFinder.SpawnHalfWayTrigger();
        }

    }

}
