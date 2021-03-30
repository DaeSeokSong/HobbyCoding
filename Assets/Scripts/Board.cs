using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // public
    public int m_Width = 8;
    public int m_Height = 8;

    // private
    // 게임 오브젝트 관련 파라미터
    private GameObject m_TileRed;
    private GameObject m_TileBlue;
    private GameObject m_TileGreen;
    private GameObject m_TilePurple;
    private GameObject m_TileOrange;
    private GameObject m_TileYellow;
    private GameObject[] m_TileTypes;
    private Tile[,] m_TileArray;

    // Start is called before the first frame update
    void Start()
    {
        // Prefab = 제작틀(거푸집), 게임 오브젝트를 동일하게 다수 생성 가능
        // 게임 오브젝트 종류 초기화
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
    /// Prefab 이용하여 새로운 Tile들 생성
    /// </summary>
    private void CreateTiles()
    {
        // 실제 타일 배열 초기화
        m_TileArray = new Tile[m_Width, m_Height];

        for (int x=0; x<m_TileArray.GetLength(0); x++)
        {
            for(int y=0; y<m_TileArray.GetLength(1); y++)
            {
                Tile tile = Instantiate<Tile>(m_TileTypes[Random.Range(0, m_TileTypes.Length)].transform.GetComponent<Tile>());

                // 부모 설정
                tile.transform.SetParent(this.transform);
                // 위치 설정
                tile.transform.position = new Vector2(x + (x * tile.transform.localScale.x)/2, y + (y * tile.transform.localScale.y)/2);

                m_TileArray[x, y] = tile;
            }
        }
    }
}
