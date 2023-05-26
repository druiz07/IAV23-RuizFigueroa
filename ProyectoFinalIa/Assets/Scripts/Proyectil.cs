using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    // Start is called before the first frame update
    private float radioDestruccion;
    private Vector3 posIni;
    public int danho = 1;

    public float tiempoDestruccion;
    void Start()
    {
        posIni = transform.position;
        Invoke("Destruirse", tiempoDestruccion);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(posIni, this.transform.position) > radioDestruccion / 2)
        {
            Destroy(this.gameObject);

        }
    }


    public void setDamage(int damage)
    {
        danho = damage;
    }
    private void Destruirse()
    {
        Destroy(this.gameObject);
    }

    public void setRadio(float radio)
    {
        radioDestruccion = radio;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemigo>())
        {
            collision.gameObject.GetComponent<Enemigo>().recibeDanho(danho);
            Destroy(this.gameObject);
        }
    }
}
