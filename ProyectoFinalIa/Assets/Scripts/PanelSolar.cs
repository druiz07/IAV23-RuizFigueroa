using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSolar : MonoBehaviour
{

    public int dinero = 10;
    public float contador;
    public float dineroSpeed=10f;
    public bool updateMoney = false;


    // Update is called once per frame
    void Update()
    {
        if (contador >= dineroSpeed)
        {
            GameManager.Instance.AddMoney(dinero);
            contador = 0;
        }
        else contador += Time.deltaTime;
    }


    public void moneyUpdate()
    {
        dineroSpeed = 7f;
        updateMoney = true;
    }

    public bool getUpdateMoney()
    {
        return updateMoney;
    }
}
