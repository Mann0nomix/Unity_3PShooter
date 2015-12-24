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
    static private bool sheJumps;
    static private bool dash;
    static private bool goDown;
    static private bool landed;
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
        sheJumps = Input.GetKeyDown("space");
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
        //need to do this because the other way does not work with the booleans in C#
        if (sheJumps) {
            rb.MovePosition(rb.position + Vector3.up * speed * Time.fixedDeltaTime);
            anim.SetBool("jump", true);
        }

        //if you have slightly left the ground, continue moving upward until you hit 8 points in the y direction, otherwise, come back down
        if (!landed && rb.position.y > .01) {
            rb.MovePosition(rb.position + Vector3.up * speed * Time.fixedDeltaTime);
            if (rb.position.y > 8) {
                goDown = true;
            }
            if (goDown) {
                rb.MovePosition(rb.position + Vector3.down * Time.fixedDeltaTime / 4);
                goDown = false;

                if (rb.position.y < 1) {
                    landed = true;
                }
            }
        } else {
            anim.SetBool("jump", false);
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
