using UnityEngine;
using UnityEngine.EventSystems;
using Movement;
using UnityEngine.Tilemaps;

namespace Hover {
    public class GridSpace : MonoBehaviour {

        public Camera cam;

        public Tilemap map;

        public MeshRenderer[] children;

        public GameObject previousTile;

        private void Update() {
            var focusedTileHit = GetFocusedOnTile();
            if (focusedTileHit.HasValue) {
                GameObject tile = focusedTileHit.Value.collider.gameObject;
                if (previousTile != null && previousTile != tile) {
                    children = previousTile.transform.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer child in children) {
                        if (child.tag == "Highlight") {
                            child.enabled = false;
                        }
                    }
                }
                previousTile = tile;
                children = tile.transform.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer child in children) {
                    if (child.tag == "Highlight") {
                        child.enabled = true;
                    }
                }
            }

        }

        public RaycastHit? GetFocusedOnTile() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit) {
                Vector3Int gridPos = map.WorldToCell(hit.point);
                Ray ray2 = cam.ScreenPointToRay(gridPos);
                RaycastHit[] hits;
                hits = Physics.RaycastAll(cam.ScreenPointToRay(Input.mousePosition));
                foreach (RaycastHit thing in hits) {
                    if (thing.transform.tag == "Walkable") {
                        return thing;
                    }
                }
            }

            return null;
        }

        private void OnMouseExit() {

        }
    }

}