/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using UnityEngine;
using System.Collections.Generic;

// Puntos representativos o v�rtice (com�n a todos los esquemas de divisi�n, o a la mayor�a de ellos)
[System.Serializable]
public class Vertex : MonoBehaviour
{
    /// <summary>
    /// Identificador del v�rtice 
    /// </summary>
    public int id;

    /// <summary>
    /// Vecinos del v�rtice
    /// </summary>
    public List<Edge> vecinos;

    // V�rtice previo
    [HideInInspector]
    public Vertex prev;
}

