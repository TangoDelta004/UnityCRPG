using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Movement;
using UnityEngine.AI;

namespace Control {

    public class GridBehaviour : MonoBehaviour {

        public bool findDistance = false;
        public int rows = 10;
        public int columns = 10;

        public int scale = 1;
        public GameObject gridPrefab;
        public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
        public GameObject[,] gridArray;
        public int startX = 0;
        public int startY = 0;
        public int endX = 0;
        public int endY = 0;

        public List<GameObject> path = new List<GameObject>();

        void Awake() {

            gridArray = new GameObject[columns, rows];

            if (gridPrefab) {
                GenerateGrid();
            } else {
                print("missing gridprefab");
            }
        }

        void Update() {
            if (findDistance) {
                InitialSetup();
                SetPath();
                findDistance = false;
            }
        }


        // generates a grid of size specified
        void GenerateGrid() {
            for (int i = 0; i < columns; i++) {
                for (int j = 0; j < rows; j++) {
                    GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y + scale * j, leftBottomLocation.z + scale * j), Quaternion.identity);
                    obj.transform.SetParent(gameObject.transform);
                    obj.name = "GridSpace" + i + j;
                    obj.GetComponent<GridStat>().x = i;
                    obj.GetComponent<GridStat>().y = j;
                    gridArray[i, j] = obj;
                }
            }
        }

        void SetPath() {
            int step;
            int x = endX;
            int y = endY;

            List<GameObject> tempList = new List<GameObject>();
            path.Clear();
            if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStat>().visited > 0) {
                path.Add(gridArray[x, y]);
                step = gridArray[x, y].GetComponent<GridStat>().visited - 1;
            } else {
                print("cant reach desired location");
                return;
            }
            for (int i = step; step > -1; step--) {
                if (TestDirection(x, y, step, 1)) {
                    tempList.Add(gridArray[x, y + 1]);
                }
                if (TestDirection(x, y, step, 2)) {
                    tempList.Add(gridArray[x + 1, y]);
                }
                if (TestDirection(x, y, step, 3)) {
                    tempList.Add(gridArray[x, y - 1]);
                }
                if (TestDirection(x, y, step, 4)) {
                    tempList.Add(gridArray[x - 1, y]);
                }
                GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
                path.Add(tempObj);
                x = tempObj.GetComponent<GridStat>().x;
                y = tempObj.GetComponent<GridStat>().y;
                tempList.Clear();
            }


        }

        // marks all nodes in grid with numbers based on possible paths from the start
        void InitialSetup() {
            foreach (GameObject obj in gridArray) {
                obj.GetComponent<GridStat>().visited = -1;
            }
            gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
            for (int step = 1; step < rows * columns; step++) {
                foreach (GameObject obj in gridArray) {
                    if (obj && obj.GetComponent<GridStat>().visited == step - 1) {
                        TestFourDirections(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);

                    }
                }
            }
        }

        bool TestDirection(int x, int y, int step, int direction) {
            // 1 is up, 2 is right, 3 is down, 4 is left
            switch (direction) {
                case 1:
                    if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStat>().visited == step) {
                        return true;
                    } else {
                        return false;
                    }
                case 2:
                    if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStat>().visited == step) {
                        return true;
                    } else {
                        return false;
                    }
                case 3:
                    if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStat>().visited == step) {
                        return true;
                    } else {
                        return false;
                    }
                case 4:
                    if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStat>().visited == step) {
                        return true;
                    } else {
                        return false;
                    }
            }
            return false;
        }

        void TestFourDirections(int x, int y, int step) {
            if (TestDirection(x, y, -1, 1)) {
                SetVisited(x, y + 1, step);
            }
            if (TestDirection(x, y, -1, 2)) {
                SetVisited(x + 1, y, step);
            }
            if (TestDirection(x, y, -1, 3)) {
                SetVisited(x, y - 1, step);
            }
            if (TestDirection(x, y, -1, 4)) {
                SetVisited(x - 1, y, step);
            }
        }


        void SetVisited(int x, int y, int step) {
            if (gridArray[x, y]) {
                gridArray[x, y].GetComponent<GridStat>().visited = step;
            }
        }

        GameObject FindClosest(Transform targetLocation, List<GameObject> list) {
            float currentDistance = scale * rows * columns;
            int indexNumber = 0;
            for (int i = 0; i < list.Count; i++) {
                if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance) {
                    currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                    indexNumber = i;
                }
            }
            return list[indexNumber];
        }

    }
}