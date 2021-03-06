using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(PlayerSounds))]
public class Player : MonoBehaviour
{
    public bool verbose = false;
    public bool isGrounded;
    public AudioClip jumpClip;
    public AudioMixerGroup soundFXGroup;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    PlayerSounds ps;

    [SerializeField]
    float speed;
    [SerializeField]
    int jumpForce;
    [SerializeField]
    int bounceForce;

    [SerializeField]
    float groundCheckRadius;
        
    public LayerMask isGroundLayer;
    public Transform groundCheck;

    bool coroutineRunning = false;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        ps = GetComponent<PlayerSounds>();

        if (speed <= 0)
        {
            speed = 5.0f;
            if (verbose)
                Debug.Log("Speed value is garbage - setting default speed to 5");
        }

        if (jumpForce <= 0)
        {
            jumpForce = 300;
            if (verbose)
                Debug.Log("Jump Force value is garbage - setting default jump force to 300");
        }

        if (bounceForce <= 0)
        {
            bounceForce = 100;
            if (verbose)
                Debug.Log("Bounce Force value is garbage - setting default bounce force to 100");
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.05f;
            if (verbose)
                Debug.Log("Ground check radius value is garbage - setting default ground check to 0.05");
        }

        if (!groundCheck)
        {
            if (verbose)
                Debug.Log("Ground check transform is not set, please create empty gameobject and assign to groundcheck");
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = 0;
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Fire"))
            horizontalInput = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
            if (jumpClip)
                ps.Play(jumpClip, soundFXGroup
                    );
        }

        Vector2 moveDirection = new Vector2(horizontalInput * speed, rb.velocity.y);
        rb.velocity = moveDirection;

        anim.SetFloat("speed", Mathf.Abs(horizontalInput));
        anim.SetBool("isGrounded", isGrounded);



        if (sr.flipX && horizontalInput > 0 || !sr.flipX && horizontalInput < 0)
            sr.flipX = !sr.flipX;
    }

    public void StartJumpForceChange()
    {
        if (!coroutineRunning)
        {
            StartCoroutine("JumpForceChange");
        }
        else
        {
            StopCoroutine("JumpForceChange");
            jumpForce /= 2;
            StartCoroutine("JumpForceChange");
        }
    }

    IEnumerator JumpForceChange()
    {
        coroutineRunning = true;
        jumpForce *= 2;

        yield return new WaitForSeconds(5.0f);
        
        jumpForce /= 2;
        coroutineRunning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Squish"))
        {
            collision.gameObject.GetComponentInParent<EnemyWalker>().IsSquished();
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * bounceForce);
            Destroy(collision.gameObject);
        }
    }
}
