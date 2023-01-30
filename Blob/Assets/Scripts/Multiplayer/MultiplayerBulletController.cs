using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class MultiplayerBulletController : MonoBehaviour
{

    Rigidbody rb;

    public float bulletSpeed = 15f;
    public float bulletUptime = 5f;
    public GameObject bulletImpactEffect;

    public AudioClip bulletHitAudio;

    public int damage = 1;

    [HideInInspector]
    public Photon.Realtime.Player owner;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void InitialiseBullet(Vector3 originalDirection, Photon.Realtime.Player givenPlayer)
    {
        transform.forward = originalDirection;
        rb.velocity = transform.forward * bulletSpeed;

        owner = givenPlayer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.Play3D(bulletHitAudio, transform.position);

        VFXManager.Instance.PlayVFX(bulletImpactEffect, transform.position);

        Destroy(gameObject);
    }

    //The game object is destroyed after a certain number of seconds if it doesn't collide with anything
    private void FixedUpdate()
    {
        StartCoroutine(DestroyAfter(gameObject, bulletUptime));
    }

    IEnumerator DestroyAfter(GameObject gb, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gb);
    }

}
