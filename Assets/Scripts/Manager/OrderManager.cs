using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public List<int> order;

    // Start is called before the first frame update
    void Start()
    {
        order = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private string OutList()
    {
        string OrderList = null;

        return OrderList;
    }
    public void Order(int orderNumber,int position)
    {
        this.order.Add(orderNumber);
    }
    public void DOrder(int orderNumber)
    {
        this.order.Remove(orderNumber);
    }

}
