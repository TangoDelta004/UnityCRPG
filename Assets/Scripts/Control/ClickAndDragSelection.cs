using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClickAndDragSelection : MonoBehaviour
{
    public RectTransform selectionBox; // Assign your UI Image here
    public GameObject selectionRingPrefab; // Assign your selection ring prefab here
    private Vector2 startPos;
    private Vector2 endPos;
    private bool isDragging = false;
    private float dragThreshold = 30.0f; // Adjust this threshold as needed
    private float holdTime = 0.0f; // Time the mouse button has been held down
    private float holdTimeThreshold = 0.1f; // Time threshold to start dragging
    public List<GameObject> selectedPlayers = new List<GameObject>();

    void Start() {
        // Ensure the selection box is hidden at the start
        selectionBox.gameObject.SetActive(false);

        // Select all players at the start
        SelectAllPlayers();
    }

    void Update() {
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
                UpdateSelectionBox();
            }
        }

        // End click or drag
        if (Input.GetMouseButtonUp(0)) {
            if (isDragging) {
                isDragging = false;
                SelectCharacters();
                selectionBox.gameObject.SetActive(false);
            } else {
                // Check if clicked on a player character or selection ring
                CheckForCharacterClick();
            }
        }
    }

    void UpdateSelectionBox() {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        endPos = Input.mousePosition;

        Vector2 boxStart = startPos;
        Vector2 boxEnd = endPos;

        // Convert screen coordinates to Canvas local coordinates
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            selectionBox.parent as RectTransform, startPos, null, out boxStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            selectionBox.parent as RectTransform, endPos, null, out boxEnd);

        float width = boxEnd.x - boxStart.x;
        float height = boxEnd.y - boxStart.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = boxStart + (boxEnd - boxStart) / 2;
    }

    void SelectCharacters() {
        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        List<GameObject> tempSelectedPlayers = new List<GameObject>(); // Temporary list

        // Assuming characters are tagged as "Player"
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject character in characters) {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(character.transform.position);
            // Convert screenPos to Canvas local coordinates
            Vector2 characterPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                selectionBox.parent as RectTransform, screenPos, null, out characterPos);

            if (characterPos.x > min.x && characterPos.x < max.x && characterPos.y > min.y && characterPos.y < max.y) {
                // Character is within selection box
                tempSelectedPlayers.Add(character);
                Debug.Log("Selected: " + character.name);
            }
        }

        if (tempSelectedPlayers.Count > 0) {
            selectedPlayers = tempSelectedPlayers; // Only update if at least one character is selected
        }
    }

    void SelectAllPlayers() {
        // Assuming characters are tagged as "Player"
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject character in characters) {
            selectedPlayers.Add(character);
            Debug.Log("Initially selected: " + character.name);
        }
    }

    void CheckForCharacterClick() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            // Check if the clicked object is a selection ring
           if (hit.transform.CompareTag("SelectionRing")) {
                SelectSinglePlayer(hit.transform.parent.gameObject);
            }
        }
    }

    void SelectSinglePlayer(GameObject player) {
        selectedPlayers.Clear();
        selectedPlayers.Add(player);
    }
    
}
