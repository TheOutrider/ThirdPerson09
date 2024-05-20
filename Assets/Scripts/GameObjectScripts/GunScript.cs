using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    [Header("FireRate")]
    [SerializeField] private float fireRate;
    [SerializeField] bool semiAuto;
    float fireRateTimer;

    [Header("Bullet Properties")]
    [SerializeField] private GameObject rifleBullet;
    [SerializeField] private Transform barrelPosition, bulletParent;
    [SerializeField]
    private float bulletHitMissDistance = 155.0f;

    private Transform cameraMainTransform;
    [SerializeField]
    private Slider gunHeatSlider;

    [SerializeField]
    private GameObject smoke, muzzleFlash;

    private AudioSource audioSource;

    private float fireLimit = 0;
    [SerializeField] private float heatFactor;
    private bool heatOverload;

    void Start()
    {
        fireRateTimer = fireRate;
        cameraMainTransform = Camera.main.transform;
        heatOverload = false;
        smoke.GetComponent<ParticleSystem>().Stop();
        muzzleFlash.GetComponent<ParticleSystem>().Stop();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        smoke.GetComponent<ParticleSystem>().Stop();
        muzzleFlash.GetComponent<ParticleSystem>().Stop();
    }

    private void Update()
    {

        if (ShouldFire()) Fire();
        if (!heatOverload)
        {
            if (fireLimit > 0)
            {
                fireLimit -= Time.deltaTime;
                heatOverload = false;
            }
            if (fireLimit > 1.2f)
            {
                heatOverload = true;
                audioSource.Play();
            }
        }
        else
        {

            StartCoroutine(ResetHeat());
        }
        gunHeatSlider.value = fireLimit;
    }

    public bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

    public void Fire()
    {
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        StartCoroutine(ResetMuzzleFlash());
        if (!heatOverload)
        {
            fireRateTimer = 0;
            smoke.GetComponent<ParticleSystem>().Stop();
            RaycastHit hit;
            GameObject bullet = GameObject.Instantiate(rifleBullet, barrelPosition.position, Quaternion.identity, bulletParent);

            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (Physics.Raycast(cameraMainTransform.position, cameraMainTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }
            else
            {
                bulletController.target = cameraMainTransform.position + cameraMainTransform.forward * bulletHitMissDistance;
                bulletController.hit = true;
            }

            fireLimit += Time.deltaTime * heatFactor;
        }

    }

    IEnumerator ResetMuzzleFlash()
    {
        yield return new WaitForSeconds(0.10f);
        muzzleFlash.GetComponent<ParticleSystem>().Stop();
    }

    IEnumerator ResetHeat()
    {
        smoke.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1.25f);
        smoke.GetComponent<ParticleSystem>().Stop();
        audioSource.Stop();
        heatOverload = false;
    }


}
