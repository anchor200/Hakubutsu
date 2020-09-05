using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daimen : MonoBehaviour
{
    public GameObject warn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        warn.SetActive(false);
        Collider[] hitColliders = Physics.OverlapBox(this.transform.position, 0.5f * this.transform.localScale, this.transform.localRotation);
        int i = 0;
        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            //Debug.Log("Hit : " + hitColliders[i].name + i);
            if (hitColliders[i].tag == "Tenjimen")
            {
                warn.SetActive(true);
            }
            //Increase the number of Colliders in the array
            i++;
        }



    }

    /*void OnCollisionStay(Collision other)
    {
        Debug.Log("dai koli");
        if (other.gameObject.tag == "Tenjimen")
        {
            warn.SetActive(true);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Tenjimen")
        {
            warn.SetActive(false);
        }
    }*/
}
