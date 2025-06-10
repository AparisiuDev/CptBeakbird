using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleSad : MonoBehaviour
{
    public Material sadMaterial; // Material to apply when the whale is sad
                                 // Start is called before the first frame update
    public void Start()
    {
        GetComponent<Renderer>().material = sadMaterial;
    }
}
