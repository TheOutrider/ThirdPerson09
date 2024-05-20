using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayHud : MonoBehaviour
{

    [SerializeField]
    private Slider healthBar, heatBar;
    [SerializeField]
    public Image hudImage, bloodScreen;
    [SerializeField]
    private TextMeshProUGUI healthText, promptText, ammoText;
    [SerializeField]
    private Image weaponImage;

    public Sprite rifleSprite, pistolSprite;

    [SerializeField]
    private GameObject mainPlayer;

    [SerializeField]
    private Canvas playerDeadCanvas;

    [SerializeField]
    private Image deadImage;

    [SerializeField]
    private GameObject playerTorso;

    private int maxHealth = 100;
    public float fadeTime = 0, alphaValue = 1, fadeSeconds = 0.35f;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private InputActionReference interactControl;

    private void OnEnable()
    {
        interactControl.action.Enable();
    }

    void Update()
    {
        promptText.text = "";
        Ray ray = new Ray(playerTorso.transform.position, playerTorso.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                promptText.text = interactable.promptMessage;
                if (interactControl.action.triggered)
                {
                    Debug.Log("INTERACTION COMPLETED");
                    interactable.BaseInteract();
                }
            }
        }

        PlayerController playerController = mainPlayer.gameObject.GetComponent<PlayerController>();
        UpdateHealthBar(playerController.playerHealth, maxHealth);

        if (playerController.activeWeaponIndex == 0)
        {
            weaponImage.sprite = rifleSprite;
        }
        else if (playerController.activeWeaponIndex == 1)
        {
            weaponImage.sprite = pistolSprite;
        }

        if (playerController.playerHealth <= 0)
        {
            ShowDeadScreen();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void UpdateHealthBar(float currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
        // float bloodAlpha = 1-healthBar.value * 20;
        // bloodScreen.color = new Color(bloodScreen.color.r, bloodScreen.color.g, bloodScreen.color.b, bloodAlpha);
    }

    public void HideHUD()
    {
        hudImage.enabled = false;
        healthBar.gameObject.SetActive(false);
        heatBar.gameObject.SetActive(false);
        healthText.enabled = false;
        ammoText.enabled = false;
        weaponImage.enabled = false;

    }

    public void ShowHUD()
    {
        hudImage.enabled = true;
        healthBar.gameObject.SetActive(true);
        heatBar.gameObject.SetActive(true);
        ammoText.enabled = true;
        healthText.enabled = true;
        weaponImage.enabled = true;
    }

    public void ShowDeadScreen()
    {
        playerDeadCanvas.gameObject.SetActive(true);
        if (alphaValue < 255)
        {
            alphaValue += fadeSeconds * Time.deltaTime;
        }

        deadImage.color = new Color(deadImage.color.r, deadImage.color.g, deadImage.color.b, alphaValue);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MenuScene");
        PlayerPrefs.SetInt("FORCE_CONTINUE", 1);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

}
