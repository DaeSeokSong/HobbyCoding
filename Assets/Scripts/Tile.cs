using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Public
    // const
    public const int RIGHT = 1;
    public const int LEFT = 2;
    public const int UP = 3;
    public const int DOWN = 4;
    // member
    public int DIRECTION = 0;
    public bool MATCH = false;

    // Private
    // 이동 시간
    private const float m_Duration = 0.5f;
    // 이동 좌표
    private int x;
    private int y;
    // 이동 목표 타일
    private Tile m_TargetTile;
    // 타일 행동 객체
    private TileBehaivour m_TileBehaivour;
    // 클릭 시 좌우상하 비교
    private Vector2 m_ClickedVec;
    private Vector2 m_MovedVec;
    // MatchUp 리스트
    private List<Tile> m_MatchList = new List<Tile>();

    void Start()
    {
        // 타일 행동 객체
        m_TileBehaivour = new TileBehaivour(this.transform);
        m_TargetTile = new Tile();
    }

    private void Move(Vector3 to)
    {
        m_TileBehaivour.StartCoroutine(m_TileBehaivour.MoveTo(this, to, m_Duration));
    }

    private void OnMouseDown()
    {
        m_ClickedVec = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        m_MovedVec = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        // 타겟 타일 세팅
        SetDirection();
        // 타겟과 위치 스왑
        SwapPosition();
    }

    public void SetX(int x) { this.x = x; }
    public int GetX() { return this.x; }
    public void SetY(int y) { this.y = y; }
    public int GetY() { return this.y; }

    /// <summary>
    /// 타겟 타일 세팅
    /// </summary>
    private void SetDirection()
    {
        // X좌표 이동값이 더 큰지 Y좌표 이동값이 더 큰지 비교
        if (Mathf.Abs(m_MovedVec.x - m_ClickedVec.x) > Mathf.Abs(m_MovedVec.y - m_ClickedVec.y))
        {
            // X좌표 +
            if (m_MovedVec.x - m_ClickedVec.x > 0)
            {
                DIRECTION = RIGHT;
                SetTarget(this.x + 1, this.y);
            }
            // X좌표 -
            if (m_MovedVec.x - m_ClickedVec.x < 0)
            {
                DIRECTION = LEFT;
                SetTarget(this.x - 1, this.y);
            }
        }
        else
        {
            // Y좌표 +
            if (m_MovedVec.y - m_ClickedVec.y > 0)
            {
                DIRECTION = UP;
                SetTarget(this.x, this.y + 1);
            }
            // Y좌표 -
            if (m_MovedVec.y - m_ClickedVec.y < 0)
            {
                DIRECTION = DOWN;
                SetTarget(this.x, this.y - 1);
            }
        }
    }

    public void SetTarget(int x, int y)
    {
        m_TargetTile = Board.GetTile(x, y);
    }

    public void SwapPosition()
    {
        m_TileBehaivour.StartCoroutine(MoveFromTo());
        m_TileBehaivour.StartCoroutine(MoveToFrom());

        Board.SwapIdx(this, m_TargetTile);
    }

    private IEnumerator MoveFromTo()
    {
        return m_TileBehaivour.MoveTo(this, m_TargetTile.transform.position, m_Duration);
    }

    private IEnumerator MoveToFrom()
    {
        return m_TileBehaivour.MoveTo(m_TargetTile, this.transform.position, m_Duration);
    }

    /*
    private void Match()
    {
        if (MatchUpX() || MatchUpY())
        {
            
        }
    }

    private bool MatchUpX()
    {
        // 비교 타일
        Tile comparedTile = new Tile();

        // MatchUp 리스트에 자신 포함
        m_MatchList.Add(this);

        // "왼쪽" MatchUp 검사
        for (int x = GetX() - 1; x > 0; x--)
        {
            comparedTile = Board.GetTile(x, m_TargetTile.GetY());
            if (x != GetX())
            {
                if (this.name.Equals(comparedTile.name) && !m_MatchList.Contains(comparedTile)) m_MatchList.Add(comparedTile);
                else break;
            }
        }

        // "오른쪽" MatchUp 검사
        for (int x = GetX() + 1; x < Board.m_Width; x++)
        {
            comparedTile = Board.GetTile(x, m_TargetTile.GetY());
            if (x != GetX())
            {
                if (this.name.Equals(comparedTile.name) && !m_MatchList.Contains(comparedTile)) m_MatchList.Add(comparedTile);
                else break;
            }
        }

        return UpdateMatchStateOfTiles();
    }

    private bool MatchUpY()
    {
        // 비교 타일
        Tile comparedTile = new Tile();

        // MatchUp 리스트에 자신 포함
        m_MatchList.Add(this);

        // "아래쪽" MatchUp 검사
        for (int y = GetY() - 1; y > 0; y--)
        {
            comparedTile = Board.GetTile(m_TargetTile.GetX(), y);
            if (y != GetY())
            {
                if (this.name.Equals(comparedTile.name) && !m_MatchList.Contains(comparedTile)) m_MatchList.Add(comparedTile);
                else break;
            }
        }

        // "위쪽" MatchUp 검사
        for (int y = GetY() + 1; y < Board.m_Height; y++)
        {
            comparedTile = Board.GetTile(m_TargetTile.GetX(), y);
            if (y != GetY())
            {
                if (this.name.Equals(comparedTile.name) && !m_MatchList.Contains(comparedTile)) m_MatchList.Add(comparedTile);
                else break;
            }
        }

        return UpdateMatchStateOfTiles();
    }

    private bool UpdateMatchStateOfTiles()
    {
        bool result = false;

        if (m_MatchList.Count > 2)
        {
            for (int idx = 0; idx < m_MatchList.Count; idx++) m_MatchList[idx].MATCH = true;
            result = true;
        }

        return result;
    }
    */
}
