using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	private NavMeshAgent bossAgent;
	[SerializeField]
	private Rigidbody playerBody;
	private bool onLeft;
	private Transform startTransform;

	// Use this for initialization
	void Start () {
		bossAgent = GetComponent<NavMeshAgent> ();
		playerBody = GameObject.FindGameObjectWithTag("Mychan").GetComponent<Rigidbody> ();
		onLeft = true;
		StartCoroutine (bossMove());
	}

	// Update is called once per frame
	void FixedUpdate () {
		//bossAgent.SetDestination (playerBody.position);
		//Vector3.Lerp(this.transform.position, playerBody.position, 1)

		//important note - movement must be processed under update, so use boolean in coroutine to handle switching of movement flow
		if (!onLeft) {
			//move right
			//transform.position = new Vector3(20 * Time.deltaTime, 3, transform.position.z);
			transform.position = Vector3.Lerp(transform.position, new Vector3(30, 6, transform.position.z), Time.deltaTime);
		} else {
			//move left
			//transform.position = new Vector3(-20 * Time.deltaTime, 3, transform.position.z);
			transform.position = Vector3.Lerp(transform.position, new Vector3(-30, 6, transform.position.z), Time.deltaTime);
		}
	}

	//Perform boss jump every 3 seconds
	IEnumerator bossMove(){
		while (true) {
			bossJump ();
			yield return new WaitForSeconds (1);
		}
	}

	void bossJump(){
		if (!onLeft) {
			onLeft = true;
		} else {
			onLeft = false;
		}
	}
}
