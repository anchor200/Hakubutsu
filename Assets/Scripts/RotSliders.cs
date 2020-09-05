using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotSliders : MonoBehaviour
{
    public static float hori;
    public static float ver;
    public GameObject horiSlider;
    public GameObject verSlider;
    static Slider hs;
    static Slider vs;
    public static bool toBeProcessed;
    // Start is called before the first frame update
    void Start()
    {
        hori = 0f;
        ver = 0f;
        hs = horiSlider.GetComponent<Slider>();
        vs = verSlider.GetComponent<Slider>();
        toBeProcessed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChanged()
    {
        toBeProcessed = true;
        hori = hs.value;
        ver = vs.value;
    }


    public static void ReqReset(float horio, float vero)
    {
        hori = horio;
        ver = vero;
        hs.value = hori;
        vs.value = ver;
    }
    public static void doneOut()
    {
        toBeProcessed = false;  // 外から呼びだす。処理が終わった後
    }
}
