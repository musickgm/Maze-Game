using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player movement and rotation using player input
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float speed = 4;             //How fast does the player move
    public float sensitivity = 1;       //Sensitivity of rotation
    public float sprintFactor = 3f;     //Factor to increase speed by sprinting

    private bool gameOver = false;
    private LayerMask mask;             //The mask to account for walls


    /// <summary>
    /// Start is called before the first frame update - set the mask to walls
    /// </summary>
    void Start()
    {
        mask = LayerMask.GetMask("Wall");
    }

    /// <summary>
    /// Move and rotate the player every frame
    /// </summary>
    private void Update()
    {
        //Player input + other factors
        float xMovement = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float zMovement = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        float xRot = Input.GetAxis("Mouse X")  * sensitivity;

        //Handle sprinting
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            //If in front of a wall, stop sprinting
            if (!Physics.Raycast(transform.position, transform.forward, out _, 1.0f, mask))
            {
                zMovement *= sprintFactor;
            }
        }

        //Player can't move (but can rotate) if the game is over. Move the player
        if(!gameOver)
        {
            transform.Translate(xMovement, 0, zMovement);
        }
        //Rotate the player
        transform.Rotate(0, xRot, 0);
        //Prevent bad movements/rotations with clamping
        Clamp();
    }


    /// <summary>
    /// Clamp the player rotations and position to prevent wall climbing and unintentional x/z rotations
    /// </summary>
    private void Clamp()
    {
        float y = Mathf.Clamp(transform.position.y, 0, 2);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);

        float xRot = Mathf.Clamp(transform.eulerAngles.x, -1, 1);
        float zRot = Mathf.Clamp(transform.eulerAngles.z, -1, 1);
        transform.eulerAngles = new Vector3(xRot, transform.eulerAngles.y, zRot);
    }

    /// <summary>
    /// When the game is over, change the bool
    /// </summary>
    public void SetGameOver()
    {
        gameOver = true;
    }
}

