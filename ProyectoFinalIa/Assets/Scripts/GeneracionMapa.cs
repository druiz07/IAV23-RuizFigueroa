using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneracionMapa : Graph
{
    //Prefabs
    public List<GameObject> mapCells;
    public GameObject floorPrefab;
    public GameObject initPrefab;
    public GameObject exitPrefab;
    public GameObject spawnerPrefab;
    public GameObject objetive;

    //DatosMapa
    [SerializeField]
    private int mapWidth;
    [SerializeField]
    private int mapHeight;

    //Listas
    private List<GameObject> pathCells = new List<GameObject>();
    private List<GameObject> pathCells1 = new List<GameObject>();
    private List<GameObject> mapCellss = new List<GameObject>();

    public List<GameObject> decorations = new List<GameObject>();

    public List<GameObject> corner = new List<GameObject>();
    public List<GameObject> corner1 = new List<GameObject>();

    GameObject[] vertexObjs;
    bool[,] mapVertices;
    float defaultCost = 1f;

    private bool x = false;
    private bool y = false;
    private GameObject actualCell;
    private GameObject initCell;
    private GameObject exitCell;
    private GameObject floorCell;
    private int index;
    private int nextIndex;

    private GameObject previous;

    private List<GameObject> getBordeSuperior()
    {
        List<GameObject> casillasBorde = new List<GameObject>();

        for (int x = mapWidth * (mapHeight - 1); x < mapWidth * mapHeight; x++)
        {
            casillasBorde.Add(mapCellss[x]);
        }

        return casillasBorde;
    }
    private List<GameObject> getBordeInferior()
    {
        List<GameObject> casillasBorde = new List<GameObject>();

        for (int x = 0; x < mapWidth; x++)
        {
            casillasBorde.Add(mapCellss[x]);
        }

        return casillasBorde;
    }
    private List<GameObject> getBordeIzq()
    {
        List<GameObject> casillasBorde = new List<GameObject>();
        int x = 0;
        while (x < mapWidth * mapHeight)
        {
            casillasBorde.Add(mapCellss[x]);
            x = x + mapWidth;
        }
        return casillasBorde;
    }
    private List<GameObject> getBordeDer()
    {
        List<GameObject> casillasBorde = new List<GameObject>();
        int x = mapWidth - 1;
        while (x < mapWidth * mapHeight)
        {
            casillasBorde.Add(mapCellss[x]);
            x = x + mapWidth;
        }
        return casillasBorde;
    }

    public override void Load()
    {
        generaMapa();
    }

    private void generaMapa()
    {
        int id = 0;

        vertex = new Vertex[mapHeight * mapWidth];
        neighbors = new List<List<Vertex>>(mapHeight * mapWidth);
        costs = new List<List<float>>(mapHeight * mapWidth);
        vertexObjs = new GameObject[mapHeight * mapWidth];
        mapVertices = new bool[mapHeight, mapWidth];

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                //Debug.Log("Casilla x: " + i + "  y: " + j);
                id = GridToId(j, i);
                int rcas = UnityEngine.Random.Range(0, mapCells.Count);
                vertexObjs[id] = Instantiate(mapCells[rcas]);
                int rnad = UnityEngine.Random.Range(0, decorations.Count);
                int rObs = UnityEngine.Random.Range(0, 10);
                if (rObs < 2)
                {
                    Instantiate(decorations[rnad], vertexObjs[id].transform);
                }

                mapCellss.Add(vertexObjs[id]);
                vertexObjs[id].transform.position = new Vector3(i, -0.5f, j);
                Vertex v = vertexObjs[id].AddComponent<Vertex>();
                v.id = id;
                vertex[id] = v;
                neighbors.Add(new List<Vertex>());
                costs.Add(new List<float>());
            }
        }

        List<GameObject> casillasArriba = getBordeSuperior();
        List<GameObject> casillasAbajo = getBordeInferior();
        List<GameObject> casillasIzq = getBordeIzq();
        List<GameObject> casillasDer = getBordeDer();

        GameObject inicio;
        GameObject final;

        int rand1 = UnityEngine.Random.Range(0, mapWidth);
        int rand2 = UnityEngine.Random.Range(0, mapWidth);

        int tipoMapa = UnityEngine.Random.Range(0, 2);
        if (tipoMapa == 0)
        {
            inicio = casillasArriba[rand1];
            final = casillasAbajo[rand2];
        }
        else
        {
            inicio = casillasDer[rand1];
            final = casillasIzq[rand2];
        }



        actualCell = inicio;

        if (tipoMapa == 0) moveDown();
        else moveLeft();

        while (!x || !y)
        {
            int number = UnityEngine.Random.Range(0, 2);
            if (!x && number == 0)
            {
                if (actualCell.transform.position.z > final.transform.position.z) moveLeft();
                else if (actualCell.transform.position.z < final.transform.position.z) moveRight();
                else x = true;
            }
            else if (!y && number == 1)
            {
                if (actualCell.transform.position.x > final.transform.position.x) moveDown();
                else if (actualCell.transform.position.x < final.transform.position.x) moveUp();
                else y = true;
            }
        }
        pathCells.Add(final);

        x = false;
        y = false;
        actualCell = inicio;

        if (tipoMapa == 0) moveDown2();
        else moveLeft2();

        while (!x)
        {
            if (actualCell.transform.position.z > final.transform.position.z) moveLeft2();
            else if (actualCell.transform.position.z < final.transform.position.z) moveRight2();
            else x = true;
        }
        while (!y)
        {
            if (actualCell.transform.position.x > final.transform.position.x) moveDown2();
            else if (actualCell.transform.position.x < final.transform.position.x) moveUp2();
            else y = true;
        }
        pathCells1.Add(final);

        int index = 0;
        foreach (GameObject obj in pathCells)
        {
            if (obj == pathCells[0])
            {
                mapVertices[(int)pathCells[index].transform.position.x, (int)pathCells[index].transform.position.z] = true;

                Vector3 pos = pathCells[index].transform.position;
                Destroy(pathCells[index]);
                int b = GridToId((int)pos.z, (int)pos.x);
                vertexObjs[b] = Instantiate(initPrefab);
                initCell = vertexObjs[b];
                vertexObjs[b].transform.position = pos;
                Vertex v = vertexObjs[b].AddComponent<Vertex>();
                v.id = b;
                vertex[b] = v;
                mapCellss.Add(initCell);
                initCell.transform.position = pos;

                Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                Instantiate(spawnerPrefab, a, transform.rotation);
            }
            else if (obj == pathCells[pathCells.Count - 1])
            {
                mapVertices[(int)pathCells[index].transform.position.x, (int)pathCells[index].transform.position.z] = true;

                Vector3 pos = pathCells[index].transform.position;
                Destroy(pathCells[index]);
                int b = GridToId((int)pos.z, (int)pos.x);
                vertexObjs[b] = Instantiate(exitPrefab);
                exitCell = vertexObjs[b];
                vertexObjs[b].transform.position = pos;
                Vertex v = vertexObjs[b].AddComponent<Vertex>();
                v.id = b;
                vertex[b] = v;
                mapCellss.Add(exitCell);
                exitCell.transform.position = pos;

                Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                Instantiate(objetive, a, transform.rotation);
            }
            else
            {
                mapVertices[(int)pathCells[index].transform.position.x, (int)pathCells[index].transform.position.z] = true;
                Vector3 pos = pathCells[index].transform.position;
                Destroy(pathCells[index]);
                int b = GridToId((int)pos.z, (int)pos.x);
                vertexObjs[b] = Instantiate(floorPrefab);
                floorCell = vertexObjs[b];
                vertexObjs[b].transform.position = pos;
                Vertex v = vertexObjs[b].AddComponent<Vertex>();
                v.id = b;
                vertex[b] = v;
                mapCellss.Add(floorCell);
                floorCell.transform.position = pos;

            }
            index++;
        }

        index = 0;
        foreach (GameObject obj in pathCells1)
        {
            if (obj == pathCells1[0])
            {
                if (!mapVertices[(int)pathCells1[index].transform.position.x, (int)pathCells1[index].transform.position.z])
                {
                    mapVertices[(int)pathCells1[index].transform.position.x, (int)pathCells1[index].transform.position.z] = true;

                    Vector3 pos = pathCells1[index].transform.position;
                    Destroy(pathCells1[index]);
                    initCell = Instantiate(initPrefab);
                    mapCellss.Add(initCell);
                    initCell.transform.position = pos;

                    Vector3 a = new Vector3(pos.x, pos.y + 0.5f, pos.z);
                }

            }
            else if (obj == pathCells1[pathCells1.Count - 1])
            {
                if (!mapVertices[(int)pathCells1[index].transform.position.x, (int)pathCells1[index].transform.position.z])
                {
                    mapVertices[(int)pathCells1[index].transform.position.x, (int)pathCells1[index].transform.position.z] = true;

                }
            }
            else
            {
                if (!mapVertices[(int)pathCells1[index].transform.position.x, (int)pathCells1[index].transform.position.z])
                {
                    mapVertices[(int)pathCells1[index].transform.position.x, (int)pathCells1[index].transform.position.z] = true;


                    Vector3 pos = pathCells1[index].transform.position;
                    Destroy(pathCells1[index]);
                    int b = GridToId((int)pos.z, (int)pos.x);
                    vertexObjs[b] = Instantiate(floorPrefab);
                    floorCell = vertexObjs[b];
                    vertexObjs[b].transform.position = pos;
                    Vertex v = vertexObjs[b].AddComponent<Vertex>();
                    v.id = b;
                    vertex[b] = v;
                    mapCellss.Add(floorCell);
                    floorCell.transform.position = pos;

                }
            }
            index++;
        }

        pathCells[0] = initCell;
        pathCells[pathCells.Count - 1] = exitCell;


        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                SetNeighbours(j, i);
            }
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
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index + mapWidth;
            actualCell = mapCellss[nextIndex];
            if (actualCell.transform.position.z != previous.transform.position.z && actualCell.transform.position.x != previous.transform.position.x)
            {
                corner.Add(anterior);
            }
        }
        else
        {
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index + mapWidth;
            actualCell = mapCellss[nextIndex];
        }

    }
    private void moveDown()
    {
        pathCells.Add(actualCell);
        if (pathCells.Count >= 2)
        {
            previous = pathCells[pathCells.Count - 2];
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index - mapWidth;
            actualCell = mapCellss[nextIndex];

        }
        else
        {
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index - mapWidth;
            actualCell = mapCellss[nextIndex];
        }
    }
    private void moveLeft()
    {
        pathCells.Add(actualCell);
        if (pathCells.Count >= 2)
        {
            previous = pathCells[pathCells.Count - 2];
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index - 1;
            actualCell = mapCellss[nextIndex];
        }
        else
        {
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index - 1;
            actualCell = mapCellss[nextIndex];
        }

    }
    private void moveRight()
    {
        pathCells.Add(actualCell);
        if (pathCells.Count >= 2)
        {
            previous = pathCells[pathCells.Count - 2];

            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index + 1;
            actualCell = mapCellss[nextIndex];
        }
        else
        {
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index + 1;
            actualCell = mapCellss[nextIndex];
        }

    }
    public List<GameObject> getEsquinas()
    {
        return corner;
    }
    public List<GameObject> getEsquinas2()
    {
        return corner1;
    }

    private void moveUp2()
    {
        pathCells1.Add(actualCell);
        if (pathCells1.Count >= 2)
        {
            previous = pathCells1[pathCells1.Count - 2];
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index + mapWidth;
            actualCell = mapCellss[nextIndex];
            if (actualCell.transform.position.z != previous.transform.position.z && actualCell.transform.position.x != previous.transform.position.x)
            {
                corner1.Add(anterior);
            }
        }
        else
        {
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index + mapWidth;
            actualCell = mapCellss[nextIndex];
        }

    }
    private void moveDown2()
    {
        pathCells1.Add(actualCell);
        if (pathCells1.Count >= 2)
        {
            previous = pathCells1[pathCells1.Count - 2];
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index - mapWidth;
            actualCell = mapCellss[nextIndex];
        }
        else
        {
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index - mapWidth;
            actualCell = mapCellss[nextIndex];
        }
    }
    private void moveLeft2()
    {
        pathCells1.Add(actualCell);
        if (pathCells1.Count >= 2)
        {
            previous = pathCells1[pathCells1.Count - 2];
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index - 1;
            actualCell = mapCellss[nextIndex];
        }
        else
        {
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index - 1;
            actualCell = mapCellss[nextIndex];
        }

    }
    private void moveRight2()
    {
        pathCells1.Add(actualCell);
        if (pathCells1.Count >= 2)
        {
            previous = pathCells1[pathCells1.Count - 2];

            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index + 1;
            actualCell = mapCellss[nextIndex];

        }
        else
        {
            GameObject anterior = actualCell;
            index = mapCellss.IndexOf(actualCell);
            nextIndex = index + 1;
            actualCell = mapCellss[nextIndex];
        }
    }

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
            if (!mapVertices[i, j])
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
            if (mapVertices[col, row])
            {
                return vertex[id];
            }

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