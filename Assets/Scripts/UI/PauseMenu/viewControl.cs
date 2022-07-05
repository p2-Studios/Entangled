using Game.CustomKeybinds;
using UnityEngine.UI;
using UnityEngine;

public class viewControl : MonoBehaviour
{
	// keybinds
	public Image jump;
	public Image walk_left;
	public Image walk_right;
	public Image push_pull;
	public Image reset;
	public Image interact;
	public Image clear;
	public Image entangle;
	public Image swap;
	public Image pause;

	// Sprites
	public Sprite key_short;
	public Sprite key_long;
	public Sprite mouse_r;
	public Sprite mouse_l;

	// Grab - text
	public Text grab_text;

	// For reference, simple check if menu is paused
	public PauseMenu pause_menu;

	// Controls view
	public GameObject control;

	// Start is called before the first frame update
	void Start()
    {
		UpdateKeys();
    }

    private void Update() {

        if (!pause_menu.paused && Input.GetKeyDown("tab")) {
			control.SetActive(true);
        }
		else if (Input.GetKeyUp("tab")) {
			control.SetActive(false);
		}

    }
    public void UpdateKeys() {

		jump.GetComponentInChildren<Text>().text = Keybinds.GetInstance().keys[0];
		updateSprite(Keybinds.GetInstance().jump, jump);

		walk_left.GetComponentInChildren<Text>().text = Keybinds.GetInstance().keys[1];
		updateSprite(Keybinds.GetInstance().moveLeft, walk_left);

		walk_right.GetComponentInChildren<Text>().text = Keybinds.GetInstance().keys[2];
		updateSprite(Keybinds.GetInstance().moveRight, walk_right);

		push_pull.GetComponentInChildren<Text>().text = Keybinds.GetInstance().keys[3];
		updateSprite(Keybinds.GetInstance().grabRelease, push_pull);

		reset.GetComponentInChildren<Text>().text = Keybinds.GetInstance().keys[4];
		updateSprite(Keybinds.GetInstance().reset, reset);

		interact.GetComponentInChildren<Text>().text = Keybinds.GetInstance().keys[5];
		updateSprite(Keybinds.GetInstance().interact, interact);

		clear.GetComponentInChildren<Text>().text = Keybinds.GetInstance().keys[6];
		updateSprite(Keybinds.GetInstance().clearAllEntangled, clear);

		entangle.GetComponentInChildren<Text>().text = Keybinds.GetInstance().keys[7];
		updateSprite(Keybinds.GetInstance().entangle, entangle);

		swap.GetComponentInChildren<Text>().text = Keybinds.GetInstance().keys[8];
		updateSprite(Keybinds.GetInstance().swapEntangle, swap);

		pause.GetComponentInChildren<Text>().text = Keybinds.GetInstance().keys[9];
		updateSprite(Keybinds.GetInstance().pause, pause);

		if (Keybinds.GetInstance().hold) {
			grab_text.text = "Grab \n - Hold";
        }
		else {
			grab_text.text = "Grab \n - Toggle";
		}
	}

	void updateSprite(KeyCode key, Image image) {
		switch (key) {
			case KeyCode.Mouse0:
			case KeyCode.Mouse1:
			case KeyCode.Return:
			case KeyCode.RightShift:
			case KeyCode.LeftShift:
			case KeyCode.Backspace:
			case KeyCode.Space:
				image.sprite = key_long;
				image.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 28);
				break;
			default:
				image.sprite = key_short;
				image.GetComponent<RectTransform>().sizeDelta = new Vector2(28, 28);
				break;
		}
	}
}
