using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraDolly : MonoBehaviour
{
    // マウスホイールの回転値を格納する変数
    private float scroll;
    // カメラ移動の速度
    public float speed = 5f;
    public static bool upperView =false;
    public Toggle panelToggleLig;
    public Toggle panelTogglePic;
    public Toggle panelToggle3D;
    public static float defField;
    float begField;
    int loopInd = 0;


    // ゲーム実行中の繰り返し処理
    void Start()
    {
        defField = Camera.main.fieldOfView;
    }
    void Update()
    {
        if (panelToggleLig.isOn || panelToggle3D.isOn || panelTogglePic.isOn)
        {
            if (Input.GetMouseButton(1))
            {
                // マウスホイールの回転値を変数 scroll に渡す
                scroll = Input.GetAxis("Mouse ScrollWheel");
                if (!panelToggleLig.isOn)
                    Camera.main.transform.position += transform.up * scroll * speed;
            }
            if (!upperView && Input.GetMouseButtonUp(1))
            {
                Camera.main.transform.localPosition = new Vector3(0f, 0.353f, 0f);
            }

            if (upperView)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 34f, Camera.main.transform.position.z);
            }
            if (!panelToggleLig.isOn && panelToggleLig.isOn != upperView)
            {
                Camera.main.transform.localPosition = new Vector3(0f, 0.353f, 0f);
            }
            if (panelToggleLig.isOn)
            {
                upperView = true;
            }
            else
            {
                upperView = false;
            }


        }
        else
        {
            upperView = false;
            scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Camera.main.fieldOfView >= defField / 3f && Camera.main.fieldOfView <= defField)
                Camera.main.fieldOfView = Camera.main.fieldOfView + speed * 5f * scroll;
            else if (Camera.main.fieldOfView <= defField / 3f)
            {
                Camera.main.fieldOfView = defField / 3f + 1f;
            }
            else
            {
                Camera.main.fieldOfView = defField;
            }
            if (Tenjimen.isDoubleTapped)
            {
                //Camera.main.fieldOfView = defField;
                begField = Camera.main.fieldOfView;
                loopInd = 0;
                dollySousa();
            }
        }



    }

    void dollySousa()
    {
        if (loopInd++ < 50)
        {
            float comp = (float)loopInd / 50f;

            Camera.main.fieldOfView = comp * defField + (1f - comp) * begField;
            Invoke("dollySousa", 0.01f);
        }
        else
        {
            loopInd = 0;

        }

    }
}