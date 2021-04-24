using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customerdefault : CustomerManger
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelfRemove()
    {
        GameObject.Find("OrderManager").GetComponent<OrderManager>().DOrder(this.orderNumber);
        GameObject.Find("NPC").GetComponent<NPCManger>().npcPosition[this.position] = false;
    }
}
