using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private enum CameraFollowType
    {
        Horizontal,
        Vertical,
        Both
    }
    [SerializeField] private CameraFollowType cameraFollowType;

    //Variables
    public bool isFollowing = true; //if the camera is following the player

    [SerializeField] private GameObject followTarget;

    [SerializeField] private Camera cameraMain;
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if you want to make the camera freeze once we get in a trigger location, then set the GameObject to Lock Camera.
        if (!gameObject.CompareTag("LockCamera"))
            return;

        if(other.gameObject.CompareTag("Player"))
        {
            //if kirby enters the trigger where the boss area is, initiate the boss fight and lock the camera position 
            isFollowing = false;

            cameraMain.transform.position = new Vector3(0,0,-10);
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isFollowing)
            return;

        //if the player is within the bounds of the camera follow, then follow
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 diff = (followTarget.transform.position - cameraMain.transform.position);

            CheckFollowType(diff);
        }
    }
   
    private void CheckFollowType(Vector2 diff)
    {
        switch(cameraFollowType)
        {
            case CameraFollowType.Horizontal:
                FollowHorizontal(diff);
                break;

            case CameraFollowType.Vertical:
                FollowVertical(diff);
                break;

            case CameraFollowType.Both:
                FollowHorizontal(diff);
                FollowVertical(diff);
                break;
        }
    }
    private void FollowHorizontal(Vector3 diff)
    {
        cameraMain.transform.Translate(Vector3.right * diff.x);
    }
    private void FollowVertical(Vector3 diff)
    {
        cameraMain.transform.Translate(Vector3.up * diff.y);
    }
}
