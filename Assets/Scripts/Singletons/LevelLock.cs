using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLock : MonoBehaviour
{
    // Bools de si los levels estan unlocked
    public bool _lvl1 = false;
    public bool _lvl2 = false;
    public bool _lvl3 = false;

    public static LevelLock instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }
}
