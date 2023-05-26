using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    static LevelMusic instance = null;

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else{
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    public static void Stop()
    {
        if(instance != null)
        {
            instance = null;
            Destroy(instance.gameObject);
        }
    }
}
