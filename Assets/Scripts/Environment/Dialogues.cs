using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogues : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private bool DelayDialogues = false;

    [SerializeField]
    private float delaySeconds = 2f, minimumDistance = 20;

    [SerializeField]
    public GameObject player;

    [SerializeField]
    private AudioClip audioClip;

    private bool playerReached = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < minimumDistance && !playerReached)
        {
            playerReached = true;
            if (DelayDialogues)
            {
                StartCoroutine(PlayDialogues());
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
                Destroy(gameObject, audioClip.length + 1);
            }

        }
    }

    IEnumerator PlayDialogues()
    {
        yield return new WaitForSeconds(delaySeconds);
        audioSource.PlayOneShot(audioClip);
        Destroy(gameObject, audioClip.length + 1);
    }
}
