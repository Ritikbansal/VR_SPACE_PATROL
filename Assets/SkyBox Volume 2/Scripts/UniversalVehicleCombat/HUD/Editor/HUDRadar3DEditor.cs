using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using VSX.UniversalVehicleCombat;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// This class modifies the inspector of the Radar3D class so that the user can set style information for different teams in
    /// the Team enum.
    /// </summary>
	[CustomEditor(typeof(HUDRadar3D))]
	public class HUDRadar3DEditor : Editor
    {
		
        // Reference to the object
		HUDRadar3D script;

        // References to all the properties on the object
        SerializedProperty displayableTypesProperty;
        SerializedProperty equatorRadiusProperty;
        SerializedProperty scaleExponentProperty;
        SerializedProperty zoomSpeedProperty;
        SerializedProperty fadeMinAlphaProperty;
        SerializedProperty fadeMaxAlphaProperty;
        SerializedProperty maxNewTargetsEachFrameProperty;


		void OnEnable()
		{

            // Get a reference to the object
			script = (HUDRadar3D)target;

            // Get references to all of the object's properties
            displayableTypesProperty = serializedObject.FindProperty("displayableTypes");
            equatorRadiusProperty = serializedObject.FindProperty("equatorRadius");
            scaleExponentProperty = serializedObject.FindProperty("scaleExponent");
            zoomSpeedProperty = serializedObject.FindProperty("zoomSpeed");
            fadeMinAlphaProperty = serializedObject.FindProperty("fadeMinAlpha");
            fadeMaxAlphaProperty = serializedObject.FindProperty("fadeMaxAlpha");
            maxNewTargetsEachFrameProperty = serializedObject.FindProperty("maxNewTargetsEachFrame");

        }


		public override void OnInspectorGUI()
		{

			// Setup
			serializedObject.Update();
			
            // Get an array of all the trackable types
			string[] typeNames = Enum.GetNames(typeof(TrackableType));

            // Get an array of all the teams
			string[] teamNames = Enum.GetNames(typeof(Team));
	                
			// Resize lists in the layout (not Repaint!) phase
			if (Event.current.type == EventType.Layout)
			{
                // Resize the list of widget settings according to the number of trackable types
				StaticFunctions.ResizeList(script.widgetSettingsByType, typeNames.Length);

                // Flag whether the colors for the teams have been initialized 
                bool colorsInitialized = script.colorByTeam.Count > 0;

                // Resize the team colors list according to the number of teams
                StaticFunctions.ResizeList(script.colorByTeam, teamNames.Length);

                // If team colors not initialized, initialize them to a default color
                if (!colorsInitialized)
                {
                    for (int i = 0; i < script.colorByTeam.Count; ++i)
                    {
                        script.colorByTeam[i] = Color.white;
                    }
                }
				
                // Apply modifications
				serializedObject.ApplyModifiedProperties();
				serializedObject.Update();

			}


			// Show settings in the inspector

			EditorGUILayout.BeginVertical("box");

            EditorGUILayout.PropertyField(displayableTypesProperty, true);
			
            EditorGUILayout.PropertyField(equatorRadiusProperty);

            EditorGUILayout.PropertyField(scaleExponentProperty);

            EditorGUILayout.PropertyField(zoomSpeedProperty);

            EditorGUILayout.PropertyField(fadeMinAlphaProperty);

            EditorGUILayout.PropertyField(fadeMaxAlphaProperty);

            EditorGUILayout.PropertyField(maxNewTargetsEachFrameProperty);

			EditorGUILayout.EndVertical();


			// Show team colors in the inspector
			EditorGUILayout.BeginVertical("box");

			EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);
			
			for (int i = 0; i < script.colorByTeam.Count; ++i)
			{				
				script.colorByTeam[i] = EditorGUILayout.ColorField(teamNames[i] + " Color", script.colorByTeam[i]);				
			}

			EditorGUILayout.EndVertical();
			

			// Show widget settings in the inspector for each of the trackable types
			for (int i = 0; i < script.widgetSettingsByType.Count; ++i)
			{
				EditorGUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("TrackableType " + typeNames[i] + " Visualization Settings", EditorStyles.boldLabel);

				script.widgetSettingsByType[i].ignore = EditorGUILayout.Toggle("Ignore", script.widgetSettingsByType[i].ignore);

				script.widgetSettingsByType[i].fadeUnselectedByDistance = EditorGUILayout.Toggle("Fade Unselected By Distance", script.widgetSettingsByType[i].fadeUnselectedByDistance);
		
				script.widgetSettingsByType[i].widgetPrefab = EditorGUILayout.ObjectField("Widget Prefab", script.widgetSettingsByType[i].widgetPrefab, typeof(GameObject), false) as GameObject;
            	
				EditorGUILayout.Space();
				EditorGUILayout.EndVertical();
			}
			
			// Apply modifications
			EditorUtility.SetDirty(script);
			serializedObject.ApplyModifiedProperties();

	    }
	}
}
