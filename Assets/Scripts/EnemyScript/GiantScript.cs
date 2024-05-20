using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GiantScript : MonoBehaviour
{

    private Animator animator;
    private NavMeshAgent agent;

    private GameObject player;

    [SerializeField]
    private GameObject giantsHand;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private GameObject blood;


    private PlayerController playerController;

    private bool isAttacking = false, spawned = false, playerDead = false, gotHit = false;
    private int health = 250, maxHealth = 250;

    private string[] attacks = new string[] { "Attack1", "Attack2" };

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] giantAudioClips;


    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        playerController = player.gameObject.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        transform.LookAt(player.transform);
        StartCoroutine(Initiate());
        blood.GetComponent<ParticleSystem>().Stop();
    }

    IEnumerator Initiate()
    {
        audioSource.PlayOneShot(giantAudioClips[0]);
        yield return new WaitForSeconds(4.5f);
        animator.SetBool("Spawned", true);
        spawned = true;
        giantsHand.gameObject.SetActive(false);
    }

    void Update()
    {
        if (spawned && health > 0 && !playerDead)
        {
            CheckIfPlayerDead();
            if (!isAttacking)
            {
                audioSource.Stop();
                agent.SetDestination(player.transform.position);
                audioSource.PlayOneShot(giantAudioClips[1]);
            }

            if (agent.velocity.magnitude == 0)
            {
                animator.SetBool("Walk", false);
                if (Vector3.Distance(player.transform.position, transform.position) < 3)
                {
                    transform.LookAt(player.transform);
                    if (!isAttacking)
                    {
                        isAttacking = true;
                        int attackIndex = Random.Range(0, 2);
                        animator.SetBool(attacks[attackIndex], true);
                        giantsHand.gameObject.SetActive(true);
                        audioSource.PlayOneShot(giantAudioClips[2]);
                        StartCoroutine(ResetAttack(attackIndex));
                    }
                }
            }
            else
            {
                animator.SetBool("Walk", true);
            }
            UpdateHealthBar(health, maxHealth);
        }
        if (health <= 0)
        {
            animator.SetTrigger("Dead");
            audioSource.Stop();
            audioSource.PlayOneShot(giantAudioClips[3], 0.35f);
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            healthBar.gameObject.SetActive(false);
        }
    }

    IEnumerator ResetAttack(int attackIndex)
    {
        yield return new WaitForSeconds(2.20f);
        animator.SetBool(attacks[attackIndex], false);
        isAttacking = false;
        giantsHand.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerSword" && health > 0 && spawned && !gotHit)
        {
            gotHit = true;
            health -= 25;
            StartCoroutine(SplashBlood());
        }

        if (other.gameObject.tag == "RifleBullet" && health > 0 && spawned && !gotHit)
        {
            health -= 10;
            StartCoroutine(SplashBlood());
        }
    }

    IEnumerator SplashBlood()
    {
        blood.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.10f);
        blood.GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(1f);
        gotHit = false;
    }

    private void UpdateHealthBar(float currentValue, float maxValue)
    {
        healthBar.value = currentValue / maxValue;
    }

    private void CheckIfPlayerDead()
    {
        if (playerController.playerHealth <= 0)
        {
            playerDead = true;
            isAttacking = false;
            giantsHand.gameObject.SetActive(false);
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            healthBar.gameObject.SetActive(false);
            animator.SetBool("Walk", false);
        }
    }
}
