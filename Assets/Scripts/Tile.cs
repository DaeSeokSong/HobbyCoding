using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour: Unity's inner class
public class Tile : MonoBehaviour // :(�ݷ�) = ���
{
    // ��ǥ ���� �Ķ����
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
         * ����� ȯ�濡�� Mouse�� �ƴ� Touch ���� �̺�Ʈ �� ó�� �ڵ�
        if(Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log("��ġ ����");
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Debug.Log("��ġ ����");
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
        // �ӽ� ��ǥ��
        Vector2 tmpVec = new Vector2();

        // X��ǥ �̵����� �� ū�� Y��ǥ �̵����� �� ū�� ��
        if (Mathf.Abs(MovedVector.x - Clicked_x) > Mathf.Abs(MovedVector.y - Clicked_y))
        {
            // X��ǥ �̵�
            // +
            if (MovedVector.x - Clicked_x > 0 && int.Parse(this.name.Split(',')[0]) < (Board.m_Width - 1))
            {
                string targetName = (int.Parse(this.name.Split(',')[0]) + 1).ToString() + "," + this.name.Split(',')[1];
                Debug.Log("this.name: " + this.name);
                Debug.Log("targetName: " + targetName);
                GameObject target = GameObject.Find(targetName);

                // ��ǥ�� Swap
                tmpVec = this.transform.localPosition;
                this.transform.localPosition = target.transform.localPosition;
                target.transform.localPosition = tmpVec;

                // ������Ʈ�� Swap
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

                // ��ǥ�� Swap
                tmpVec = this.transform.localPosition;
                this.transform.localPosition = target.transform.localPosition;
                target.transform.localPosition = tmpVec;

                // ������Ʈ�� Swap
                target.name = this.name;
                this.name = targetName;
            }
        }
        else
        {
            // Y��ǥ �̵�
            // +
            if (MovedVector.y - Clicked_y > 0 && int.Parse(this.name.Split(',')[1]) > 0)
            {
                string targetName = this.name.Split(',')[0] + "," + (int.Parse(this.name.Split(',')[1]) - 1).ToString();
                Debug.Log("this.name: " + this.name);
                Debug.Log("targetName: " + targetName);
                GameObject target = GameObject.Find(targetName);

                // ��ǥ�� Swap
                tmpVec = this.transform.localPosition;
                this.transform.localPosition = target.transform.localPosition;
                target.transform.localPosition = tmpVec;

                // ������Ʈ�� Swap
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

                // ��ǥ�� Swap
                tmpVec = this.transform.localPosition;
                this.transform.localPosition = target.transform.localPosition;
                target.transform.localPosition = tmpVec;

                // ������Ʈ�� Swap
                target.name = this.name;
                this.name = targetName;
            }
        }
    }
}
