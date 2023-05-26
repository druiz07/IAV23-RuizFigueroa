using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void EnterSettings()
    {
        animator.SetBool("Settings", true);
    }

    public void ExitSettings()
    {
        animator.SetBool("Settings", false);
    }

    public void EnterLevels()
    {
        animator.SetBool("Levels", true);
    }

    public void ExitLevels()
    {
        animator.SetBool("Levels", false);
    }
}
