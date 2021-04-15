using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaivour : MonoBehaviour
{
    // Private
    // MonoBehaviour
    private MonoBehaviour m_MonoBehaviour;

    // 컨테이너 (Board GameObject)
    Transform m_Container;

    public TileBehaivour(Transform container)
    {
        m_Container = container;
        m_MonoBehaviour = container.gameObject.GetComponent<MonoBehaviour>();
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return m_MonoBehaviour.StartCoroutine(routine);
    }

    public IEnumerator MoveTo(Tile moved, Vector3 to, float duration)
    {
        Vector2 startPos = moved.transform.position;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.smoothDeltaTime;
            moved.transform.position = Vector2.Lerp(startPos, to, elapsed / duration);

            yield return null;
        }
        moved.transform.position = to;

        yield break;
    }
}
