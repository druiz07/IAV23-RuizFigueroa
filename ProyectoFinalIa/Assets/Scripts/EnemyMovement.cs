using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float maxVelocity;
    int index = 0;
    bool dead = false;

    List<Vertex> path;

    private Rigidbody rb;
    private Vector3 velocity;
    private Transform cellWalk;

    Graph graph;

    private GameObject init;
    private GameObject exit;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        init = GameObject.FindGameObjectWithTag("Inicio");
        exit = GameObject.FindGameObjectWithTag("Exit");
        graph = GameObject.FindGameObjectWithTag("Generador").GetComponent<GeneracionMapa>();
        path = graph.GetPathAstar(this.init, this.exit, graph.ManhattanDist);

        updateExitPath(path);
    }

    void Update()
    {
        SeguirHilo();
    }

    public void setDead(bool isdead)
    {
        dead = isdead;
    }
    private void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            rb.AddForce(velocity, ForceMode.Force);
        }
    }

    public void updateExitPath(List<Vertex> pathList)
    {
        transform.forward = Vector3.forward;
        path = pathList;

        if (pathList.Count > 0)
        {
            index = path.Count - 1;
            cellWalk = path[index].GetComponent<Transform>();
        }
        else
        {
            cellWalk = null;
            index = 0;
        }
    }

   
    private void SeguirHilo()
    {
        
        if (cellWalk == null) return;
        
        Vector3 cellWalkPosition = cellWalk.position;
        Vector3 tPosç = transform.position;
        tPosç.y = 0;
        cellWalkPosition.y = 0;

        float distance = (cellWalkPosition - tPosç).magnitude;


        if (distance < 0.2f && index > 0)
        {
            Vertex actualCell = path[index];

            if (index > 0)
            {

                index--;
                actualCell = path[index];
                cellWalk = actualCell.GetComponent<Transform>();
            }
            else rb.isKinematic = false;

        }

        Vector3 dir = (cellWalk.position - transform.position);
        dir.y = 0;
        dir *= maxVelocity;
        GetComponent<Rigidbody>().velocity = finalVelocity(dir);
        transform.LookAt(new Vector3(cellWalk.position.x, transform.position.y, cellWalk.position.z));
    }

    Vector3 finalVelocity(Vector3 destDir)
    {
        float magnitud = Mathf.Sqrt(Mathf.Pow(destDir.x, 2) + Mathf.Pow(destDir.y, 2) + Mathf.Pow(destDir.z, 2));
        float a = maxVelocity / magnitud;
        Vector3 finalVelocity_ = new Vector3(a * destDir.x, a * destDir.y, a * destDir.z);
        return finalVelocity_;
    }

    public void end()
    {
        if (GameManager.enemies.Count > 0 && !dead)
        {
            GameManager.enemies.Remove(this.gameObject);
            Destroy(this.gameObject);
            dead = true;

            GameManager.Instance.subLives((int)(GetComponent<Enemigo>().tipoEnem + 1));
        }
    }
}
