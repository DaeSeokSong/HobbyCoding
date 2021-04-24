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
        if (Input.GetKeyDown(KeyCode.Space)) Customercreate();//�׽�Ʈ�� NPC����
    }

    public void Customercreate()
    {
        int i = 0;
        int chaType = Random.Range(0, 2);
        int orderNumber = Random.Range(0, 3);
        Object resource;

        Vector3 position = new Vector3(0, 0, 0);
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
            default:
                resource = Resources.Load("Prefabs/NPC/YoungMan");
                break;
        }

        
        for(i = 0; i < npcPosition.Length; i++)
        {
            if (!npcPosition[i])
            {
                npcPosition[i] = true;
                position = new Vector3(-1.5f + ((float)i * 1.5f), 0, 0);
                break;
            }
        }

        


        parent = GameObject.Find("NPC");
        createNPC = Instantiate(resource, position, Quaternion.identity, parent.transform) as GameObject;
        createNPC.GetComponent<customerdefault>().Customerinit(orderNumber, i, chaType);



        switch (orderNumber)
        {
            case 0:
                createNPC.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/ball1");
                break;
            case 1:
                createNPC.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/banana");
                break;
            case 2:
                createNPC.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Image/box");
                break;
            default:
                Debug.Log("ĳ���� ���� �� �ֹ� ����");
                break;
        }


        orderMG.Order(orderNumber, i);
    }



    
    IEnumerator CreateCool()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(5f);
            if (Random.Range(0, 10) < 9)//�մ� ���� Ȯ��
            {
                if (GameObject.Find("NPC").transform.childCount < 3)
                {
                    Debug.Log("�մ� ����");
                    Customercreate();
                }
            }
        }//IEnumerable�� �ް����� �ʱ�
        
    }
    

}