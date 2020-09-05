
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	static bool viewResReq;
	static Vector3 pRef;
	static Vector3 pFow;
	static float dist;

	Vector3 begp;
	Vector3 bega;
	Vector3 endp;
	Vector3 enda;
	Vector3 tate;

	Vector3 nonColliPos;

	public float speed = 10.0f;
	public static Vector3 playerPos;
	public static Vector3 playerFow;

	int loopInd = 0;

	// Start is called before the first frame update
	void Start()
	{
		playerPos = transform.position;
		viewResReq = false;
		nonColliPos = playerPos;
	}

	// Update is called once per frame
	void Update()
	{
		playerPos = transform.position;
		playerFow = transform.forward;

		if (Input.GetKey(KeyCode.W))
		{
			transform.position += transform.forward * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S))
		{
			transform.position -= transform.forward * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.position += transform.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.position -= transform.right * speed * Time.deltaTime;
		}

		if (viewResReq)
		{
			loopInd = 0;
			begp = playerPos;
			bega = playerPos + 1 * playerFow;
			endp = pRef - dist * pFow;
			enda = pRef;
			tate = transform.Find("Main Camera").gameObject.transform.localEulerAngles;
			Debug.Log(tate.x);
			if (tate.x >= 180f)
            {
				tate = new Vector3(tate.x - 360, tate.y, tate.z);
            }
			viewResReq = false;
			lookSousa();

			//float temp = transform.position.y;
			//transform.position = pRef - dist * pFow;
			//transform.LookAt(pRef);
			//transform.position = new Vector3(transform.position.x, temp, transform.position.z);
			//viewResReq = false;
		}


		if (Input.GetKey(KeyCode.R))
        {
			transform.position = new Vector3(-18.8f, 8f, 14.4f);
			transform.localEulerAngles = new Vector3(0f,0f,0f);
			Camera.main.fieldOfView = CameraDolly.defField;
			transform.Find("Main Camera").gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

		}

		Collider[] hitColliders = Physics.OverlapBox(this.transform.position, 0.7f * this.transform.localScale, this.transform.localRotation);
		int i = 0;
		bool temp = false;
		//Check when there is a new collider coming into contact with the box
		while (i < hitColliders.Length)
		{
			if (hitColliders[i].tag == "Tenjimen" || hitColliders[i].tag == "Sakuhin" || hitColliders[i].tag == "Dai" || hitColliders[i].tag != "Untagged")
			{
				temp = true;
			}
			i++;
		}
		if (temp)
        {
			transform.position = transform.position + (nonColliPos - transform.position) * 1f;

		}
        else
        {
			nonColliPos = playerPos;
        }

	}

	public static void resetReq(Vector3 pReft, Vector3 pFowt, float distt)
	{
		pRef = pReft;
		pFow = pFowt;
		dist = distt;
		viewResReq = true;

	}

	void lookSousa()
    {
		if (loopInd++ < 70)
        {
			float comp = (float)loopInd / 70f;
			transform.position = comp * endp + (1f - comp) * begp;
			transform.LookAt(comp * enda + (1f - comp) * bega);
			transform.position = new Vector3(transform.position.x, begp.y, transform.position.z);
			
			transform.Find("Main Camera").gameObject.transform.localEulerAngles = tate * (1f - comp);
			Invoke("lookSousa", 0.01f);
		}
        else
        {
			loopInd = 0;

		}

	}
}
