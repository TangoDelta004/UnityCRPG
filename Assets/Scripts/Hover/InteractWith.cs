using UnityEngine;
using UnityEngine.EventSystems;
using Movement;
using UnityEngine.Tilemaps;

namespace Hover {
    public class InteractWith : MonoBehaviour {

        public Camera cam;

        public GameObject openChestCursor;

        private void Update() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit) {
                if (hit.transform.tag == "Interactable") {

                }

            }

        }
    }

}