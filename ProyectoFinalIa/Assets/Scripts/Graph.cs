/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using UnityEngine;
using System.Collections.Generic;


public abstract class Graph : MonoBehaviour
{
    protected Vertex[] vertex;
    protected List<List<Vertex>> neighbors;
    protected List<List<float>> costs;

    public delegate float Heuristic(Vertex a, Vertex b);

    public List<Vertex> path;
    float actualTime;
    int long_;

    public virtual void Start()
    {
        Load();
        actualTime = 0.0f;
        long_ = 0;
    }

    public virtual void Load() { }

    public virtual int GetSize()
    {
        if (ReferenceEquals(vertex, null))
            return 0;
        return vertex.Length;
    }

    public virtual Vertex GetNearestVertex(Vector3 position)
    {
        return null;
    }


    public virtual Vertex[] GetNeighbours(Vertex v)
    {
        if (ReferenceEquals(neighbors, null) || neighbors.Count == 0)
            return new Vertex[0];
        if (v.id < 0 || v.id >= neighbors.Count)
            return new Vertex[0];
        return neighbors[v.id].ToArray();
    }

    private Edge[] GetNeighborEdge(Vertex v)
    {
        if (ReferenceEquals(neighbors, null) || v.id < 0 || v.id >= neighbors.Count)
            return new Edge[0];

        int nEdges = neighbors[v.id].Count;
        Edge[] edges = new Edge[nEdges];
        List<Vertex> nodosVecino = neighbors[v.id];
        List<float> costes = costs[v.id];

        for (int x = 0; x < nEdges; x++)
        {
            edges[x] = new Edge();
            edges[x].vertex = nodosVecino[x];
            edges[x].cost = costes[x];
        }
        return edges;
    }

    public List<Vertex> GetPathAstar(GameObject srcO, GameObject dstO, Heuristic h = null)
    {
        //Reseteo
        actualTime = Time.realtimeSinceStartup;
        long_ = 0;

        if (!srcO || !dstO)
            return new List<Vertex>();
        Vertex src = GetNearestVertex(srcO.transform.position);
        Vertex dst = GetNearestVertex(dstO.transform.position);

        List<Vertex>[] arrayVertices = new List<Vertex>[GameManager.towers.Count];
        Edge[][] aristaTower = new Edge[GameManager.towers.Count][];


        for (int i = 0; i < GameManager.towers.Count; i++)
        {
            Vector3 pos = GameManager.towers[i].transform.position;
            Vertex verticeTower = GetNearestVertex(pos);
            aristaTower[i] = GetNeighborEdge(verticeTower);
            arrayVertices[i] = new List<Vertex>();
            arrayVertices[i].Add(verticeTower);

            foreach (Edge e in aristaTower[i])
            {
                arrayVertices[i].Add(e.vertex);
            }
        }

        BinaryHeap<Edge> priorityQueueAristas = new BinaryHeap<Edge>();

        Edge vertice, hijo;
        Edge[] aristas;

        float[] distancias = new float[vertex.Length];
        int[] verticeAnterior = new int[vertex.Length];

        vertice = new Edge(src, 0);
        priorityQueueAristas.Add(vertice);
        distancias[src.id] = 0;
        verticeAnterior[src.id] = src.id;

        for (int i = 0; i < vertex.Length; i++)
        {
            if (i != src.id)
            {
                verticeAnterior[i] = -1;
                distancias[i] = Mathf.Infinity;
            }
        }

        while (priorityQueueAristas.Count > 0)
        {

            vertice = priorityQueueAristas.Remove();
            int nodeId = vertice.vertex.id;

            if (ReferenceEquals(vertice.vertex, dst))
            {
                actualTime = Time.realtimeSinceStartup - actualTime;
                List<Vertex> lista = BuildPath(src.id, vertice.vertex.id, ref verticeAnterior);
                long_ = lista.Count;
                return lista;
            }

            aristas = GetNeighborEdge(vertice.vertex);
            foreach (Edge neigh in aristas)
            {
                int nID = neigh.vertex.id;

                if (verticeAnterior[nID] == -1)
                {
                    float cost = distancias[nodeId] + neigh.cost;

                    for (int i = 0; i < GameManager.towers.Count; i++)
                        foreach (Edge e in aristaTower[i]) 
                            if (neigh.vertex == e.vertex) cost *= 5;

                    cost += h(vertice.vertex, neigh.vertex);

                    if (cost < distancias[neigh.vertex.id])
                    {
                        distancias[nID] = cost;
                        verticeAnterior[nID] = nodeId;
                        priorityQueueAristas.Remove(neigh);
                        hijo = new Edge(neigh.vertex, cost);
                        if (!priorityQueueAristas.Contains(hijo))
                            priorityQueueAristas.Add(hijo);
                    }
                }
            }
        }
        return new List<Vertex>();
    }

    private List<Vertex> BuildPath(int srcId, int dstId, ref int[] prevList)
    {
        List<Vertex> path = new List<Vertex>();
        int prev = dstId;
        do
        {
            path.Add(vertex[prev]);
            prev = prevList[prev];
        } while (prev != srcId);
        return path;
    }

    // Heurística de distancia Manhattan
    public float ManhattanDist(Vertex a, Vertex b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        float ac = Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
        return ac;
    }
}
