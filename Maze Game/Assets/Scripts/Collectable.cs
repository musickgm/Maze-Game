using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType {Coin, Camera, Invisible, Objective};

    public CollectableType collectableType;
    public float rotationValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Rotation in x axis (the coin is a rotated cylinder)
        transform.Rotate(new Vector3(rotationValue, 0, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.CollectObject(collectableType);


            Destroy(gameObject);
        }
    }
}
