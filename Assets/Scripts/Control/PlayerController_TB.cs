using UnityEngine;
using UnityEngine.EventSystems;
using Movement;
using UnityEngine.AI;

namespace Control {

    public class PlayerController_TB : MonoBehaviour {

        public Camera cam;

        public Transform player;

        private LineRenderer lr;

        public NavMeshAgent nav;

        void Start() {

            lr = GetComponent<LineRenderer>();

            lr.startWidth = 0.15f;
            lr.endWidth = 0.15f;
            lr.positionCount = 0;
        }

        private void Update() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit) {
                float distanceInFeet = Vector3.Distance(player.transform.position, hit.point) * 3;
                //Debug.Log(distanceInFeet);
                DrawPath();
            }
        }

        private void MoveToCursor() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit) {
                GetComponent<Mover>().MoveTo(hit.point);
            }
        }

        void DrawPath() {
            lr.positionCount = nav.path.corners.Length;
            lr.SetPosition(0, transform.position);
            if (nav.path.corners.Length < 2) {
                return;
            }
            for (int i = 1; i < nav.path.corners.Length; i++) {
                Vector3 pointPos = new Vector3(nav.path.corners[i].x, nav.path.corners[i].y, nav.path.corners[i].z);
                lr.SetPosition(i, pointPos);
            }
        }
    }
}