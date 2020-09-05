using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO; //System.IO.FileInfo, System.IO.StreamReader, System.IO.StreamWriter
using System; //Exception
using System.Text;


public class Saver : MonoBehaviour
{

    public GameObject boxObjPrefab;
    public GameObject boxesObj;

    public InputField IOname;
    public static string ioName = "";


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
        ioName = IOname.text;
    }

    public void OnSave()
    {
        OnSaving();
    }

    public static void OnSaving(string modori = "")
    {
        //ボタン押しの保存かどうか、履歴用の保存か
        string datetimeStr = modori;
        string tempname = ioName;
        if (modori != "")
        {
            tempname = "";  // 元に戻す用のログのための保存の時は番号は変えない
        }

        // 取得する値: 年
        /*string datetimeStr = "";
        if (isJustForLog)
        {
            datetimeStr += System.DateTime.Now.Year.ToString();
            // 取得する値: 月
            datetimeStr += System.DateTime.Now.Month.ToString();
            // 取得する値: 日
            datetimeStr += System.DateTime.Now.Day.ToString();
            // 取得する値: 時
            datetimeStr += System.DateTime.Now.Hour.ToString();
            // 取得する値: 分
            datetimeStr += System.DateTime.Now.Minute.ToString();
            // 取得する値: 秒
            datetimeStr += System.DateTime.Now.Second.ToString();
            Debug.Log(datetimeStr);

            ioName = "";
        }
        else
        {
        }*/



        /* 画像の保存 */
        StreamWriter sw = new StreamWriter(Application.dataPath + "/MyResources/SavedData/LogDataPic" + tempname + datetimeStr + ".tsv", false); //true=追記 false=上書き

        string saveText = "ID\tdata\twallCenter\twallRight\twallUp\twallFront\tx\ty\tz\tsize\tframeTitle\n";
        for (int i=0; i<Tenjimen.sakuhinList.Count; i++)
        {
            string temp = Tenjimen.sakuhinList[i].id.ToString();
            temp += "\t" + Tenjimen.sakuhinList[i].data;
            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallCenter.x) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.sakuhinList[i].wallCenter.x.ToString();
            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallCenter.y) < 0.00001f)
                temp += "," + (0f).ToString();
            else
                temp += "," + Tenjimen.sakuhinList[i].wallCenter.y.ToString();
            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallCenter.z) < 0.00001f)
                temp += "," + (0f).ToString();
            else
                temp += "," + Tenjimen.sakuhinList[i].wallCenter.z.ToString();

            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallRight.x) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.sakuhinList[i].wallRight.x.ToString();
            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallRight.y) < 0.00001f)
                temp += "," + (0f).ToString();
            else
                temp += "," + Tenjimen.sakuhinList[i].wallRight.y.ToString();
            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallRight.z) < 0.00001f)
                temp += "," + (0f).ToString();
            else
                temp += "," + Tenjimen.sakuhinList[i].wallRight.z.ToString();

            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallUp.x) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.sakuhinList[i].wallUp.x.ToString();
            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallUp.y) < 0.00001f)
                temp += "," + (0f).ToString();
            else
                temp += "," + Tenjimen.sakuhinList[i].wallUp.y.ToString();
            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallUp.z) < 0.00001f)
                temp += "," + (0f).ToString();
            else
                temp += "," + Tenjimen.sakuhinList[i].wallUp.z.ToString();

            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallFront.x) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.sakuhinList[i].wallFront.x.ToString();
            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallFront.y) < 0.00001f)
                temp += "," + (0f).ToString();
            else
                temp += "," + Tenjimen.sakuhinList[i].wallFront.y.ToString();
            if (Mathf.Abs(Tenjimen.sakuhinList[i].wallFront.z) < 0.00001f)
                temp += "," + (0f).ToString();
            else
                temp += "," + Tenjimen.sakuhinList[i].wallFront.z.ToString();

            if (Mathf.Abs(Tenjimen.sakuhinList[i].x) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.sakuhinList[i].x.ToString();
            if (Mathf.Abs(Tenjimen.sakuhinList[i].y) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.sakuhinList[i].y.ToString();
            if (Mathf.Abs(Tenjimen.sakuhinList[i].z) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.sakuhinList[i].z.ToString();

            temp += "\t" + Tenjimen.sakuhinList[i].size.ToString() + "\t" + Tenjimen.sakuhinList[i].frameTitle;
            if (i != Tenjimen.sakuhinList.Count - 1)
            {
                temp += "\n";
            }
            saveText += temp;

            
        }


        sw.WriteLine(saveText);
        sw.Flush();
        sw.Close();
        /* 画像の保存 */

        /* 展示台の保存 */
        StreamWriter sw2 = new StreamWriter(Application.dataPath + "/MyResources/SavedData/LogDataDai" + tempname + datetimeStr + ".tsv", false); //true=追記 false=上書き
        saveText = "ID\ttype\tx\tz\ttheta\n";
        for (int i = 0; i < Tenjimen.daiList.Count; i++)
        {
            string temp = Tenjimen.daiList[i].id.ToString();
            temp += "\t" + Tenjimen.daiList[i].type;
            if (Mathf.Abs(Tenjimen.daiList[i].x) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.daiList[i].x.ToString();
            if (Mathf.Abs(Tenjimen.daiList[i].z) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.daiList[i].z.ToString();
            if (Mathf.Abs(Tenjimen.daiList[i].theta) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.daiList[i].theta.ToString();
            if (i != Tenjimen.daiList.Count - 1)
            {
                temp += "\n";
            }
            saveText += temp;

        }
        sw2.WriteLine(saveText);
        sw2.Flush();
        sw2.Close();

        /* 展示台の保存 */

        /* 台上の立体の保存 */
        StreamWriter sw3 = new StreamWriter(Application.dataPath + "/MyResources/SavedData/LogDataRit" + tempname + datetimeStr + ".tsv", false); //true=追記 false=上書き
        saveText = "id\tdata\tx\ty\tz\ttheta\tsize\tparent\n";
        for (int i = 0; i < Tenjimen.rittaiList.Count; i++)
        {
            string temp = Tenjimen.rittaiList[i].id.ToString() + "\t" + Tenjimen.rittaiList[i].data.ToString();
            if (Mathf.Abs(Tenjimen.rittaiList[i].x) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.rittaiList[i].x.ToString();
            if (Mathf.Abs(Tenjimen.rittaiList[i].y) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.rittaiList[i].y.ToString();
            if (Mathf.Abs(Tenjimen.rittaiList[i].z) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.rittaiList[i].z.ToString();
            if (Mathf.Abs(Tenjimen.rittaiList[i].theta) < 0.00001f)
                temp += "\t" + (0f).ToString();
            else
                temp += "\t" + Tenjimen.rittaiList[i].theta.ToString();
            temp += "\t" + Tenjimen.rittaiList[i].size.ToString();
            temp += "\t" + Tenjimen.rittaiList[i].parentNum.ToString();

            if (i != Tenjimen.rittaiList.Count - 1)
            {
                temp += "\n";
            }
            saveText += temp;

        }
        sw3.WriteLine(saveText);
        sw3.Flush();
        sw3.Close();
        /* 台上の立体の保存 */

        /* ライトの保存 */
        StreamWriter sw4 = new StreamWriter(Application.dataPath + "/MyResources/SavedData/LogDataLit" + tempname + datetimeStr + ".tsv", false); //true=追記 false=上書き
        saveText = "id\ttype\tx\ty\tz\ttheta\tgyokaku\n";
        for (int i = 0; i < Tenjimen.lightList.Count; i++)
        {
            string temp = Tenjimen.lightList[i].id.ToString() + "\t" + Tenjimen.lightList[i].type.ToString();
            temp += "\t" + Tenjimen.lightList[i].x.ToString();
            temp += "\t" + Tenjimen.lightList[i].y.ToString();
            temp += "\t" + Tenjimen.lightList[i].z.ToString();
            temp += "\t" + Tenjimen.lightList[i].gyokaku.ToString();
            temp += "\t" + Tenjimen.lightList[i].theta.ToString();

            if (i != Tenjimen.lightList.Count - 1)
            {
                temp += "\n";
            }
            saveText += temp;

        }
        sw4.WriteLine(saveText);
        sw4.Flush();
        sw4.Close();

        /* ライトの保存 */



    }

    public void OnRoad()
    {
        OnRoading();
        Tenjimen.modori = 0;
        Tenjimen.saveSatu = 0;
        Tenjimen.saishin = 0;
        Debug.Log("modori reset" + Tenjimen.modori);
        Tenjimen.saveWorth = true;
    }


    public static void OnRoading(string modori = "")
    {
        if (modori != "")
        {
            ioName = modori;
            // road back
        }

        //Tenjimen.selectedSakuhin = -1;
        //Tenjimen.selectedDai = -1;
        //Tenjimen.selectedDaimen = -1;
        //Tenjimen.selectedRittai = -1;
        //Tenjimen.selectedLight = -1;
        //Tenjimen.uiResetReq = true;

        /*画像の読み込み*/
        for (int i=0; i< Tenjimen.sakuhinList.Count; i++)  // 一旦全消し
        {
            Destroy(Tenjimen.sakuhinList[i].g);
        }

        Tenjimen.sakuhinList.Clear();  // 一旦全消し

        List<string[]> iniRoad = ReadTSV("SavedData/LogDataPic" + ioName);
        Debug.Log("SavedData/LogDataPic" + ioName);
        for (int i=1; i< iniRoad.Count; i++)
        {
            if (iniRoad[i].Length < 9)
                break;
            Debug.Log(iniRoad[i][6]);
            Tenjimen.sakuhin temp;
            temp.x = float.Parse(iniRoad[i][6]);
            temp.y = float.Parse(iniRoad[i][7]);
            temp.z = float.Parse(iniRoad[i][8]);
            temp.wallCenter = new Vector3(float.Parse(iniRoad[i][2].Split(',')[0]), float.Parse(iniRoad[i][2].Split(',')[1]), float.Parse(iniRoad[i][2].Split(',')[2]));
            temp.wallRight = new Vector3(float.Parse(iniRoad[i][3].Split(',')[0]), float.Parse(iniRoad[i][3].Split(',')[1]), float.Parse(iniRoad[i][3].Split(',')[2]));
            temp.wallUp = new Vector3(float.Parse(iniRoad[i][4].Split(',')[0]), float.Parse(iniRoad[i][4].Split(',')[1]), float.Parse(iniRoad[i][4].Split(',')[2]));
            temp.wallFront = new Vector3(float.Parse(iniRoad[i][5].Split(',')[0]), float.Parse(iniRoad[i][5].Split(',')[1]), float.Parse(iniRoad[i][5].Split(',')[2]));
            temp.id = int.Parse(iniRoad[i][0]);
            temp.data = iniRoad[i][1];
            temp.size = float.Parse(iniRoad[i][9]);
            temp.frameTitle = iniRoad[i][10];

            temp.g = Tenjimen.instanceSet();

            temp.g.transform.position = temp.wallCenter - temp.z * temp.wallFront + temp.x * temp.wallRight + temp.y * temp.wallUp;
            temp.g.transform.LookAt(temp.wallCenter - temp.z * temp.wallFront + temp.x * temp.wallRight + temp.y * temp.wallUp + 100f * temp.wallFront);
            temp.g.tag = "Sakuhin";

            //Texture image = Resources.Load<Texture>(temp.data.Split(',')[0]);
            //MeshRenderer drawer = temp.g.transform.Find("Drawer").gameObject.GetComponent<MeshRenderer>();
            //drawer.material.mainTexture = image;
            var texture = ReadTexture(Application.dataPath + "/MyResources/" + temp.data.Split(',')[0]);
            Texture drawer = temp.g.transform.Find("Drawer").GetComponent<MeshRenderer>().material.mainTexture = texture;

            float rate = float.Parse(temp.data.Split(',')[1].Split('_')[1]) / float.Parse(temp.data.Split(',')[1].Split('_')[0]);
            temp.g.transform.Find("Drawer").gameObject.transform.localScale = new Vector3(1f, rate, 0.01f) * temp.size;

            string frameTitle = temp.frameTitle;
            GameObject framePrefab = (GameObject)Resources.Load("Frames/" + frameTitle);
            Debug.Log("Frames/" + frameTitle);
            Instantiate(framePrefab, temp.g.transform);
            temp.g.transform.Find(frameTitle + "(Clone)").gameObject.transform.localScale = new Vector3(1f, rate * 1920f / 1080f, 40f) * temp.size;


            Tenjimen.sakuhinList.Add(temp);

        }
        if (Tenjimen.sakuhinList.Count - 1 >= 0)
            Tenjimen.sakuhinNum = Tenjimen.sakuhinList[Tenjimen.sakuhinList.Count-1].id;
        /*画像の読み込み*/

        /*台の読み込み*/
        for (int i=0; i< Tenjimen.daiList.Count; i++)  // 一旦全消し
        {
            Destroy(Tenjimen.daiList[i].g);
        }

        Tenjimen.daiList.Clear();  // 一旦全消し

        iniRoad = ReadTSV("SavedData/LogDataDai" + ioName);
        Debug.Log(iniRoad.Count.ToString());
        for (int i=1; i< iniRoad.Count; i++)
        {
            if (iniRoad[i].Length < 5)
                break;
            Tenjimen.dai temp;
            temp.id = int.Parse(iniRoad[i][0]);
            temp.type = iniRoad[i][1];
            temp.x = float.Parse(iniRoad[i][2]);
            temp.z = float.Parse(iniRoad[i][3]);
            temp.theta = float.Parse(iniRoad[i][4]);
            temp.g = Tenjimen.daiInstanceSet();

            float xx;
            float zz;
            float height;

            if (temp.type.Split('x').Length >= 2)
            {
                xx = float.Parse(temp.type.Split('x')[0]);
                zz = float.Parse(temp.type.Split('x')[2]);
                height = float.Parse(temp.type.Split('x')[1]);
            }
            else
            {
                xx = 8;
                zz = 8;
                height = 2;
                temp.g.GetComponent<MeshRenderer>().enabled = false;
                temp.g.transform.Find("Daimen").gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

            temp.g.transform.position = new Vector3(temp.x, height/2f, temp.z);
            temp.g.transform.localScale = new Vector3(xx, height, zz);
            temp.g.transform.Rotate(new Vector3(0, 1, 0), temp.theta);
            Tenjimen.daiList.Add(temp);
        }
        if (Tenjimen.daiList.Count - 1 >= 0)
            Tenjimen.daiNum = Tenjimen.daiList[Tenjimen.daiList.Count - 1].id;
        /*台の読み込み*/

        /*台上の立体の読み込み*/
        for (int i = 0; i < Tenjimen.rittaiList.Count; i++)  // 一旦全消し
        {
            Destroy(Tenjimen.rittaiList[i].g);
        }

        Tenjimen.rittaiList.Clear();  // 一旦全消し

        iniRoad = ReadTSV("SavedData/LogDataRit" + ioName);
        Debug.Log(iniRoad.Count.ToString());
        for (int i = 1; i < iniRoad.Count; i++)
        {
            if (iniRoad[i].Length < 5)
                break;
            Tenjimen.rittai temp;
            temp.id = int.Parse(iniRoad[i][0]);
            temp.data = iniRoad[i][1];
            temp.x = float.Parse(iniRoad[i][2]);
            temp.y = float.Parse(iniRoad[i][3]);
            temp.z = float.Parse(iniRoad[i][4]);
            temp.theta = float.Parse(iniRoad[i][5]);
            temp.size = float.Parse(iniRoad[i][6]);
            temp.parentNum = int.Parse(iniRoad[i][7]);

            temp.g = Tenjimen.rittaiInstanceSet(temp.data);
            Tenjimen.rittaiList.Add(temp);
            Debug.Log(iniRoad[i][1] + "parent" + temp.parentNum.ToString() + "moto" + iniRoad[i][7]);
        }
        Tenjimen.rittaiReDraw();
        if (Tenjimen.rittaiList.Count - 1 >= 0)
            Tenjimen.rittaiNum = Tenjimen.rittaiList[Tenjimen.rittaiList.Count - 1].id;
        /*台上の立体の読み込み*/

        /* ライトの読み込み */
        for (int i = 0; i < Tenjimen.lightList.Count; i++)  // 一旦全消し
        {
            Destroy(Tenjimen.lightList[i].g);
        }

        Tenjimen.lightList.Clear();  // 一旦全消し

        iniRoad = ReadTSV("SavedData/LogDataLit" + ioName);
        for (int i = 1; i < iniRoad.Count; i++)
        {
            if (iniRoad[i].Length < 5)
                break;
            Tenjimen.light temp;
            temp.id = int.Parse(iniRoad[i][0]);
            temp.type = iniRoad[i][1];
            temp.x = float.Parse(iniRoad[i][2]);
            temp.y = float.Parse(iniRoad[i][3]);
            temp.z = float.Parse(iniRoad[i][4]);
            temp.gyokaku = float.Parse(iniRoad[i][5]);
            temp.theta = float.Parse(iniRoad[i][6]);

            temp.g = Tenjimen.lightInstanceSet();
            Tenjimen.lightList.Add(temp);
            temp.g.transform.position = new Vector3(temp.x, temp.y, temp.z);
            temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.localEulerAngles = new Vector3(0f, temp.theta, temp.gyokaku);

            if (temp.type == "spot")
            {
                temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.Find("Spot Light").gameObject.GetComponent<Light>().intensity = 1.8f;
                temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.Find("Spot Light").gameObject.GetComponent<Light>().spotAngle = 40f;
                temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.Find("Spot Light").gameObject.GetComponent<Light>().range = 60f;
            }
            if (temp.type == "wide")
            {
                Debug.Log("wide");
                temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.Find("Spot Light").gameObject.GetComponent<Light>().intensity = 1f;
                temp.g.transform.Find("Arm").gameObject.transform.Find("Top").gameObject.transform.Find("Spot Light").gameObject.GetComponent<Light>().spotAngle = 65f;
            }


        }
        if (Tenjimen.lightList.Count - 1 >= 0)
            Tenjimen.lightNum = Tenjimen.lightList[Tenjimen.lightList.Count - 1].id;

        /* ライトの読み込み */

    }

    public static List<string[]> ReadTSV(string filename)
    {
        List<string[]> tempList = new List<string[]>();

        FileInfo fi = new FileInfo(Application.dataPath + "/MyResources/" + filename + ".tsv");
        try
        {
            StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8);

            while (sr.Peek() != -1)
            {
                string line = sr.ReadLine();
                tempList.Add(line.Split('\t'));
                // Debug.Log(line);
            }
            sr.Close();

        }
        catch (Exception e)
        {
            string[] err = { "新春シャンソンショー" };
            tempList.Add(err);
        }
        return tempList;

    }
    public static Texture ReadTexture(string path)
    {
        byte[] readBinary = ReadFile(path);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(readBinary);

        return texture;
    }

    public static byte[] ReadFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }
}

