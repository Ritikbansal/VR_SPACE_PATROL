using UnityEngine;
using System.Collections;

namespace VSX.UniversalVehicleCombat
{

	/// <summary>
    /// This class implements player input for actions that are not related to a specific vehicle.
    /// </summary>
	public class PlayerGeneralControls : MonoBehaviour 
	{


		void Update()
		{
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
                {
                    GameStateManager.Instance.EnterGameState(GameState.PauseMenu);
                }
                else if (GameStateManager.Instance.CurrentGameState == GameState.PauseMenu)
                {
                    GameStateManager.Instance.EnterGameState(GameState.Gameplay);
                }
            }
		}
	}
}