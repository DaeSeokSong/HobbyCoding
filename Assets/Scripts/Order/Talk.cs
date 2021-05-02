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
        yield return new WaitForSeconds(1f);
        gameObject.transform.GetChild(4).transform.GetChild(0).GetComponent<TalkBox>().talkBoxOn(1);
    }
}
