using UnityEngine;

namespace Control {
    public class Pause : MonoBehaviour {

        private bool isPaused = false;

        void Update() {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TogglePause();
            }
        }

        private void TogglePause() {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0;
                // Optionally, display a pause menu or visual indicator here
            }
            else
            {
                Time.timeScale = 1;
                // Optionally, hide the pause menu or visual indicator here
            }
        }
    }
}