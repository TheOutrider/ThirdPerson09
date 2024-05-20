using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoSaveCanvasScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI saveText;

    [SerializeField]
    private Slider loader;

    private float rotateSpeed = 250f;

    public void ShowAutoSave()
    {
        saveText.gameObject.SetActive(true);
        loader.gameObject.SetActive(true);
        loader.transform.Rotate(0f, 0f, -(rotateSpeed * Time.deltaTime), Space.Self);
        StartCoroutine(HideAutoSave());
    }

    IEnumerator HideAutoSave()
    {
        yield return new WaitForSeconds(3.5f);
        saveText.gameObject.SetActive(false);
        loader.gameObject.SetActive(false);
    }
}
