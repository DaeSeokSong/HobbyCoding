using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkBox : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    int id = 1;
    bool state = false;

    // Start is called before the first frame update
    void Start()
    {
        talkData = new Dictionary<int, string[]>();
        initTalk();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        if(state) StartCoroutine("Talk");
    }

    public void initTalk()//대화 관련한 Dictionary init
    {
        talkData.Add(1, new string[] { "누구 허락받고 여기서 장사하는거지?", "장사를 할려면 자릿세를 내야지", "돈이 없다고?", "빌려줄게", "망간에 다시 찾아오지" });
    }

    
    public IEnumerator Talk()
    {
        if (this.talkData.ContainsKey(id))
        {
            string[] talk = this.talkData[id];

            foreach (string i in talk)
            {
                yield return ReadLine(i);
                while (!Input.GetKey(KeyCode.Space)) yield return null;
            }
            state = false;
            gameObject.SetActive(false);
            
        }
        
    }

    IEnumerator ReadLine(string readtext)
    {
        float speed = 0.1f;
        for(int i = 0; i < readtext.Length + 1; i++)
        {
            if (Input.GetKey(KeyCode.Space)) speed = 0;
            yield return new WaitForSeconds(speed);
            gameObject.transform.GetChild(1).GetComponent<Text>().text = readtext.Substring(0, i);
        }
    }

    public void talkBoxOn(int gid)
    {
        this.id = gid;
        state = true;
        gameObject.SetActive(true);
    }
}
