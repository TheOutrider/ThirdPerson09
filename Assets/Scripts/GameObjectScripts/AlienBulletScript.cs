using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBulletScript : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletShell;
    private float timeToDestroy = 1.5f, countDown;


    private AudioSource audioSource;

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    void Update()
    {
        countDown += Time.deltaTime;
        if (countDown > timeToDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        bulletShell.SetActive(false);
        Destroy(gameObject);
    }
}
