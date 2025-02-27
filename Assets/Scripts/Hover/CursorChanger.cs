using UnityEngine;

namespace Hover {
    public class InteractableCursorChanger : MonoBehaviour {
        public Texture2D interactableCursor; // The cursor texture to use when hovering over an interactable object
        public Texture2D defaultCursor; // The default cursor texture

        // Update is called once per frame
        void Update() {
            // Perform a raycast from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                // Check if the object hit by the raycast has the Interactable tag
                if (hit.collider.CompareTag("Interactable")) {
                    // Change the cursor to the interactable cursor
                    Cursor.SetCursor(interactableCursor, Vector2.zero, CursorMode.Auto);
                }
                else {
                    // Revert to the default cursor
                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                }
            }
            else {
                // Revert to the default cursor if no object is hit
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            }
        }
    }
}