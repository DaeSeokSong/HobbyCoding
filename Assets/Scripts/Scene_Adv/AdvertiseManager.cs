using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvertiseManager : MonoBehaviour
{
    public GameObject Talk;

    void Start()
    {
        Talk.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool isActive = Talk.activeSelf;
            if (isActive == true)
            {
                Talk.SetActive(false);

            }
        }

    }

    public void ShowTalkBox()
    {
        bool isActive = Talk.activeSelf;
        if (isActive != true)
        {
            Talk.SetActive(true);
        }
    }
}
