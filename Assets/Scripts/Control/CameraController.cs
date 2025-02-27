using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control {
    public class CameraController : MonoBehaviour {

        public Transform cameraTransform;
        public float movementSpeed;
        public float movementTime;

        public Camera cam;

        public float minOrthoZoom;
        public float maxOrthoZoom;
        public float currentOrthoZoom;

        public Vector3 newPosition;

        private Vector3 lastMousePosition;

        public Vector3 offset;
        private Transform[] playerCharacters;

        // Start is called before the first frame update
        void Start() {
            newPosition = transform.position;
            FindAndFocusOnPlayer();
        }

        // Update is called once per frame
        void Update() {
            HandleMovementInput();
            HandleOrthographicZoom();
        }

        void HandleOrthographicZoom() {

            // camera zoom In
            if (Input.mouseScrollDelta.y > 0) {
                if (currentOrthoZoom > minOrthoZoom) {
                    currentOrthoZoom -= 0.5f;
                }
            }
            // camera zoom out
            if (Input.mouseScrollDelta.y < 0) {
                if (currentOrthoZoom < maxOrthoZoom) {
                    currentOrthoZoom += 0.5f;
                }
            }
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, currentOrthoZoom, Time.deltaTime * movementTime);
        }

        // Handle panning with middle mouse button
        void HandleMovementInput() {
            if (Input.GetMouseButtonDown(2)) {
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(2)) {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 cameraRight = cameraTransform.right;
                Vector3 cameraUp = cameraTransform.up;

                // Adjust movement directions
                newPosition += (-cameraRight * delta.x + -cameraUp * delta.y) * movementSpeed * Time.deltaTime;

                lastMousePosition = Input.mousePosition;
            }

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        }

        void FindAndFocusOnPlayer() {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

            if (playerObjects.Length > 0) {
                Transform firstPlayer = playerObjects[0].transform;
                transform.position = firstPlayer.position + offset;
                newPosition = transform.position;
            } else {
                Debug.LogWarning("No player characters found in the scene with the tag 'Player'");
            }
        }
    }
}
