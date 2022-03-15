using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
	// Screens
	public GameObject option_screen;
	public GameObject control_screen;
	
	public GameObject control1;
	public GameObject control2;
	
	// keybinds
	public Button walk_up;
	public Button walk_down;
	public Button walk_left;
	public Button walk_right;
	public Button push_pull;
	public Button reset;
	public Button interact;
	public Button entangle;
	
	// confirmation
	public Button cancel;
	public Button apply;
	
	// prev
	public Button next;
	public Button prev;
	
	private int button_assign = -1;
	private string[] prev_keys = { "W", "S", "A", "D",
									"E", "R", "F", ""};
	private string[] curr_keys = { "W", "S", "A", "D",
									"E", "R", "F", "" };								
	
	// Start is called before the first frame update
    void Start()
    {
		// -- ADD CODE TO FETCH KEYBINDS --
		walk_up.onClick.AddListener(btnWalkUp);
		walk_down.onClick.AddListener(btnWalkDown);
		walk_left.onClick.AddListener(btnWalkLeft);
		walk_right.onClick.AddListener(btnWalkRight);
		push_pull.onClick.AddListener(btnPushPull);
		reset.onClick.AddListener(btnReset);
		interact.onClick.AddListener(btnInteract);
		entangle.onClick.AddListener(btnEntangle);
		
		// next/back page for screen
		next.onClick.AddListener(btnNext);
		prev.onClick.AddListener(btnPrev);
		
		// control
		cancel.onClick.AddListener(btnCancel);
		
	}
	
    // Update is called once per frame
    void OnGUI()
    {
		if (button_assign != -1) {
			Event e = Event.current;
			if (e.isKey) {
				curr_keys[button_assign] = e.character.ToString();
				applyText(false);
				button_assign = -1;
			}
		}
    }
	
	void btnWalkUp() {
		if (button_assign != -1)
			applyText(false);
		button_assign = 0;
	}
	
	void btnWalkDown() {
		if (button_assign != -1)
			applyText(false);
		button_assign = 1;
	}
	
	void btnWalkLeft() {
		if (button_assign != -1)
			applyText(false);
		button_assign = 2;
	}
	
	void btnWalkRight() {
		if (button_assign != -1)
			applyText(false);
		button_assign = 3;
	}
	
	void btnPushPull() {
		if (button_assign != -1)
			applyText(false);
		button_assign = 4;
	}
	
	void btnReset() {
		if (button_assign != -1)
			applyText(false);
		button_assign = 5;
	}
	
	void btnInteract() {
		if (button_assign != -1)
			applyText(false);
		button_assign = 6;
	}
	
	void btnEntangle() {
		if (button_assign != -1)
			applyText(false);
		button_assign = 7;
	}
	
	void applyText(bool revert) {
		
		string[] chosen_keys = curr_keys;
		
		if (revert) {
			chosen_keys = prev_keys;
		}
	
		getSelectedButton().GetComponentInChildren<Text>().text = chosen_keys[button_assign];

	}
	
	Button getSelectedButton() {
		
		switch (button_assign) {
			case 0 : return walk_up;
			case 1 : return walk_down;
			case 2 : return walk_left;
			case 3 : return walk_right;
			case 4 : return push_pull;
			case 5 : return reset;
			case 6 : return interact;
			default: return null;
		}
		
	}
	
	// button cancel.onclick function
	void btnCancel() {
		// Show option_screen
		option_screen.SetActive(true);
		
		// Hide control_screen
		control_screen.SetActive(false);
	}
	
	void btnNext() {
		showList2();
	}
	
	void btnPrev() {
		showList1();
	}
	
	void showList1() {
		control1.SetActive(true);
		control2.SetActive(false);
	}
	
	void showList2() {
		control1.SetActive(false);
		control2.SetActive(true);
	}
}
