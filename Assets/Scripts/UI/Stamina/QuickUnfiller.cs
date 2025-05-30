using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickUnfiller : MonoBehaviour
{
    public Image dependant;
    private Image unfill;
    // Start is called before the first frame update
    void Start()
    {
        unfill = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        unfill.fillAmount = dependant.fillAmount;
    }
}
