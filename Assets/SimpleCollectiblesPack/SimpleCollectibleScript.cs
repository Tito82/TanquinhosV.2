using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	public enum CollectibleTypes {NoType, Type1, Type2, Type3, Type4, Type5}; // para crear mas power ups

	public CollectibleTypes CollectibleType; // tipo de gameobject

	public bool rotate; 

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (rotate)
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			Collect ();
		}
	}

	public void Collect()
	{
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if(collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);

		if (CollectibleType == CollectibleTypes.Type1) {
			
			TankHealth tnk = FindObjectOfType(typeof(TankHealth)) as TankHealth;
			tnk.HealthPowerUp();
	
			Debug.Log ("recogiendo el pickup");
		}
		
		Destroy (gameObject);
	}
}
