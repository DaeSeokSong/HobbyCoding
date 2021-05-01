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

    public void SelfRemove(bool flag)//셀프 지움 플레그를 넣어준 이유는 지우는 경우가 2가지 이기 때문에
    {
        if(flag == false) GameObject.Find("OrderManager").GetComponent<OrderManager>().DOrder(this.orderNumber);
        GameObject.Find("NPC").GetComponent<NPCManger>().npcPosition[this.position] = false;
    }

    public void TouchFoodEvent()//음식풍선 터치 시 값 확인 후 서빙 성고하면 오브젝트 지움
    {
        OrderManager orderManager = GameObject.Find("OrderManager").GetComponent<OrderManager>();

        if (orderManager.SeveFood(this.orderNumber)) SelfRemove(true);//서빙 성공하면 지움
    }

    public void Customerinit(int orderNumber, int position, int type)//손님 오브젝트 생성 시 기본 값 적용을 위한 함수
    {
        this.orderNumber = orderNumber;
        this.position = position;
        this.type = type;
    }
}
