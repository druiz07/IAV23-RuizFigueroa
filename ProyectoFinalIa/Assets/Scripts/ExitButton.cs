using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasTransition;

    [SerializeField]
    private Button button;

    [SerializeField]
    private string newSceneName;

    private bool exit = false;

    private void Update()
    {

    }
}
