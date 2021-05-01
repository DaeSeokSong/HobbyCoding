using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkBox : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    // Start is called before the first frame update
    void Start()
    {
        talkData = new Dictionary<int, string[]>();
        initTalk();
        Debug.Log(talkData);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initTalk()//대화 관련한 Dictionary init
    {
        talkData.Add(1, new string[] { "누구 허락받고 여기서 장사하는거지?", "장사를 할려면 자릿세를 내야지", "돈이 없다고?", "빌려줄게", "망간에 다시 찾아오지" });
    }

    public void Talk(int id)
    {
        Debug.Log(talkData);
        Debug.Log(gameObject.transform.GetChild(1).GetComponent<Text>().text);
        if (talkData.ContainsKey(id))
        {
            string[] talk = talkData[id];
            foreach(string i in talk)
            {
                gameObject.transform.GetChild(1).GetComponent<Text>().text = i;
                while (!Input.GetKeyDown(KeyCode.Space)) { } //오른쪽 아래 끝에 넣기 위함
            }
        }
    }
}
