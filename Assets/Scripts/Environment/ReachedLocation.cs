using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReachedLocation : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private String locationName, description, currentObjective;

    [SerializeField]
    private Image backdrop, seperactor;

    [SerializeField]
    private TextMeshProUGUI locationText, descriptionText;

    [SerializeField]
    private Canvas HUD, autoSave;

    private GamePlayHud gamePlayHud;
    private AutoSaveCanvasScript autoSaveCanvasScript;

    private bool reachedLocation = false, hideBackDrop = false;

    public float fadeTime = 0, alphaValue = 0, fadeSeconds = 0.35f;

    private AudioSource audioSource;



    private void Start()
    {
        locationText.text = "";
        descriptionText.text = "";
        backdrop.enabled = false;
        seperactor.enabled = false;
        gamePlayHud = HUD.gameObject.GetComponent<GamePlayHud>();
        autoSaveCanvasScript = autoSave.gameObject.GetComponent<AutoSaveCanvasScript>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!reachedLocation)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 20)
            {
                reachedLocation = true;
                locationText.text = locationName;
                descriptionText.text = description;
                backdrop.enabled = true;
                seperactor.enabled = true;
                gamePlayHud.HideHUD();
                audioSource.Play();

                PlayerPrefs.SetString("OBJECTIVE", currentObjective);

                Player currentPlayer = new Player();
                PlayerController playerController = player.GetComponent<PlayerController>();

                currentPlayer.health = playerController.playerHealth;
                currentPlayer.playerTransform = player.transform;
                currentPlayer.recentLocationName = locationName;
                SaveSystem.SavePlayer(currentPlayer);
            }
        }
        else
        {
            ShowUI();
        }
    }

    private void ShowUI()
    {
        if (alphaValue < 1 && !hideBackDrop)
        {

            alphaValue += fadeSeconds * Time.deltaTime;
            locationText.color = new Color(locationText.color.r, locationText.color.g, locationText.color.b, alphaValue);
            descriptionText.color = new Color(descriptionText.color.r, descriptionText.color.g, descriptionText.color.b, alphaValue);


        }
        else
        {
            hideBackDrop = true;
            StartCoroutine(HideUI());
        }
    }

    IEnumerator HideUI()
    {
        yield return new WaitForSeconds(0.5f);
        if (alphaValue > 0)
        {
            alphaValue -= fadeSeconds * Time.deltaTime;
            locationText.color = new Color(locationText.color.r, locationText.color.g, locationText.color.b, alphaValue);
            descriptionText.color = new Color(descriptionText.color.r, descriptionText.color.g, descriptionText.color.b, alphaValue);
        }
        else
        {
            backdrop.enabled = false;
            seperactor.enabled = false;
            locationText.text = "";
            descriptionText.text = "";
            gamePlayHud.ShowHUD();
            audioSource.Stop();
            yield return new WaitForSeconds(1.5f);
            autoSaveCanvasScript.ShowAutoSave();
            Destroy(gameObject, 1.5f);
        }


    }

}
