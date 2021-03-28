using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// MonoBehaviour: Unity's inner class
public class Tile : MonoBehaviour // :(콜론) = 상속
{
    public int m_ID = 101;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Vector3 punch = new Vector3(0.1f, 0.1f, 0.1f);

        transform.DOPunchScale(punch, 1f);
        Debug.Log("OnMouseDown() 호출 테스트");
    }
}
