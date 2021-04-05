using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour: Unity's inner class
public class Tile : MonoBehaviour // :(콜론) = 상속
{
    // 좌표 관련 파라미터
    private Vector2 m_clickedVec;
    private Vector2 m_movedVec;
    private int m_movedCoordinate; // 0 == X, 1 == Y

    // 오브젝트 관련 파라미터
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
        m_clickedVec = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        m_movedVec = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        // 타겟 타일 세팅
        setTarget();

        // 해당 타일과 타겟 타일의 좌표값 교환
        moveCoordinate(m_thisObj, m_target);

        // 요리 재료 획득(퍼즐 맞추기)
        GetGradient();
    }

    /// <summary>
    /// 타겟 타일 세팅
    /// </summary>
    private void setTarget()
    {
        // X좌표 이동값이 더 큰지 Y좌표 이동값이 더 큰지 비교
        if (Mathf.Abs(m_movedVec.x - m_clickedVec.x) > Mathf.Abs(m_movedVec.y - m_clickedVec.y))
        {
            m_movedCoordinate = 0;

            // X좌표 +
            if (m_movedVec.x - m_clickedVec.x > 0 && int.Parse(this.name.Split(',')[0]) < (Board.m_Width - 1))
            {
                m_targetName = (int.Parse(this.name.Split(',')[0]) + 1).ToString() + "," + this.name.Split(',')[1];
            }
            // X좌표 -
            if (m_movedVec.x - m_clickedVec.x < 0 && int.Parse(this.name.Split(',')[0]) > 0)
            {
                m_targetName = (int.Parse(this.name.Split(',')[0]) + -1).ToString() + "," + this.name.Split(',')[1];
            }
        }
        else
        {
            m_movedCoordinate = 1;

            // Y좌표 +
            if (m_movedVec.y - m_clickedVec.y > 0 && int.Parse(this.name.Split(',')[1]) > 0)
            {
                m_targetName = this.name.Split(',')[0] + "," + (int.Parse(this.name.Split(',')[1]) - 1).ToString();
            }
            // Y좌표 -
            if (m_movedVec.y - m_clickedVec.y < 0 && int.Parse(this.name.Split(',')[1]) < (Board.m_Height - 1))
            {
                m_targetName = this.name.Split(',')[0] + "," + (int.Parse(this.name.Split(',')[1]) + 1).ToString();
            }
        }

        // 오브젝트 불러오기
        m_target = GameObject.Find(m_targetName);
    }

    /// <summary>
    /// 양 오브젝트간 위치 전환
    /// </summary>
    /// <param name="obj1">오브젝트 1</param>
    /// <param name="obj2">오브젝트 2</param>
    private void moveCoordinate(GameObject obj1, GameObject obj2)
    {
        // 임시값
        Vector2 tmpVec = new Vector2();
        string tmpTile = string.Empty;
        string tmpName = string.Empty;

        if (m_movedCoordinate == 0 || m_movedCoordinate == 1)
        {
            // Board내에 명시된 타일의 순서 Swap
            tmpTile = Board.m_Tiles[int.Parse(obj1.name.Split(',')[0]), int.Parse(obj1.name.Split(',')[1])];
            Board.m_Tiles[int.Parse(obj1.name.Split(',')[0]), int.Parse(obj1.name.Split(',')[1])] = Board.m_Tiles[int.Parse(obj2.name.Split(',')[0]), int.Parse(obj2.name.Split(',')[1])];
            Board.m_Tiles[int.Parse(obj2.name.Split(',')[0]), int.Parse(obj2.name.Split(',')[1])] = tmpTile;

            // 좌표값 Swap
            tmpVec = obj1.transform.localPosition;
            obj1.transform.localPosition = obj2.transform.localPosition;
            obj2.transform.localPosition = tmpVec;

            // 이름 Swap
            tmpName = obj1.name;
            obj1.name = obj2.name;
            obj2.name = tmpName;
        }
    }

    /// <summary>
    /// 퍼즐 맞추기 함수
    /// </summary>
    private void GetGradient()
    {
        int flagSuccess = 0;
  
        // 가로로 퍼즐 이동시 X축은 동일하니 X축 1개 / Y축 2개 검사
        if (m_movedCoordinate == 0)
        {
            examAxisX(int.Parse(this.name.Split(',')[0]));

            examAxisY(int.Parse(this.name.Split(',')[1]));
            examAxisY(int.Parse(m_targetName.Split(',')[1]));
        }
        // 세로로 퍼즐 이동시 Y축은 동일하니 Y축 1개 / X축 2개 검사
        else
        {
            examAxisX(int.Parse(this.name.Split(',')[0]));
            examAxisX(int.Parse(m_targetName.Split(',')[0]));
            
            examAxisY(int.Parse(m_targetName.Split(',')[1]));
        }

        if (delTiles())
        {
            // 없어진 음식 재료(블럭) 대신에 새 재료 생성
            createGradient();
        }
        else
        {
            // 양 오브젝트를 원래 좌표값으로 돌려놓기
            moveCoordinate(m_target, m_thisObj);
        }
    }

    /// <summary>
    /// 해당 X축에 대해 없애야할 타일이 있는지 검사하는 함수
    /// </summary>
    /// <param name="basePointX">X축 기준 좌표</param>
    private void examAxisX(int basePointX)
    {
        int delCnt = 0;
        int delFlag = 0;
        string beforeTile = string.Empty;
        for (int y = 0; y < Board.m_Height; y++)
        {
            // delCnt 및 delFlag 값 계산
            if (y > 0 && Board.m_Tiles[basePointX, y].Equals(beforeTile)) delCnt++;
            else
            {
                delCnt = 0;
                delFlag = 1;
            }
            if (y == Board.m_Height - 1) delFlag = 1;

            // m_delTiles 목록 추가
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
    /// <param name="basePointY">Y축 기준 좌표</param>
    private void examAxisY(int basePointY)
    {
        int delCnt = 0;
        int delFlag = 0;
        string beforeTile = string.Empty;
        for (int x = 0; x < Board.m_Width; x++)
        {
            // delCnt 및 delFlag 값 계산
            if (x > 0 && Board.m_Tiles[x, basePointY].Equals(beforeTile)) delCnt++;
            else
            {
                delCnt = 0;
                delFlag = 1;
            }
            if (x == Board.m_Width - 1) delFlag = 1;

            // m_delTiles 목록 추가
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
    /// 3개 이상 연속된 블럭 목록을 삭제
    /// </summary>
    /// <returns>삭제 성공시 true / 실패시 false</returns>
    private bool delTiles()
    {
        bool result = false;

        if (m_delTiles.Count != 0)
        {
            for (int idx = 0; idx < m_delTiles.Count; idx++)
            {
                // 3개 초과(4개 이상)부터 삭제가 안 되는 점 고쳐야 함
                Destroy(GameObject.Find(m_delTiles[idx].ToString()));
            }

            result = true;
        }

        return result;
    }

    /// <summary>
    /// 없어진 블럭 만큼 새 블록 생성
    /// </summary>
    private void createGradient()
    {
        
    }
}
