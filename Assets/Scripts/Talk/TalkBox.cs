using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    void OnEnable()
    {
        if(state) StartCoroutine("Talk");
    }

    public void initTalk()//��ȭ ������ Dictionary init
    {
        talkData.Add(1, new string[] { "������" , "�������� �� ���Դ� Player���̾�", "�����, �����������񡤰�����ΰǺ� ���� �����ؼ�...", "�� ���ļ� �� 300�����̾�~!", "�ʹ� ���!" , "���� �ʸ��� ���Ը� ������ϱ�.", "��а��� �����ؼ� �� ������ ���ݾ� ��������. Player, ȭ����" });
    }

    
    public IEnumerator Talk()
    {
        Sprite[] sprite = Resources.LoadAll<Sprite>("Image/Customer/cha");
        if (this.talkData.ContainsKey(id))
        {
            string[] talk = this.talkData[id];

            foreach (string i in talk)
            {
                gameObject.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Customer/creditor");
                gameObject.transform.GetChild(2).GetComponent<Text>().text = "��ä����";
                if (i == "�ʹ� ���!")
                {
                    gameObject.transform.GetChild(0).GetComponent<Image>().sprite = sprite[0];
                    gameObject.transform.GetChild(2).GetComponent<Text>().text = "Player";
                }

                yield return ReadLine(i);
                while (!Input.GetMouseButtonDown(0)) yield return null;
            }
            //GameObject.Find("NPC").GetComponent<NPCManger>().stop = false;
            SceneManager.LoadScene("Project_C");
            state = false;
            gameObject.SetActive(false);
        }
    }

    IEnumerator ReadLine(string readtext)
    {
        float speed = 0.1f;
        for(int i = 0; i < readtext.Length + 1; i++)
        {
            //if (Input.GetKey(KeyCode.Space)) speed = 0;
            yield return new WaitForSeconds(speed);
            gameObject.transform.GetChild(1).GetComponent<Text>().text = readtext.Substring(0, i);
            //speed = 0.1f;
        }
    }

    public void talkBoxOn(int gid)
    {
        this.id = gid;
        //GameObject.Find("NPC").GetComponent<NPCManger>().stop = true;
        state = true;
        gameObject.SetActive(true);
    }
}
