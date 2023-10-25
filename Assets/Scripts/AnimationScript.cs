using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator anim;
    private PlayerController move;
    private Collision coll;
    [HideInInspector]
    public SpriteRenderer sr;
    int random;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponentInParent<Collision>();
        move = GetComponentInParent<PlayerController>();
        sr = GetComponent<SpriteRenderer>();

        StartCoroutine(RandomParpadeo());
    }

    void Update()
    {
        anim.SetBool("onGround", coll.onGround);
        anim.SetBool("onWall", coll.onWall);
        anim.SetBool("onRightWall", coll.onRightWall);
        anim.SetBool("wallGrab", move.wallGrab);
        anim.SetBool("wallSlide", move.wallSlide);
        anim.SetBool("canMove", move.canMove);
        anim.SetBool("isDashing", move.isDashing);
        anim.SetBool("isShoting", move.isShoting);

        if(move.currentShot == PlayerController.ShotingType.bolas) 
        {
            anim.SetInteger("Bullet", 0);
        }
        else
            anim.SetInteger("Bullet", 1);


    }

    public void SetHorizontalMovement(float x, float y, float yVel)
    {
        anim.SetFloat("HorizontalAxis", x);
        anim.SetFloat("VerticalAxis", y);
        anim.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void Flip(int side)
    {

        if (move.wallGrab || move.wallSlide)
        {
            if (side == -1 && sr.flipX)
                return;

            if (side == 1 && !sr.flipX)
            {
                return;
            }
        }

        bool state = (side == 1) ? false : true;
        sr.flipX = state;
    }

    IEnumerator RandomParpadeo()
    {
        yield return new WaitForSeconds(1);
        random = Random.Range(0, 2);
        if(random==0) 
        {
            anim.SetTrigger("Parpadeo");
        }
        StartCoroutine(RandomParpadeo());
    }
}
