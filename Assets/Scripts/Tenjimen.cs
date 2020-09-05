using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.IO;

public class Tenjimen : MonoBehaviour
{
    GameObject clickedGameObject;

    public GameObject sakuhinPrefab;
    public GameObject sakuhinContainer;

    public GameObject daiPrefab;  // 展示台 パラメータを離散で指定
    public GameObject dais;
    public GameObject rittaiContainer;

    public GameObject lightPrefab;
    public GameObject lightContainer;

    public const float sizeRate = 0.2f;

    public static GameObject boxObjPrefab;
    public static GameObject boxesObj;
    public static GameObject boxObjPrefabd;
    public static GameObject boxesObjd;
    public static GameObject rittaiCont;
    public static GameObject boxLightPrefab;
    public static GameObject lightCont;
    public static int selectedSakuhin = -1;
    public static int selectedDai = -1;
    public static int selectedDaimen = -1;
    public static int selectedRittai = -1;
    public static int selectedLight = -1;

    public GameObject selectButton;
    public GameObject sizeField;
    public GameObject rotButton;
    public GameObject rotSlider;
    public GameObject detailPanelPic;
    public GameObject detailPanel3D;
    public GameObject detailPanelLig;
    public GameObject rittaiDropdown;
    public GameObject daiDropdown;
    public Dropdown frames;

    public Toggle panelTogglePic;
    public Toggle panelToggle3D;
    public Toggle panelToggleLig;
    public GameObject sampleImage;
    public Dropdown sakuhins;
    public Dropdown daiss;
    public Dropdown rittais;
    public Dropdown lights;
    public GameObject reDo;
    public GameObject unDo;

    Vector3 wallCenter;
    Vector3 wallFront;
    Vector3 wallRight;
    Vector3 wallUp;

    private bool isDoubleTapStart;
    private float doubleTapTime;
    public static bool saveWorth = false;
    public static bool isDoubleTapped = false;
    public static int modori = -1;
    public static int saveSatu = 0;
    public static int saishin = -1;
    private const int modoriMax = 100;

    public static int sakuhinNum = 0;
    public static int daiNum = 0;
    public static int rittaiNum = 0;
    public static int lightNum = 0;
    public static bool uiResetReq = false;

    public struct sakuhin
    {
        public float x;
        public float y;
        public float z;
        public float size;
        public GameObject g;
        public int id; // 通し番号。プログラム中の検索では使っていない
        public string data;
        public Vector3 wallFront;
        public Vector3 wallRight;
        public Vector3 wallUp;
        public Vector3 wallCenter;
        public string frameTitle;
    }

    public static List<sakuhin> sakuhinList; // 絵画のみ

    public struct dai
    {
        public float x;
        public float z;
        public string type;
        public GameObject g;
        public int id;
        public float theta;
    }

    public static List<dai> daiList; // 台の一覧

    public struct rittai
    {
        public int id;
        public string data;
        public float x;
        public float y;
        public float z;
        public float theta;
        public float size;
        public GameObject g;
        public int parentNum;
    }

    public static List<rittai> rittaiList; // 台の一覧

    public struct light
    {
        public float x;
        public float y;
        public float z;
        public string type;  // 中角とか広角とか
        public GameObject g;
        public int id;
        public float gyokaku;
        public float theta;
    }

    public static List<light> lightList; // 台の一覧

    // Start is called before the first frame update
    void Start()
    {
        modori = 0;
        sakuhinNum = 0;
        sakuhinList = new List<sakuhin>();
        daiList = new List<dai>();
        rittaiList = new List<rittai>();
        lightList = new List<light>();
        uiReset();
        sakuhinReset();
        daiReset();
        rittaiReset();
        lightReset();
        frameReset();

        boxObjPrefab = sakuhinPrefab;
        boxesObj = sakuhinContainer;
        boxObjPrefabd = daiPrefab;
        boxesObjd = dais;
        rittaiCont = rittaiContainer;
        boxLightPrefab = lightPrefab;
        lightCont = lightContainer;
    }

    // Update is called once per frame
    void Update()
    {


        reDo.SetActive(true);
        unDo.SetActive(true);

        if (modori == saishin || (modori == modoriMax && saishin == 0) || saishin < 0 || saveSatu < 1)
        {
            reDo.SetActive(false);
        }
        if (modori == saishin + 1 || (modori == 0 && saishin == modoriMax) || (modori == -1 && saveSatu != modoriMax) || (modori == 0 && saveSatu != 9))
        {
            unDo.SetActive(false);
        }

        if (selectedSakuhin < 0 && selectedDai < 0 && selectedDaimen < 0 && selectedLight < 0 && selectedRittai < 0)
        {
            selectButton.SetActive(false);
        }
        else
        {
            selectButton.SetActive(true);
            reDo.SetActive(false);
            unDo.SetActive(false);
        }



        // double tap
        isDoubleTapped = false;
        if (isDoubleTapStart)
        {
            doubleTapTime += Time.deltaTime;
            if (doubleTapTime < 0.3f)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    Debug.Log("double tap");
                    isDoubleTapStart = false;
                    isDoubleTapped = true;
                    doubleTapTime = 0.0f;
                }
            }
            else
            {
                // reset
                isDoubleTapStart = false;
                doubleTapTime = 0.0f;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                //Debug.Log("down");
                isDoubleTapStart = true;
            }
        }
        // double tap

        if (panelTogglePic.isOn)
        {
            if (Input.GetMouseButtonDown(0))
            {

                clickedGameObject = null;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    clickedGameObject = hit.collider.gameObject;
                }

                Debug.Log(clickedGameObject);
                try
                {
                    if (clickedGameObject.tag == "Tenjimen" && !selectedSome())
                    {
                        //Debug.Log(clickedGameObject.GetComponent<BoxCollider>().center.x);
                        BoxCollider tenjimen = clickedGameObject.GetComponent<BoxCollider>();
                        Transform wallInfo = clickedGameObject.GetComponent<Transform>();


                        // 展示面の中心のワールド座標
                        wallCenter = wallInfo.TransformPoint(tenjimen.center);
                        Debug.Log("new added center: " + wallCenter);
                        // 一番小さい向きを選ぶ
                        // 展示面上の座標軸を右手系で取得
                        if (Min<float>(tenjimen.size.x, tenjimen.size.y, tenjimen.size.z) == tenjimen.size.x)
                        {
                            wallFront = wallInfo.TransformDirection(Vector3.right);
                            wallRight = wallInfo.TransformDirection(-1.0f * Vector3.forward);
                            wallUp = wallInfo.TransformDirection(Vector3.up);
                        }
                        else if (Min<float>(tenjimen.size.x, tenjimen.size.y, tenjimen.size.z) == tenjimen.size.y)
                        {
                            wallFront = wallInfo.TransformDirection(Vector3.up);
                            wallRight = wallInfo.TransformDirection(-1.0f * Vector3.right);
                            wallUp = wallInfo.TransformDirection(Vector3.forward);
                        }
                        else
                        {
                            wallFront = wallInfo.TransformDirection(Vector3.forward);
                            wallRight = wallInfo.TransformDirection(-1.0f * Vector3.right);
                            wallUp = wallInfo.TransformDirection(Vector3.up);
                        }

                        // プレイヤーの向きが法線ベクトルの正の向きになるようにする
                        float cross = -1f * (wallCenter.x - Player.playerPos.x) * wallFront.x + (wallCenter.y - Player.playerPos.y) * wallFront.y + (wallCenter.z - Player.playerPos.z) * wallFront.z;
                        if (cross < 0.0)
                        {
                            wallFront = -1.0f * wallFront;
                            wallRight = -1.0f * wallRight;
                            wallUp = 1.0f * wallUp;
                        }
                        if (Min<float>(tenjimen.size.x, tenjimen.size.y, tenjimen.size.z) == tenjimen.size.x)
                        {
                            wallFront = -1.0f * wallFront;  //行列の変換がちゃんとできないのでこんなとこで補正している…恥ずかしい
                        }
                        wallFront = -1.0f * wallFront; // 左手系にする

                        Debug.Log("shomen" + wallFront.x.ToString() + ", " + wallFront.y.ToString() + ", " + wallFront.z.ToString());
                        Debug.Log("migi x" + wallRight.x.ToString() + ", " + wallRight.y.ToString() + ", " + wallRight.z.ToString());
                        Debug.Log("ue y" + wallUp.x.ToString() + ", " + wallUp.y.ToString() + ", " + wallUp.z.ToString());

                        if (Input.GetKey(KeyCode.LeftControl) || isDoubleTapped)
                        {
                            if (selectedSakuhin < 0)
                                sakuhinSet();// wallCenter, wallFront, wallRight, wallUp);
                        }


                    }
                    else if (clickedGameObject.tag == "Sakuhin")
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || true)
                        {
                            sakuhinSelect();
                            sizeField.SetActive(true);
                            //Debug.Log(selectedSakuhin);
                            sizeField.GetComponent<InputField>().text = sakuhinList[selectedSakuhin].size.ToString();
                        }
                    }





                }
                catch (NullReferenceException e)
                {

                }

            }


            if (selectedSakuhin >= 0)
            {
                sakuhinList[selectedSakuhin].g.transform.position = sakuhinList[selectedSakuhin].wallCenter - sakuhinList[selectedSakuhin].z * sakuhinList[selectedSakuhin].wallFront + sakuhinList[selectedSakuhin].x * sakuhinList[selectedSakuhin].wallRight + sakuhinList[selectedSakuhin].y * sakuhinList[selectedSakuhin].wallUp;
                float rate = float.Parse(sakuhinList[selectedSakuhin].data.Split(',')[1].Split('_')[1]) / float.Parse(sakuhinList[selectedSakuhin].data.Split(',')[1].Split('_')[0]);
                sakuhinList[selectedSakuhin].g.transform.Find(sakuhinList[selectedSakuhin].frameTitle + "(Clone)").gameObject.transform.localScale = new Vector3(1f, rate * 1920f / 1080f, 40f) * sakuhinList[selectedSakuhin].size;
                sakuhinList[selectedSakuhin].g.transform.Find("Drawer").gameObject.transform.localScale = new Vector3(1f, rate, 0.01f) * sakuhinList[selectedSakuhin].size;
                sakuhinMove();

            }
        }
        //==========================================================================================================================
        if (panelToggle3D.isOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Input.GetKey(KeyCode.LeftControl) || isDoubleTapped)
                {
                    if (selectedDai < 0 && selectedDaimen < 0)
                        daiSet();
                }

                clickedGameObject = null;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    clickedGameObject = hit.collider.gameObject;
                }

                Debug.Log(clickedGameObject);
                try
                {
                    if (clickedGameObject.tag == "Dai" && selectedDaimen < 0) // 本体の下の方
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || true)
                        {
                            rotButton.SetActive(true);
                            daiSelect();
                        }
                    }

                    if (clickedGameObject.tag == "ViewReset")
                    {
                        Player.resetReq(clickedGameObject.transform.position, clickedGameObject.transform.parent.transform.parent.transform.forward, 20f);
                    }

                    if (clickedGameObject.tag == "Daimen") // 本体の下の方
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || true)
                        {
                            rotButton.SetActive(false);
                            if (selectedDai >= 0)
                            {
                                daiList[selectedDai].g.transform.Find("TenjidaiAWaku").gameObject.SetActive(false);
                                selectedDai = -1;
                            }
                            daimenSelect();
                        }
                    }

                    if (selectedDaimen >= 0)
                    {
                        if (Input.GetKey(KeyCode.LeftControl) || isDoubleTapped)
                        {
                            if (selectedRittai < 0) // 選択されていない
                            {
                                rittaiSet();  // 作品を設置する
                            }
                        }

                        if (clickedGameObject.tag == "Rittai") // 立体作品の選択は台上でないとできない
                        {
                            if(selectedRittai < 0) // 選択されていない
                            {
                                rittaiSelect();
                            }

                        }

                    }



                }
                catch (NullReferenceException e)
                {

                }

            }

            if (selectedDai >= 0)
            {
                daiList[selectedDai].g.transform.position = new Vector3(daiList[selectedDai].x, daiList[selectedDai].g.transform.position.y, daiList[selectedDai].z);
                daiMove();


            }
            if (selectedDaimen >= 0)
            {
                daimenOperate();

            }
        }

        if (panelToggleLig.isOn)
        {
            if (Input.GetMouseButtonDown(0))
            {

                clickedGameObject = null;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {
                    clickedGameObject = hit.collider.gameObject;
                }

                Debug.Log(clickedGameObject);
                try
                {
                    if (clickedGameObject.tag == "Lightbar")
                    {
                        if (Input.GetKey(KeyCode.LeftControl) || isDoubleTapped)
                        {
                            lightSet(hit.point);
                        }
                    }

                    if (clickedGameObject.tag == "Light")
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || true)
                        {
                            lightSelect();
                            RotSliders.ReqReset(lightList[selectedLight].theta, lightList[selectedLight].gyokaku);
                        }
                    }
                }
                catch (NullReferenceException e)
                {

                }
            }

            if (selectedLight >= 0)
            {
                lightList[selectedLight].g.transform.position = new Vector3(lightList[selectedLight].x, lightList[selectedLight].g.transform.position.y, lightList[selectedLight].z);
                lightList[selectedLight].g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.localEulerAngles = new Vector3(0f, lightList[selectedLight].theta, lightList[selectedLight].gyokaku);
                lightMove();

            }

        }

        if (Input.GetKey(KeyCode.Q))
        {
            for (int i = 0; i < sakuhinList.Count; i++)
            {
                sakuhinList[i].g.transform.Find("Drawer").transform.Find("Highlight").gameObject.SetActive(true);
            }
            for (int i = 0; i < daiList.Count; i++)
            {
                daiList[i].g.transform.Find("Highlight").gameObject.SetActive(true);
            }
            for (int i = 0; i < lightList.Count; i++)
            {
                lightList[i].g.transform.Find("Waku2").gameObject.SetActive(true);
            }

        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            for (int i = 0; i < sakuhinList.Count; i++)
            {
                sakuhinList[i].g.transform.Find("Drawer").transform.Find("Highlight").gameObject.SetActive(false);
            }
            for (int i = 0; i < daiList.Count; i++)
            {
                daiList[i].g.transform.Find("Highlight").gameObject.SetActive(false);
            }
            for (int i = 0; i < lightList.Count; i++)
            {
                lightList[i].g.transform.Find("Waku2").gameObject.SetActive(false);
            }
        }

        if (!panelToggleLig.isOn && !panelToggle3D.isOn && !panelTogglePic.isOn)  // 全てoff：viewerモード
        {
            /*画像のoff*/
            rittaiDropdown.SetActive(false);
            daiDropdown.SetActive(true);
            panelTogglePic.isOn = false;
            if (selectedSakuhin >= 0)
            {
                sakuhinList[selectedSakuhin].g.transform.Find("Drawer").transform.Find("Waku").gameObject.SetActive(false);
                selectedSakuhin = -1;
                sizeField.SetActive(false);
            }
            /*画像のoff*/

            /*立体のoff*/
            panelToggle3D.isOn = false;
            if (selectedDai >= 0)
            {
                daiList[selectedDai].g.transform.Find("TenjidaiAWaku").gameObject.SetActive(false);
                selectedDai = -1;
                selectedDaimen = -1;
                rotButton.SetActive(false);
            }
            if (selectedDaimen >= 0)
            {
                daiList[selectedDaimen].g.transform.Find("DaimenWaku").gameObject.SetActive(false);
                selectedDaimen = -1;
            }
            if (selectedRittai >= 0)
            {
                rittaiList[selectedRittai].g.transform.Find("Sentaku").gameObject.SetActive(false);
                selectedRittai = -1;
                sizeField.SetActive(false);
            }
            /*立体のoff*/

            /* ライトのoff */
            detailPanelLig.SetActive(false);
            panelToggleLig.isOn = false;
            if (selectedLight >= 0)
            {
                rotSlider.SetActive(false);
                lightList[selectedLight].g.transform.Find("Waku").gameObject.SetActive(false);
                rotSlider.SetActive(false);
                selectedLight = -1;
            }
            /* ライトのoff */

            clickedGameObject = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                clickedGameObject = hit.collider.gameObject;
            }

            try
            {
                if (isDoubleTapped && clickedGameObject.tag == "Tenjimen" && Input.GetMouseButton(0))
                {

                    //Debug.Log(clickedGameObject.GetComponent<BoxCollider>().center.x);
                    BoxCollider tenjimen = clickedGameObject.GetComponent<BoxCollider>();
                    Transform wallInfo = clickedGameObject.GetComponent<Transform>();


                    // 展示面の中心のワールド座標
                    wallCenter = wallInfo.TransformPoint(tenjimen.center);
                    Debug.Log("new added center: " + wallCenter);
                    // 一番小さい向きを選ぶ
                    // 展示面上の座標軸を右手系で取得
                    if (Min<float>(tenjimen.size.x, tenjimen.size.y, tenjimen.size.z) == tenjimen.size.x)
                    {
                        wallFront = wallInfo.TransformDirection(Vector3.right);
                        wallRight = wallInfo.TransformDirection(-1.0f * Vector3.forward);
                        wallUp = wallInfo.TransformDirection(Vector3.up);
                    }
                    else if (Min<float>(tenjimen.size.x, tenjimen.size.y, tenjimen.size.z) == tenjimen.size.y)
                    {
                        wallFront = wallInfo.TransformDirection(Vector3.up);
                        wallRight = wallInfo.TransformDirection(-1.0f * Vector3.right);
                        wallUp = wallInfo.TransformDirection(Vector3.forward);
                    }
                    else
                    {
                        wallFront = wallInfo.TransformDirection(Vector3.forward);
                        wallRight = wallInfo.TransformDirection(-1.0f * Vector3.right);
                        wallUp = wallInfo.TransformDirection(Vector3.up);
                    }

                    // プレイヤーの向きが法線ベクトルの正の向きになるようにする
                    float cross = -1f * (wallCenter.x - Player.playerPos.x) * wallFront.x + (wallCenter.y - Player.playerPos.y) * wallFront.y + (wallCenter.z - Player.playerPos.z) * wallFront.z;
                    if (cross < 0.0)
                    {
                        wallFront = -1.0f * wallFront;
                        wallRight = -1.0f * wallRight;
                        wallUp = 1.0f * wallUp;
                    }
                    if (Min<float>(tenjimen.size.x, tenjimen.size.y, tenjimen.size.z) == tenjimen.size.x)
                    {
                        wallFront = -1.0f * wallFront;  //行列の変換がちゃんとできないのでこんなとこで補正している…恥ずかしい
                    }
                    wallFront = -1.0f * wallFront; // 左手系にする

                    Debug.Log("shomen" + wallFront.x.ToString() + ", " + wallFront.y.ToString() + ", " + wallFront.z.ToString());
                    Debug.Log("migi x" + wallRight.x.ToString() + ", " + wallRight.y.ToString() + ", " + wallRight.z.ToString());
                    Debug.Log("ue y" + wallUp.x.ToString() + ", " + wallUp.y.ToString() + ", " + wallUp.z.ToString());

                    //temp.g.transform.LookAt(temp.g.transform.position + 100f * wallFront);
                    //temp.g.transform.localEulerAngles = new Vector3(0f, temp.g.transform.localEulerAngles.y, 0f);
                    //temp.theta = temp.g.transform.localEulerAngles.y;

                    Player.resetReq(Player.playerPos + 10f * wallFront, wallFront, 10f);
                }
                if (isDoubleTapped && clickedGameObject.tag == "Sakuhin" && Input.GetMouseButton(0))
                {
                    for (int i = 0; i < sakuhinList.Count; i++)
                    {
                        if (sakuhinList[i].g == clickedGameObject)
                        {
                            Debug.Log("found" + i.ToString());
                            Player.resetReq(new Vector3(clickedGameObject.transform.position.x, Player.playerPos.y, clickedGameObject.transform.position.z), sakuhinList[i].wallFront, 15f);

                        }
                    }
                }

            }
            catch (NullReferenceException e)
            {
                Debug.Log("tenjimen seitai dame");
            }
        }



        if (saveWorth)
        {
            Debug.Log("modori(endOfUpdate)" + modori.ToString() + saishin.ToString());
            modori++;
            saveWorth = false;
            if (modori > modoriMax)
            {
                modori = 0;
            }
            Saver.OnSaving(modori.ToString());
            if (saveSatu < modori)
            {
                saveSatu = modori; //　今まで出てきた最大値
                Debug.Log("savesatu" + saveSatu.ToString());
            }
            saishin = modori;
            Debug.Log("modori(endOfUpdate aftersave)" + modori.ToString() + saishin.ToString());
        }

        if (uiResetReq)
        {
            uiResetReq = false;
            uiReset();
        }

    }

    public T Min<T>(params T[] nums) where T : IComparable
    {
        if (nums.Length == 0) return default(T);

        T min = nums[0];
        for (int i = 1; i < nums.Length; i++)
        {
            min = min.CompareTo(nums[i]) < 0 ? min : nums[i];
            // Minの場合は不等号を逆にすればOK
        }
        return min;
    }

    public static GameObject instanceSet()
    {
        return Instantiate(boxObjPrefab, boxesObj.transform);
    }

    void sakuhinSet()//Vector3 wallCenter, Vector3 wallFront, Vector3 wallRight, Vector3 wallUp)
    {
        GameObject g = instanceSet();//Instantiate(boxObjPrefab, boxesObj.transform);

        sakuhinNum += 1;
        g.transform.position = wallCenter - 0.16f * wallFront;// + 5f * wallRight + 10f * wallUp;
        g.transform.LookAt(wallCenter + 100f * wallFront);
        g.tag = "Sakuhin";

        sakuhin temp;
        temp.wallCenter = wallCenter;
        temp.wallRight = wallRight;
        temp.wallUp = wallUp;
        temp.wallFront = wallFront;
        temp.x = 0f;
        temp.y = 0f;
        temp.z = 0.16f;
        temp.g = g;
        temp.id = sakuhinNum;
        temp.data = sakuhins.options[sakuhins.value].text.Remove(sakuhins.options[sakuhins.value].text.Length - 1);//"Images/image4,2836_2812";
        //temp.data = "Images/image10,1471_1114";
        temp.size = 1.0f;

        //kokoko
        Debug.Log("congrats");
        var texture = ReadTexture(Application.dataPath + "/MyResources/" + temp.data.Split(',')[0]);
        Texture drawer = g.transform.Find("Drawer").GetComponent<MeshRenderer>().material.mainTexture = texture;
        //kokoko

        //Texture image = Resources.Load<Texture>(temp.data.Split(',')[0]);
        //MeshRenderer drawer = g.transform.Find("Drawer").gameObject.GetComponent<MeshRenderer>();
        //drawer.material.mainTexture = image;

        float rate = float.Parse(temp.data.Split(',')[1].Split('_')[1]) / float.Parse(temp.data.Split(',')[1].Split('_')[0]);
        g.transform.Find("Drawer").gameObject.transform.localScale = new Vector3(1f, rate, 0.01f) * temp.size;

        string frameTitle = frames.options[frames.value].text.Remove(frames.options[frames.value].text.Length - 1);
        GameObject framePrefab = (GameObject)Resources.Load("Frames/" + frameTitle);
        Debug.Log("Frames/" + frameTitle);
        Instantiate(framePrefab, g.transform);

        g.transform.Find(frameTitle + "(Clone)").gameObject.transform.localScale = new Vector3(1f, rate * 1920f / 1080f, 40f);//(Mathf.Max(g.transform.Find("Drawer").gameObject.transform.localScale.x, g.transform.Find("Drawer").gameObject.transform.localScale.z) / 19.2f, g.transform.Find("Drawer").gameObject.transform.localScale.y / 10.8f, 1f);
        temp.frameTitle = frameTitle;
        //g.transform.Find("picture").gameObject.transform.localScale = new Vector3(1f, rate * 1920f/1080f, 1f);//(Mathf.Max(g.transform.Find("Drawer").gameObject.transform.localScale.x, g.transform.Find("Drawer").gameObject.transform.localScale.z) / 19.2f, g.transform.Find("Drawer").gameObject.transform.localScale.y / 10.8f, 1f);
        //Debug.Log(g.transform.Find("picture").gameObject.transform.localScale.x);


        sakuhinList.Add(temp);
        for (int i = 0; i < sakuhinList.Count; i++)
        {
            if (sakuhinList[i].g == g)
            {
                Debug.Log("stored correctly");
            }
        }

        saveWorth = true;
    }

    public static GameObject rittaiInstanceSet(string name)
    {
        GameObject prefab = (GameObject)Resources.Load("Models/" + name);
        GameObject tempDai = rittaiCont;
        return Instantiate(prefab, tempDai.transform);
    }

    void rittaiSet()
    {
        rittaiNum += 1;
        rittai temp;
        temp.data = rittais.options[rittais.value].text.Remove(rittais.options[rittais.value].text.Length - 1);

        GameObject gRit = rittaiInstanceSet(rittais.options[rittais.value].text.Remove(rittais.options[rittais.value].text.Length - 1));
        Vector3 daiCent = new Vector3(daiList[selectedDaimen].g.transform.position.x, daiList[selectedDaimen].g.transform.position.y * 2f, daiList[selectedDaimen].g.transform.position.z);
        gRit.transform.position = daiCent;
        gRit.transform.LookAt(daiCent - daiList[selectedDaimen].g.transform.forward);

        temp.id = rittaiNum;
        //temp.parentNum = selectedDaimen;
        temp.parentNum = daiList[selectedDaimen].id;
        temp.x = 0f;
        temp.y = 0f;
        temp.z = 0f;
        temp.theta = 0f;
        temp.size = 1f;
        temp.g = gRit;
        rittaiList.Add(temp);

        saveWorth = true;
    }

    public static void rittaiReDraw()
    {
        for (int i=0; i< rittaiList.Count; i++)
        {
            // idからlistの何番目かを調べる関数をここに入れる
            dai tempRef = daiList.Find(x => x.id == rittaiList[i].parentNum);
            for (int k = 0; k < daiList.Count; k++)
                Debug.Log("bagusagasi" + daiList[k].id.ToString() + "|" + rittaiList[i].parentNum.ToString());

            Vector3 daiCent = new Vector3(tempRef.g.transform.position.x, tempRef.g.transform.position.y * 2f, tempRef.g.transform.position.z);
            rittaiList[i].g.transform.position = daiCent + rittaiList[i].x * tempRef.g.transform.forward + rittaiList[i].z * tempRef.g.transform.right + rittaiList[i].y * tempRef.g.transform.up;
            //rittaiList[i].g.transform.LookAt(daiCent - daiList[selectedDaimen].g.transform.forward);
            rittaiList[i].g.transform.localEulerAngles = new Vector3(0f, rittaiList[i].theta + tempRef.theta, 0f);
            rittaiList[i].g.transform.localScale = Vector3.one * rittaiList[i].size;

        }
    }

    public static GameObject daiInstanceSet()
    {
        return Instantiate(boxObjPrefabd, boxesObjd.transform);
    }

    void daiSet()
    {
        dai temp2;
        //temp2.type = "20x10x8";
        temp2.type = daiss.options[daiss.value].text.Remove(daiss.options[daiss.value].text.Length - 1);

        GameObject gDai = daiInstanceSet();
        daiNum += 1;

        float xx;
        float zz;
        float height;

        if (temp2.type.Split('x').Length >= 2)
        {
            xx = float.Parse(temp2.type.Split('x')[0]);
            zz = float.Parse(temp2.type.Split('x')[2]);
            height = float.Parse(temp2.type.Split('x')[1]);
        }
        else
        {
            xx = 8;
            zz = 8;
            height = 2;
            gDai.GetComponent<MeshRenderer>().enabled = false;
            gDai.transform.Find("Daimen").gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        gDai.transform.localScale = new Vector3(xx, height, zz);
        gDai.transform.position = new Vector3(Player.playerPos.x, height / 2f, Player.playerPos.z) + 30f * new Vector3(Player.playerFow.x, 0f, Player.playerFow.z);
        temp2.x = gDai.transform.position.x;
        temp2.z = gDai.transform.position.z;
        temp2.id = daiNum;
        temp2.theta = 90f - 180f / Mathf.PI * Mathf.Atan2(Player.playerFow.z, Player.playerFow.x);
        gDai.transform.Rotate(new Vector3(0, 1, 0), temp2.theta);
        temp2.g = gDai;
        daiList.Add(temp2);

        saveWorth = true;

    }

    public static GameObject lightInstanceSet()
    {
        return Instantiate(boxLightPrefab, lightCont.transform);
    }

    void lightSet(Vector3 hitPoint)
    {
        lightNum += 1;

        string type = lights.options[lights.value].text.Remove(lights.options[lights.value].text.Length - 1);

        GameObject gTemp = lightInstanceSet();
        gTemp.transform.position = new Vector3(clickedGameObject.transform.position.x, clickedGameObject.transform.position.y-1.5f, hitPoint.z);

        light temp;
        temp.g = gTemp;
        temp.x = temp.g.transform.position.x;
        temp.y = temp.g.transform.position.y;
        temp.z = temp.g.transform.position.z;
        temp.gyokaku = 0f;
        temp.theta = 0f;
        temp.id = lightNum;
        temp.type = type; // 角


        if (type == "spot")
        {
            temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.Find("Spot Light").gameObject.GetComponent<Light>().intensity = 1.8f;
            temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.Find("Spot Light").gameObject.GetComponent<Light>().spotAngle = 40f;
            temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.Find("Spot Light").gameObject.GetComponent<Light>().range = 60f;
        }
        if (type == "wide")
        {
            Debug.Log("wide");
            temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.Find("Spot Light").gameObject.GetComponent<Light>().intensity = 1f;
            temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.Find("Spot Light").gameObject.GetComponent<Light>().spotAngle = 65f;
        }

        lightList.Add(temp);

        saveWorth = true;
    }

    void sakuhinMove()//Vector3 wallCenter, Vector3 wallFront, Vector3 wallRight, Vector3 wallUp)
    {
        float step = 0.03f;
        sakuhin temp;
        temp = sakuhinList[selectedSakuhin];


        if (Input.GetKey("up"))
        {
            temp.y = temp.y + step;
        }
        if (Input.GetKey("down"))
        {
            temp.y = temp.y - step;
        }
        if (Input.GetKey("right"))
        {
            temp.x = temp.x + step;
        }
        if (Input.GetKey("left"))
        {
            temp.x = temp.x - step;
        }
        if (sizeField.GetComponent<InputField>().text == "")
            temp.size = 1f;
        else
            temp.size = float.Parse(sizeField.GetComponent<InputField>().text);

        float x_mouse = Input.GetAxis("Mouse X");
        float y_mouse = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(0))
        {
            temp.x = temp.x + x_mouse * step * 21f;
            temp.y = temp.y + y_mouse * step * 21f;
        }

        sakuhinList[selectedSakuhin] = temp;

        sakuhinList[selectedSakuhin].g.transform.Find("Drawer").transform.Find("Waku").gameObject.SetActive(true);

        if (Input.GetKey(KeyCode.Return) || SelectRelease.selectReleaseReq)  // 選択の解除
        {
            SelectRelease.doneOut();
            sakuhinList[selectedSakuhin].g.transform.Find("Drawer").transform.Find("Waku").gameObject.SetActive(false);
            selectedSakuhin = -1;
            sizeField.SetActive(false);
            saveWorth = true;
        }

        if (Input.GetKey(KeyCode.Delete))  // 削除
        {
            Destroy(sakuhinList[selectedSakuhin].g);
            sakuhinList.RemoveAt(selectedSakuhin);
            selectedSakuhin = -1;
            sizeField.SetActive(false);
            saveWorth = true;
        }



    }

    void sakuhinSelect()
    {
        if (selectedSakuhin < 0)
        {
            for (int i = 0; i < sakuhinList.Count; i++)
            {
                if (sakuhinList[i].g == clickedGameObject)
                {
                    Debug.Log("found" + i.ToString());
                    selectedSakuhin = i;  // Idには基づいていない
                    selectedDaimen = -1;
                    break;
                }
            }
        }

    }

    void daiSelect()
    {
        if (selectedDai < 0)
        {
            for (int i = 0; i < daiList.Count; i++)
            {
                if (daiList[i].g == clickedGameObject)
                {
                    Debug.Log("found" + i.ToString());
                    selectedDai = i;  // Idには基づいていない

                    if (selectedDaimen >= 0)
                    {
                        daiList[selectedDaimen].g.transform.Find("DaimenWaku").gameObject.SetActive(false);
                        selectedDaimen = -1;
                    }
                    break;
                }
            }
        }
    }

    void rittaiSelect()
    {
        if (selectedRittai < 0)
        {
            for (int i = 0; i < rittaiList.Count; i++)
            {
                if (rittaiList[i].g.transform.Find("Model").gameObject == clickedGameObject)
                {
                    Debug.Log("found rittai" + i.ToString());
                    selectedRittai = i;  // Idには基づいていない
                    sizeField.SetActive(true);
                    sizeField.GetComponent<InputField>().text = rittaiList[selectedRittai].size.ToString();

                    break;
                }
            }
        }
    }

    void lightSelect()
    {
        if (selectedLight < 0)
        {
            for (int i = 0; i < lightList.Count; i++)
            {
                Debug.Log("searchig");
                if (lightList[i].g == clickedGameObject.transform.parent.gameObject)
                {
                    Debug.Log("found light" + i.ToString());
                    selectedLight = i;  // Idには基づいていない

                    break;
                }
            }
        }
    }

    void lightMove()
    {
        rotSlider.SetActive(true);
        float step = 4f;
        light temp;
        temp = lightList[selectedLight];

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 lightLine = new Vector3(0f, 0f, 1f);
        if (Vector3.Dot(Camera.main.transform.forward, lightLine) >= 0f)
        {
            temp.z = temp.z + step * scroll;
        }
        else
        {
            temp.z = temp.z - step * scroll;
        }

        if (Vector3.Dot(Camera.main.transform.forward, lightLine) >= 0f)
        {
            temp.z = temp.z + step * scroll; if (Input.GetKey("up"))
                temp.z = temp.z + 0.2f;
            if (Input.GetKey("down"))
                temp.z = temp.z - 0.2f;
        }
        else
        {
            if (Input.GetKey("up"))
                temp.z = temp.z - 0.2f;
            if (Input.GetKey("down"))
                temp.z = temp.z + 0.2f;
        }

        if (RotSliders.toBeProcessed)
        {
            temp.gyokaku = RotSliders.ver;
            temp.theta = RotSliders.hori;
            RotSliders.doneOut();
        }


        lightList[selectedLight] = temp;

        lightList[selectedLight].g.transform.Find("Waku").gameObject.SetActive(true);

        if (Input.GetKey(KeyCode.Return) || SelectRelease.selectReleaseReq)  // 選択の解除
        {
            SelectRelease.doneOut();
            lightList[selectedLight].g.transform.Find("Waku").gameObject.SetActive(false);
            rotSlider.SetActive(false);
            selectedLight = -1;
            saveWorth = true;
        }
        if (Input.GetKey(KeyCode.Delete))  // 削除
        {
            Destroy(lightList[selectedLight].g);
            lightList.RemoveAt(selectedLight);
            rotSlider.SetActive(false);
            selectedLight = -1;
            saveWorth = true;
        }
    }

    void daiMove()
    {
        float step = 0.05f;
        dai temp;
        temp = daiList[selectedDai];

        if (Input.GetKey("up"))
        {
            temp.x = temp.x + step * temp.g.transform.forward.x;
            temp.z = temp.z + step * temp.g.transform.forward.z;
        }
        if (Input.GetKey("down"))
        {
            temp.x = temp.x - step * temp.g.transform.forward.x;
            temp.z = temp.z - step * temp.g.transform.forward.z;
        }
        if (Input.GetKey("right"))
        {
            temp.x = temp.x + step * temp.g.transform.right.x;
            temp.z = temp.z + step * temp.g.transform.right.z;
        }
        if (Input.GetKey("left"))
        {
            temp.x = temp.x - step * temp.g.transform.right.x;
            temp.z = temp.z - step * temp.g.transform.right.z;
        }

        float x_mouse = Input.GetAxis("Mouse X");
        float y_mouse = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(0))
        {
            temp.x = temp.x + y_mouse * step * 10f * temp.g.transform.forward.x;
            temp.z = temp.z + y_mouse * step * 10f * temp.g.transform.forward.z;

            temp.x = temp.x + x_mouse * step * 10f * temp.g.transform.right.x;
            temp.z = temp.z + x_mouse * step * 10f * temp.g.transform.right.z;
        }

        float rotStep = 0.4f;
        if (RotButtons.buttonLR == 1)
        {
            temp.theta += rotStep;
            temp.g.transform.Rotate(new Vector3(0, 1, 0), rotStep);

            //Player.resetReq(daiList[selectedDai].g.transform.position, daiList[selectedDai].g.transform.forward, Vector3.Distance(daiList[selectedDai].g.transform.position, Player.playerPos));
            //RotButtons.done();
        }
        if (RotButtons.buttonLR == -1)
        {
            temp.theta -= rotStep;
            temp.g.transform.Rotate(new Vector3(0, 1, 0), -rotStep);
            //Player.resetReq(daiList[selectedDai].g.transform.position, daiList[selectedDai].g.transform.forward, Vector3.Distance(daiList[selectedDai].g.transform.position, Player.playerPos));
            //RotButtons.done();
        }


        try
        {
            if (isDoubleTapped && clickedGameObject.tag == "Tenjimen")
            {
                //Debug.Log(clickedGameObject.GetComponent<BoxCollider>().center.x);
                BoxCollider tenjimen = clickedGameObject.GetComponent<BoxCollider>();
                Transform wallInfo = clickedGameObject.GetComponent<Transform>();


                // 展示面の中心のワールド座標
                wallCenter = wallInfo.TransformPoint(tenjimen.center);
                Debug.Log("new added center: " + wallCenter);
                // 一番小さい向きを選ぶ
                // 展示面上の座標軸を右手系で取得
                if (Min<float>(tenjimen.size.x, tenjimen.size.y, tenjimen.size.z) == tenjimen.size.x)
                {
                    wallFront = wallInfo.TransformDirection(Vector3.right);
                    wallRight = wallInfo.TransformDirection(-1.0f * Vector3.forward);
                    wallUp = wallInfo.TransformDirection(Vector3.up);
                }
                else if (Min<float>(tenjimen.size.x, tenjimen.size.y, tenjimen.size.z) == tenjimen.size.y)
                {
                    wallFront = wallInfo.TransformDirection(Vector3.up);
                    wallRight = wallInfo.TransformDirection(-1.0f * Vector3.right);
                    wallUp = wallInfo.TransformDirection(Vector3.forward);
                }
                else
                {
                    wallFront = wallInfo.TransformDirection(Vector3.forward);
                    wallRight = wallInfo.TransformDirection(-1.0f * Vector3.right);
                    wallUp = wallInfo.TransformDirection(Vector3.up);
                }

                // プレイヤーの向きが法線ベクトルの正の向きになるようにする
                float cross = -1f * (wallCenter.x - Player.playerPos.x) * wallFront.x + (wallCenter.y - Player.playerPos.y) * wallFront.y + (wallCenter.z - Player.playerPos.z) * wallFront.z;
                if (cross < 0.0)
                {
                    wallFront = -1.0f * wallFront;
                    wallRight = -1.0f * wallRight;
                    wallUp = 1.0f * wallUp;
                }
                if (Min<float>(tenjimen.size.x, tenjimen.size.y, tenjimen.size.z) == tenjimen.size.x)
                {
                    wallFront = -1.0f * wallFront;  //行列の変換がちゃんとできないのでこんなとこで補正している…恥ずかしい
                }
                wallFront = -1.0f * wallFront; // 左手系にする

                Debug.Log("shomen" + wallFront.x.ToString() + ", " + wallFront.y.ToString() + ", " + wallFront.z.ToString());
                Debug.Log("migi x" + wallRight.x.ToString() + ", " + wallRight.y.ToString() + ", " + wallRight.z.ToString());
                Debug.Log("ue y" + wallUp.x.ToString() + ", " + wallUp.y.ToString() + ", " + wallUp.z.ToString());

                temp.g.transform.LookAt(temp.g.transform.position + 100f * wallFront);
                temp.g.transform.localEulerAngles = new Vector3(0f, temp.g.transform.localEulerAngles.y, 0f);
                temp.theta = temp.g.transform.localEulerAngles.y;
            }

        }
        catch (NullReferenceException e)
        {

        }




        rittaiReDraw();

        daiList[selectedDai] = temp;

        daiList[selectedDai].g.transform.Find("TenjidaiAWaku").gameObject.SetActive(true);

        if (Input.GetKey(KeyCode.Return) || SelectRelease.selectReleaseReq)  // 選択の解除
        {
            SelectRelease.doneOut();
            daiList[selectedDai].g.transform.Find("TenjidaiAWaku").gameObject.SetActive(false);
            selectedDai = -1;
            rotButton.SetActive(false);
            saveWorth = true;
        }

        if (Input.GetKey(KeyCode.Delete))  // 削除
        {
            /* 台の上の作品も削除 */
            for (int i=0; i<rittaiList.Count; i++)
            {
                if(rittaiList[i].parentNum == daiList[selectedDai].id)
                {
                    Destroy(rittaiList[i].g);
                } 
            }
            rittaiList.RemoveAll(p => p.parentNum == daiList[selectedDai].id);
            /* 台の上の作品も削除 */

            Destroy(daiList[selectedDai].g);
            daiList.RemoveAt(selectedDai);
            selectedDai = -1;
            rotButton.SetActive(false);
            saveWorth = true;
        }

    }

    void daimenSelect()
    {
        if (selectedDaimen < 0)
        {
            //Debug.Log("daimen select");
            for (int i = 0; i < daiList.Count; i++)
            {
                if (daiList[i].g == clickedGameObject.transform.parent.gameObject)
                {
                    Debug.Log("found daimen" + i.ToString());
                    selectedDaimen = i;  // Idには基づいていない
                    break;
                }
            }
        }
    }

    void daimenOperate()
    {
        daiList[selectedDaimen].g.transform.Find("DaimenWaku").gameObject.SetActive(true);
        rittaiDropdown.SetActive(true);
        daiDropdown.SetActive(false);
        Player.resetReq(daiList[selectedDaimen].g.transform.position, daiList[selectedDaimen].g.transform.forward, 20f);


        /* 台上の立体の操作 */
        if (selectedRittai >= 0) // 選択しているものがある場合
        {
            rittaiList[selectedRittai].g.transform.Find("Sentaku").gameObject.SetActive(true);
            rittaiDropdown.SetActive(false);
            daiDropdown.SetActive(false);
            rotButton.SetActive(true);

            /* 台の上の作品の移動 */
            float step = 0.1f;
            rittai temp;
            temp = rittaiList[selectedRittai];

            if (Input.GetKey("up"))
            {
                temp.x = temp.x + step;
            }
            if (Input.GetKey("down"))
            {
                temp.x = temp.x - step;
            }
            if (Input.GetKey("right"))
            {
                temp.z = temp.z + step;
            }
            if (Input.GetKey("left"))
            {
                temp.z = temp.z - step;
            }

            float x_mouse = Input.GetAxis("Mouse X");
            float y_mouse = Input.GetAxis("Mouse Y");
            if (Input.GetMouseButton(0))
            {
                temp.x = temp.x + y_mouse * step * 2f;
                temp.z = temp.z + x_mouse * step * 2f;
            }
            temp.y = temp.y + 5f * Input.GetAxis("Mouse ScrollWheel");



            float rotStep = 0.4f;
            if (RotButtons.buttonLR == 1)
            {
                temp.theta += rotStep;
                temp.g.transform.Rotate(new Vector3(0, 1, 0), rotStep);
                //RotButtons.done();
            }
            if (RotButtons.buttonLR == -1)
            {
                temp.theta -= rotStep;
                temp.g.transform.Rotate(new Vector3(0, 1, 0), -rotStep);
                //RotButtons.done();
            }
            if (sizeField.GetComponent<InputField>().text == "")
                temp.size = 1f;
            else
                temp.size = float.Parse(sizeField.GetComponent<InputField>().text);

            rittaiList[selectedRittai] = temp;
            rittaiReDraw();
            /* 台の上の作品の移動 */

            if (Input.GetKey(KeyCode.Return) || SelectRelease.selectReleaseReq)  // 選択の解除
            {
                SelectRelease.doneOut();
                rittaiList[selectedRittai].g.transform.Find("Sentaku").gameObject.SetActive(false);
                rittaiDropdown.SetActive(true);
                daiDropdown.SetActive(true);
                selectedRittai = -1;
                rotButton.SetActive(false);
                sizeField.SetActive(false);
                Debug.Log("sentakukaijo rittai");
                saveWorth = true;
            }

            if (Input.GetKey(KeyCode.Delete) || SelectRelease.selectReleaseReq)  // 削除
            {
                Destroy(rittaiList[selectedRittai].g);
                rittaiList.RemoveAt(selectedRittai);
                selectedRittai = -1;
                rotButton.SetActive(false);
                saveWorth = true;
            }
        }
        /* 台上の立体の操作 */


        if (Input.GetKey(KeyCode.Return) || SelectRelease.selectReleaseReq)  // 選択の解除
        {
            SelectRelease.doneOut();
            daiList[selectedDaimen].g.transform.Find("DaimenWaku").gameObject.SetActive(false);
            rittaiDropdown.SetActive(false);
            daiDropdown.SetActive(true);
            selectedDaimen = -1;
            rotButton.SetActive(false);
            if (selectedRittai >= 0)
            {
                rittaiList[selectedRittai].g.transform.Find("Sentaku").gameObject.SetActive(false);
                selectedRittai = -1;
                sizeField.SetActive(false);
            }
            saveWorth = true;

        }

    }

    public void uiReset()
    {
        panelTogglePic.isOn = true;
        panelToggle3D.isOn = false;
        panelToggleLig.isOn = false;
        sizeField.SetActive(false);
        rotButton.SetActive(false);
        rotSlider.SetActive(false);
        detailPanel3D.SetActive(false);
        detailPanelPic.SetActive(true);
        detailPanelLig.SetActive(false);
    }

    public void sakuhinReset()
    {

        /*TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("sakuhins", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
        Debug.Log(TextLines);
        string[] items = TextLines.Split('\n');*/
        string readTxt = "";
        FileInfo fi = new FileInfo(Application.dataPath + "/MyResources/sakuhins.txt");
        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                readTxt = sr.ReadToEnd();
                Debug.Log(readTxt);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        string[] items = readTxt.Split('\n');
        sakuhins.ClearOptions();    //現在の要素をクリアする
        List<string> list = new List<string>(items);
        sakuhins.AddOptions(list);  //新しく要素のリストを設定する
        sakuhins.value = 0;         //デフォルトを設定(0～n-1)

        var image = ReadTexture(Application.dataPath + "/MyResources/" + sakuhins.options[sakuhins.value].text.Split(',')[0]);
        float rate = float.Parse(sakuhins.options[sakuhins.value].text.Split(',')[1].Split('_')[1]) / float.Parse(sakuhins.options[sakuhins.value].text.Split(',')[1].Split('_')[0]);
        sampleImage.GetComponent<RawImage>().texture = image;
        sampleImage.transform.localScale = new Vector3(1f, rate, 0.01f);
    }

    public void daiReset()
    {

        /*TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("dais", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
        Debug.Log(TextLines);
        string[] items = TextLines.Split('\n');*/
        string readTxt = "";
        FileInfo fi = new FileInfo(Application.dataPath + "/MyResources/dais.txt");
        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                readTxt = sr.ReadToEnd();
                Debug.Log(readTxt);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        string[] items = readTxt.Split('\n');

        daiss.ClearOptions();    //現在の要素をクリアする
        List<string> list = new List<string>(items);
        daiss.AddOptions(list);  //新しく要素のリストを設定する
        daiss.value = 0;         //デフォルトを設定(0～n-1)

    }

    public void rittaiReset()
    {
        /*TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("rittais", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
        Debug.Log(TextLines);
        string[] items = TextLines.Split('\n');*/
        string readTxt = "";
        FileInfo fi = new FileInfo(Application.dataPath + "/MyResources/rittais.txt");
        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                readTxt = sr.ReadToEnd();
                Debug.Log(readTxt);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        string[] items = readTxt.Split('\n');

        rittais.ClearOptions();    //現在の要素をクリアする
        List<string> list = new List<string>(items);
        rittais.AddOptions(list);  //新しく要素のリストを設定する
        rittais.value = 0;         //デフォルトを設定(0～n-1)
    }

    public void lightReset()
    {
        /*TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("lights", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
        Debug.Log(TextLines);
        string[] items = TextLines.Split('\n');*/
        string readTxt = "";
        FileInfo fi = new FileInfo(Application.dataPath + "/MyResources/lights.txt");
        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                readTxt = sr.ReadToEnd();
                Debug.Log(readTxt);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        string[] items = readTxt.Split('\n');

        lights.ClearOptions();    //現在の要素をクリアする
        List<string> list = new List<string>(items);
        lights.AddOptions(list);  //新しく要素のリストを設定する
        lights.value = 0;         //デフォルトを設定(0～n-1)
    }

    public void frameReset()
    {
        /*TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("frames", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる
        Debug.Log(TextLines);
        string[] items = TextLines.Split('\n');*/
        string readTxt = "";
        FileInfo fi = new FileInfo(Application.dataPath + "/MyResources/frames.txt");
        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                readTxt = sr.ReadToEnd();
                Debug.Log(readTxt);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        string[] items = readTxt.Split('\n');

        frames.ClearOptions();    //現在の要素をクリアする
        List<string> list = new List<string>(items);
        frames.AddOptions(list);  //新しく要素のリストを設定する
        frames.value = 0;         //デフォルトを設定(0～n-1)
    }

    public bool selectedSome()
    {
        bool temp = false;
        if (selectedSakuhin >= 0)
        {
            temp = true;
        }
        return temp;
    }

    public void OnValueChanged(int value)
    {
        var image = ReadTexture(Application.dataPath + "/MyResources/" + sakuhins.options[sakuhins.value].text.Split(',')[0]);
        //Texture image = Resources.Load<Texture>(sakuhins.options[sakuhins.value].text.Split(',')[0]);
        float rate = float.Parse(sakuhins.options[sakuhins.value].text.Split(',')[1].Split('_')[1]) / float.Parse(sakuhins.options[sakuhins.value].text.Split(',')[1].Split('_')[0]);
        sampleImage.GetComponent<RawImage>().texture = image;
        sampleImage.transform.localScale = new Vector3(1f, rate, 0.01f);

    }
    public void OnToggleChangedPic(bool value)
    {
        if (panelTogglePic.isOn)
        {
            detailPanelPic.SetActive(true);

            /*立体のoff*/
            panelToggle3D.isOn = false;
            if (selectedDai >= 0)
            {
                daiList[selectedDai].g.transform.Find("TenjidaiAWaku").gameObject.SetActive(false);
                selectedDai = -1;
                selectedDaimen = -1;
                rotButton.SetActive(false);
            }
            if (selectedDaimen >= 0)
            {
                daiList[selectedDaimen].g.transform.Find("DaimenWaku").gameObject.SetActive(false);
                selectedDaimen = -1;
            }
            if (selectedRittai >= 0)
            {
                rittaiList[selectedRittai].g.transform.Find("Sentaku").gameObject.SetActive(false);
                selectedRittai = -1;
                sizeField.SetActive(false);
            }
            /*立体のoff*/

            /* ライトのoff */
            detailPanelLig.SetActive(false);
            panelToggleLig.isOn = false;
            if (selectedLight >= 0)
            {
                rotSlider.SetActive(false);
                lightList[selectedLight].g.transform.Find("Waku").gameObject.SetActive(false);
                rotSlider.SetActive(false);
                selectedLight = -1;
            }
            /* ライトのoff */

        }
        else
            detailPanelPic.SetActive(false);
        saveWorth = true;
    }
    public void OnToggleChanged3D(bool value)
    {
        if (panelToggle3D.isOn)
        {
            /* 画像のoff */
            detailPanel3D.SetActive(true);
            rittaiDropdown.SetActive(false);
            daiDropdown.SetActive(true);
            panelTogglePic.isOn = false;
            if (selectedSakuhin >= 0)
            {
                sakuhinList[selectedSakuhin].g.transform.Find("Drawer").transform.Find("Waku").gameObject.SetActive(false);
                selectedSakuhin = -1;
                sizeField.SetActive(false);
            }
            /* 画像のoff */

            /* ライトのoff */
            detailPanelLig.SetActive(false);
            panelToggleLig.isOn = false;
            if (selectedLight >= 0)
            {
                rotSlider.SetActive(false);
                lightList[selectedLight].g.transform.Find("Waku").gameObject.SetActive(false);
                rotSlider.SetActive(false);
                selectedLight = -1;
            }
            /* ライトのoff */

        }
        else
            detailPanel3D.SetActive(false);
        saveWorth = true;
    }

    public void OnToggleChangedLig(bool value)
    {
        if (panelToggleLig.isOn)
        {
            detailPanelLig.SetActive(true);

            /*画像のoff*/
            rittaiDropdown.SetActive(false);
            daiDropdown.SetActive(true);
            panelTogglePic.isOn = false;
            if (selectedSakuhin >= 0)
            {
                sakuhinList[selectedSakuhin].g.transform.Find("Drawer").transform.Find("Waku").gameObject.SetActive(false);
                selectedSakuhin = -1;
                sizeField.SetActive(false);
            }
            /*画像のoff*/

            /*立体のoff*/
            panelToggle3D.isOn = false;
            if (selectedDai >= 0)
            {
                daiList[selectedDai].g.transform.Find("TenjidaiAWaku").gameObject.SetActive(false);
                selectedDai = -1;
                selectedDaimen = -1;
                rotButton.SetActive(false);
            }
            if (selectedDaimen >= 0)
            {
                daiList[selectedDaimen].g.transform.Find("DaimenWaku").gameObject.SetActive(false);
                selectedDaimen = -1;
            }
            if (selectedRittai >= 0)
            {
                rittaiList[selectedRittai].g.transform.Find("Sentaku").gameObject.SetActive(false);
                selectedRittai = -1;
                sizeField.SetActive(false);
            }
            /*立体のoff*/

        }
        else
            detailPanelLig.SetActive(false);
        saveWorth = true;

    }

    public void OnModori()
    {
        Saver.OnRoading(modori.ToString());
        saveWorth = false;
        if (modori == 0)
        {
            if (saveSatu == modoriMax)
                modori = modoriMax;
        }
        else
        {
            modori--;
        }
        Debug.Log("modori(OnModori)" + modori.ToString() + saishin.ToString());
        Saver.OnRoading(modori.ToString());
    }

    public void OnSusumi()
    {
        saveWorth = false;
        if (saveSatu != modoriMax)
        {
            if (modori < saveSatu)
            {
                modori++;
            }
        }
        else
        {
            if (modori < modoriMax)
            {
                modori++;
            }
            else
            {
                modori = 0;
            }
        }

        Debug.Log("modori(OnSusumi)" + modori.ToString() + saishin.ToString());
        Saver.OnRoading(modori.ToString());
    }

    Texture ReadTexture(string path)
    {
        byte[] readBinary = ReadFile(path);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(readBinary);

        return texture;
    }

    byte[] ReadFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }

}
