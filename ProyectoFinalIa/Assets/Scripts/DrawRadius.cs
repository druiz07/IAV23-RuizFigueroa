using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class DrawRadius : MonoBehaviour
{
    [Range(0, 50)]
    public int segments = 50;
    [Range(0, 20)]
    public float radius = 20;
    LineRenderer line;

    //provisional 
    public float posY;
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();
        GetComponent<LineRenderer>().material.color = Color.cyan;
    }

    public void CreatePoints()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, posY, z));

            angle += (360f / segments);
        }
    }

    public float getRadio()
    {
        return radius;
    }

}