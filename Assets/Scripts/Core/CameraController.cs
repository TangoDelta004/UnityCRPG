using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;
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
    public Vector3 playerOffset;

    public bool detatched;

    // Start is called before the first frame update
    void Start() {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update() {
        if (detatched) {
            HandleMovementInput();
            HandleZoom();
            HandleRotation();
        } else {
            transform.position = player.position + playerOffset;
            HandleZoom();
            HandleRotation();
        }
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

    void HandleMovementInput() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            newPosition += (transform.right * -movementSpeed);
        }
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }


}
