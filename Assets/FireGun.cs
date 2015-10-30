using UnityEngine;

public class FireGun : MonoBehaviour {
    LineRenderer gunLine;
    ParticleSystem gunParticles;
    Light gunLight;
    AudioSource gunSound;
    LineRenderer gunRender;

    Ray shotRay;
    RaycastHit shotHit;

    float gunRange = 100f;
    float effectDisplayTime = 0.2f;
    float timeBetweenShots = 0.1f;
    float timePassed;

	// Use this for initialization
	void Start () {
        //get all components that resemble firing of gun
        gunLine = GetComponent<LineRenderer>();
        gunParticles = GetComponent<ParticleSystem>();
        gunLight = GetComponent<Light>();
        gunSound = GetComponent<AudioSource>();
        gunRender = GetComponent<LineRenderer>();
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

    void Shoot() {
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

        //handle raycast logic to draw line
        if (Physics.Raycast(shotRay, out shotHit)) {
            gunLine.SetPosition(1, shotHit.point);
        } else {
            gunLine.SetPosition(1, new Vector3(0, 0, 100));
        }
    }

    void DisableEffects() {
        gunLight.enabled = false;
        gunLine.enabled = false;
    }
}
