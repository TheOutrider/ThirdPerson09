using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float jogSpeed = 5.0f;
    [SerializeField]
    private float sprintSpeed = 7.5f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;

    public int playerHealth = 100;

    [SerializeField]
    private float mouseSensitivity = 1;

    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference jumpControl;
    [SerializeField]
    private InputActionReference sprintControl;
    [SerializeField]
    private InputActionReference aimControl;
    [SerializeField]
    private InputActionReference shootControl;

    // [SerializeField]
    // private GameObject bulletPrefab;
    // [SerializeField]
    // private Transform barellTransform, bulletParent;
    // [SerializeField]
    // private float bulletHitMissDistance = 155.0f;
    public float throwSpeed = 5f;

    private bool isJumping = false,
        isFalling = false,
        isSprinting = false,
        groundedPlayer = true,
        isAiming = false, gotHit = false, swordActive = false, isDead = false, isAttacking = false;

    private int swordClicks = 0;

    string[] swordAttacks = new string[] { "SwordHit1", "SwordHit2", "SwordHit3" };

    [SerializeField]


    private Transform cameraMainTransform;
    private Animator animator;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private RigBuilder rigBuilder;

    [SerializeField]
    private GameObject inactiveSword, activeSword, inactivePistol, activePistol, inactiveRifle, activeRifle, swordColliderObject;

    public int activeWeaponIndex = 0;

    [SerializeField]
    private Canvas pauseCanvas, gameCompletedCanvas;

    private PauseMenu pauseMenu;

    [SerializeField]
    private GameObject footsteps, footStepsRunning;
    [SerializeField]
    private AudioClip[] swordAudioClips;

    private AudioSource audioSource;




    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        sprintControl.action.Enable();
        aimControl.action.Enable();
        shootControl.action.Enable();

    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        sprintControl.action.Disable();
        aimControl.action.Disable();
        shootControl.action.Disable();
    }

    private void Start()
    {
        pauseMenu = pauseCanvas.GetComponent<PauseMenu>();

        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        rigBuilder = GetComponent<RigBuilder>();
        animator = GetComponent<Animator>();

        cameraMainTransform = Camera.main.transform;
        footsteps.SetActive(false);
        footStepsRunning.SetActive(false);
        ToggleWeapons();
        activeSword.gameObject.SetActive(false);
        activePistol.gameObject.SetActive(false);
        activeRifle.gameObject.SetActive(false);

        int continueGame = PlayerPrefs.GetInt("CONTINUE");

        if (continueGame == 1)
        {
            PlayerData data = SaveSystem.LoadPlayer();
            // Debug.Log(JsonUtility.ToJson(data));
            if (data != null)
            {
                transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
                playerHealth = data.health;
            }
        }


        PlayerPrefs.DeleteAll();

    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && !isDead && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            animator.SetBool("Jump", false);
            animator.SetBool("Falling", false);

            if (isFalling)
            {
                StartCoroutine("FallToLand");
            }
        }

        if (playerHealth > 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.PauseGame();
        }


        //=======================================================================================================

        Vector2 movement = movementControl.action.ReadValue<Vector2>();

        Vector3 move = new Vector3(movement.x, 0, movement.y);
        if (move == Vector3.zero)
        {
            footsteps.SetActive(false);
            footStepsRunning.SetActive(false);
            animator.SetFloat("Speed", 0);
        }
        else
        {
            if (!isJumping && !isFalling && !isDead)
            {
                if (!swordActive)
                    if (isSprinting && !isAiming)
                    {
                        footStepsRunning.SetActive(true);
                        footsteps.SetActive(false);
                        activeSword.gameObject.SetActive(false);
                        inactiveSword.gameObject.SetActive(true);
                    }
                    else
                    {
                        footsteps.SetActive(true);
                        footStepsRunning.SetActive(false);
                    }
            }
            animator.SetFloat("Speed", isSprinting && !isAiming ? 1.0f : 0.5f, Time.deltaTime, Time.deltaTime);
        }
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0;
        float playerSpeed = isSprinting && !isAiming ? sprintSpeed : jogSpeed;
        // float playerSpeed = isSprinting && !isAiming ?  Mathf.Lerp(sprintSpeed, jogSpeed, Time.deltaTime) : Mathf.Lerp(jogSpeed, sprintSpeed, Time.deltaTime);

        if (!isFalling && !isDead && !swordActive)
        {
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (isAiming)
            {
                AnimateAimMovement(movement);
            }
        }

        CheckIfDead();

        if (gotHit)
        {
            gotHit = false;
            // playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.5f * (gravityValue / 2));
            // playerVelocity.z -= 5f * Time.deltaTime;
            StartCoroutine(ResetGotHit());

        }

        if (jumpControl.action.triggered && groundedPlayer && !isAiming)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetBool("Jump", true);
            footsteps.SetActive(false);
            footStepsRunning.SetActive(false);
            isJumping = true;
            StartCoroutine(ResetJump());
        }
        else
        {
            if (playerVelocity.y < -10.0f)
            {
                isFalling = true;
                rigBuilder.enabled = false;
                animator.SetBool("Falling", true);
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        if (!isDead)
            controller.Move(playerVelocity * Time.deltaTime);

        sprintControl.action.performed += ctx =>
        {
            if (!isAiming || !isFalling) isSprinting = true;
        };
        sprintControl.action.canceled += ctx => isSprinting = false;

        aimControl.action.performed += ctx =>
        {
            if (!isDead && !isFalling && !isJumping && !swordActive)
                AimPerformed(movement);
        };
        aimControl.action.canceled += ctx => AimCanceled();
        if (shootControl.action.triggered && !isDead)
        {
            if (isAiming)
                ShootGun();
            else
            {
                BeginSwordAttack();
                ToggleWeapons();
                activeSword.gameObject.SetActive(true);
                inactiveSword.gameObject.SetActive(false);
            }
        }

        if (Time.deltaTime == 0)
        {
            return;
        }
        else if (movement != Vector2.zero && !isAiming && !isDead)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
        else if (isAiming && !isDead)
        {
            Vector3 targetPosition = new Vector3(cameraMainTransform.forward.x, 0, cameraMainTransform.forward.z);
            transform.forward = targetPosition * rotationSpeed * Time.deltaTime;
        }

        if (isAttacking)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 2);
        }

        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     activeWeaponIndex = 0;
        // }
        // else if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     activeWeaponIndex = 1;
        // }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "GiantsHand")
        {
            HandlePlayerHealth(5, true);
        }
        else if (other.gameObject.tag == "ZombieHand")
        {
            HandlePlayerHealth(2, false);
        }
        else if (other.gameObject.tag == "ZombieFoot")
        {
            HandlePlayerHealth(3, true);
        }
        else if (other.gameObject.tag == "AlienBullet")
        {
            HandlePlayerHealth(1, false);
        }
        else if (other.gameObject.tag == "MagicBullet")
        {
            HandlePlayerHealth(7, false);
        }
        else if (other.gameObject.tag == "Fireball")
        {
            HandlePlayerHealth(4, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "HealthPlus")
        {
            HandlePlayerHealth(-10, false);
        }

    }

    IEnumerator FallToLand()
    {
        yield return new WaitForSeconds(0.5f);
        isFalling = false;
        isJumping = false;
    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(1.0f);
        isJumping = false;
    }



    private void AimPerformed(Vector2 movement)
    {
        // rigBuilder.enabled = true;
        isAiming = true;
        audioSource.clip = swordAudioClips[2];
        audioSource.Play();
        animator.SetBool(activeWeaponIndex == 0 ? "RifleActive" : "PistolActive", true);
        if (activeWeaponIndex == 0)
        {
            ToggleWeapons();
            activeRifle.gameObject.SetActive(true);
            inactiveRifle.gameObject.SetActive(false);
            rigBuilder.enabled = true;
        }
        else if (activeWeaponIndex == 1)
        {
            ToggleWeapons();
            activePistol.gameObject.SetActive(true);
            inactivePistol.gameObject.SetActive(false);
        }
        Vector3 targetPosition = new Vector3(cameraMainTransform.forward.x, 0, cameraMainTransform.forward.z);
        transform.forward = targetPosition * rotationSpeed * Time.deltaTime;

    }

    private void AimCanceled()
    {
        rigBuilder.enabled = false;
        isAiming = false;
        animator.SetBool(activeWeaponIndex == 0 ? "RifleActive" : "PistolActive", false);
        ToggleWeapons();
        activeSword.gameObject.SetActive(true);
        inactiveSword.gameObject.SetActive(false);
    }

    private void AnimateAimMovement(Vector2 movement)
    {
        animator.SetFloat("VerticalSpeed", movement.y, 0.0f, Time.deltaTime);
        animator.SetFloat("HorizontalSpeed", movement.x, 0.0f, Time.deltaTime);
    }

    private void ShootGun()
    {
        // RaycastHit hit;
        // GameObject bullet = GameObject.Instantiate(bulletPrefab, barellTransform.position, Quaternion.identity, bulletParent);
        // audioSource.clip = swordAudioClips[3];
        // audioSource.Play();
        // BulletController bulletController = bullet.GetComponent<BulletController>();
        // if (Physics.Raycast(cameraMainTransform.position, cameraMainTransform.forward, out hit, Mathf.Infinity))
        // {
        //     bulletController.target = hit.point;
        //     bulletController.hit = true;
        // }
        // else
        // {
        //     bulletController.target = cameraMainTransform.position + cameraMainTransform.forward * bulletHitMissDistance;
        //     bulletController.hit = true;
        // }
    }

    private void BeginSwordAttack()
    {
        if (!swordActive)
        {
            swordActive = true;
            string attack = swordAttacks[swordClicks];
            swordColliderObject.gameObject.SetActive(true);
            animator.SetBool(attack, true);
            audioSource.clip = swordClicks == 2 ? swordAudioClips[1] : swordAudioClips[0];

            StartCoroutine(ResetSwordAttack(attack));
        }

    }

    IEnumerator ResetSwordAttack(string attack)
    {
        isAttacking = true;
        if (swordClicks == 2)
        {
            yield return new WaitForSeconds(0.5f);
            audioSource.Play();
            isAttacking = false;

        }
        else
        {
            audioSource.Play();
        }
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        audioSource.Stop();
        if (swordClicks < 2)
        {
            swordClicks++;
        }
        else
        {
            swordClicks = 0;
        }
        swordActive = false;
        swordColliderObject.gameObject.SetActive(false);
        animator.SetBool(attack, false);
    }


    public void HandlePlayerHealth(int damage, bool takeHit)
    {
        playerHealth -= damage;
        gotHit = takeHit;
    }

    IEnumerator ResetGotHit()
    {
        animator.SetTrigger("TakeHit");
        rigBuilder.enabled = false;
        animator.ResetTrigger("Idle");
        if (isAiming)
        {
            yield return new WaitForSeconds(0.5f);
            rigBuilder.enabled = true;
        }

        animator.ResetTrigger("TakeHit");
        if (playerHealth > 0)
        {
            animator.SetTrigger("Idle");
        }
        else
        {
            CheckIfDead();
        }
    }

    private void ToggleWeapons()
    {
        inactiveSword.gameObject.SetActive(true);
        inactivePistol.gameObject.SetActive(true);
        inactiveRifle.gameObject.SetActive(true);

        activeSword.gameObject.SetActive(false);
        activePistol.gameObject.SetActive(false);
        activeRifle.gameObject.SetActive(false);
    }

    public void Respawn()
    {
        isDead = false;
        animator.ResetTrigger("Dead");
        animator.SetTrigger("Idle");
    }

    public void GameCompleted()
    {
        StartCoroutine(CompleteGame());
    }

    IEnumerator CompleteGame()
    {
        yield return new WaitForSeconds(3);
        gameCompletedCanvas.gameObject.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MenuScene");
        PlayerPrefs.SetInt("SHOWAUTOSAVE", 1);
    }

    public void CheckIfDead()
    {
        if (playerHealth <= 0)
        {
            isDead = true;
            animator.SetTrigger("Dead");
            animator.ResetTrigger("TakeHit");
        }
    }
}
