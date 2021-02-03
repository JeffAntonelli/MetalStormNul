using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour // Salut on test. bloup bloup
{
   void Start()
       {
           ChangeState(State.Jump);
       }
   
       private void Update()
       {
           if (Input.GetButtonDown("Jump"))
           {
               _jumpButtonDown = true;
           }
       }
   
       void FixedUpdate()
       {
   
           if (foot.FootContact_ > 0 && _jumpButtonDown)
           {
               Jump();
           }
           _jumpButtonDown = false;
   
           var vel = body.velocity;
           body.velocity = new Vector2(MoveSpeed * Input.GetAxis("Horizontal"), vel.y);
           //We flip the characters when not facing in the right direction
           if (Input.GetAxis("Horizontal") > DeadZone && !_facingRight)
           {
               Flip();
           }
   
           if (Input.GetAxis("Horizontal") < -DeadZone && _facingRight)
           {
               Flip();
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
           FMODUnity.RuntimeManager.PlayOneShot(jumpEvent, transform.position);
           var vel = body.velocity;
           body.velocity = new Vector2(vel.x, JumpSpeed);
       }
   
       void ChangeState(State state)
       {
           switch (state)
           {
               case State.Idle:
                   anim.Play("Idle");
                   break;
               case State.Walk:
                   anim.Play("Walk");
                   break;
               case State.Jump:
                   anim.Play("Jump");
                   break;
               default:
                   throw new ArgumentOutOfRangeException(nameof(state), state, null);
           }
   
           _currentState = state;
       }
   
       void Flip()
       {
           spriteRenderer.flipX = !spriteRenderer.flipX;
           _facingRight = !_facingRight;
       }
       
}
