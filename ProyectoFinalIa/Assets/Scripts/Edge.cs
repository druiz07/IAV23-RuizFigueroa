/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System;

/// <summary>
/// Edge es la conexi�n entre nodos, un objeto que mantiene un v�rtice (nodo) y su coste
/// </summary>
[System.Serializable]
public class Edge : IComparable<Edge>
{
    public float cost;
    public Vertex vertex;

    public Edge(Vertex vertex = null, float cost = 1f)
    {
        this.vertex = vertex;
        this.cost = cost;
    }

    // Devuelve 0 si son iguales, positivo si este nodo es mayor que other, y negativo si es menor que other
    public int CompareTo(Edge other)
    {
        float result = cost - other.cost;
        int idA = vertex.GetInstanceID();
        int idB = other.vertex.GetInstanceID();
        if (idA == idB)
            return 0;
        return (int)result;
    }

    public bool Equals(Edge other)
    {
        return (other.vertex.id == this.vertex.id);
    }

    public override bool Equals(object obj)
    {
        Edge other = (Edge)obj;
        return (other.vertex.id == this.vertex.id);
    }

    public override int GetHashCode()
    {
        return this.vertex.GetHashCode();
    }
}
