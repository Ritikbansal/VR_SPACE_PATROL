using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using VSX.UniversalVehicleCombat;


namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// This class modifies the inspector of the HUDTargetTracking class so that the user can set style information for different teams in
    /// the Team enum.
    /// </summary>
	[CustomEditor(typeof(HUDTargetTracking))]
	public class HUDTargetTrackingEditor : Editor
    {
	
        // Reference to the object
		HUDTargetTracking script;
		
		// References to Settings properties
		SerializedProperty hudCameraProperty;
		SerializedProperty uiViewportCoefficientsProperty;
		SerializedProperty useMeshBoundsCenterProperty;
		SerializedProperty enableAspectRatioProperty;
		SerializedProperty centerOffscreenArrowsProperty;
		SerializedProperty centerOffscreenArrowsRadiusProperty;
		SerializedProperty fadeMinAlphaProperty;
		SerializedProperty fadeMaxAlphaProperty;
		SerializedProperty maxNewTargetsEachFrameProperty;
		SerializedProperty expandingTargetBoxesProperty;
        SerializedProperty displayableTypesProperty;

		// References to World Space Settings properties
		SerializedProperty useTargetWorldPositionsProperty;
		SerializedProperty worldSpaceVisorDistanceProperty;
		SerializedProperty worldSpaceScaleCoefficientProperty;


		void OnEnable()
		{

            // Assign the reference to the object
			script = (HUDTargetTracking)target;
			
			// Assign all of the General Settings property references
			hudCameraProperty = serializedObject.FindProperty("UICamera");
			uiViewportCoefficientsProperty = serializedObject.FindProperty("UIViewportCoefficients");
			useMeshBoundsCenterProperty = serializedObject.FindProperty("useMeshBoundsCenter");
			enableAspectRatioProperty = serializedObject.FindProperty("enableAspectRatio");
			centerOffscreenArrowsProperty = serializedObject.FindProperty("centerOffscreenArrows");
			centerOffscreenArrowsRadiusProperty = serializedObject.FindProperty("centerOffscreenArrowsRadius");
			fadeMinAlphaProperty = serializedObject.FindProperty("fadeMinAlpha");
			fadeMaxAlphaProperty = serializedObject.FindProperty("fadeMaxAlpha");
			maxNewTargetsEachFrameProperty = serializedObject.FindProperty("maxNewTargetsEachFrame");
			expandingTargetBoxesProperty = serializedObject.FindProperty("expandingTargetBoxes");
            displayableTypesProperty = serializedObject.FindProperty("displayableTypes");


			// Assign all of the World Space Settings property references			
			useTargetWorldPositionsProperty = serializedObject.FindProperty("useTargetWorldPositions");
			worldSpaceVisorDistanceProperty = serializedObject.FindProperty("worldSpaceTargetTrackingDistance");
			worldSpaceScaleCoefficientProperty = serializedObject.FindProperty("worldSpaceScaleCoefficient");
			
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


            // Show General Settings in the inspector

            EditorGUILayout.BeginVertical("box");

			EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(hudCameraProperty);

			EditorGUILayout.PropertyField(uiViewportCoefficientsProperty);

			EditorGUILayout.PropertyField(useMeshBoundsCenterProperty);

			EditorGUILayout.PropertyField(enableAspectRatioProperty);

			EditorGUILayout.PropertyField(centerOffscreenArrowsProperty);
			
			EditorGUILayout.PropertyField(centerOffscreenArrowsRadiusProperty);
			
			EditorGUILayout.PropertyField(fadeMinAlphaProperty);
			
			EditorGUILayout.PropertyField(fadeMaxAlphaProperty);
	
			EditorGUILayout.PropertyField(maxNewTargetsEachFrameProperty);
	
			EditorGUILayout.PropertyField(expandingTargetBoxesProperty);

            EditorGUILayout.PropertyField(displayableTypesProperty, true);
			
			EditorGUILayout.EndVertical();


            // Show World Space Settings in the inspector

            EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField("World Space Settings", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(useTargetWorldPositionsProperty);
		
			EditorGUILayout.PropertyField(worldSpaceVisorDistanceProperty);

			EditorGUILayout.PropertyField(worldSpaceScaleCoefficientProperty);

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

				script.widgetSettingsByType[i].widgetPrefab = EditorGUILayout.ObjectField("Widget Prefab", script.widgetSettingsByType[i].widgetPrefab, typeof(GameObject),false) as GameObject;

				script.widgetSettingsByType[i].showOffScreenTargets = EditorGUILayout.Toggle("Show Off Screen Targets", script.widgetSettingsByType[i].showOffScreenTargets);
	
				script.widgetSettingsByType[i].fadeUnselectedByDistance = EditorGUILayout.Toggle("Fade By Distance", script.widgetSettingsByType[i].fadeUnselectedByDistance);


                // Visible elements

                script.widgetSettingsByType[i].showLabelField = EditorGUILayout.Toggle("Show Label Field", script.widgetSettingsByType[i].showLabelField);

				script.widgetSettingsByType[i].showValueField = EditorGUILayout.Toggle("Show Value Field", script.widgetSettingsByType[i].showValueField);

				script.widgetSettingsByType[i].showBarField = EditorGUILayout.Toggle("Show Bar Field", script.widgetSettingsByType[i].showBarField);
				
				EditorGUILayout.Space();
				EditorGUILayout.EndVertical();

			}

			// Apply modifications
			EditorUtility.SetDirty(script);
			serializedObject.ApplyModifiedProperties();
			
	    }
    }
}
