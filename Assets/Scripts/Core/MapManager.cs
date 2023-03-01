using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace core {
    public class MapManager : MonoBehaviour {
        private static MapManager _instance;
        public static MapManager Instance { get { return _instance; } }

        public GameObject overlayPrefab;
        public GameObject overlayContainer;

        public Dictionary<Vector2Int, GameObject> map;
        public bool ignoreBottomTiles;

        public Tilemap tileMap;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
        }

        // Start is called before the first frame update
        void Start() {
            map = new Dictionary<Vector2Int, GameObject>();
            BoundsInt bounds = tileMap.cellBounds;
            Debug.Log(bounds);
            for (int x = bounds.min.x; x < bounds.max.x; x++) {
                for (int y = bounds.min.y; y < bounds.max.y; y++) {
                    for (int z = bounds.min.z; z < bounds.max.z; z++) {
                        var tileLocation = new Vector3Int(x, y, z);
                        var tileKey = new Vector2Int(x, y);
                        Debug.Log(tileKey);
                        if (!map.ContainsKey(tileKey)) {
                            Debug.Log(tileKey);
                            var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                            var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);
                            overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z);
                            overlayTile.GetComponent<MeshRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                            map.Add(tileKey, overlayTile);
                        }
                    }
                }
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }
}