using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woman : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FoodWait");
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Position").transform.GetChild(gameObject.GetComponent<customerdefault>().position).transform.position, 1f * Time.deltaTime);
    }

    IEnumerator FoodWait()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Image/Customer/madam");

        while (true)
        {
            yield return null;
            if ((transform.position == GameObject.Find("Position").transform.GetChild(gameObject.GetComponent<customerdefault>().position).transform.position))
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprites[i];
                    yield return new WaitForSeconds(8f);
                }
                gameObject.GetComponent<customerdefault>().SelfRemove(false);
                Destroy(gameObject);
            }
        }
    }
}
