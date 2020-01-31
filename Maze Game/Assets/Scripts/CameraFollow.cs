using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Follows and looks at the player using spring type motion for smoothing. 
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform player;                    //The object/player to follow
    public Vector3 offset;                      //Position offset of the camera
    public float springConstantPos = 0.05f;     //Spring constant for position
    public float springConstantRot = 80f;       //Spring constant for rotation
    private LayerMask mask;                     //The mask to account for wall clipping

    /// <summary>
    /// Start is called before the first frame update - set the mask to walls
    /// </summary>
    void Start()
    {
        mask = LayerMask.GetMask("Wall");
    }

    /// <summary>
    /// Lerps both position and rotation smoothly and follows/looks at the player
    /// </summary>
    void LateUpdate()
    {
        Quaternion goalRotation = LerpRotation();

        Vector3 goalPosition = player.position + (goalRotation * offset);

        //Adapted from: https://www.reddit.com/r/Unity3D/comments/cfzv5r/made_a_thirdperson_camera_collision_adjustment/eudmi61/
        //Checks to see if a wall is in between the camera and the "goal position". 
        RaycastHit camHit;
        if (Physics.Linecast(transform.position, goalPosition, out camHit, mask))
        {
            //Resets the goal position with a weighted average in favor of the wall/hit point
            goalPosition = ((camHit.point * 9) + player.position) / 10;
        }

        Vector3 lerpPosition = Vector3.Lerp(transform.position, goalPosition, springConstantPos);


        transform.position = lerpPosition;
        transform.LookAt(player);
    }


    /// <summary>
    /// Rotates/lerps the camera around the player smoothly
    /// </summary>
    /// <returns></returns>
    private Quaternion LerpRotation()
    {
        float currentY = transform.eulerAngles.y;
        float goalY = player.transform.eulerAngles.y;
        float lerpAngle = Mathf.LerpAngle(currentY, goalY, Time.deltaTime * springConstantRot);

        return  Quaternion.Euler(0, lerpAngle, 0); ;
    }

}
