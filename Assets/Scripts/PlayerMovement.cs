using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    
    private enum State
    {
        None,
        Idle,
        Walk,
        Jump
    }

    private State _currentState = State.None;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private PlayerFoot foot;

    private bool _isJumping = false;
    private bool _facingRight = true;
    private bool _jumpButtonDown = false;
    private bool _jumpButton = false;
    //private bool _jumpButtonUp = false;
    private bool _isSwitching = false;

    private bool top; // Pour la rotation.


    private float _jumpTimeCounter;
    private const float JumpTime = 0.5f;
    private const float DeadZone = 0.1f;
    private const float MoveSpeed = 4.0f;
    private float _jumpSpeed = 5.0f;

    void Start()
       {
           ChangeState(State.Jump);
           body = GetComponent<Rigidbody2D>();
       }
   
       private void Update()
       {
           if (Input.GetButtonDown("Jump"))
           {
               _jumpButtonDown = true;
           }
           
           if (Input.GetButton("Jump"))
           {
               _jumpButton = true;
           }

           if (Input.GetButtonUp("Jump"))
           {
               _isJumping = false;
           }
           
           
           if (Input.GetKeyDown(KeyCode.P)) // Pour la rotation 
           {                                //
               _isSwitching = true;
               body.gravityScale *= -1;     //
               Rotation();
           }
       }
   
       void FixedUpdate()
       {
   
           if (foot.FootContact_ > 0 && _jumpButtonDown)
           {
               if (_isSwitching)
               {
                   JumpSwitch();
               }
               else
               {
                  Jump(); 
               }
           }
           _jumpButtonDown = false;

           if (_jumpButton && _isJumping)
           {
               JumpVariation();
           }
           _jumpButton = false;
           

           /*if (_jumpButtonUp)
           {
               _isJumping = false;
           }*/
   
           var vel = body.velocity;
           body.velocity = new Vector2(MoveSpeed * Input.GetAxis("Horizontal"), vel.y);
           //We flip the characters when not facing in the right direction
           if (Input.GetAxis("Horizontal") > DeadZone && !_facingRight)
           {
               FlipX();
           }
   
           if (Input.GetAxis("Horizontal") < -DeadZone && _facingRight)
           {
               FlipX();
           }
           //We manage the state machine of the character
           switch (_currentState)
           {
               case State.Idle:
                   if (Mathf.Abs(Input.GetAxis("Horizontal")) > DeadZone)
                   {
                       ChangeState(State.Walk);
                   }
   
                   if (foot.FootContact_ == 0)
                   {
                       ChangeState(State.Jump);
                   }
                   break;
               case State.Walk:
                   if (Mathf.Abs(Input.GetAxis("Horizontal")) < DeadZone)
                   {
                       ChangeState(State.Idle);
                   }
   
                   if (foot.FootContact_ == 0)
                   {
                       ChangeState(State.Jump);
                   }
                   break;
               case State.Jump:
                   if (foot.FootContact_ > 0)
                   {
                       ChangeState(State.Idle);
                   }
                   break;
               default:
                   throw new ArgumentOutOfRangeException();
           }
           
       }
   
       private void Jump()
       {
           Debug.Log("Work");
           _isJumping = true;
           _jumpTimeCounter = JumpTime;
           var vel = body.velocity;
           body.velocity = new Vector2(vel.x, _jumpSpeed);
       }

       private void JumpSwitch()
       {
           Debug.Log("Work2");
           _isJumping = true;
           _jumpTimeCounter = JumpTime;
           _jumpSpeed = -(_jumpSpeed);
           var vel = body.velocity;
           body.velocity = new Vector2(vel.x, _jumpSpeed);
       }

       private void JumpVariation()
       {
           if (_jumpTimeCounter > 0)
           {
               var vel = body.velocity;
               body.velocity = new Vector2(vel.x, _jumpSpeed);
               _jumpTimeCounter -= Time.deltaTime;
           }
       }

       void ChangeState(State state)
       {
           switch (state)
           {
               case State.Idle:
                   anim.Play("Player_Idle");
                   break;
               case State.Walk:
                   anim.Play("Player_Walk");
                   break;
               case State.Jump:
                   anim.Play("Player_Jump");
                   break;
               default:
                   throw new ArgumentOutOfRangeException(nameof(state), state, null);
           }
   
           _currentState = state;
       }
   
       void FlipX()
       {
           spriteRenderer.flipX = !spriteRenderer.flipX;
           _facingRight = !_facingRight;
       }

       void FlipY()
       {
           spriteRenderer.flipY = !spriteRenderer.flipY;
           
       }

       void Rotation()   // Pour la rotation.
       {
           if (top == false)
           {
               
               transform.eulerAngles = new Vector3(0, 0, 180f);
               spriteRenderer.flipX = !spriteRenderer.flipX;
           }
           else
           {
               transform.eulerAngles = Vector3.zero;
               spriteRenderer.flipX = !spriteRenderer.flipX;
           }

        top = !top;
       }
}
