using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaivour : MonoBehaviour
{
    // Private
    // MonoBehaviour
    private MonoBehaviour m_MonoBehaviour;
    // 컨테이너 (Board GameObject)
    private Transform m_Container;
    // IsRunning
    private int m_IsRunningMove = 0;
    private int m_IsRunningMoveDown = 0;

    public TileBehaivour(Transform container)
    {
        m_Container = container;
        m_MonoBehaviour = container.gameObject.GetComponent<MonoBehaviour>();
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return m_MonoBehaviour.StartCoroutine(routine);
    }

    public void InitRunningMove() { this.m_IsRunningMove = 0; }

    public IEnumerator CoStartMove(Tile moved, Tile to)
    {
        if (m_IsRunningMove > 2) yield return null;
        m_IsRunningMove++;

        Vector2 startPos = moved.transform.position;

        float elapsed = 0.0f;
        while (elapsed < TileStatus.DURATION)
        {
            elapsed += Time.smoothDeltaTime;
            moved.transform.position = Vector2.Lerp(startPos, to.transform.position, elapsed / TileStatus.DURATION);

            yield return null;
        }
        moved.transform.position = to.transform.position;
        moved.transform.position = new Vector3((1.5f * moved.GetX()), (1.5f * moved.GetY()), 0);

        yield break;

        m_IsRunningMove--;
    }

    public IEnumerator CoStartDestroy(Tile destroyedTile)
    {
        // 크기가 줄어드는 액션 : 1 -> 0.3(TileStatus.DESTROY_SCALE)으로 줄어든다.
        yield return Scale(destroyedTile.transform, TileStatus.DESTROY_SCALE, TileStatus.DESTROY_SPEED);

        // 바로 삭제되는 거 지연
        yield return new WaitForSeconds(0.1f);

        // 삭제
        destroyedTile.DESTROY = true;
        Board.MatchList.Remove(destroyedTile);

        // 블럭 GameObject 객체 삭제 or make size zero
        Destroy(destroyedTile.gameObject);
    }

    private static IEnumerator Scale(Transform target, float toScale, float speed)
    {
        // 이동 중에 삭제되는 거 방지
        yield return new WaitForSeconds(TileStatus.DURATION);

        float factor;

        while (true)
        {
            factor = Time.deltaTime * speed * -1;
            target.localScale = new Vector3(target.localScale.x + factor, target.localScale.y + factor, target.localScale.z);

            if (target.localScale.x <= toScale) break;

            yield return null;
        }

        yield break;
    }

    public IEnumerator CoStartMoveDown(Tile moved, Vector2 to)
    {
        // 이동 및 삭제 중에 MoveDown되는 거 방지
        yield return new WaitForSeconds(TileStatus.DURATION);

        Vector2 startPos = moved.transform.position;

        float elapsed = 0.0f;
        while (elapsed < TileStatus.DURATION)
        {
            elapsed += Time.smoothDeltaTime;
            moved.transform.position = Vector2.Lerp(startPos, to, elapsed / TileStatus.DURATION);

            yield return null;
        }
        moved.transform.position = to;

        yield break;
    }
}
