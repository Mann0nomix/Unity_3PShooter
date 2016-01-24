using UnityEngine;
using System.Collections;

public class ChanMovement : MonoBehaviour {
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float speed = 5f;
    static private Animator anim;
    
    private float moveX;
    private float moveZ;
    static private bool startJump;
	static private bool midJump;
	static private bool endJump;
    static private bool dash;
    static private bool notJumping;
    static private bool walking;
    static private bool running;
    static private bool disableDash;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        moveX = Input.GetAxis("Horizontal");
        //move backwards should be half the speed
        //moveZ = Mathf.Clamp(Input.GetAxis("Vertical"), -0.5f, 1f);

		//normal speed backwards movement
		moveZ = Input.GetAxis("Vertical");

        //set the float values in the animator controller to the values detected by the input device the client is using
        anim.SetFloat("moveX", moveX);
        anim.SetFloat("moveZ", moveZ);

        //If you are holdin down the key = getkey
        startJump = Input.GetKeyDown("space");
        dash = Input.GetKeyDown("return");

        //only can dash for 2 seconds
        if (dash && !disableDash) {
            StartCoroutine(TimeDash());
        }
    }

    void FixedUpdate() {
        //calculate vector for movement
        Vector3 movementVec = new Vector3(moveX, 0, moveZ);
        
        //dash functionality else normal movement
        if (anim.GetBool("dash")) {
            rb.MovePosition(rb.position + Vector3.forward * (speed * 3) * Time.fixedDeltaTime);   //Dash = move forward 3x as fast
        } else {
            rb.MovePosition(rb.position + movementVec * speed * Time.deltaTime);
        }

        Jump();
    }

    void Jump() {
		if (startJump) {  //state for starting the jump
			endJump = false;
			midJump = true;
			anim.SetBool ("jump", true);
			rb.MovePosition (rb.position + Vector3.up * speed * 9.8f * Time.fixedDeltaTime);
			notJumping = false;
		} else {
			if (!notJumping) {  //state for not jumping
				if (rb.position.y >= 6) {
					midJump = false;
					endJump = true;
				}

				//if you have slightly left the ground, continue moving upward until you hit 8 points in the y direction, otherwise, come back down
				if (midJump) {  //state for middle of jump
					anim.SetBool ("jump", true);
					rb.MovePosition (rb.position + Vector3.up * speed * Time.fixedDeltaTime);
					midJump = true;
				} else {
					if (endJump) {  //state for ending of jump
						//Create a vector direction which consists of the difference between the target position vector and the current position vector
						if (rb.position.y > 0) {
							//Logic to lerp entire vector position of a rigidbody by direction
							//Vector3 direction = new Vector3 (moveX, 0, moveZ) - rb.position;
							//rb.MovePosition (rb.position + direction * speed * 2 * Time.fixedDeltaTime); //double speed to fake acceleration on jump

							//Logic to lerp y position of a rigid body only
							Vector3 posChange = rb.position;
							posChange.y = Mathf.MoveTowards (posChange.y, 0, speed * 2.5f * Time.fixedDeltaTime);
							rb.position = posChange;
							anim.SetBool ("jump", false);
						} 

						if(rb.position.y < .0005) {  //I had to ballpark this number because the math above will still push past 0. This logic stops it from going below 0
							endJump = false;
							notJumping = true;
						}
					}
				}
			}
		}
    }

    IEnumerator TimeDash() {
        anim.SetBool("dash", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("dash", false);
        disableDash = true;

        //wait another 2 seconds before being able to perform another dash
        yield return new WaitForSeconds(2f);
        disableDash = false;
    }
}
