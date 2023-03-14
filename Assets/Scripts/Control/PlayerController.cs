using UnityEngine;
using UnityEngine.EventSystems;
using Movement;
using UnityEngine.Tilemaps;

namespace Control {
    public class PlayerController : MonoBehaviour {

        public Camera cam;

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                if (EventSystem.current.IsPointerOverGameObject()) {
                    return;
                }
                MoveToCursor();
            }
        }

        private void MoveToCursor() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit) {
                if (hit.transform.tag == "Walkable") {
                    GetComponent<Mover>().MoveTo(hit.point);
                }

            }


        }
    }

}