using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Patrol : MonoBehaviour
{
   private const float Speed = 5f;
   public float distance;

   private bool _movingRight = true;

   [SerializeField] private Transform groundDetection;

   private void Update()
   {
      transform.Translate(Vector2.right * Time.deltaTime);

      RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
      if (groundInfo.collider == false)
      {
         if (_movingRight == true)
         {
            transform.eulerAngles = new Vector3(0, -180, 0);
            _movingRight = false;
         }
         else
         {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _movingRight = true;
         }
      }
      

   }
}
