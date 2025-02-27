using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Control {
    public class PlayerController : MonoBehaviour {

        public Camera cam;
        private Vector2 startPos;
        private bool isDragging = false;
        private float dragThreshold = 30.0f;
        private float holdTime = 0.0f;
        private float holdTimeThreshold = 0.1f;
        private float unitSpacing = 2.0f;
        private float minSpeed = 9.0f; // Define a minimum speed threshold


        private void Update() {
            // Start click
            if (Input.GetMouseButtonDown(0)) {
                startPos = Input.mousePosition;
                holdTime = 0.0f; // Reset hold time
            }

            // Detect mouse movement and initiate drag
            if (Input.GetMouseButton(0)) {
                holdTime += Time.deltaTime; // Increment hold time

                float distance = Vector2.Distance(startPos, Input.mousePosition);
                if (distance > dragThreshold && holdTime > holdTimeThreshold) {
                    isDragging = true;
                }
            }

            // End click or drag
            if (Input.GetMouseButtonUp(0)) {
                if (!isDragging) {
                    MoveToCursor();
                }
                isDragging = false;
            }
        }

        public void MoveToPosition(Vector3 position) {
            GetComponent<Mover>().MoveTo(position);
        }

        private void MoveToCursor() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit) {
                if (hit.transform.tag == "Walkable") {
                    ClickAndDragSelection selection = FindObjectOfType<ClickAndDragSelection>();
                    if (selection != null && selection.selectedPlayers.Count > 0) {
                        MoveSelectedPlayers(hit.point, selection.selectedPlayers);
                    } else {
                        GetComponent<Mover>().MoveTo(hit.point);
                    }
                }
            }
        }

public void MoveSelectedPlayers(Vector3 targetPosition, List<GameObject> selectedPlayers) {
    if (selectedPlayers.Count == 0) return;

    // Move the first player to the target position
    Vector3 leaderStartPosition = selectedPlayers[0].transform.position;
    selectedPlayers[0].GetComponent<Mover>().MoveTo(targetPosition);

    // Calculate the direction from which the leader moved
    Vector3 moveDirection = (targetPosition - leaderStartPosition).normalized;

    // Define offsets for the remaining players in a grid pattern behind the leader
    List<Vector3> offsets = new List<Vector3>();
    for (int i = 1; i < selectedPlayers.Count; i++) {
        int row = (i - 1) / 3;
        int column = (i - 1) % 3;
        Vector3 offset = new Vector3((column - 1) * unitSpacing, 0, -(row + 1) * unitSpacing);
        offsets.Add(offset);
    }

    // Adjust offsets to be relative to the move direction
    for (int i = 0; i < offsets.Count; i++) {
        offsets[i] = Quaternion.LookRotation(moveDirection) * offsets[i];
    }

    // Calculate the maximum distance any player has to travel
    float maxDistance = Vector3.Distance(targetPosition, selectedPlayers[0].transform.position);
    for (int i = 1; i < selectedPlayers.Count; i++) {
        maxDistance = Mathf.Max(maxDistance, Vector3.Distance(targetPosition + offsets[i - 1], selectedPlayers[i].transform.position));
    }

    // Move the remaining players and adjust their speed
    for (int i = 1; i < selectedPlayers.Count; i++) {
        Vector3 newPosition = targetPosition + offsets[i - 1];
        float distance = Vector3.Distance(newPosition, selectedPlayers[i].transform.position);

        // Adjust speed so that all players arrive at the same time with a minimum speed threshold
        float speed = Mathf.Max(minSpeed, distance / maxDistance * selectedPlayers[0].GetComponent<UnityEngine.AI.NavMeshAgent>().speed);
        selectedPlayers[i].GetComponent<Mover>().SetSpeed(speed);
        selectedPlayers[i].GetComponent<Mover>().MoveTo(newPosition);
    }
}




    }
}
