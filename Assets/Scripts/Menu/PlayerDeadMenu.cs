using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeadMenu : MonoBehaviour
{

    [SerializeField]
    public GameObject player;

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        // Debug.Log("SCREEN ACTIVE : PLayer DEAD");
    }

    public void RestartGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            player.GetComponent<PlayerController>().playerHealth = data.health;
            player.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            gameObject.SetActive(false);
            player.GetComponent<PlayerController>().Respawn();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }
        else
        {
            QuitToMainMenu();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

    public void QuitToMainMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("MenuScene");
        PlayerPrefs.SetInt("SHOWAUTOSAVE", 1);
    }
}
