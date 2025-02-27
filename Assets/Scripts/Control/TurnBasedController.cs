using UnityEngine;

namespace Control {
    public class TurnBasedController : MonoBehaviour {
        private bool isTurnBasedMode = false;
        private PlayerController playerController;

        void Start() {
            playerController = GetComponent<PlayerController>();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleTurnBasedMode();
            }
        }

        void ToggleTurnBasedMode() {
            isTurnBasedMode = !isTurnBasedMode;

            if (isTurnBasedMode)
            {
                // Turn on turn-based mode
                playerController.enabled = false; // Disable normal player controls
                StartTurnBasedMode();
            }
            else
            {
                // Turn off turn-based mode
                playerController.enabled = true; // Enable normal player controls
                EndTurnBasedMode();
            }
        }

        void StartTurnBasedMode() {
            // Initialize turn-based mode logic here
            Debug.Log("Turn-based mode started.");
            // Implement your turn-based logic here
        }

        void EndTurnBasedMode() {
            // End turn-based mode logic here
            Debug.Log("Turn-based mode ended.");
            // Implement any cleanup logic here
        }
    }
}