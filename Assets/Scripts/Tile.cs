using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Public
    public int DIRECTION = 0;
    public bool MATCH = false;
    public bool DESTROY = false;

    // Private
    // (this) Tile's coordination
    private int x;
    private int y;
    // Target tile to move
    private Tile m_TargetTile;
    // (this) Tile's behaivor
    private TileBehaivour m_TileBehaivour;
    // Compared clicked position
    private Vector2 m_ClickedVec;
    private Vector2 m_MovedVec;

    void Start()
    {
        // 타일 행동 객체
        m_TileBehaivour = new TileBehaivour(this.transform);
        m_TargetTile = new Tile();
    }

    /*
     * =================================================== private ===================================================
     */
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
        // Set move direction and set target tile
        SetDirection();
        // TileBehaivour execute
        if (m_TargetTile != null) StartBehaivor();
    }

    /// <summary>
    /// Set move direction
    /// </summary>
    private void SetDirection()
    {
        // Move about X or Y?
        if (Mathf.Abs(m_MovedVec.x - m_ClickedVec.x) > Mathf.Abs(m_MovedVec.y - m_ClickedVec.y))
        {
            // X++
            if (m_MovedVec.x - m_ClickedVec.x > 0)
            {
                DIRECTION = TileStatus.RIGHT;
                SetTarget(this.x + 1, this.y);
            }
            // X--
            if (m_MovedVec.x - m_ClickedVec.x < 0)
            {
                DIRECTION = TileStatus.LEFT;
                SetTarget(this.x - 1, this.y);
            }
        }
        else
        {
            // Y++
            if (m_MovedVec.y - m_ClickedVec.y > 0)
            {
                DIRECTION = TileStatus.UP;
                SetTarget(this.x, this.y + 1);
            }
            // Y--
            if (m_MovedVec.y - m_ClickedVec.y < 0)
            {
                DIRECTION = TileStatus.DOWN;
                SetTarget(this.x, this.y - 1);
            }
        }
    }

    /// <summary>
    /// Set target tile to move
    /// </summary>
    /// <param name="x">Coordination X</param>
    /// <param name="y">Coordination Y</param>
    private void SetTarget(int x, int y)
    {
        m_TargetTile = Board.GetTile(x, y);
        if (this.DIRECTION % 2 == 0) m_TargetTile.DIRECTION = this.DIRECTION + 1;
        else m_TargetTile.DIRECTION = this.DIRECTION - 1;
    }

    /// <summary>
    /// Start (this) Tile's behaivor
    /// </summary>
    private void StartBehaivor()
    {
        // Swap position and index
        SwapPosition();
        // MatchUp
        Match();
    }

    /// <summary>
    /// Swap position between this tile and target tile
    /// </summary>
    private void SwapPosition()
    {
        m_TileBehaivour.StartCoroutine(m_TileBehaivour.CoStartMove(this, m_TargetTile.transform.localPosition));
        m_TileBehaivour.StartCoroutine(m_TileBehaivour.CoStartMove(m_TargetTile, this.transform.localPosition));

        Board.SwapIdx(this, m_TargetTile);
    }

    /// <summary>
    /// MatchUp tiles
    /// </summary>
    private void Match()
    {
        int beforeCnt = 0;

        // X Axis MathUp
        this.MatchUpX();
        // Y Axis MathUp
        this.MatchUpY();
        // MatchUp put in movedTile
        if (Board.MatchList.Count != 0)
        {
            Board.MatchList.Add(this);
            beforeCnt = Board.MatchList.Count;
        }

        // TargetTile Axis MathUp
        if (m_TargetTile.DIRECTION == TileStatus.RIGHT || m_TargetTile.DIRECTION == TileStatus.LEFT) m_TargetTile.MatchUpY();
        else m_TargetTile.MatchUpX();
        if (!Board.MatchList.Contains(m_TargetTile) && (Board.MatchList.Count - beforeCnt) >= 2) Board.MatchList.Add(m_TargetTile);

        // MatchUped Tile is exist?
        if (UpdateMatchStateOfTiles())
        {
            // MatchUped Tile set up for Destroy
            SetUpDestroy();
            m_TileBehaivour.StartCoroutine(ReplaceTile());
        }
        else
        {
            // Reset Position and Index
            SwapPosition();
            m_TileBehaivour.InitRunningMove();
        }
    }

    /// <summary>
    /// State update about MatchUp
    /// </summary>
    /// <returns></returns>
    private bool UpdateMatchStateOfTiles()
    {
        bool result = false;

        if (Board.MatchList.Count > 2)
        {
            for (int idx = 0; idx < Board.MatchList.Count; idx++) Board.MatchList[idx].MATCH = true;
            result = true;
        }

        return result;
    }

    /// <summary>
    /// MatchUped tiles destroy
    /// </summary>
    private void SetUpDestroy()
    {
        for (int idx = 0; idx < Board.MatchList.Count; idx++)
        {
            Tile destroiedTile = Board.MatchList[idx];
            destroiedTile.Destroy();
        }
    }

    /// <summary>
    /// Destroy tile
    /// </summary>
    private void Destroy()
    {
        m_TileBehaivour.StartCoroutine(CoDoDestroy());
    }

    private IEnumerator CoDoDestroy()
    {
        return m_TileBehaivour.CoStartDestroy(this);
    }

    /// <summary>
    /// (this) Tile move down
    /// </summary>
    /// <returns>Coroutine</returns>
    private IEnumerator ReplaceTile()
    {
        if (Board.MatchList.Count != 0) yield return null;
        Board.MOVEDOWN = true;

        yield break;
    }

    /*
     * =================================================== public ===================================================
     */
    public void SetX(int x) { this.x = x; }
    public int GetX() { return this.x; }
    public void SetY(int y) { this.y = y; }
    public int GetY() { return this.y; }

    /// <summary>
    /// MatchUp X axis
    /// </summary>
    public void MatchUpX()
    {
        // Init tmpList
        List<Tile> tmpMatchList = new List<Tile>();
        // Init tmpTile
        Tile comparedTile = new Tile();

        // Leftside MatchUp
        for (int x = GetX() - 1; x >= 0; x--)
        {
            comparedTile = Board.GetTile(x, this.GetY());
            if (this.name.Equals(comparedTile.name) && !tmpMatchList.Contains(comparedTile)) tmpMatchList.Add(comparedTile);
            else break;
        }

        // Rightside MatchUp
        for (int x = GetX() + 1; x < Board.Width; x++)
        {
            comparedTile = Board.GetTile(x, this.GetY());
            if (this.name.Equals(comparedTile.name) && !tmpMatchList.Contains(comparedTile)) tmpMatchList.Add(comparedTile);
            else break;
        }

        if (tmpMatchList.Count < 2) tmpMatchList.Clear();
        else
        {
            for (int idx = 0; idx < tmpMatchList.Count; idx++)
                if (!Board.MatchList.Contains(tmpMatchList[idx])) Board.MatchList.Add(tmpMatchList[idx]);
        }
    }

    public void MatchUpY()
    {
        // Init tmpList
        List<Tile> tmpMatchList = new List<Tile>();
        // Init tmpTile
        Tile comparedTile = new Tile();

        // Downside MatchUp
        for (int y = GetY() - 1; y >= 0; y--)
        {
            comparedTile = Board.GetTile(this.GetX(), y);
            if (this.name.Equals(comparedTile.name) && !tmpMatchList.Contains(comparedTile)) tmpMatchList.Add(comparedTile);
            else break;
        }

        // Upside MatchUp
        for (int y = GetY() + 1; y < Board.Height; y++)
        {
            comparedTile = Board.GetTile(this.GetX(), y);
            if (this.name.Equals(comparedTile.name) && !tmpMatchList.Contains(comparedTile)) tmpMatchList.Add(comparedTile);
            else break;
        }

        if (tmpMatchList.Count < 2) tmpMatchList.Clear();
        else
        {
            for (int idx = 0; idx < tmpMatchList.Count; idx++)
                if (!Board.MatchList.Contains(tmpMatchList[idx])) Board.MatchList.Add(tmpMatchList[idx]);
        }
    }

    /// <summary>
    /// (this) Tile move down
    /// </summary>
    /// <param name="downScale">how much down coordination(y)</param>
    public void MoveDown(int downScale)
    {
        // Activate this Tile
        this.gameObject.SetActive(true);

        // Compute DownTo Vector2
        Vector3 downTo = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - (downScale * TileStatus.DIAMETER), this.transform.position.z);

        // Move Down
        if (m_TileBehaivour == null) m_TileBehaivour = new TileBehaivour(this.transform);
        m_TileBehaivour.StartCoroutine(m_TileBehaivour.CoStartMoveDown(this, downTo));
    }

    /// <summary>
    /// MatchUp after action about move down
    /// </summary>
    public bool MatchAfterMovedown()
    {
        bool result = false;

        // X Axis MathUp
        this.MatchUpX();
        // Y Axis MathUp
        this.MatchUpY();
        // MatchUp put in movedTile
        if (Board.MatchList.Count != 0) Board.MatchList.Add(this);

        // MatchUped Tile is exist?
        if (UpdateMatchStateOfTiles())
        {
            result = true;

            // MatchUped Tile set up for Destroy
            SetUpDestroy();
            m_TileBehaivour.StartCoroutine(ReplaceTile());
        }

        return result;
    }
}