using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour: Unity's inner class
public class Tile : MonoBehaviour // :(�ݷ�) = ���
{
    // ��ǥ ���� �Ķ����
    private Vector2 m_clickedVec;
    private Vector2 m_movedVec;
    private int m_movedCoordinate; // 0 == X, 1 == Y

    // ������Ʈ ���� �Ķ����
    private string m_targetName;
    private GameObject m_thisObj;
    private GameObject m_target;
    private ArrayList m_delTiles;

    // Start is called before the first frame update
    void Start()
    {
        m_targetName = string.Empty;
        m_thisObj = GameObject.Find(this.name);
        m_target = new GameObject();
        m_delTiles = new ArrayList();
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
        m_clickedVec = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        m_movedVec = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        // Ÿ�� Ÿ�� ����
        setTarget();

        // �ش� Ÿ�ϰ� Ÿ�� Ÿ���� ��ǥ�� ��ȯ
        moveCoordinate(m_thisObj, m_target);

        // �丮 ��� ȹ��(���� ���߱�)
        GetGradient();
    }

    /// <summary>
    /// Ÿ�� Ÿ�� ����
    /// </summary>
    private void setTarget()
    {
        // X��ǥ �̵����� �� ū�� Y��ǥ �̵����� �� ū�� ��
        if (Mathf.Abs(m_movedVec.x - m_clickedVec.x) > Mathf.Abs(m_movedVec.y - m_clickedVec.y))
        {
            m_movedCoordinate = 0;

            // X��ǥ +
            if (m_movedVec.x - m_clickedVec.x > 0 && int.Parse(this.name.Split(',')[0]) < (Board.m_Width - 1))
            {
                m_targetName = (int.Parse(this.name.Split(',')[0]) + 1).ToString() + "," + this.name.Split(',')[1];
            }
            // X��ǥ -
            if (m_movedVec.x - m_clickedVec.x < 0 && int.Parse(this.name.Split(',')[0]) > 0)
            {
                m_targetName = (int.Parse(this.name.Split(',')[0]) + -1).ToString() + "," + this.name.Split(',')[1];
            }
        }
        else
        {
            m_movedCoordinate = 1;

            // Y��ǥ +
            if (m_movedVec.y - m_clickedVec.y > 0 && int.Parse(this.name.Split(',')[1]) > 0)
            {
                m_targetName = this.name.Split(',')[0] + "," + (int.Parse(this.name.Split(',')[1]) - 1).ToString();
            }
            // Y��ǥ -
            if (m_movedVec.y - m_clickedVec.y < 0 && int.Parse(this.name.Split(',')[1]) < (Board.m_Height - 1))
            {
                m_targetName = this.name.Split(',')[0] + "," + (int.Parse(this.name.Split(',')[1]) + 1).ToString();
            }
        }

        // ������Ʈ �ҷ�����
        m_target = GameObject.Find(m_targetName);
    }

    /// <summary>
    /// �� ������Ʈ�� ��ġ ��ȯ
    /// </summary>
    /// <param name="obj1">������Ʈ 1</param>
    /// <param name="obj2">������Ʈ 2</param>
    private void moveCoordinate(GameObject obj1, GameObject obj2)
    {
        // �ӽð�
        Vector2 tmpVec = new Vector2();
        string tmpTile = string.Empty;
        string tmpName = string.Empty;

        if (m_movedCoordinate == 0 || m_movedCoordinate == 1)
        {
            // Board���� ��õ� Ÿ���� ���� Swap
            tmpTile = Board.m_Tiles[int.Parse(obj1.name.Split(',')[0]), int.Parse(obj1.name.Split(',')[1])];
            Board.m_Tiles[int.Parse(obj1.name.Split(',')[0]), int.Parse(obj1.name.Split(',')[1])] = Board.m_Tiles[int.Parse(obj2.name.Split(',')[0]), int.Parse(obj2.name.Split(',')[1])];
            Board.m_Tiles[int.Parse(obj2.name.Split(',')[0]), int.Parse(obj2.name.Split(',')[1])] = tmpTile;

            // ��ǥ�� Swap
            tmpVec = obj1.transform.localPosition;
            obj1.transform.localPosition = obj2.transform.localPosition;
            obj2.transform.localPosition = tmpVec;

            // �̸� Swap
            tmpName = obj1.name;
            obj1.name = obj2.name;
            obj2.name = tmpName;
        }
    }

    /// <summary>
    /// ���� ���߱� �Լ�
    /// </summary>
    private void GetGradient()
    {
        int flagSuccess = 0;
  
        // ���η� ���� �̵��� X���� �����ϴ� X�� 1�� / Y�� 2�� �˻�
        if (m_movedCoordinate == 0)
        {
            examAxisX(int.Parse(this.name.Split(',')[0]));

            examAxisY(int.Parse(this.name.Split(',')[1]));
            examAxisY(int.Parse(m_targetName.Split(',')[1]));
        }
        // ���η� ���� �̵��� Y���� �����ϴ� Y�� 1�� / X�� 2�� �˻�
        else
        {
            examAxisX(int.Parse(this.name.Split(',')[0]));
            examAxisX(int.Parse(m_targetName.Split(',')[0]));
            
            examAxisY(int.Parse(m_targetName.Split(',')[1]));
        }

        if (delTiles())
        {
            // ������ ���� ���(��) ��ſ� �� ��� ����
            createGradient();
        }
        else
        {
            // �� ������Ʈ�� ���� ��ǥ������ ��������
            moveCoordinate(m_target, m_thisObj);
        }
    }

    /// <summary>
    /// �ش� X�࿡ ���� ���־��� Ÿ���� �ִ��� �˻��ϴ� �Լ�
    /// </summary>
    /// <param name="basePointX">X�� ���� ��ǥ</param>
    private void examAxisX(int basePointX)
    {
        int delCnt = 0;
        int delFlag = 0;
        string beforeTile = string.Empty;
        for (int y = 0; y < Board.m_Height; y++)
        {
            // delCnt �� delFlag �� ���
            if (y > 0 && Board.m_Tiles[basePointX, y].Equals(beforeTile)) delCnt++;
            else
            {
                delCnt = 0;
                delFlag = 1;
            }
            if (y == Board.m_Height - 1) delFlag = 1;

            // m_delTiles ��� �߰�
            if (delCnt >= 2 && delFlag == 1)
            {
                for(int cnt = 0; cnt <= delCnt; cnt++)
                {
                    string objCoordi = basePointX.ToString() + "," + (y - cnt).ToString();
                    if (!m_delTiles.Contains(objCoordi)) m_delTiles.Add(objCoordi);
                }

                delCnt = 0;
                delFlag = 0;
            }

            beforeTile = Board.m_Tiles[basePointX, y];
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="basePointY">Y�� ���� ��ǥ</param>
    private void examAxisY(int basePointY)
    {
        int delCnt = 0;
        int delFlag = 0;
        string beforeTile = string.Empty;
        for (int x = 0; x < Board.m_Width; x++)
        {
            // delCnt �� delFlag �� ���
            if (x > 0 && Board.m_Tiles[x, basePointY].Equals(beforeTile)) delCnt++;
            else
            {
                delCnt = 0;
                delFlag = 1;
            }
            if (x == Board.m_Width - 1) delFlag = 1;

            // m_delTiles ��� �߰�
            if (delCnt >= 2 && delFlag == 1)
            {
                for (int cnt = 0; cnt <= delCnt; cnt++)
                {
                    string objCoordi = (x - cnt).ToString() + "," + basePointY.ToString();
                    if (!m_delTiles.Contains(objCoordi)) m_delTiles.Add(objCoordi);
                }

                delCnt = 0;
                delFlag = 0;
            }

            beforeTile = Board.m_Tiles[x, basePointY];
        }
    }

    /// <summary>
    /// 3�� �̻� ���ӵ� �� ����� ����
    /// </summary>
    /// <returns>���� ������ true / ���н� false</returns>
    private bool delTiles()
    {
        bool result = false;

        if (m_delTiles.Count != 0)
        {
            for (int idx = 0; idx < m_delTiles.Count; idx++)
            {
                // 3�� �ʰ�(4�� �̻�)���� ������ �� �Ǵ� �� ���ľ� ��
                Destroy(GameObject.Find(m_delTiles[idx].ToString()));
            }

            result = true;
        }

        return result;
    }

    /// <summary>
    /// ������ �� ��ŭ �� ��� ����
    /// </summary>
    private void createGradient()
    {
        
    }
}
