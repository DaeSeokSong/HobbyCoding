using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public List<int> order;
    public List<int> createFood;

    // Start is called before the first frame update
    void Start()
    {
        order = new List<int>();
        createFood = new List<int>();
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
    public void CookFood(int orderNumber)
    {
        this.createFood.Add(orderNumber);
    }
    public bool SeveFood(int orderNumber)
    {
        bool successed = false;

        foreach (int i in order)
        {
            if (i == orderNumber)
            {
                successed = true;
                break;
            }
        }
        if (successed)
        {
            foreach (int i in createFood)
            {
                if (i == orderNumber)
                {
                    this.createFood.Remove(orderNumber);
                    this.order.Remove(orderNumber);
                    return successed;
                }
            }
        }

        return false;
    }

}
