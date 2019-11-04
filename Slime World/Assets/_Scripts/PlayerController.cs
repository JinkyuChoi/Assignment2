using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

//2019-11-03 by Jinkyu Choi
public class PlayerController : MonoBehaviour
{
    [Header("Properties")]
    public Animator playerAnimator;
    public SpriteRenderer playerSpriteRenderer;
    public Rigidbody2D playerRigidBody;
    public GameController gameController;


    [Header("Animation Control")]
    public PlayerAnimState playerAnimState;

    [Header("Movement Control")]
    public float moveForce;
    public float jumpForce;
    public Vector2 maximumVelocity = new Vector2();
    public bool isGrounded;
    public Transform groundTarget;

    [Header("Audio Control")]
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

    //code from Tom Tsiliopoulos "In class"
    void Move()
    {
        //Checking if the Enemy is on ground or midair
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

        //Jump Sound
        if (Input.GetButtonDown("Jump"))
        {
            jumpSound.Play();
        }

        //Restricts maximum velocity to certain amount
        playerRigidBody.velocity = new Vector2(
            Mathf.Clamp(playerRigidBody.velocity.x, -maximumVelocity.x, maximumVelocity.x),
            Mathf.Clamp(playerRigidBody.velocity.y, -maximumVelocity.y, maximumVelocity.y)
            );
    }

    //code from Tom Tsiliopoulos "Mail Pilot"
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

            //If you enter finish object the game will set to gameEnd true in gameController
            case "Finish":
                gameController.gameEnd = true;
                Destroy(this.gameObject);
                break;
        }
    }

    //code from Tom Tsiliopoulos "Mail Pilot"
    //If player gets hit by enemy or fall down it will decrease the score and reset player
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                deathSound.Play();
                gameController.Hitpoint -= 1;
                break;

            case "Death Plane":
                deathSound.Play();
                gameController.Hitpoint -= 100;
                break;
        }
    }



}