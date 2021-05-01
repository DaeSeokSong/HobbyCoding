using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Favol : MonoBehaviour
{
    Image fillBar;
    OrderManager st;
    // Start is called before the first frame update
    void Start()
    {
        st = GameObject.Find("OrderManager").GetComponent<OrderManager>();
        fillBar = gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (st.favor != fillBar.fillAmount) fillBar.fillAmount = st.favor / 10f;
    }
}
