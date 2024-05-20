using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuAutoSave : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI savingText, instructionText;

    [SerializeField]
    private Slider loader;

    private float textAlpha = 0;

    private float rotateSpeed = 250f;

    void Start()
    {
        textAlpha = 0;
    }


    void Update()
    {
        if (textAlpha < 1)
        {
            textAlpha += Time.deltaTime * 0.2f;
            instructionText.color = new Color(instructionText.color.r, instructionText.color.g, instructionText.color.b, textAlpha);
        }
        else
        {
            savingText.gameObject.SetActive(true);
            loader.gameObject.SetActive(true);
            loader.transform.Rotate(0f, 0f, -(rotateSpeed * Time.deltaTime), Space.Self);
            StartCoroutine(LoadScene());
        }

    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MenuScene");
    }
}
