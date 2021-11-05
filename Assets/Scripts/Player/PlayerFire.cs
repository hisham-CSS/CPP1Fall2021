using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerFire : MonoBehaviour
{
    public bool verbose;

    SpriteRenderer sr;
    Animator anim;

    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    //This is an important distinction from the speed that is on the projectile itself
    //paying attention to how we use this variable is key to solving the problem of the lab
    public float projectileSpeed;

    public Projectile projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {

        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (projectileSpeed <= 0)
            projectileSpeed = 7.0f;

        if (!spawnPointLeft || !spawnPointRight || !projectilePrefab)
            if (verbose)
                Debug.Log("Inspector values not set");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Fire");
        }
    }

    void FireProjectile()
    {
        if (sr.flipX)
        {
            Projectile projectileInstance = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            projectileInstance.speed = projectileSpeed;
        }
        else
        {
            Projectile projectileInstance = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
            projectileInstance.speed = projectileSpeed;
        }
        
    }
}
