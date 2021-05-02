using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void SceneChange_Adv()
    {
        SceneManager.LoadScene("Advertise");
    }

    public void SceneChange_Pl()
    {
        SceneManager.LoadScene("Police_Line");
    }
}
