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

// Puntos representativos o vértice (común a todos los esquemas de división, o a la mayoría de ellos)
[System.Serializable]
public class Vertex : MonoBehaviour
{
    /// <summary>
    /// Identificador del vértice 
    /// </summary>
    public int id;

    /// <summary>
    /// Vecinos del vértice
    /// </summary>
    public List<Edge> vecinos;

    // Vértice previo
    [HideInInspector]
    public Vertex prev;
}

