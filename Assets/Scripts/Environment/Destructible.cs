using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
	public float required_impulse = 1.0f;
	private Collider2D thisCollider;
	
    // Start is called before the first frame update
    void Start()
    {
        thisCollider = this.GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
		
		// check initial collision for the impulse ...
		
		foreach (ContactPoint2D contact in collision.contacts) {
			
			if (contact.normalImpulse >= required_impulse) {
				thisCollider.enabled = false;
				return;
			}
		}
	}
	
	private void OnCollisionStay2D(Collision2D collision) {
		
		// check other incoming impulse of collision hitting panel
		
		ContactPoint2D[] cps = new ContactPoint2D[25];
		
		thisCollider.GetContacts(cps);
		
		foreach (ContactPoint2D contact in cps) {
			if (contact.normalImpulse >= required_impulse) {
				thisCollider.enabled = false;
				return;
			}
		}
	}
	
	public bool isDestroyed() {
		return thisCollider.enabled;
	}
	
}
