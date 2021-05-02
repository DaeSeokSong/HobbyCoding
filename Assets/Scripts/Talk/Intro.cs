using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("GameIntro");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GameIntro()
    {
        yield return new WaitForSeconds(1f);
        gameObject.transform.GetChild(4).transform.GetChild(0).GetComponent<TalkBox>().talkBoxOn(1);
    }
}
