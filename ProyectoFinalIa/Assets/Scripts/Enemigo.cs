using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TipoEnemigo { DEFAULT = 0, RAPIDO = 1, FUERTE = 2 }

public class Enemigo : MonoBehaviour
{
    public int vidas = 1;
    public TipoEnemigo tipoEnem = TipoEnemigo.DEFAULT;

    public void recibeDanho(int danho)
    {
        vidas -= danho;
        if (vidas <= 0)
        {
            GameManager.enemies.Remove(this.gameObject);
            Destroy(this.gameObject);

        }
    }


    private void OnDestroy()
    {
        if(GameManager.Instance)GameManager.Instance.removeFromTowers(this.gameObject);
    }
}
