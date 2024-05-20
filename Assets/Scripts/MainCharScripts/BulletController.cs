using System;
using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal, bulletShell, sparkParticleSystem;

    private float speed = 200f;
    private float timeToDestroy = 1.5f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }



    private AudioSource audioSource;

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        sparkParticleSystem.GetComponent<ParticleSystem>().Stop();
    }



    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < 0.01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bulletShell.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.GetContact(0);
        hit = true;
        bulletShell.SetActive(false);
        sparkParticleSystem.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DestroyBullet());

    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.25f);
        sparkParticleSystem.GetComponent<ParticleSystem>().Stop();
        Destroy(gameObject);
    }
}
