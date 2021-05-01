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

    public void initTalk()//��ȭ ������ Dictionary init
    {
        talkData.Add(1, new string[] { "���� ����ް� ���⼭ ����ϴ°���?", "��縦 �ҷ��� �ڸ����� ������", "���� ���ٰ�?", "�����ٰ�", "������ �ٽ� ã�ƿ���" });
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
                while (!Input.GetKeyDown(KeyCode.Space)) { } //������ �Ʒ� ���� �ֱ� ����
            }
        }
    }
}
