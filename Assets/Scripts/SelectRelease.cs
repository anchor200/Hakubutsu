using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRelease : MonoBehaviour
{
    public static bool selectReleaseReq = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnReleaseButton()
    {
        selectReleaseReq = true;
    }

    public static void doneOut()
    {
        selectReleaseReq = false;  // 外から呼びだす。処理が終わった後
    }
}
