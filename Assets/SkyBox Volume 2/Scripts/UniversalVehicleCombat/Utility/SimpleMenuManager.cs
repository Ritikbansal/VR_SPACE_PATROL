using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// This class provides a simple way to enable and disable a set of UI objects, and set the first UI element selection,
    /// when the game enters a specified game state.
    /// </summary>
    public class SimpleMenuManager : MonoBehaviour
    {

        [SerializeField]
        private GameState gameState;

        [SerializeField]
        private List<GameObject> UIObjects = new List<GameObject>();

        [SerializeField]
        private GameObject firstSelected;
        bool waitingForHighlight = false;

        [SerializeField]
        private float pauseBeforeActivation;

        

        void Awake()
        {
            GameStateManager.Instance.onGameStateChangedEventHandler += OnGameStateChanged;
        }


        // Event called when the game enters a new game state
        void OnGameStateChanged(GameState newGameState)
        {
            // If the game enters the game state this manager refers to, activate all UI
            if (newGameState == gameState)
            {
                StartCoroutine(WaitForActivation(pauseBeforeActivation));
            }
            // If the game state is not the one this manager refers to, disable all UI
            else
            {
                for (int i = 0; i < UIObjects.Count; ++i)
                {
                    UIObjects[i].SetActive(false);
                }

                waitingForHighlight = false;
            }
        }

        IEnumerator WaitForActivation(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            if (GameStateManager.Instance.CurrentGameState == gameState)
            {
                for (int i = 0; i < UIObjects.Count; ++i)
                {
                    UIObjects[i].SetActive(true);
                }

                if (firstSelected != null)
                {
                    // When the menu activates, flag the first item to be selected, and clear the currently selected item.
                    // The new selected gameobject must be selected in OnGUI.
                    EventSystem.current.SetSelectedGameObject(null);
                    waitingForHighlight = true;
                }
            }
        }


        // Called when the UI is updated
        private void OnGUI()
        {
            // If the flag is still up, highlight the first button
            if (waitingForHighlight)
            {
                // Highlight the first button
                EventSystem.current.SetSelectedGameObject(firstSelected);
                
                // Reset the flag
                waitingForHighlight = false;
            }
        }
    }
}