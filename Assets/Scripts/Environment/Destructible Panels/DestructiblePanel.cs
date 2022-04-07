using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePanel : MonoBehaviour
{
	public float required_impulse = 10.0f;
	private Collider2D panel_col;

	private Animator anim;

	List <GameObject> currentCollisions = new List <GameObject> ();

	public void Start() {
		anim = GetComponent<Animator>();
        panel_col = GetComponent<Collider2D>();
    }

	private void OnCollisionEnter2D(Collision2D collision) {
		
		// check initial collision for the impulse ...
		currentCollisions.Add (collision.gameObject);

		foreach (ContactPoint2D contact in collision.contacts) {

			if (contact.normalImpulse >= required_impulse) {
				panel_col.enabled = false;
				anim.SetBool("Destroyed", true);
				return;
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision) {
		float total = 0;
		foreach (GameObject gObject in currentCollisions) {
			total += collision.rigidbody.mass;
            print (total);
        }
		if(total >= required_impulse){
			panel_col.enabled = false;
			anim.SetBool("Destroyed", true);
			return;
		}

		// check other incoming impulse of collision hitting panel

		ContactPoint2D[] cps = new ContactPoint2D[25];

		panel_col.GetContacts(cps);
		
		foreach (ContactPoint2D contact in cps) {
			if (contact.normalImpulse >= required_impulse ) {
				panel_col.enabled = false;
				anim.SetBool("Destroyed", true);
				return;
			}
		}
	}

	public bool isDestroyed() {
        return !panel_col;
    } 

}