using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Public
    // Ÿ�� ����Ʈ
    public static Tile[,] TileArray;
    // MatchUp ����Ʈ
    public static List<Tile> MatchList = new List<Tile>();
    // ����/���� ����
    public static int Width = 8;
    public static int Height = 8;
    public static bool MOVEDOWN = false;

    // Private
    // ���� ������Ʈ ���� �Ķ����
    private GameObject m_TileRed;
    private GameObject m_TileBlue;
    private GameObject m_TileGreen;
    private GameObject m_TilePurple;
    private GameObject m_TileOrange;
    private GameObject m_TileYellow;
    private GameObject[] m_TileTypes;

    // Start is called before the first frame update
    void Start()
    {
        // Prefab = ����Ʋ(��Ǫ��), ���� ������Ʈ�� �����ϰ� �ټ� ���� ����
        // ���� ������Ʈ ���� �ʱ�ȭ
        m_TileRed = Resources.Load("Prefabs/CandyRed") as GameObject;
        m_TileBlue = Resources.Load("Prefabs/CandyBlue") as GameObject;
        m_TileGreen = Resources.Load("Prefabs/CandyGreen") as GameObject;
        m_TilePurple = Resources.Load("Prefabs/CandyPurple") as GameObject;
        m_TileOrange = Resources.Load("Prefabs/CandyOrange") as GameObject;
        m_TileYellow = Resources.Load("Prefabs/CandyYellow") as GameObject;
        m_TileTypes = new GameObject[] { m_TileRed, m_TileBlue, m_TileGreen, m_TilePurple, m_TileOrange, m_TileYellow };

        CreateTiles();
    }

    void Update()
    {
        if (MOVEDOWN)
        {
            ReplaceTiles();
            ArrangeReplaceTiles();
        }
    }

    /// <summary>
    /// Prefab �̿��Ͽ� ���ο� Tile ����
    /// </summary>
    private void CreateTiles()
    {
        // ���� Ÿ�� �迭 �ʱ�ȭ
        TileArray = new Tile[Width, Height * 2];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                GameObject prefab = m_TileTypes[Random.Range(0, m_TileTypes.Length)];
                Tile tile = Instantiate<Tile>(prefab.transform.GetComponent<Tile>());
                tile.name = prefab.name;

                // ��ǥ ����
                tile.SetX(x);
                tile.SetY(y);

                // �θ� ����
                tile.transform.SetParent(this.transform);
                // ��ġ ����
                tile.transform.position = new Vector2(x + (x * tile.transform.localScale.x) / 2, y + (y * tile.transform.localScale.y) / 2);

                TileArray[x, y] = tile;
            }
        }

        ArrangeReplaceTiles();
    }

    private void ArrangeReplaceTiles()
    {
        for (int x = 0; x < Width; x++)
        {
            // MoveDown�� Ÿ�� ���� (Active = false)
            for (int y = Height; y < Height * 2; y++)
            {
                GameObject prefab = m_TileTypes[Random.Range(0, m_TileTypes.Length)];
                Tile tile = Instantiate<Tile>(prefab.transform.GetComponent<Tile>());
                // ��Ȱ��ȭ ���·� ���
                tile.gameObject.SetActive(false);
                tile.name = prefab.name;

                // ��ǥ ����
                tile.SetX(x);
                tile.SetY(y);

                // �θ� ����
                tile.transform.SetParent(this.transform);
                // ��ġ ����
                tile.transform.position = new Vector2(x + (x * tile.transform.localScale.x) / 2, y + (y * tile.transform.localScale.y) / 2);

                TileArray[x, y] = tile;
            }
        }
    }

    public static Tile GetTile(int x, int y) { return TileArray[x, y]; }

    public static void SwapIdx(Tile moved, Tile to)
    {
        TileArray[moved.GetX(), moved.GetY()] = to;
        TileArray[to.GetX(), to.GetY()] = moved;

        int tmpX = moved.GetX();
        int tmpY = moved.GetY();
        moved.SetX(to.GetX());
        moved.SetY(to.GetY());
        to.SetX(tmpX);
        to.SetY(tmpY);
    }

    public static void ReplaceTiles()
    {
        int startMoveDownIdxY = 0;
        for (int x = 0; x < Width; x++)
        {
            int moveDownCnt = 0;
            for (int y = 0; y < Height; y++)
            {
                if (TileArray[x, y].DESTROY)
                {
                    moveDownCnt++;
                    startMoveDownIdxY = y;
                }
            }

            if (moveDownCnt != 0)
            {
                for (int idx = startMoveDownIdxY + 1; idx < Height + moveDownCnt; idx++)
                { 
                    TileArray[x, idx].MoveDown(moveDownCnt);
                    TileArray[x, idx - moveDownCnt] = TileArray[x, idx];
                    TileArray[x, idx - moveDownCnt].SetX(x);
                    TileArray[x, idx - moveDownCnt].SetY(idx - moveDownCnt);
                }
            }
        }
    }
}
