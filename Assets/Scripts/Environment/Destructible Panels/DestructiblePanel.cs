using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePanel : MonoBehaviour
{
	public float required_impulse = 10.0f;
	public float required_mass = 10.0f;
	private Collider2D panel_col;

	private Animator anim;
	bool check = true;

	List <GameObject> currentCollisions = new List <GameObject> ();
	float totalMass = 0;
	public void Start() {
		check = true;
		StartCoroutine(LoadCheck());
		anim = GetComponent<Animator>();
        panel_col = GetComponent<Collider2D>();
    }

	IEnumerator LoadCheck()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1.5f);
		check = false;
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
	
	private void OnCollisionEnter2D(Collision2D collision) {

		if(!check){
			//check collision for mass
			if(collision.gameObject.tag == "Pushable" || collision.gameObject.tag == "Player"){
				currentCollisions.Add(collision.gameObject);
				totalMass += collision.gameObject.GetComponent<Rigidbody2D>().mass;
			}
			// check collision for the impulse
			foreach (ContactPoint2D contact in collision.contacts) {
				if (contact.normalImpulse >= required_impulse) {
					panel_col.enabled = false;
					anim.SetBool("Destroyed", true);
					AudioManager.instance.Play("wall_break");
					return;
				}
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision) {
		if(!check){
			//if total mass is currently greater than break force for mass 
			if(totalMass >= required_mass){
				panel_col.enabled = false;
				anim.SetBool("Destroyed", true);
				AudioManager.instance.Play("wall_break");
				return;
			}

			// check other incoming impulse of collision hitting panel
			ContactPoint2D[] cps = new ContactPoint2D[25];

			panel_col.GetContacts(cps);
			foreach (ContactPoint2D contact in cps) {
				if (contact.normalImpulse >= required_impulse ) {
					panel_col.enabled = false;
					anim.SetBool("Destroyed", true);
					AudioManager.instance.Play("wall_break");
					return;
				}
			}
		}
		
	}

	private void OnCollisionExit2D(Collision2D collision){
		//if object leaves then remove it from mass total
		if(currentCollisions.Contains(collision.gameObject)){
			totalMass -= collision.gameObject.GetComponent<Rigidbody2D>().mass;
			currentCollisions.Remove(collision.gameObject);
		}
	}

	public bool isDestroyed() {
        return !panel_col;
    } 

}