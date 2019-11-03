using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerController : MonoBehaviour
{

    public PlayerAnimState playerAnimState;

    [Header("Properties")]
    public Animator playerAnimator;
    public SpriteRenderer playerSpriteRenderer;
    public Rigidbody2D playerRigidBody;

    [Header("Movement")]
    public float moveForce;
    public float jumpForce;
    public Vector2 maximumVelocity = new Vector2();

    public bool isGrounded;
    public Transform groundTarget;

    public float attackCD;
    private float myTime = 0f;

    public GameController gameController;
    public Transform respawnPoint;

    public AudioSource movemnetSound;
    public AudioSource deathSound;
    public AudioSource coinSound;
    public AudioSource jumpSound;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimState = PlayerAnimState.IDLE;
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        isGrounded = Physics2D.BoxCast(
            transform.position,
            new Vector2(2.0f, 1.0f),
            0.0f,
            Vector2.down,
            1.0f,
            1 << LayerMask.NameToLayer("Ground"));


        //Stop
        if (Input.GetAxis("Horizontal") == 0 && isGrounded)
        {
            playerAnimState = PlayerAnimState.IDLE;
            playerAnimator.SetInteger("AnimState", (int)PlayerAnimState.IDLE);
        }

        //Right
        if (Input.GetAxis("Horizontal") > 0)
        {
            playerSpriteRenderer.flipX = true;
            playerAnimState = PlayerAnimState.WALK;
            playerAnimator.SetInteger("AnimState", (int)PlayerAnimState.WALK);
            playerRigidBody.AddForce(Vector2.right * moveForce);
        }

        //Left
        if (Input.GetAxis("Horizontal") < 0)
        {

            playerSpriteRenderer.flipX = false;
            playerAnimState = PlayerAnimState.WALK;
            playerAnimator.SetInteger("AnimState", (int)PlayerAnimState.WALK);
            playerRigidBody.AddForce(Vector2.left * moveForce);
        }

        //Jump
        if (Input.GetAxis("Jump") > 0 && isGrounded)
        {
            playerAnimState = PlayerAnimState.JUMP;
            playerAnimator.SetInteger("AnimState", (int)PlayerAnimState.JUMP);
            playerRigidBody.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }

        //Midair
        if (!isGrounded)
        {
            playerAnimState = PlayerAnimState.JUMP;
        }

        //Movement Sound
        if (Input.GetButtonDown("Jump"))
        {
            jumpSound.Play();
        }


        playerRigidBody.velocity = new Vector2(
            Mathf.Clamp(playerRigidBody.velocity.x, -maximumVelocity.x, maximumVelocity.x),
            Mathf.Clamp(playerRigidBody.velocity.y, -maximumVelocity.y, maximumVelocity.y)
            );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Coin":
                gameController.Score += 100;
                Destroy(other.gameObject);
                coinSound.Play();
                break;

            case "Super Coin":
                gameController.Score += 500;
                Destroy(other.gameObject);
                coinSound.Play();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                deathSound.Play();
                gameController.Score -= 1000;
                Reset();
                break;

            case "Death Plane":
                deathSound.Play();
                gameController.Score -= 1000;
                Reset();
                break;
        }
    }

    private void Reset()
    {
        gameObject.transform.position = respawnPoint.position;
    }
}