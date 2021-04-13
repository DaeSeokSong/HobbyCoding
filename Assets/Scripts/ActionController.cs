using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    // Private
    // MonoBehaviour
    private MonoBehaviour m_MonoBehaviour;
    // 컨테이너 (Board GameObject)
    private Transform m_Container;

    public ActionController(Transform container)
    {
        m_Container = container;
        m_MonoBehaviour = container.gameObject.GetComponent<MonoBehaviour>();
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return m_MonoBehaviour.StartCoroutine(routine);
    }
}
