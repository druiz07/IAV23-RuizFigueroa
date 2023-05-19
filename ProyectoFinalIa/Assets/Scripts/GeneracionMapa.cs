using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneracionMapa : Graph
{


    //DatosMapa
    [SerializeField]
    private int mapWidth;  // The width of the map grid
    [SerializeField]
    private int mapHeight; // The height of the map grid

    private int index; // Current index in the grid
    private int nextIndex; // Next index in the grid

    float defaultCost = 1f; // Default cost for edges

    bool[,] vertexMap; // 2D array to track vertices

    private bool x = false; // Flag for horizontal movement
    private bool y = false; // Flag for vertical movement

    // Lists
    private List<GameObject> pathCells = new List<GameObject>(); // List of path cells
    private List<GameObject> secondPathCells = new List<GameObject>(); // List of cells for a second path
    private List<GameObject> mapCells = new List<GameObject>(); // List of all map cells
    private List<GameObject> edgeCells = new List<GameObject>(); // List of edge cells

    public List<GameObject> corners = new List<GameObject>(); // List of corner cells
    public List<GameObject> corners2 = new List<GameObject>(); // List of corner cells for the second path
    public List<GameObject> cellsPrefabs; // List of cell prefabs
    public List<GameObject> decorationPrefabs = new List<GameObject>(); // List of decoration prefabs

    // Game Objects
    private GameObject actualCell; // Current cell
    private GameObject startingCell; // Starting cell
    private GameObject exitCell; // Exit cell
    private GameObject pathCell; // Path cell
    private GameObject previous; // Previous cell
    private GameObject[] vertexObjs; // Array of vertex game objects

    public GameObject edgeCellPrefab; // Prefab for edge cells
    public GameObject pathPrefab; // Prefab for path cells
    public GameObject startPathPrefab; // Prefab for starting path cells
    public GameObject exitPrefab; // Prefab for exit cell
    public GameObject spawnerPrefab; // Prefab for spawner cell
    public GameObject castlePrefab; // Prefab for castle cell


    private List<GameObject> GetTopBorder()
    {
        List<GameObject> casillasBorde = new List<GameObject>();

        for (int x = mapWidth * (mapHeight - 1); x < mapWidth * mapHeight; x++)
        {
            casillasBorde.Add(mapCells[x]);
        }

        return casillasBorde;
    }

    private List<GameObject> GetBottomBorder()
    {
        List<GameObject> casillasBorde = new List<GameObject>();

        for (int x = 0; x < mapWidth; x++)
        {
            casillasBorde.Add(mapCells[x]);
        }

        return casillasBorde;
    }

    private List<GameObject> GetLeftBorder()
    {
        List<GameObject> casillasBorde = new List<GameObject>();
        int x = 0;
        while (x < mapWidth * mapHeight)
        {
            casillasBorde.Add(mapCells[x]);
            x = x + mapWidth;
        }
        return casillasBorde;
    }

    private List<GameObject> GetRightBorder()
    {

        List<GameObject> casillasBorde = new List<GameObject>();
        int x = mapWidth - 1;
        while (x < mapWidth * mapHeight)
        {
            casillasBorde.Add(mapCells[x]);
            x = x + mapWidth;
        }
        return casillasBorde;
    }

    public override void Load()
    {
        GenerateMap();
    }

    //MAP 
    private void GenerateMap()
    {
        int id = 0;

        vertex = new Vertex[mapHeight * mapWidth];
        neighbors = new List<List<Vertex>>(mapHeight * mapWidth);
        costs = new List<List<float>>(mapHeight * mapWidth);
        vertexObjs = new GameObject[mapHeight * mapWidth];
        vertexMap = new bool[mapHeight, mapWidth];

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                id = GridToId(j, i);
                // Instantiate a new cell prefab
                int cellRandom = UnityEngine.Random.Range(0, cellsPrefabs.Count);
                vertexObjs[id] = Instantiate(cellsPrefabs[cellRandom]);
                int decorationRandom = UnityEngine.Random.Range(0, decorationPrefabs.Count);
                int createObjectProbability = UnityEngine.Random.Range(0, 10);
                if (createObjectProbability < 2)
                {
                    Instantiate(decorationPrefabs[decorationRandom], vertexObjs[id].transform);
                }
                
                mapCells.Add(vertexObjs[id]);
                vertexObjs[id].transform.position = new Vector3(i, -0.5f, j);
                Vertex v = vertexObjs[id].AddComponent<Vertex>();
                v.id = id;
                vertex[id] = v;
                neighbors.Add(new List<Vertex>());
                costs.Add(new List<float>());
            }
        }

        List<GameObject> upCells = GetTopBorder();
        List<GameObject> downCells = GetBottomBorder();
        List<GameObject> leftCells = GetLeftBorder();
        List<GameObject> rigthCells = GetRightBorder();

        GameObject init;
        GameObject final;

        int rand1 = UnityEngine.Random.Range(0, mapWidth);
        int rand2 = UnityEngine.Random.Range(0, mapHeight);

        int mapType = UnityEngine.Random.Range(0, 2);
        if (mapType == 0)
        {
            init = upCells[rand1];
            final = downCells[rand2];
        }
        else
        {
            init = rigthCells[rand1];
            final = leftCells[rand2];
        }



        actualCell = init;
        //Si empieza desde la izquierda, se mueve hacia la derecha, sino hacia abajo
        if (mapType == 0)
            moveDown();
        else moveLeft();

        while (!x || !y)
        {
            int randomNumber = UnityEngine.Random.Range(0, 2);
            if (!x && randomNumber == 0)
            {
                if (actualCell.transform.position.z > final.transform.position.z)
                    moveLeft();
                else if (actualCell.transform.position.z < final.transform.position.z)
                    moveRight();
                else x = true;
            }
            else if (!y && randomNumber == 1)
            {
                if (actualCell.transform.position.x > final.transform.position.x)
                    moveDown();
                else if (actualCell.transform.position.x < final.transform.position.x)
                    moveUp();
                else y = true;
            }
        }
        pathCells.Add(final);

        x = false;
        y = false;
        actualCell = init;

        if (mapType == 0) moveDown2();
        else moveLeft2();

        while (!x)
        {
            if (actualCell.transform.position.z > final.transform.position.z)
                moveLeft2();
            else if (actualCell.transform.position.z < final.transform.position.z)
                moveRight2();   
            else x = true;
        }
        while (!y)
        {
            if (actualCell.transform.position.x > final.transform.position.x)
                moveDown2();
            else if (actualCell.transform.position.x < final.transform.position.x)
                moveUp2();
            else y = true;
        }
        secondPathCells.Add(final);

        //Cambia la malla del mapa para hacerla camino
        int index = 0;
        foreach (GameObject obj in pathCells)
        {
            if (obj == pathCells[0])
            {
                vertexMap[(int)pathCells[index].transform.position.x, (int)pathCells[index].transform.position.z] = true;

                Vector3 pos = pathCells[index].transform.position;
                Destroy(pathCells[index]);
                int b = GridToId((int)pos.z, (int)pos.x);
                vertexObjs[b] = Instantiate(startPathPrefab);
                startingCell = vertexObjs[b];
                vertexObjs[b].transform.position = pos;
                Vertex v = vertexObjs[b].AddComponent<Vertex>();
                v.id = b;
                vertex[b] = v;
                mapCells.Add(startingCell);
                startingCell.transform.position = pos;

                Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                Instantiate(spawnerPrefab, a, transform.rotation);
            }
            else if (obj == pathCells[pathCells.Count - 1])
            {
                vertexMap[(int)pathCells[index].transform.position.x, (int)pathCells[index].transform.position.z] = true;

                Vector3 pos = pathCells[index].transform.position;
                Destroy(pathCells[index]);
                int b = GridToId((int)pos.z, (int)pos.x);
                vertexObjs[b] = Instantiate(exitPrefab);
                exitCell = vertexObjs[b];
                vertexObjs[b].transform.position = pos;
                Vertex v = vertexObjs[b].AddComponent<Vertex>();
                v.id = b;
                vertex[b] = v;
                mapCells.Add(exitCell);
                exitCell.transform.position = pos;

                Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                Instantiate(castlePrefab, a, transform.rotation);
            }
            else
            {
                vertexMap[(int)pathCells[index].transform.position.x, (int)pathCells[index].transform.position.z] = true;

                Vector3 pos = pathCells[index].transform.position;
                Destroy(pathCells[index]);
                int b = GridToId((int)pos.z, (int)pos.x);
                vertexObjs[b] = Instantiate(pathPrefab);
                pathCell = vertexObjs[b];
                vertexObjs[b].transform.position = pos;
                Vertex v = vertexObjs[b].AddComponent<Vertex>();
                v.id = b;
                vertex[b] = v;
                mapCells.Add(pathCell);
                pathCell.transform.position = pos;
            }
            if(index< pathCells.Count-1)index++;
        }

        index = 0;
        foreach (GameObject obj in secondPathCells)
        {
            if (obj == secondPathCells[0])
            {
                if (!vertexMap[(int)secondPathCells[index].transform.position.x, (int)secondPathCells[index].transform.position.z])
                {
                    vertexMap[(int)secondPathCells[index].transform.position.x, (int)secondPathCells[index].transform.position.z] = true;

                    Vector3 pos = secondPathCells[index].transform.position;
                    Destroy(secondPathCells[index]);
                    startingCell = Instantiate(startPathPrefab);
                    mapCells.Add(startingCell);
                    startingCell.transform.position = pos;

                    Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                }

            }
            else if (obj == secondPathCells[secondPathCells.Count - 1])
                if (!vertexMap[(int)secondPathCells[index].transform.position.x, (int)secondPathCells[index].transform.position.z])
                    vertexMap[(int)secondPathCells[index].transform.position.x, (int)secondPathCells[index].transform.position.z] = true;
            else
            {
                if (!vertexMap[(int)secondPathCells[index].transform.position.x, (int)secondPathCells[index].transform.position.z])
                {
                    vertexMap[(int)secondPathCells[index].transform.position.x, (int)secondPathCells[index].transform.position.z] = true;

                    Vector3 pos = secondPathCells[index].transform.position;
                    Destroy(secondPathCells[index]);
                    int b = GridToId((int)pos.z, (int)pos.x);
                    vertexObjs[b] = Instantiate(pathPrefab);
                    pathCell = vertexObjs[b];
                    vertexObjs[b].transform.position = pos;
                    Vertex v = vertexObjs[b].AddComponent<Vertex>();
                    v.id = b;
                    vertex[b] = v;
                    mapCells.Add(pathCell);
                    pathCell.transform.position = pos;

                }
            }
            index++;
        }

        pathCells[0] = startingCell;
        pathCells[pathCells.Count - 1] = exitCell;

        //Crea y pinta los bordes de negro
        for (int l = -1; l < mapHeight + 1; l++)
        {
            if (l == -1 || l == mapHeight)
            {
                for (int k = -1; k < mapWidth + 1; k++)
                {
                    GameObject newEdge = Instantiate(edgeCellPrefab);
                    newEdge.transform.position = new Vector3(l, 0, k);
                    newEdge.GetComponent<Renderer>().material.color = Color.black;

                }
            }
            else
            {
                GameObject newEdge = Instantiate(edgeCellPrefab);
                newEdge.transform.position = new Vector3(l, 0, -1);
                GameObject newEdge2 = Instantiate(edgeCellPrefab);
                newEdge2.transform.position = new Vector3(l, 0, mapWidth);
            }
        }

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
                SetNeighbours(j, i);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Load();
        
    }

    private void moveUp()
    {
        pathCells.Add(actualCell);
        if (pathCells.Count >= 2)
        {
            previous = pathCells[pathCells.Count - 2];
            GameObject anterior = actualCell;
            index = mapCells.IndexOf(actualCell);
            nextIndex = index + mapWidth;
            actualCell = mapCells[nextIndex];
            if (actualCell.transform.position.z != previous.transform.position.z && actualCell.transform.position.x != previous.transform.position.x)
                corners.Add(anterior);
        }
        else
        {
            GameObject anterior = actualCell;
            index = mapCells.IndexOf(actualCell);
            nextIndex = index + mapWidth;
            actualCell = mapCells[nextIndex];
        }

    }
    private void moveDown()
    {
        pathCells.Add(actualCell);
        if (pathCells.Count >= 2)
        {
            previous = pathCells[pathCells.Count - 2];
            index = mapCells.IndexOf(actualCell);
            nextIndex = index - mapWidth;
            actualCell = mapCells[nextIndex];
        }
        else
        {
            index = mapCells.IndexOf(actualCell);
            nextIndex = index - mapWidth;
            actualCell = mapCells[nextIndex];
        }
    }
    private void moveLeft()
    {
        pathCells.Add(actualCell);
        if (pathCells.Count >= 2)
        {
            previous = pathCells[pathCells.Count - 2];
            index = mapCells.IndexOf(actualCell);
            nextIndex = index - 1;
            actualCell = mapCells[nextIndex];
        }
        else
        {
            index = mapCells.IndexOf(actualCell);
            nextIndex = index - 1;
            actualCell = mapCells[nextIndex];
        }

    }
    private void moveRight()
    {
        pathCells.Add(actualCell);
        if (pathCells.Count >= 2)
        {
            previous = pathCells[pathCells.Count - 2];
            index = mapCells.IndexOf(actualCell);
            nextIndex = index + 1;
            actualCell = mapCells[nextIndex];
        }
        else
        {
            index = mapCells.IndexOf(actualCell);
            nextIndex = index + 1;
            actualCell = mapCells[nextIndex];
        }

    }
    public List<GameObject> getEsquinas()
    {
        return corners;
    }
    public List<GameObject> getEsquinas2()
    {
        return corners2;
    }

    private void moveUp2()
    {
        secondPathCells.Add(actualCell);
        if (secondPathCells.Count >= 2)
        {
            previous = secondPathCells[secondPathCells.Count - 2];
            GameObject anterior = actualCell;
            index = mapCells.IndexOf(actualCell);
            nextIndex = index + mapWidth;
            actualCell = mapCells[nextIndex];
            if (actualCell.transform.position.z != previous.transform.position.z && actualCell.transform.position.x != previous.transform.position.x)
            {
                corners2.Add(anterior);
            }
        }
        else
        {
            index = mapCells.IndexOf(actualCell);
            nextIndex = index + mapWidth;
            actualCell = mapCells[nextIndex];
        }

    }
    private void moveDown2()
    {
        secondPathCells.Add(actualCell);
        if (secondPathCells.Count >= 2)
        {
            previous = secondPathCells[secondPathCells.Count - 2];
            index = mapCells.IndexOf(actualCell);
            nextIndex = index - mapWidth;
            actualCell = mapCells[nextIndex];
        }
        else
        {
            index = mapCells.IndexOf(actualCell);
            nextIndex = index - mapWidth;
            actualCell = mapCells[nextIndex];
        }
    }
    private void moveLeft2()
    {
        secondPathCells.Add(actualCell);
        if (secondPathCells.Count >= 2)
        {
            previous = secondPathCells[secondPathCells.Count - 2];
            index = mapCells.IndexOf(actualCell);
            nextIndex = index - 1;
            actualCell = mapCells[nextIndex];
        }
        else
        {
            index = mapCells.IndexOf(actualCell);
            nextIndex = index - 1;
            actualCell = mapCells[nextIndex];
        }

    }
    private void moveRight2()
    {
        secondPathCells.Add(actualCell);
        if (secondPathCells.Count >= 2)
        {
            previous = secondPathCells[secondPathCells.Count - 2];
            GameObject anterior = actualCell;
            index = mapCells.IndexOf(actualCell);
            nextIndex = index + 1;
            actualCell = mapCells[nextIndex];
        }
        else
        {
            GameObject anterior = actualCell;
            index = mapCells.IndexOf(actualCell);
            nextIndex = index + 1;
            actualCell = mapCells[nextIndex];
        }
    }

    // -------------------------------- GRAPH ----------------------------------------

    private int GridToId(int x, int y)
    {
        return Math.Max(mapHeight, mapWidth) * y + x;
    }

    protected void SetNeighbours(int x, int y, bool get8 = false)
    {
        int col = x;
        int row = y;
        int i, j;
        int vertexId = GridToId(x, y);
        neighbors[vertexId] = new List<Vertex>();
        costs[vertexId] = new List<float>();
        Vector2[] pos = new Vector2[0];
        if (get8)
        {
            pos = new Vector2[8];
            int c = 0;
            for (i = row - 1; i <= row + 1; i++)
            {
                for (j = col - 1; j <= col; j++)
                {
                    pos[c] = new Vector2(j, i);
                    c++;
                }
            }
        }
        else
        {
            pos = new Vector2[4];
            pos[0] = new Vector2(col, row - 1);
            pos[1] = new Vector2(col - 1, row);
            pos[2] = new Vector2(col + 1, row);
            pos[3] = new Vector2(col, row + 1);
        }
        foreach (Vector2 p in pos)
        {
            i = (int)p.y;
            j = (int)p.x;
            if (i < 0 || j < 0)
                continue;
            if (i >= mapWidth || j >= mapHeight)
                continue;
            if (i == row && j == col)
                continue;
            if (!vertexMap[i, j])
                continue;

            int id = GridToId(j, i);
            neighbors[vertexId].Add(vertex[id]);
            costs[vertexId].Add(defaultCost);
        }
    }

    public override Vertex GetNearestVertex(Vector3 position)
    {
  
        int col = (int)(position.x);
        int row = (int)(position.z);
        Vector2 p = new Vector2(col, row);
        List<Vector2> explored = new List<Vector2>();
        Queue<Vector2> queue = new Queue<Vector2>();
        queue.Enqueue(p);
        do
        {
            p = queue.Dequeue();
            col = (int)p.x;
            row = (int)p.y;
            int id = GridToId(row, col);
            if (vertexMap[col, row]) return vertex[id];

            if (!explored.Contains(p))
            {
                explored.Add(p);
                int i, j;
                for (i = row - 1; i <= row + 1; i++)
                {
                    for (j = col - 1; j <= col + 1; j++)
                    {
                        if (i < 0 || j < 0)
                            continue;
                        if (j >= mapWidth || i >= mapHeight)
                            continue;
                        if (i == row && j == col)
                            continue;
                        queue.Enqueue(new Vector2(j, i));
                    }
                }
            }
        } while (queue.Count != 0);
        return null;
    }
}