using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManger : MonoBehaviour
{
    public int position;
    public int orderNumber;
    public int type;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    public void Customerinit(int orderNumber, int position, int type)
    {
        this.orderNumber = orderNumber;
        this.position = position;
        this.type = type;
    }

  
}
