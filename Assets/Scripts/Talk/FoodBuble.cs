using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBuble : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void touchEvent()
    {
        Touch tempTouchs;//터치의 상태
        Vector3 touchPos;//터치의 위치
        bool touchBool;//터치이다라고 반환!

        if(Input.touchCount > 0)
        {
            for(int i = 0; i < Input.touchCount; i++)
            {
                tempTouchs = Input.GetTouch(i);
                if(tempTouchs.phase == TouchPhase.Began)
                {//해당 터치가 시작됐다면.
                    touchPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//월드 좌표로 터치 위치 불러온다.
                    touchBool = true;

                    break;
                }
            }
        }
    }


}
