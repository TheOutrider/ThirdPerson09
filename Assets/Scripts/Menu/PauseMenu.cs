using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;

    [SerializeField]
    private TextMeshProUGUI objectiveText;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            ContinueGame();
        }
        objectiveText.text = PlayerPrefs.GetString("OBJECTIVE");
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void ContinueGame()
    {
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitToMainMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MenuScene");
        PlayerPrefs.SetInt("SHOWAUTOSAVE", 1);
    }
}
