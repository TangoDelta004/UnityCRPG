using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace control {
    public class MouseController_TB : MonoBehaviour {
        public GameObject cursor;
        public Camera cam;


        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void LateUpdate() {
            RaycastHit? hit = CastRay();
            if (hit.HasValue) {
                GameObject overlayTile = hit.Value.collider.gameObject;
                if (hit.Value.collider.tag == "Tile") {
                    cursor.transform.position = overlayTile.transform.position;
                    gameObject.GetComponentInChildren<MeshRenderer>().sortingOrder = overlayTile.GetComponentInChildren<MeshRenderer>().sortingOrder;
                } else {
                    cursor.transform.position = overlayTile.transform.position;
                }

                if (Input.GetMouseButtonDown(0)) {
                    //overlayTile.GetComponent<MeshRenderer>().GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                }
            }
        }
        public RaycastHit? CastRay() {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit) {
                return hit;
            }
            return null;
        }
    }
}