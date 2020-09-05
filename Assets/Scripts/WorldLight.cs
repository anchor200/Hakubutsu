using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldLight : MonoBehaviour
{
    public GameObject direL;
    public Toggle lightOn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged()
    {
        if (lightOn.isOn)
        {
            direL.SetActive(false);
        }
        else
        {
            direL.SetActive(true);
        }
    }
}
