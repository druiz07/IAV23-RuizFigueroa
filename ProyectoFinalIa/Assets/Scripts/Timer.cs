using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float tiempoInicial = 600.0f; // 10 minutos en segundos
    private float tiempoRestante;
    private bool contadorActivo = true;
    public TextMeshProUGUI textoContador; // Referencia al componente de texto en tu interfaz de usuario

    void Start()
    {
        tiempoRestante = tiempoInicial;
        ActualizarTextoContador();
    }


    void Update()
    {
        if (contadorActivo)
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante -= Time.deltaTime;
            }
            else
            {
                // El tiempo ha llegado a cero, realiza una acción aquí
                contadorActivo = false;
            }
            ActualizarTextoContador();
        }
    }


    void ActualizarTextoContador()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoContador.text = string.Format("{0:00}:{1:00}", minutos, segundos);


        if (tiempoRestante < 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }


    public void IniciarContador()
    {
        contadorActivo = true;
    }

    public void DetenerContador()
    {
        contadorActivo = false;
    }
}