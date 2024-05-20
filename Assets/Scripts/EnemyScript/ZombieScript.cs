using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieScript : MonoBehaviour
{

    private Animator animator;
    private NavMeshAgent agent;

    private GameObject player;

    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private GameObject zombieHand, zombieFoot, zombieLeftHand;

    private PlayerController playerController;

    private bool spawned = false, isAttacking = false, playerDead = false, gotHit = false;
    private int health = 150, maxHealth = 150;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] zombieAudioClips;

    [SerializeField]
    private GameObject blood;

    public ZombieAttack[] zombieAttacks = new ZombieAttack[]
    {
        new ZombieAttack{ ColliderId=1, Name="Attack1" },
        new ZombieAttack{ ColliderId=2, Name="Attack2"},
        new ZombieAttack{ ColliderId=3, Name="Attack3"}
    };


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
        // audioSource.clip = zombieAudioClips[2];
        // audioSource.Play();

        audioSource.PlayOneShot(zombieAudioClips[2]);

        yield return new WaitForSeconds(2);
        animator.SetBool("Spawned", true);
        spawned = true;
        zombieHand.gameObject.SetActive(false);
        zombieLeftHand.gameObject.SetActive(false);
        zombieFoot.gameObject.SetActive(false);
    }

    void Update()
    {
        if (spawned && health > 0 && !playerDead)
        {
            CheckIfPlayerDead();
            if (!isAttacking && !gotHit)
            {
                agent.SetDestination(player.transform.position);
            }

            if (agent.velocity.magnitude == 0)
            {
                int attackIndex = Random.Range(0, 3);

                animator.SetBool("Walk", false);
                if (Vector3.Distance(player.transform.position, transform.position) < 2)
                {
                    transform.LookAt(player.transform);
                    if (!isAttacking && !gotHit)
                    {
                        animator.SetBool(zombieAttacks[attackIndex].Name, true);
                        DisableAttacks();
                        if (attackIndex == 0)
                            zombieHand.gameObject.SetActive(true);
                        else if (attackIndex == 1)
                            zombieLeftHand.gameObject.SetActive(true);
                        else if (attackIndex == 2)
                            zombieFoot.gameObject.SetActive(true);
                        isAttacking = true;

                        StartCoroutine(ResetAttack(attackIndex));
                    }

                }
                else
                {
                    animator.SetBool(zombieAttacks[attackIndex].Name, false);
                    animator.SetBool("Walk", true);
                    audioSource.clip = zombieAudioClips[1];
                    audioSource.loop = true;
                    isAttacking = false;
                }
            }
            else
            {
                animator.SetBool("Walk", true);
                audioSource.clip = zombieAudioClips[1];
                audioSource.loop = true;
                audioSource.Play();
            }
            UpdateHealthBar(health, maxHealth);
        }
        if (health <= 0)
        {
            animator.SetTrigger("Dead");
            audioSource.Stop();
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            healthBar.gameObject.SetActive(false);
        }
    }

    IEnumerator ResetAttack(int attackIndex)
    {
        // audioSource.clip = zombieAudioClips[0];
        // audioSource.Play();
        audioSource.PlayOneShot(zombieAudioClips[0]);
        yield return new WaitForSeconds(2.0f);
        animator.SetBool(zombieAttacks[attackIndex].Name, false);
        isAttacking = false;
        yield return new WaitForSeconds(0.20f);
        DisableAttacks();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerSword" && health > 0 && spawned)
        {
            health -= 35;
            gotHit = true;
            animator.SetBool("Walk", false);
            animator.SetBool("Hit", true);
            StartCoroutine(SplashBlood());
            StartCoroutine(ResetHit());
        }


        if (other.gameObject.tag == "RifleBullet" && health > 0 && spawned)
        {
            health -= 20;
            StartCoroutine(SplashBlood());
        }
    }

    IEnumerator SplashBlood()
    {
        blood.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.10f);
        blood.GetComponent<ParticleSystem>().Stop();
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
            zombieHand.gameObject.SetActive(false);
            zombieLeftHand.gameObject.SetActive(false);
            zombieFoot.gameObject.SetActive(false);

            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            healthBar.gameObject.SetActive(false);
            animator.SetBool("Walk", false);
            gotHit = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("COLLIDED WITH : " + other.gameObject.tag);
    }

    IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(1.2f);
        animator.SetBool("Hit", false);
        gotHit = false;

    }

    private void DisableAttacks()
    {
        zombieHand.gameObject.SetActive(false);
        zombieLeftHand.gameObject.SetActive(false);
        zombieFoot.gameObject.SetActive(false);
    }
}


