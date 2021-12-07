using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] float loadSceneDelay = 3f;

    //AudioPlayer audioPlayer;

    void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;
        //another way to do this is
        //numberOfGameSessions = FindObjectsOfType(GetType()).Length;

        if (numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    //void Start()
    //{
    //    audioPlayer = GetComponent<AudioPlayer>();
    //}

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void ProcessExit()
    {
        StartCoroutine(LoadNextLevel());
    }

    void TakeLife()
    {
        playerLives--;
        StartCoroutine(LoadCurrentLevel());
    }

    void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }


    IEnumerator LoadCurrentLevel()
    {
        yield return new WaitForSecondsRealtime(loadSceneDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(loadSceneDelay);
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextLevelIndex);
        }

    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
        
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LoadFirstLevelScene()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //For later use
    //Call this like:
    //StartCourutine(WaitAndLoad("LoadGameOverScene", 3f)
    IEnumerator WaitAndLoad(String sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }


}
