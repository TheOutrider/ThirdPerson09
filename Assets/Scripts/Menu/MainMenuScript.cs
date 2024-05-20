using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField]
    private Button ContinueButton;

    private int forceContinue;

    private void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        Debug.Log("PLAYER DATA");
        Debug.Log(JsonUtility.ToJson(data));
        if (data != null)
        {
            // ContinueButton.enabled = true;
            ContinueButton.gameObject.SetActive(true);
            forceContinue = PlayerPrefs.GetInt("FORCE_CONTINUE", 0);
            if (forceContinue == 1)
            {
                ContinueGame();
            }
        }
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt("CONTINUE", 0);
        PlayerPrefs.SetString("OBJECTIVE", "Mission Objective : Investigate the location");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(LoadYourAsyncScene());

    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("CONTINUE", 1);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene");


        while (!asyncLoad.isDone)
        {
            Debug.Log("LOADING . . . . .  ");
            yield return null;
        }

        if (asyncLoad.isDone)
        {
            Debug.Log("LOADING COMPLETED ");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
