﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header(" Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 50;
    [Header("Enemy Shoot")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyLaser;
    [SerializeField] float laserSpeed = 5f;
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionTime = 3f;
    [Header("SFX")]
    [SerializeField] AudioClip laserSFX;
    [SerializeField] AudioClip explosionSFX;
    
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
       GameObject laser =  Instantiate(enemyLaser,
           transform.position, 
           Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);
        AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position);
    }

    //using other refering to the other gameobject collding with enemy.
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        DieFromHit(damageDealer);
    }

   
    private void DieFromHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();

        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject boom = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(boom, explosionTime);
        AudioSource.PlayClipAtPoint(explosionSFX,
            Camera.main.transform.position);
        
    }
}
