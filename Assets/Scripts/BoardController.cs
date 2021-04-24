using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    // Private
    // MonoBehaviour
    private MonoBehaviour m_MonoBehaviour;
    // Container
    private Transform m_Container;

    public BoardController(Transform container)
    {
        m_Container = container;
        m_MonoBehaviour = container.gameObject.GetComponent<MonoBehaviour>();
    }

    /*
     * =================================================== public ===================================================
     */
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return m_MonoBehaviour.StartCoroutine(routine);
    }

    public IEnumerator EvoluteMatchUp(int axisX)
    {
        if (Board.AFTERMATCHUP == true) yield return null;
        else 
        {
            bool retryFlag = false;
            for (int y = 0; y < Board.Height; y++) if (Board.TileArray[axisX, y].MatchAfterMovedown()) retryFlag = true;

            if (retryFlag)
            {
                yield return new WaitForSeconds(TileStatus.DURATION * 2);
                Board.AFTERMATCHUP = true;
            }
        }

        yield break;
    }
}
