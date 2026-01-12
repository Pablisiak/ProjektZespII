using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public static int playerCount = 1;
    public void StartGameSP()
    {
        playerCount = 1;
        SceneManager.LoadScene(1);
    }

    public void StartGameMP(int count)
    {
        playerCount = count;
        SceneManager.LoadScene(1);   
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Options()
    {
        SceneManager.LoadScene(2);
    }

    public void Credits()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
