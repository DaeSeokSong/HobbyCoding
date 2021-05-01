using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManger : MonoBehaviour
{
    public bool[] npcPosition = new bool[] { false, false, false };


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CreateCool");
    }


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) Customercreate();//테스트용 NPC생성
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject.Find("Canvas").transform.GetChild(0).GetComponent<TalkBox>().talkBoxOn(1);
        }
    }

    public void Customercreate()
    {
        int i = 0;
        int chaType = Random.Range(0, 3);
        int orderNumber = Random.Range(0, 3);
        orderNumber = 0;
        Object resource;

        Vector3 position = new Vector3(0, 0, 1);
        GameObject parent;
        GameObject createNPC;
        OrderManager orderMG = GameObject.Find("OrderManager").GetComponent<OrderManager>();


        switch (chaType)
        {
            case 0:
                resource = Resources.Load("Prefabs/NPC/YoungMan");
                break;
            case 1:
                resource = Resources.Load("Prefabs/NPC/YoungWoman");
                break;
            case 2:
                resource = Resources.Load("Prefabs/NPC/Woman");
                break;
            default:
                resource = Resources.Load("Prefabs/NPC/YoungMan");
                break;
        }

      
        for(i = 0; i < npcPosition.Length; i++)
        {
            if (!npcPosition[i])
            {
                npcPosition[i] = true;
                position = new Vector3(-6f, 1.2f, 1);
                break;
            }
        }

        


        parent = GameObject.Find("NPC");
        createNPC = Instantiate(resource, position, Quaternion.identity, parent.transform) as GameObject;
        createNPC.GetComponent<customerdefault>().Customerinit(orderNumber, i, chaType);

        //createNPC.GetComponent<>

        switch (orderNumber)
        {
            case 0:
                createNPC.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/Food/Menu1");
                break;
            case 1:
                createNPC.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/Food/Menu1");
                break;
            case 2:
                createNPC.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/Food/Menu1");
                break;
            default:
                Debug.Log("캐릭터 생성 후 주문 에러");
                break;
        }


        orderMG.Order(orderNumber, i);
    }



    
    IEnumerator CreateCool()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (GameObject.Find("NPC").transform.childCount < 3)//손님 들어올 확률
            {
                if (Random.Range(0, 10) < 9)
                {
                    Customercreate();
                }
            }
        }//IEnumerable과 햇갈리지 않기
        
    }
    

}
