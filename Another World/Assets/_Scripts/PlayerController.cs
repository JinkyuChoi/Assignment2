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

        //Attack
        /*
        myTime += Time.deltaTime;


        if (Input.GetButton("Fire1") && myTime > attackCD)
        {
            playerAnimState = PlayerAnimState.ATTACK;
            playerAnimator.SetInteger("AnimState", (int)PlayerAnimState.ATTACK);

            myTime = 0.0f;
        }
        */

        playerRigidBody.velocity = new Vector2(
            Mathf.Clamp(playerRigidBody.velocity.x, -maximumVelocity.x, maximumVelocity.x),
            Mathf.Clamp(playerRigidBody.velocity.y, -maximumVelocity.y, maximumVelocity.y)
            );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                //_thunderSound.Play();
                gameController.Lives -= 1;
                break;
            case "Coin":
                //_yaySound.Play();
                gameController.Score += 100;
                break;
        }
    }
}