using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovingState : MonoBehaviour
{
    public void Raycasting()
    {

    }
}

//public class Enemy : MonoBehaviour
//{

//    private Transform myTransform;
//    private float speed = 5.0f;
//    private bool isWalking = true;
//    private Vector3 curNormal = Vector3.up;
//    private Vector3 hitNormal = Vector3.zero;

//    void Start()
//    {
//        myTransform = transform;
//    }

//    void Update()
//    {
//        switch (isWalking)
//        {
//            case true:
//                // check for wall
//                RaycastHit rayHit;
//                if (Physics.Raycast(myTransform.position, myTransform.forward, out rayHit, 1))
//                {
//                    hitNormal = rayHit.normal;
//                    isWalking = false;
//                }
//                Debug.DrawRay(myTransform.position, myTransform.forward * 1.0f, Color.red);    // show forward ray    

//                // check for no floor    
//                Vector3 checkRear = myTransform.position + (-myTransform.forward * 0.25f);
//                if (Physics.Raycast(checkRear, -myTransform.up, out rayHit, 1))
//                {
//                    // there is a floor!
//                }
//                else
//                {
//                    // find the floor around the corner
//                    Vector3 checkPos = myTransform.position + (myTransform.forward * 0.5f) + (-myTransform.up * 0.51f);
//                    Debug.DrawRay(checkPos, -myTransform.forward * 1.5f, Color.green);    // show floor check ray
//                    if (Physics.Raycast(checkPos, -myTransform.forward, out rayHit, 1))
//                    {
//                        Debug.Log("HitNormal " + rayHit.normal);
//                        hitNormal = rayHit.normal;
//                        isWalking = false;
//                    }
//                }
//                Debug.DrawRay(myTransform.position, -myTransform.up * 1.0f, Color.red);    // show down ray
//                // move forward
//                MoveForward();
//                break;

//            case false:
//                curNormal = Vector3.Lerp(curNormal, hitNormal, 4.0f * Time.deltaTime);
//                Quaternion grndTilt = Quaternion.FromToRotation(Vector3.up, curNormal);
//                transform.rotation = grndTilt;
//                float check = (curNormal - hitNormal).sqrMagnitude;
//                if (check < 0.001f)
//                {
//                    grndTilt = Quaternion.FromToRotation(Vector3.up, hitNormal);
//                    transform.rotation = grndTilt;
//                    isWalking = true;
//                }
//                break;
//        }
//    }

//    void MoveForward()
//    {
//        myTransform.position += transform.forward * speed * Time.deltaTime;
//    }

//}