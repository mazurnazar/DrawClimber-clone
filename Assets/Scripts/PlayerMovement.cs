using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rigidbody;
   
    public GameObject player;
    public float speed = 1;
    public GameObject feet;
    public float stepHeight = 0.3f;
    public float stepsmooth = 0.1f;
    [SerializeField]Animator playerAnim;
    public bool isGameStop;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerAnim.SetInteger("walk", 1);
        

        //feet.transform.position = new Vector3(feet.transform.position.x, stepHeight, feet.transform.position.z);

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
            if (!Physics.Raycast(feet.transform.position, transform.TransformDirection(Vector3.forward), out hit, 0.1f))
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
