using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control {
    public class CameraController : MonoBehaviour {

        public Transform cameraTransform;
        public float movementSpeed;
        public float movementTime;
        public float rotationAmount;
        public float minZoom;
        public float maxZoom;
        public Quaternion newRotation;
        public Vector3 newPosition;
        public Vector3 newZoom;
        public Vector3 zoomAmount;

        public Vector3 offset;

        // these exist to swap the camera to a particular player when a button is pressed

        public Transform currentPlayer;

        public bool detached;

        // Start is called before the first frame update
        void Start() {
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;
        }

        // Update is called once per frame
        void Update() {
            if (detached) {
                HandleMovementInput();
            } else {
                transform.position = currentPlayer.position + offset;
            }
            HandleZoom();
            //HandleRotation();
        }

        void HandleZoom() {
            // camera zoom In
            if (Input.mouseScrollDelta.y > 0) {
                if (maxZoom < newZoom.y) {
                    newZoom += zoomAmount;
                }
            }
            // camera zoom out
            if (Input.mouseScrollDelta.y < 0) {
                if (minZoom > newZoom.y) {
                    newZoom -= zoomAmount;
                }
            }
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }

        void HandleRotation() {
            // camera rotation if middle mouse is down
            if (Input.GetMouseButton(2)) {
                var axis = Input.GetAxis("Mouse X");
                newRotation *= Quaternion.Euler(Vector3.up * axis * rotationAmount);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        }

        // handle movement uses diagnals instead of cardinal movement, because the world is isometric
        void HandleMovementInput() {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                newPosition += (transform.forward * movementSpeed + transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                newPosition += (transform.forward * -movementSpeed + transform.right * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                newPosition += (transform.forward * -movementSpeed + transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                newPosition += (transform.forward * movementSpeed + transform.right * -movementSpeed);
            }
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        }

        void OnGUI() {
            CheckDetatch();
        }

        void CheckDetatch() {
            if (Event.current.Equals(Event.KeyboardEvent("Space"))) {
                detached = false;
            }
            if (Event.current.Equals(Event.KeyboardEvent("W")) || Event.current.Equals(Event.KeyboardEvent("A")) || Event.current.Equals(Event.KeyboardEvent("S")) || Event.current.Equals(Event.KeyboardEvent("D"))) {
                // if we are already detached, dont re-detach
                if (!detached) {
                    // reset camera position and newposition to the player so detaching happens on top of player and 
                    // not the last place you reattached from
                    transform.position = currentPlayer.position + offset;
                    newPosition = currentPlayer.position + offset;
                    detached = true;

                }
            }
        }
    }

}
