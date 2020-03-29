using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public static bool GameisPaused = false;
 public GameObject pauseMenuUI;

 void Start () 
 {
  pauseMenuUI.SetActive (false);
 }

 // Update is called once per frame
 void Update () {
  if (Input.GetKeyDown(KeyCode.Escape))
  {
   if (GameisPaused) 
   {
    Resume ();
   } else 
   {
    Pause ();
   }
 }
}
 public void Resume ()
 {
  pauseMenuUI.SetActive (false);
  Time.timeScale = 1f;
  GameisPaused = false;
 }
 void Pause ()
 {
  pauseMenuUI.SetActive (true);
  Time.timeScale = 0f;
  GameisPaused = true;
 }

    public void RestartGame()
    {
        Debug.Log("Restarting...");
        SceneManager.LoadScene("StartScreen 1");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
