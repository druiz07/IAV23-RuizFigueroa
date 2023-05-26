using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SensitivitySettings : MonoBehaviour
{
    [SerializeField]
    private GameObject inputX;
    [SerializeField]
    private GameObject inputY;

    [SerializeField]
    private bool isInputX;

    private Slider slider;
    private TMP_InputField sensX;
    private TMP_InputField sensY;


    private void Start()
    {
        slider = GetComponent<Slider>();

        sensX = inputX.GetComponent<TMP_InputField>();
        sensY = inputY.GetComponent<TMP_InputField>();
    }
    public void SetLevel(float sliderValue)
    {
        slider.SetValueWithoutNotify(sliderValue);

        sliderValue = Mathf.Floor(sliderValue * 100) / 100;

        // Change input field text
        if (isInputX)
            sensX.text = sliderValue.ToString();
        else
            sensY.text = sliderValue.ToString();

        GameManager.Instance.UpdateCameraInfo(float.Parse(sensX.text), float.Parse(sensY.text));
    }
}
