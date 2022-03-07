using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using VSX.UniversalVehicleCombat;


namespace VSX.UniversalVehicleCombat
{

    /// <summary>
    /// This class provides a way for the user to set parameters for each of the game states.
    /// </summary>
    [System.Serializable]
    public class GameStateParameters
    {
        // The game state that these parameters refer to
        public GameState gameState;

        // Whether time should be frozen when the game enters this state
        public bool freezeTimeOnEntry;

        // How long to pause before freezing time
        public float pauseBeforeTimeFreeze = 0;

    }

    /// <summary>
    /// An enum for all the different states that the game can be in. Used to manage input, camera etc
    /// </summary>
    public enum GameState
    {
        Gameplay,
        PauseMenu,
        GameOver,
        PowerManagementMenu,
        TriggerGroupsMenu,
        CockpitMenu,
        Loadout,
        ControlsMenu,
        MainMenu
    }

    // Delegate to attach functions to be called when the game enters a new state
    public delegate void OnGameStateChangedEventHandler(GameState newGameState);


    /// <summary>
    /// This class provides a single location to store the current state of the game.
    /// </summary>
    public class GameStateManager : MonoBehaviour
    {

        [SerializeField]
        private GameState startingGameState;

        protected GameState currentGameState;
        public GameState CurrentGameState { get { return currentGameState; } }

        // A list that stores the parameters associated with each of the game state
        public List<GameStateParameters> gameStateParameters = new List<GameStateParameters>();

        // Attach functions to be called when the game enters a new state 
        public OnGameStateChangedEventHandler onGameStateChangedEventHandler;

        // The singleton instance for this component
        public static GameStateManager Instance;

        private bool pauseTime = false;



        private void Awake()
        {
            // Enforce the singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }  
        }


        protected virtual void Start()
        {
            EnterGameState(startingGameState);
        }


        /// <summary>
        /// Called to make the game enter a new game state.
        /// </summary>
        /// <param name="newGameStateString">The new game state, in string form.</param>
        public void EnterGameState(string newGameStateString)
        {
            // Parse the string to the enum value
            GameState newGameState = (GameState)Enum.Parse(typeof(GameState), newGameStateString);
            
            // Enter the new game state
            EnterGameState(newGameState);
        }

        /// <summary>
        /// Called to make the game enter a new game state.
        /// </summary>
        /// <param name="newGameState">The new game state.</param>
        public void EnterGameState(GameState newGameState)
        {

            // Update the game state
            currentGameState = newGameState;
            
            // Freeze time if applicable
            if (gameStateParameters[(int)newGameState].freezeTimeOnEntry)
            {
                pauseTime = true;
                StartCoroutine(WaitForTimePause(gameStateParameters[(int)newGameState].pauseBeforeTimeFreeze));
            }
            else
            {
                pauseTime = false;
                Time.timeScale = 1;
            }
            
            // Call the event 
            if (onGameStateChangedEventHandler != null) onGameStateChangedEventHandler(currentGameState);
        }

        IEnumerator WaitForTimePause(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            if (pauseTime)
            {
                Time.timeScale = 0;
            }
        }
    }
}
