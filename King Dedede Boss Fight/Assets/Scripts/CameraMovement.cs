using NodeCanvas.Tasks.Actions;
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

    //Other Objects
    [SerializeField] private GameObject followTarget;

    [SerializeField] private Camera cameraMain;

    [SerializeField] private float catchUpSpeed;
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if you want to make the camera freeze once we get in a trigger location, then set this GameObject tag to Lock Camera
        //PERMANENTLY locks camera
        if (!gameObject.CompareTag("LockCamera"))
            return;

        if(other.gameObject.CompareTag("Player"))
            isFollowing = false;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isFollowing)
            return;

        
        //if the player is within the bounds of the camera follow, then follow
        if (collision.gameObject.CompareTag("Player"))
        {
            //once the diff is 0, then regular movement
            Vector3 diff = (followTarget.transform.position - cameraMain.transform.position);

            CheckFollowType(diff);
        }
    }

    private void CheckFollowType(Vector3 diff)
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

    //Puts in either X, Y, or both to check to see if needs to catch up, then catches up
    private void CheckCameraCatchUp(Vector3 diffMagXY)
    {
        diffMagXY.Normalize();
        cameraMain.transform.position += diffMagXY * catchUpSpeed * Time.deltaTime;
    }

    private void FollowHorizontal(Vector3 diff)
    {
        Vector3 direction = Vector3.right * diff.x;

        bool camNeedsToCatchUp = Mathf.Abs(diff.x) > 0.05f;
        if (camNeedsToCatchUp)
            CheckCameraCatchUp(direction);
        else
            cameraMain.transform.Translate(direction);
    }

    private void FollowVertical(Vector3 diff)
    {
        Vector3 direction = Vector3.up * diff.y;

        bool camNeedsToCatchUp = Mathf.Abs(diff.y) > 0.1f;
        if (camNeedsToCatchUp)
            CheckCameraCatchUp(direction);
        else
            cameraMain.transform.Translate(direction);
    }
}
