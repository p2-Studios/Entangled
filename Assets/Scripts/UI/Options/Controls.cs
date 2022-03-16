using System;
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
	public Button clear;
	public Button entangle;
	public Button unentangle;
	
	// confirmation
	public Button cancel;
	public Button apply;
	
	// prev
	public Button next;
	public Button prev;

	// button images
	public Sprite key_short;
	public Sprite key_long;
	public Sprite mouse_r;
	public Sprite mouse_l;

	// The button selected value
	public int i = -1;

	// String representation of keys
	private string[] prev_keys = { "W", "S", "A", "D",
									"E", "R", "F", "Q", "", ""};
	private string[] curr_keys = { "W", "S", "A", "D",
									"E", "R", "F", "Q", "", ""};
	// keycodes
	private KeyCode[] prev_keycodes = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D,
									KeyCode.E, KeyCode.R, KeyCode.F, KeyCode.Q, KeyCode.Mouse0, KeyCode.Mouse1};
	private KeyCode[] curr_keycodes = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D,
									KeyCode.E, KeyCode.R, KeyCode.F, KeyCode.Q, KeyCode.Mouse0, KeyCode.Mouse1};

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
		clear.onClick.AddListener(btnClear);
		entangle.onClick.AddListener(btnEntangle);
		unentangle.onClick.AddListener(btnUnEntangle);
		
		// next/back page for screen
		next.onClick.AddListener(btnNext);
		prev.onClick.AddListener(btnPrev);
		
		// control
		cancel.onClick.AddListener(btnCancel);
		apply.onClick.AddListener(btnApply);
	}
	
    // Update is called once per frame
    void Update()
    {
		if (i != -1) {
			
			// defaults
			getSelectedButton(i).GetComponentInChildren<Text>().text = "???";
			getSelectedButton(i).GetComponent<Image>().sprite = key_short;
			getSelectedButton(i).GetComponent<RectTransform>().sizeDelta = new Vector2(50, 35);
			
			if (Input.GetMouseButtonDown(0)) {
				int val = findfrom(curr_keycodes, KeyCode.Mouse0);
				curr_keys[i] = "";
				curr_keycodes[i] = KeyCode.Mouse0;
				getSelectedButton(i).GetComponent<Image>().sprite = mouse_l;
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x < prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}
			else if (Input.GetMouseButtonDown(1)) {
				int val = findfrom(curr_keycodes, KeyCode.Mouse1);
				curr_keys[i] = "";
				curr_keycodes[i] = KeyCode.Mouse1;
				getSelectedButton(i).GetComponent<Image>().sprite = mouse_r;
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x < prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}
			else if (Input.GetKeyDown(KeyCode.LeftControl)) {
				int val = findfrom(curr_keycodes, KeyCode.LeftControl);
				curr_keys[i] = "L-ctrl";
				curr_keycodes[i] = KeyCode.LeftControl;
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x < prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}
			else if (Input.GetKeyDown(KeyCode.RightControl)) {
				int val = findfrom(curr_keycodes, KeyCode.RightControl);
				curr_keys[i] = "R-ctrl";
				curr_keycodes[i] = KeyCode.RightControl;
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x < prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}
			else if (Input.GetKeyDown(KeyCode.LeftAlt)) {
				int val = findfrom(curr_keycodes, KeyCode.LeftAlt);
				curr_keys[i] = "L-alt";
				curr_keycodes[i] = KeyCode.LeftAlt;
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x < prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}
			else if (Input.GetKeyDown(KeyCode.RightAlt)) {
				int val = findfrom(curr_keycodes, KeyCode.RightAlt);
				curr_keys[i] = "R-alt";
				curr_keycodes[i] = KeyCode.RightAlt;
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x < prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}
			else if (Input.GetKeyDown(KeyCode.Return)) {
				int val = findfrom(curr_keycodes, KeyCode.Return);
				curr_keys[i] = "Enter";
				curr_keycodes[i] = KeyCode.Return;
				getSprite(curr_keycodes[i], i);
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x < prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}
			else if (Input.GetKeyDown(KeyCode.LeftShift)) {
				int val = findfrom(curr_keycodes, KeyCode.LeftShift);
				curr_keys[i] = "L-Shift";
				curr_keycodes[i] = KeyCode.LeftShift;
				getSprite(curr_keycodes[i], i);
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x < prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}
			else if (Input.GetKeyDown(KeyCode.RightShift)) {
				int val = findfrom(curr_keycodes, KeyCode.RightShift);
				curr_keys[i] = "R-Shift";
				curr_keycodes[i] = KeyCode.RightShift;
				getSprite(curr_keycodes[i], i);
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x <prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}
			else if (Input.GetKeyDown(KeyCode.Backspace)) {
				int val = findfrom(curr_keycodes, KeyCode.Backspace);
				curr_keys[i] = "Backspace";
				curr_keycodes[i] = KeyCode.Backspace;
				getSprite(curr_keycodes[i], i);
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x < prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}
			else if (Input.inputString.Length != 0) {
				curr_keys[i] = Input.inputString[0].ToString().ToUpper();
				int val = findfrom(curr_keycodes, (KeyCode)System.Enum.Parse(typeof(KeyCode), curr_keys[i]));
				curr_keycodes[i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), curr_keys[i]);
				applyText(curr_keys);
				
				if (val == -1) {
					for (int x = 0; x < prev_keycodes.Length; x++) {
						getSelectedButton(x).interactable = true;
					}
				}
				else {
					getSelectedButton(i).interactable = false;
					getSelectedButton(val).interactable = true;
				}
				i = val;
			}

			//change screen automatically if not on page of key
			if (i >= 0 && i < 7) {
				if (control2.activeSelf)
					btnPrev();
			}
			else if (i >= 7) {
				if (control1.activeSelf)
					btnNext();
			}

		}
    }
	
	void btnWalkUp() {
		if (i != -1) {
			resetButton();
		}
		i = 0;
		for (int x = 0; x < prev_keycodes.Length; x++) {
			if (i != x)
				getSelectedButton(x).interactable = false;
		}

	}
	
	void btnWalkDown() {
		if (i != -1) {
			resetButton();
		}
		i = 1;
		for (int x = 0; x < prev_keycodes.Length; x++) {
			if (i != x)
				getSelectedButton(x).interactable = false;
		}
	}
	
	void btnWalkLeft() {
		if (i != -1) {
			resetButton();
		}
		i = 2;
		for (int x = 0; x < prev_keycodes.Length; x++) {
			if (i != x)
				getSelectedButton(x).interactable = false;
		}
	}
	
	void btnWalkRight() {
		if (i != -1) {
			resetButton();
		}
		i = 3;
		for (int x = 0; x < prev_keycodes.Length; x++) {
			if (i != x)
				getSelectedButton(x).interactable = false;
		}
	}
	
	void btnPushPull() {
		if (i != -1) {
			resetButton();
		}
		i = 4;
		for (int x = 0; x < prev_keycodes.Length; x++) {
			if (i != x)
				getSelectedButton(x).interactable = false;
		}
	}
	
	void btnReset() {
		if (i != -1) {
			resetButton();
		}
		i = 5;
		for (int x = 0; x < prev_keycodes.Length; x++) {
			if (i != x)
				getSelectedButton(x).interactable = false;
		}
	}
	
	void btnInteract() {
		if (i != -1) {
			resetButton();
		}
		i = 6;
		for (int x = 0; x < prev_keycodes.Length; x++) {
			if (i != x)
				getSelectedButton(x).interactable = false;
		}
	}
	
	void btnClear() {
		if (i != -1) {
			resetButton();
		}
		i = 7;
		for (int x = 0; x < prev_keycodes.Length; x++) {
			if (i != x)
				getSelectedButton(x).interactable = false;
		}
	}
	
	void btnEntangle() {
		if (i != -1) {
			resetButton();
		}
		i = 8;
		for (int x = 0; x < prev_keycodes.Length; x++) {
			if (i != x)
				getSelectedButton(x).interactable = false;
		}
	}
	
	void btnUnEntangle() {
		if (i != -1) {
			resetButton();
		}
		i = 9;
		for (int x = 0; x < prev_keycodes.Length; x++) {
			if (i != x)
				getSelectedButton(x).interactable = false;
		}
	}
	
	void applyText(string[] chosen_keys) {
	
		getSelectedButton(i).GetComponentInChildren<Text>().text = chosen_keys[i];

	}
	
	void getSprite(KeyCode key, int button) {
		switch (key) {
			case KeyCode.Mouse0: 
				getSelectedButton(button).GetComponent<Image>().sprite = mouse_l;
				getSelectedButton(button).GetComponent<RectTransform>().sizeDelta = new Vector2(50,35);
				break;
			case KeyCode.Mouse1:
				getSelectedButton(button).GetComponent<Image>().sprite = mouse_r;
				getSelectedButton(button).GetComponent<RectTransform>().sizeDelta = new Vector2(50, 35);
				break;
			case KeyCode.Return:
				getSelectedButton(button).GetComponent<Image>().sprite = key_long;
				getSelectedButton(button).GetComponent<RectTransform>().sizeDelta = new Vector2(50, 35);
				break;
			case KeyCode.RightShift:
				getSelectedButton(button).GetComponent<Image>().sprite = key_long;
				getSelectedButton(button).GetComponent<RectTransform>().sizeDelta = new Vector2(75, 35);
				break;
			case KeyCode.LeftShift:
				getSelectedButton(button).GetComponent<Image>().sprite = key_long;
				getSelectedButton(button).GetComponent<RectTransform>().sizeDelta = new Vector2(75, 35);
				break;
			case KeyCode.Backspace:
				getSelectedButton(button).GetComponent<Image>().sprite = key_long;
				getSelectedButton(button).GetComponent<RectTransform>().sizeDelta = new Vector2(75, 35);
				break;
			default:
				getSelectedButton(button).GetComponent<Image>().sprite = key_short;
				getSelectedButton(button).GetComponent<RectTransform>().sizeDelta = new Vector2(50, 35);
				break;
		}
    }

	Button getSelectedButton(int number) {
		
		switch (number) {
			case 0 : return walk_up;
			case 1 : return walk_down;
			case 2 : return walk_left;
			case 3 : return walk_right;
			case 4 : return push_pull;
			case 5 : return reset;
			case 6 : return interact;
			case 7 : return clear;
			case 8 : return entangle;
			case 9 : return unentangle;
			default: return null;
		}
		
	}

	int findfrom(KeyCode[] arr,KeyCode key) {
		for (int x = 0; x < arr.Length; x++) {
			if (x != i && ((int)key) == ((int)arr[x]))
				return x;
        }
		return -1;
    }
	
	// button cancel.onclick function
	void btnCancel() {

		for (int x = 0; x < prev_keycodes.Length; x++) {
			curr_keycodes[x] = prev_keycodes[x];
			curr_keys[x] = prev_keys[x];
			getSelectedButton(x).GetComponentInChildren<Text>().text = prev_keys[x];
			getSprite(prev_keycodes[x], x);
		}
		
		// Show option_screen
		option_screen.SetActive(true);
		
		// Hide control_screen
		control_screen.SetActive(false);

	}

	void btnApply() {
		if (i == -1) {

			for (int x = 0; x < curr_keycodes.Length; x++) {
				prev_keycodes[x] = curr_keycodes[x];
				prev_keys[x] = curr_keys[x];
				getSprite(curr_keycodes[x], x);
			}

			// Show option_screen
			option_screen.SetActive(true);

			// Hide control_screen
			control_screen.SetActive(false);

		}
	}
	
	void btnNext() {
		showList2();
	}
	
	void btnPrev() {
		showList1();
	}
	
	// reset button if currently button is click
	void resetButton() {
		curr_keycodes[i] = prev_keycodes[i];
		curr_keys[i] = prev_keys[i];
		getSprite(curr_keycodes[i],i);
		applyText(curr_keys);
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
