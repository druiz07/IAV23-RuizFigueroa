using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField]
    private string newSceneName;
	public void OnClick()
    {
        SceneManager.LoadScene(newSceneName);
    }
}
