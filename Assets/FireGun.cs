using UnityEngine;
using UnityEngine.UI;

public class FireGun : MonoBehaviour {
    LineRenderer gunLine;
    //ParticleSystem gunParticles;
    Light gunLight;
    AudioSource gunSound;
    //LineRenderer gunRender;

    Ray shotRay;
    RaycastHit shotHit;

	Canvas gameUI;
	Slider enemyHealth;
	Text enemyRatio;

    float gunRange = 100f;
    float effectDisplayTime = 0.2f;
    float timeBetweenShots = 0.1f;
    float timePassed;

	// Use this for initialization
	void Start () {
		initUI ();
		initGunComponents ();
	}
	
	// Update is called once per frame
	void Update () {
        timePassed += Time.deltaTime;
        //detect if the user hits the enter key, and the time between shots then fire
        if (Input.GetKeyDown(KeyCode.Z) && timePassed >= timeBetweenShots && Time.timeScale != 0) {
            Shoot();
        }

        //if time passes while effects are on, turn them off based on effectDisplaytime
        if(timePassed >= timeBetweenShots * effectDisplayTime) {
            DisableEffects();
        }
	}

	private void initUI(){
		//UI Initializations - Grab Canvas with find object, then grab children for efficiency
		gameUI = FindObjectOfType<Canvas>();

		//get all child sliders
		Component[] canvasSliders = gameUI.GetComponentsInChildren<Slider> ();
		//get all text sliders
		Component[] canvasTexts = gameUI.GetComponentsInChildren<Text> ();

		//loop through and find specific slider
		foreach (Slider child in canvasSliders) {
			if (child.tag.Equals("Enemy HP")) {
				enemyHealth = child;
			}
		}

		//loop through and find specific text
		foreach (Text child in canvasTexts) {
			if (child.tag.Equals("Enemy Ratio")) {
				enemyRatio = child;
			}
		}

		//create listener to run delegate function for updating text ratio of enemy
		enemyHealth.onValueChanged.AddListener (updateEnemyRatio);
	}

	private void initGunComponents(){
		//Gun Component Initializations
		gunLine = GetComponent<LineRenderer>();
		//gunParticles = GetComponent<ParticleSystem>();
		gunLight = GetComponent<Light>();
		gunSound = GetComponent<AudioSource>();
		//gunRender = GetComponent<LineRenderer>();
	}

    private void Shoot() {
        //reset timer
        timePassed = 0f;

        //play the gun shot sound
        gunSound.Play();

        //Stop particles if they were playing, then restart particles for shot
        //gunParticles.Stop();
        //gunParticles.Play();

        //enable light and renderer
        gunLight.enabled = true;
        gunLine.enabled = true;

        //set up ray so that it starts at the end of the barrel and points forward
        shotRay.origin = transform.position;
        shotRay.direction = transform.forward;

		gunLine.SetPosition(1, new Vector3(0, 0, gunRange));
        //handle raycast logic to draw line
        if (Physics.Raycast(shotRay, out shotHit)) {
			//do hit animation on boss here
			if(shotHit.transform.tag.Equals("BallBoss")){
				enemyHealth.value -= 500;
			}
        }
    }

	private void updateEnemyRatio(float value){
		enemyRatio.text = enemyHealth.value + "/" + enemyHealth.maxValue;
	}

    public void DisableEffects() {
        gunLight.enabled = false;
        gunLine.enabled = false;
    }
}
