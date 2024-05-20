using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBulletScript : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletShell, explosion;
    private float timeToDestroy = 2.5f, countDown;
    Rigidbody bulletRig;

    [SerializeField]
    private float magicBulletSpeed;

    private GameObject player;

    private AudioSource audioSource;

    private bool isCollided = false;

    private Vector3 targetPosition;

    private void Start()
    {
        bulletRig = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        explosion.gameObject.SetActive(false);
        targetPosition = player.transform.position;
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    void Update()
    {
        countDown += Time.deltaTime;
        if (!isCollided)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, magicBulletSpeed * Time.deltaTime);
        }
        if (countDown > timeToDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.tag == "Player")
        {
            isCollided = true;
            bulletRig.velocity = Vector3.zero;
        }
        explosion.gameObject.SetActive(true);
        bulletShell.SetActive(false);
        Destroy(gameObject, timeToDestroy);
    }
}
