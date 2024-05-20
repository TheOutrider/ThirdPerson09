using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackScript : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] audioClips;



    private GameObject boss;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(playAudioSequentially());
    }

    void Update()
    {
        boss = GameObject.FindWithTag("Boss");
        if (boss != null)
        {
            audioSource.Stop();
        }
    }

    IEnumerator playAudioSequentially()
    {
        yield return null;
        for (int i = 0; i < audioClips.Length; i++)
        {
            audioSource.clip = audioClips[i];
            audioSource.Play();
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }
    }
}
