using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }


    private void BasicFollowCamera (Vector3 tPos, Vector3 tUp, Vector3 tForward, float hDist, float vDist)
    {
        //Eye is offset from the target position
        Vector3 eye = tPos - tForward * hDist + tUp * vDist;

        //Camera forward is from eye to target
        Vector3 cameraForward = tPos - eye;
        cameraForward.Normalize();

        

    }

}
