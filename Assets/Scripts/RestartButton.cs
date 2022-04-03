using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{


    // Update is called once per frame
    public void restartGame()
    {
        SceneManager.LoadScene(0);
    }
}
