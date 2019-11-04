using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

//2019-11-03 by Jinkyu Choi
public class EnemyController : MonoBehaviour
{
    [Header("Properties")]
    public Animator enemyAnimator;
    public Rigidbody2D enemyRigidBody;
    public Transform lookAhead;
    public Transform wallAhead;

    [Header("Movement Control")]
    public bool isGrounded;
    public bool hasGroundAhead;
    public bool hasWallAhead;
    public bool isFacingLeft = true;
    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {

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
            transform.position, new Vector2(2.0f, 1.0f), 0.0f, Vector2.down, 1.0f, 1 << LayerMask.NameToLayer("Ground"));

        //Checking if the Enemy is going to fall
        hasGroundAhead = Physics2D.Linecast(
            transform.position,
            lookAhead.position,
            1 << LayerMask.NameToLayer("Ground"));

        //Checking if the Enemy is going to hit the wall
        hasWallAhead = Physics2D.Linecast(
            transform.position,
            wallAhead.position,
            1 << LayerMask.NameToLayer("Ground"));

        if (isGrounded)
        {
            //Move left if it's going to fall or didn't hit the wall 
            if (isFacingLeft)
            {
                enemyRigidBody.velocity = new Vector2(-movementSpeed, 0.0f);
            }

            //Move right if it's going to fall or didn't hit the wall 
            if (!isFacingLeft)
            {
                enemyRigidBody.velocity = new Vector2(movementSpeed, 0.0f);
            }

            //If there is no ground in front or if there is wall in front turn around
            if (!hasGroundAhead || hasWallAhead)
            {
                transform.localScale = new Vector3(-transform.localScale.x, 3.0f, 1.0f);
                isFacingLeft = !isFacingLeft;
            }
        }
    }
}
