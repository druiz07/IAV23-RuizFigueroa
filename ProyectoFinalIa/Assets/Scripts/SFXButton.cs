using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SFXButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField]
    private AudioSource sfx;

    [SerializeField]
    private AudioSource sfx2;
    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Do something.
        sfx.Play();
    }
    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        // Do something.
        sfx2.Play();
    }
}