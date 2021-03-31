using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour: Unity's inner class
public class Tile : MonoBehaviour // :(콜론) = 상속
{
    // 좌표 관련 파라미터
    private float Original_x, Original_y;
    private float Clicked_x, Clicked_y;
    private Vector2 MovedVector;

    // Start is called before the first frame update
    void Start()
    {
        Original_x = this.transform.localPosition.x;
        Original_y = this.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * 모바일 환경에서 Mouse가 아닌 Touch 관련 이벤트 시 처리 코드
        if(Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log("터치 시작");
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Debug.Log("터치 종료");
            }
        }
        */
    }

    private void OnMouseDown()
    {
        Clicked_x = Input.mousePosition.x;
        Clicked_y = Input.mousePosition.y;
    }

    private void OnMouseDrag()
    {
        MovedVector = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        // 임시 좌표값
        Vector2 tmpVec = new Vector2();

        // X좌표 이동값이 더 큰지 Y좌표 이동값이 더 큰지 비교
        if (Mathf.Abs(MovedVector.x - Clicked_x) > Mathf.Abs(MovedVector.y - Clicked_y))
        {
            // X좌표 이동
            // +
            if (MovedVector.x - Clicked_x > 0 && int.Parse(this.name.Split(',')[0]) < (Board.m_Width - 1))
            {
                string targetName = (int.Parse(this.name.Split(',')[0]) + 1).ToString() + "," + this.name.Split(',')[1];
                Debug.Log("this.name: " + this.name);
                Debug.Log("targetName: " + targetName);
                GameObject target = GameObject.Find(targetName);

                // 좌표값 Swap
                tmpVec = this.transform.localPosition;
                this.transform.localPosition = target.transform.localPosition;
                target.transform.localPosition = tmpVec;

                // 오브젝트명 Swap
                target.name = this.name;
                this.name = targetName;
            }
            // -
            if (MovedVector.x - Clicked_x < 0 && int.Parse(this.name.Split(',')[0]) > 0)
            {
                string targetName = (int.Parse(this.name.Split(',')[0]) + -1).ToString() + "," + this.name.Split(',')[1];
                Debug.Log("this.name: " + this.name);
                Debug.Log("targetName: " + targetName);
                GameObject target = GameObject.Find(targetName);

                // 좌표값 Swap
                tmpVec = this.transform.localPosition;
                this.transform.localPosition = target.transform.localPosition;
                target.transform.localPosition = tmpVec;

                // 오브젝트명 Swap
                target.name = this.name;
                this.name = targetName;
            }
        }
        else
        {
            // Y좌표 이동
            // +
            if (MovedVector.y - Clicked_y > 0 && int.Parse(this.name.Split(',')[1]) > 0)
            {
                string targetName = this.name.Split(',')[0] + "," + (int.Parse(this.name.Split(',')[1]) - 1).ToString();
                Debug.Log("this.name: " + this.name);
                Debug.Log("targetName: " + targetName);
                GameObject target = GameObject.Find(targetName);

                // 좌표값 Swap
                tmpVec = this.transform.localPosition;
                this.transform.localPosition = target.transform.localPosition;
                target.transform.localPosition = tmpVec;

                // 오브젝트명 Swap
                target.name = this.name;
                this.name = targetName;
            }
            // -
            if (MovedVector.y - Clicked_y < 0 && int.Parse(this.name.Split(',')[1]) < (Board.m_Height - 1))
            {
                string targetName = this.name.Split(',')[0] + "," + (int.Parse(this.name.Split(',')[1]) + 1).ToString();
                Debug.Log("this.name: " + this.name);
                Debug.Log("targetName: " + targetName);
                GameObject target = GameObject.Find(targetName);

                // 좌표값 Swap
                tmpVec = this.transform.localPosition;
                this.transform.localPosition = target.transform.localPosition;
                target.transform.localPosition = tmpVec;

                // 오브젝트명 Swap
                target.name = this.name;
                this.name = targetName;
            }
        }
    }
}
