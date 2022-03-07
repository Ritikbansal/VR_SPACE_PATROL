using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// This class is used to store information about an instance of a rumble.
    /// </summary>
    public class Rumble
    {

        // The strength of the rumble
        public float maxLevel;

        // How long it takes to reach maximum strength
        public float attackTime;

        // How long it holds at maximum strength
        public float sustainTime;

        // How long it takes to fade away
        public float decayTime;

        // The time that the rumble began
        public float startTime;

        public Rumble(float maxLevel, float attackTime, float sustainTime, float decayTime)
        {

            this.maxLevel = maxLevel;

            this.attackTime = attackTime;
            this.sustainTime = sustainTime;
            this.decayTime = decayTime;

            this.startTime = Time.time;
        }

    }


    /// <summary>
    /// This class provides a way the create 'rumbles' based on things like being hit, colliding, and boosting.
    /// These rumbles can be used to drive camera shaking or controller vibration.
    /// </summary>
    public class RumbleManager : MonoBehaviour
    {

        [SerializeField]
        private float boostRumbleStrength = 0.0085f;

        [SerializeField]
        private float deathRumbleStrength;

        [SerializeField]
        private float deathRumbleAttackTime;

        [SerializeField]
        private float deathRumbleSustainTime;

        [SerializeField]
        private float deathRumbleFadeTime;

        float currentSingleRumbleStrength = 0;
        List<Rumble> rumbles = new List<Rumble>();

        float currentLevel = 0;
        public float CurrentLevel { get { return currentLevel; } }

        [SerializeField]
        private float damageToRumbleCoefficient;

        Vehicle focusedVehicle;



        private void Start()
        {
            // Subscribe to the event of a new vehicle becoming focused
            UVCEventManager.Instance.StartListening(UVCEventType.OnFocusedVehicleChanged, OnFocusedVehicleChanged);
        }


        /// <summary>
        /// Add a new rumble.
        /// </summary>
        /// <param name="strength">The shake strength.</param>
        /// <param name="attackTime">How fast the camera shake builds up.</param>
        /// <param name="sustainTime">How long the shake sustains at maximum strength.</param>
        /// <param name="decayTime">How long the shake takes to decay/disappear.</param>
        public void AddRumble(float maxLevel, float attackTime, float sustainTime, float decayTime)
        {

            Rumble newRumble = new Rumble(maxLevel, attackTime, sustainTime, decayTime);
            rumbles.Add(newRumble);

        }


        /// <summary>
        /// Event called when a new vehicle comes into focus by the game.
        /// </summary>
        /// <param name="newVehicle"></param>
        void OnFocusedVehicleChanged(Vehicle newVehicle)
        {
            // Un-subscribe from the damage event on the previous vehicle
            if (focusedVehicle != null && focusedVehicle.HasHealth)
            {
                focusedVehicle.Health.damageEventHandler -= OnVehicleDamaged;
            }

            // Set the new vehicle
            focusedVehicle = newVehicle;

            // Subscribe to the damage event on the new vehicle
            if (newVehicle != null && newVehicle.HasHealth)
            {
                newVehicle.Health.damageEventHandler += OnVehicleDamaged;
            }
        }

        /// <summary>
        /// Event called when the vehicle that this component is currently focused on receives damage.
        /// </summary>
        /// <param name="damage">The amount of damage.</param>
        /// <param name="hitPosition">The world space hit position.</param>
        /// <param name="attacker">The attacker (null if collision).</param>
        void OnVehicleDamaged(float damage, Vector3 hitPosition, GameAgent attacker)
        {

            // Calculate the level for the rumble
            float level = Mathf.Clamp(damage * damageToRumbleCoefficient, 0f, 1f);

            // Add a new rumble
            AddRumble(level, 0f, 0.15f, 0.25f);

        }

        /// <summary>
        /// Event called when a vehicle in the game is destroyed.
        /// </summary>
        /// <param name="destroyedVehicle">The vehicle that has been destroyed</param>
        /// <param name="attacker">The attacker that destroyed the vehicle.</param>
        private void OnVehicleDestroyed(Vehicle destroyedVehicle, GameAgent attacker)
        {
            // If this is the focused vehicle
            if (destroyedVehicle == focusedVehicle)
            {
                // Add a death rumble
                AddRumble(deathRumbleStrength, deathRumbleAttackTime, deathRumbleSustainTime, deathRumbleFadeTime);
            }
        }
        

        void Update()
        {
            
            // If the focused vehicle has engines, add a rumble for boosting
            if (focusedVehicle != null && focusedVehicle.ActivationState == VehicleActivationState.ActiveInScene && focusedVehicle.HasEngines)
            {
                // If boost is on, rumble
                currentSingleRumbleStrength = Mathf.Max(currentSingleRumbleStrength, 
                    focusedVehicle.Engines.GetCurrentBoostValues().z * boostRumbleStrength);

            }
           
            // Update the current combined rumble level for this manager
            currentLevel = 0;
            for (int i = 0; i < rumbles.Count; ++i)
            {

                float timeSinceStart = Time.time - rumbles[i].startTime;

                if (timeSinceStart > (rumbles[i].attackTime + rumbles[i].sustainTime + rumbles[i].decayTime))
                {
                    rumbles.RemoveAt(i);
                    i--;
                    continue;
                }

                float level = 0;

                // Get the current level of the rumble based on its current state and parameters
                if (timeSinceStart < rumbles[i].attackTime)
                {
                    level = timeSinceStart / rumbles[i].attackTime;
                }
                else if (timeSinceStart < rumbles[i].attackTime + rumbles[i].sustainTime)
                {
                    level = 1;
                }
                else
                {
                    float timeSinceBeganDecay = timeSinceStart - rumbles[i].attackTime - rumbles[i].sustainTime;
                    level = Mathf.Clamp(1 - timeSinceBeganDecay / rumbles[i].decayTime, 0f, 1f);
                }
                currentLevel = Mathf.Max(currentLevel, level * rumbles[i].maxLevel);
            }

            // Update the current level with the single shake level
            currentLevel = Mathf.Max(currentLevel, currentSingleRumbleStrength);

            // Reset the single shake level
            currentSingleRumbleStrength = 0;

        }
    }
}
