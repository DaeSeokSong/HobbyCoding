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
        Touch tempTouchs;//��ġ�� ����
        Vector3 touchPos;//��ġ�� ��ġ
        bool touchBool;//��ġ�̴ٶ�� ��ȯ!

        if(Input.touchCount > 0)
        {
            for(int i = 0; i < Input.touchCount; i++)
            {
                tempTouchs = Input.GetTouch(i);
                if(tempTouchs.phase == TouchPhase.Began)
                {//�ش� ��ġ�� ���۵ƴٸ�.
                    touchPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//���� ��ǥ�� ��ġ ��ġ �ҷ��´�.
                    touchBool = true;

                    break;
                }
            }
        }
    }


}
