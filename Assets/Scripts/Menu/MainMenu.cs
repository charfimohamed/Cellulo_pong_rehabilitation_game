using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Dropdown dropRehabOption;
    public GameObject player1;
    public GameObject player2;

    public void PlayGame()
    {
        if (dropRehabOption.options[dropRehabOption.value] == dropRehabOption.options[0])
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);

    }
    public void startTimer()
    {
        player1.GetComponent<CelluloAgent>().MoveOnIce();
        player2.GetComponent<CelluloAgent>().MoveOnIce();
        Timer.enable = true;
    }

    public void PauseGame()
    {
        Timer.enable = false;
    }

    public void ResumeGame()
    {
        Timer.enable = true;
    }
}
