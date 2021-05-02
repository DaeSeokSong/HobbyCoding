using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CreditorEvent");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CreditorEvent()
    {
        yield return new WaitForSeconds(5f);
        //Debug.Log(GameObject.Find("Open").transform.GetChild(3).gameObject.SetActive(true));
        GameObject.Find("Open").transform.GetChild(3).GetComponent<TalkBox>().talkBoxOn(1);
        //GameObject.Find("NPC").GetComponent<NPCManger>().stop = true;
    }
}
