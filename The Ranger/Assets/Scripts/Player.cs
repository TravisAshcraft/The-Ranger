using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding;
    [SerializeField] int health = 100;
    float explosionTime = 1f;
    [SerializeField] GameObject explosion;
    [SerializeField] AudioClip explosionSFX;

    [Header("Fire")]
    [SerializeField] GameObject playerLaser;
    [SerializeField] float laserSpeed;
    [SerializeField] float delayGameOver = 3f;
    Coroutine firingCoroutine;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    private float projectileFiringPeriod = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        setUpMoveBoundaries();

    }

    private void setUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        DieOnZero(damageDealer);
    }

    private void DieOnZero(DamageDealer damageDealer)
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
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject boom = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(boom, explosionTime);
        AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position);


    }

    
   

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireCountinously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireCountinously()
    {  
        while (true)
        {
            GameObject laser = Instantiate(playerLaser,
                transform.position,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
            
        }
           
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXpos = Mathf.Clamp(transform.position.x + deltaX, xMin,xMax) ;
        
        var newYpos = Mathf.Clamp(transform.position.y + deltaY, yMin,yMax) ;

        transform.position = new Vector2(newXpos, newYpos);
    } 
}
