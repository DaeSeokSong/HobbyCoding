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
        
    }
    IEnumerator FoodWait()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Image/Customer/woman 1");
        for(int i = 0; i < 3; i++)
        {
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprites[i];
            yield return new WaitForSeconds(5f);
        }
        gameObject.GetComponent<customerdefault>().SelfRemove();
        Destroy(gameObject);
    }
}
