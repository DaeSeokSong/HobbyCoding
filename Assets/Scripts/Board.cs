using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Public
    public static Tile[,] m_TileArray;
    public static int m_Width = 8;
    public static int m_Height = 8;

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

    /// <summary>
    /// Prefab �̿��Ͽ� ���ο� Tile ����
    /// </summary>
    private void CreateTiles()
    {
        // ���� Ÿ�� �迭 �ʱ�ȭ
        m_TileArray = new Tile[m_Width, m_Height];

        for (int x = 0; x < m_TileArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_TileArray.GetLength(1); y++)
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

                m_TileArray[x, y] = tile;
            }
        }
    }

    public static Tile GetTile(int x, int y) { return m_TileArray[x, y]; }

    public static void SwapIdx(Tile moved, Tile to)
    {
        m_TileArray[moved.GetX(), moved.GetY()] = to;
        m_TileArray[to.GetX(), to.GetY()] = moved;

        int tmpX = moved.GetX();
        int tmpY = moved.GetY();
        moved.SetX(to.GetX());
        moved.SetY(to.GetY());
        to.SetX(tmpX);
        to.SetY(tmpY);
    }
}
