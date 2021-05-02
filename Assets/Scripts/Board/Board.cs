using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Public
    // Tile List
    public static Tile[,] TileArray;
    // MatchUp List
    public static List<Tile> MatchList = new List<Tile>();
    // Is?
    public static bool MOVEDOWN = false;
    // IsAfterMatchUp?
    public static bool AFTERMATCHUP = false;
    // Width, Height
    public static int Width = 8;
    public static int Height = 8;

    // Private
    // Tiles
    private GameObject m_Tile1;
    private GameObject m_Tile2;
    private GameObject m_Tile3;
    private GameObject m_Tile4;
    private GameObject m_Tile5;
    private GameObject[] m_TileTypes;
    // Tile's background
    private GameObject m_TileBackground;

    // Start is called before the first frame update
    void Start()
    {
        // Init Tiles
        m_Tile1 = Resources.Load("Prefabs/Tile1") as GameObject;
        m_Tile2 = Resources.Load("Prefabs/Tile2") as GameObject;
        m_Tile3 = Resources.Load("Prefabs/Tile3") as GameObject;
        m_Tile4 = Resources.Load("Prefabs/Tile4") as GameObject;
        m_Tile5 = Resources.Load("Prefabs/Tile5") as GameObject;
        m_TileTypes = new GameObject[] { m_Tile1, m_Tile2, m_Tile3, m_Tile4, m_Tile5 };

        // Init Tile's background
        m_TileBackground = Resources.Load("Prefabs/Ingredient") as GameObject;

        CreateTiles();
        
    }

    void Update()
    {
        if (MOVEDOWN)
        {
            ArrangeReplaceTiles();
            OverlapTest();
            MOVEDOWN = false;
        }
    }

    /*
     * =================================================== public ===================================================
     */
    /// <summary>
    /// Get specific Tile about X, Y
    /// </summary>
    /// <param name="x">Coordination X</param>
    /// <param name="y">Coordination Y</param>
    /// <returns>Tile about (X, Y)</returns>
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

    /*
     * =================================================== private ===================================================
     */
    /// <summary>
    /// Init GameObjects(Tiles)
    /// </summary>
    private void CreateTiles()
    {
        // Init list for store about tiles
        TileArray = new Tile[Width, Height * 2];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // Tile
                GameObject prefab = m_TileTypes[Random.Range(0, m_TileTypes.Length)];
                Tile tile = Instantiate<Tile>(prefab.transform.GetComponent<Tile>());
                tile.name = prefab.name;

                // Background
                GameObject background = Instantiate<GameObject>(m_TileBackground.transform.gameObject);
                background.name = "Background";

                // Set Coordination
                tile.SetX(x);
                tile.SetY(y);

                // Set parent about transform
                tile.transform.SetParent(this.transform);
                background.transform.SetParent(this.transform);

                // Set Position
                // Set Scale
                tile.transform.localScale = new Vector3(70, 70, 0);
                background.transform.localScale = new Vector3(110, 110, 0);
                // Compute Position
                float initX = -365 + ((x * tile.transform.localScale.x) + (x * (tile.transform.localScale.x / 2)));
                float initY = -790 + ((y * tile.transform.localScale.y) + (y * (tile.transform.localScale.y / 2)));
                // Set Position
                tile.transform.localPosition = new Vector3(initX, initY, 100);
                background.transform.localPosition = new Vector3(initX, initY, 200);

                TileArray[x, y] = tile;
            }
        }

        ArrangeReplaceTiles();
        OverlapTest();
    }

    /// <summary>
    /// Init ArrangedTiles
    /// </summary>
    private void ArrangeReplaceTiles()
    {
        // Arranged Tiles Create (Active = false)
        for (int x = 0; x < Width; x++)
        {
            for (int y = Height; y < Height * 2; y++)
            {
                GameObject prefab = m_TileTypes[Random.Range(0, m_TileTypes.Length)];
                Tile tile = Instantiate<Tile>(prefab.transform.GetComponent<Tile>());

                // Set Deactivate
                tile.gameObject.SetActive(false);
                tile.name = prefab.name;

                // Set Coordination
                tile.SetX(x);
                tile.SetY(y);

                // Set parent about transform
                tile.transform.SetParent(this.transform);
                // Set position
                tile.transform.localScale = new Vector3(70, 70, 0);
                // Compute Position
                float initX = -365 + ((x * tile.transform.localScale.x) + (x * (tile.transform.localScale.x / 2)));
                float initY = -790 + ((y * tile.transform.localScale.y) + (y * (tile.transform.localScale.y / 2)));
                tile.transform.localPosition = new Vector3(initX, initY, 100);

                TileArray[x, y] = tile;
            }
        }
    }

    private bool OverlapTile(int x, int y, int a)//특정 좌표의 중복 여부를 확인해주는 함수 송대석을 저주한다.
    {
        int count = 0;
        GameObject Base = GameObject.Find("Base").transform.GetChild(2).gameObject;
        int standard = GameObject.Find("Base").transform.GetChild(2).childCount;

        string name = Base.transform.GetChild(8 - y + (8 * x) - 1 + a).name;

        if ((8 - y + 1 + (8 * x) - 1 + a) < standard) if (name == Base.transform.GetChild(8 - y + 1 + (8 * x) - 1 + a).name) count++;//위
        if ((8 - y - 1 + (8 * x) - 1 + a) < standard) if (name == Base.transform.GetChild(8 - y - 1 + (8 * x) - 1 + a).name) count++;//아래
        if ((8 - y + (8 * (x + 1)) - 1 + a) < standard) if (name == Base.transform.GetChild(8 - y + (8 * (x + 1)) - 1 + a).name) count++;//오른쪽
        if ((8 - y + (8 * (x - 1)) - 1 + a) < standard) if (name == Base.transform.GetChild(8 - y + (8 * (x - 1)) - 1 + a).name) count++;//왼쪽

        if (count > 2) return true;
        else return false;
    }

    private void OverlapTest()
    {
        int x, y;
        GameObject Base = GameObject.Find("Base").transform.GetChild(2).gameObject;

        for (int j = 0; j < 64; j++)
        {
            x = j / 8;
            y = j % 8;
            if (OverlapTile(x, y, 64 * 2))
            {
                Destroy(Base.transform.GetChild(8 - y + (8 * x) - 1 + (2 * 64)).gameObject);

                GameObject prefab = m_TileTypes[Random.Range(0, m_TileTypes.Length)];
                GameObject newTile = Instantiate(prefab, gameObject.transform.GetChild(8 - y + (8 * x) - 1 + (2 * 64)).position, Quaternion.identity, gameObject.transform) as GameObject;
                newTile.transform.localScale = new Vector3(70, 70, 0);
                newTile.transform.SetSiblingIndex(8 - y + (8 * x) - 1 + (2 * 64));
                newTile.SetActive(false);
                newTile.name = prefab.name;

                j--;
            }
        }
    }
}
