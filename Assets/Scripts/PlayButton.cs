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
            SceneManager. LoadScene("Level_1_Feb25"); // whatever our first scene is falled
        }
    }
}
