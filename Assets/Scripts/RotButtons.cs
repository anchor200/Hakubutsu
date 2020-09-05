using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotButtons : MonoBehaviour
{
    //public GameObject right;
    //public GameObject left;
    public static int buttonLR;

    // Start is called before the first frame update
    void Start()
    {
        buttonLR = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickR()
    {
        buttonLR = 1;
    }
    public void OnClickL()
    {
        buttonLR = -1;
    }

    public void done()
    {
        buttonLR = 0;
    }

    public static void doneOut()
    {
        buttonLR = 0;  // 外から呼びだす。処理が終わった後
    }

}
