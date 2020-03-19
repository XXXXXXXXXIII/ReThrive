using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{

    void Update()
    {
        if (Input.anyKey)
        {
            // whatever our first scene is called
            SceneManager.LoadScene(1);
        }
    }
}
