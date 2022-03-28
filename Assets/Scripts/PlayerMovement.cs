using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rigidbody;

    [SerializeField] GameObject player;
    [SerializeField] float speed = 1;
    [SerializeField] GameObject feet;
    float stepHeight = 0.3f;
    float stepsmooth = 0.1f;
    [SerializeField]Animator playerAnim;
    public bool isGameStop;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    { 
        StepClimb();
    }
    public void StepClimb()
    {
        if (!isGameStop)
        {
            RaycastHit hit;
            if (!Physics.Raycast(feet.transform.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f))
            {
                player.transform.position += player.transform.forward * speed;
                rigidbody.position += new Vector3(0, stepsmooth, 0);
            }
            else
            {
                player.transform.position += player.transform.forward * 0;
               
            }
        }

    }
    

}
