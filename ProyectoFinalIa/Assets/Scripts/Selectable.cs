using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectableLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayHit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit,Mathf.Infinity, selectableLayer))
                GameManager.Instance.setSelectCell(rayHit.collider.gameObject);
        }
        else if (Input.GetMouseButtonDown(1))
            GameManager.Instance.deselectCell();
    }
}
