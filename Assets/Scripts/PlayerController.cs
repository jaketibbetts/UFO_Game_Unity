﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;

	private Rigidbody2D rb2d;
	private int count;
	private int goal;
	private int activeScene;
	private GameObject[] holes;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		count = 0;
		goal = GameObject.FindGameObjectsWithTag("PickUp").Length;
		holes = GameObject.FindGameObjectsWithTag("Blackhole");
		winText.text = "";
		setCountText();
		activeScene = SceneManager.GetActiveScene().buildIndex;
	}

	void FixedUpdate(){
    float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector2 scalar = new Vector2(0,0);

		for(int i = 0; i < holes.Length; i++) {
			scalar = scalar + calculateForceScalar(holes[i].transform);
		}

		Debug.Log(scalar.ToString());

		Vector2 movement = new Vector2(moveHorizontal - (float)((0.01) * scalar.x),moveVertical - (float)((0.01) * scalar.y));
		rb2d.AddForce(movement * speed);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("PickUp")){
			other.gameObject.SetActive(false);
			count = count + 1;
			setCountText();
		}
	}
	
	void setCountText(){
		countText.text = "Count: " + count.ToString() + " of " + goal.ToString();
		if (count >= goal) {
			winText.text = "You Win";
			//Any way to create some kind of delay?
			SceneManager.LoadScene((activeScene + 1) % 3);
			//Not the cleanest way to transition on win, but it's simple I guess
			//Is there a better way in code or the Editor to automatically transition between scenes?
		}
	}

	Vector2 calculateForceScalar(Transform other){
		return (new Vector2((float)((transform.position.x - other.position.x)), 
						(float)((transform.position.y - other.position.y))));
	}
	
}
