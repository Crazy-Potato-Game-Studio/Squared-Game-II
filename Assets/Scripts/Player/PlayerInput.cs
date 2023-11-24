using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
public class PlayerInput : MonoBehaviour {

	[SerializeField] private float moveSpeed;
	[SerializeField] private float gravity;
	Vector3 velocity;

	PlayerController controller;

	void Start() {
		controller = GetComponent<PlayerController> ();
	}

	void Update() {

		if(controller.collisions.above || controller.collisions.below){
			velocity.y = 0;
		}

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		velocity.x = input.x * moveSpeed;
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}
}