using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customerdefault : MonoBehaviour
{
    public int position;
    public int orderNumber;
    public int type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelfRemove(bool flag)//���� ���� �÷��׸� �־��� ������ ����� ��찡 2���� �̱� ������
    {
        if(flag == false) GameObject.Find("OrderManager").GetComponent<OrderManager>().DOrder(this.orderNumber);
        GameObject.Find("NPC").GetComponent<NPCManger>().npcPosition[this.position] = false;
    }

    public void TouchFoodEvent()//����ǳ�� ��ġ �� �� Ȯ�� �� ���� �����ϸ� ������Ʈ ����
    {
        OrderManager orderManager = GameObject.Find("OrderManager").GetComponent<OrderManager>();

        if (orderManager.SeveFood(this.orderNumber)) SelfRemove(true);//���� �����ϸ� ����
    }

    public void Customerinit(int orderNumber, int position, int type)//�մ� ������Ʈ ���� �� �⺻ �� ������ ���� �Լ�
    {
        this.orderNumber = orderNumber;
        this.position = position;
        this.type = type;
    }
}
