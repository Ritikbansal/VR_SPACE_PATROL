using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using VSX.UniversalVehicleCombat;


namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// This class modifies the inspector of the HUDHologram class so that the user can set style information for different teams in
    /// the Team enum.
    /// </summary>
	[CustomEditor(typeof(HUDHologram))]
	public class HUDHologramEditor : Editor
    {

        HUDHologram script;

        SerializedProperty targetHologramProperty;


        private void OnEnable()
        {

            // Get a reference to the script
            script = (HUDHologram)target;

            // Get references to the properties on this object
            targetHologramProperty = serializedObject.FindProperty("targetHologram");
        }

        public override void OnInspectorGUI()
		{

			// Setup
			serializedObject.Update();
			
            // Resize the list of team colors
			string[] teamNames = Enum.GetNames(typeof(Team));               
			StaticFunctions.ResizeList(script.colorByTeam, teamNames.Length);
			

            // Settings

			EditorGUILayout.BeginVertical("box");

			EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(targetHologramProperty);
			
			EditorGUILayout.EndVertical();


            // Team colors

			EditorGUILayout.BeginVertical("box");

			EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);
			
			for (int i = 0; i < script.colorByTeam.Count; ++i)
			{
				script.colorByTeam[i] = EditorGUILayout.ColorField(teamNames[i] + " Color", script.colorByTeam[i]);			
			}

			EditorGUILayout.EndVertical();


            // Apply modifications

			EditorUtility.SetDirty(script);
			
			serializedObject.ApplyModifiedProperties();
	    }
	}
}
