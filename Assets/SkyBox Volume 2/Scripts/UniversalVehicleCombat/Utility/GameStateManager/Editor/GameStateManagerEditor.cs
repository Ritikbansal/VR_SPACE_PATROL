using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VSX.UniversalVehicleCombat;

/// <summary>
/// This class provides a custom inspector for the GameStateManager component. This allows the inspector to 
/// provide settings for all the values in the GameState enum.
/// </summary>
[CustomEditor(typeof(GameStateManager))]
public class GameStateManagerEditor : Editor
{

    GameStateManager script;
    SerializedProperty startingGameStateProperty;

    private void OnEnable()
    {

        script = (GameStateManager)target;

        startingGameStateProperty = serializedObject.FindProperty("startingGameState");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        string[] gameStateNames = System.Enum.GetNames(typeof(GameState));

        // Resize lists in the layout (not Repaint!) phase
        if (Event.current.type == EventType.Layout)
        {
            // Resize the list of widget settings according to the number of trackable types
            ResizeList(script.gameStateParameters, gameStateNames.Length);

            // Apply modifications
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        EditorGUILayout.PropertyField(startingGameStateProperty);
        
        for (int i = 0; i < script.gameStateParameters.Count; ++i)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField(gameStateNames[i] + " Settings", EditorStyles.boldLabel);

            script.gameStateParameters[i].gameState = (GameState)i;

            script.gameStateParameters[i].freezeTimeOnEntry = EditorGUILayout.Toggle("Freeze Time On Entry", script.gameStateParameters[i].freezeTimeOnEntry);

            if (script.gameStateParameters[i].freezeTimeOnEntry)
            {
                script.gameStateParameters[i].pauseBeforeTimeFreeze = EditorGUILayout.FloatField("Pause Before Time Freeze", script.gameStateParameters[i].pauseBeforeTimeFreeze);
            }

            EditorGUILayout.EndVertical();
        }

        // Apply modifications
        EditorUtility.SetDirty(script);
        serializedObject.ApplyModifiedProperties();
    }


    // Resize the game state parameters list
    private static void ResizeList(List<GameStateParameters> list, int newSize)
    {
        if (list.Count == newSize)
            return;

        if (list.Count < newSize)
        {
            int numAdditions = newSize - list.Count;
            for (int i = 0; i < numAdditions; ++i)
            {
                list.Add(new GameStateParameters());
            }
        }
        else
        {
            int numRemovals = list.Count - newSize;
            for (int i = 0; i < numRemovals; ++i)
            {
                //Remove the last one in the list
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
