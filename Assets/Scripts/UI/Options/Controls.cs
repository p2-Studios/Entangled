using UnityEngine;
using UnityEngine.UI;
using Game.CustomKeybinds;

public class Controls : MonoBehaviour {
    // Screens
    public GameObject option_screen;
    public GameObject control_screen;

    public GameObject control1;
    public GameObject control2;

    // keybinds
    public Button jump;
    public Button walk_left;
    public Button walk_right;
    public Button push_pull;
    public Toggle grab_hold;
    public Button reset;
    public Button interact;
    public Button clear;
    public Button entangle;
    public Button swap;
    public Button pause;

    // confirmation
    public Button cancel;
    public Button apply;

    // prev
    public Button next;
    public Button prev;

    // button images
    public Sprite key_short;
    public Sprite key_long;

    // The button selected value
    public int i = -1;

    // prev toggle
    private bool prev_hold_val;

    // String representation of keys
    private string[] prev_keys = {
        "Space", "A", "D",
        "L-Shift", "R", "F", "Q", "Mouse 1", "Mouse 2", "Esc"
    };
    private string[] curr_keys = {
        "Space", "A", "D",
        "L-Shift", "R", "F", "Q", "Mouse 1", "Mouse 2", "Esc"
    };
    // keycodes
    private KeyCode[] prev_keycodes = {
        KeyCode.Space, KeyCode.A, KeyCode.D,
        KeyCode.LeftShift, KeyCode.R, KeyCode.F, KeyCode.Q, KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Escape
    };
    private KeyCode[] curr_keycodes = {
        KeyCode.Space, KeyCode.A, KeyCode.D,
        KeyCode.LeftShift, KeyCode.R, KeyCode.F, KeyCode.Q, KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Escape
    };

    // Awake is called before Start
    void Awake() {
        SaveLoadKeybinds.LoadControlScheme();

        for (int x = 0; x < prev_keycodes.Length; x++) {
            prev_keycodes[x] = KeyCodeInstance(x);
            curr_keycodes[x] = KeyCodeInstance(x);
            prev_keys[x] = Keybinds.GetInstance().keys[x];
            curr_keys[x] = Keybinds.GetInstance().keys[x];
            getSprite(curr_keycodes[x], x);
            getSelectedButton(x).GetComponentInChildren<Text>().text = curr_keys[x];
        }

        grab_hold.isOn = Keybinds.GetInstance().hold;
        prev_hold_val = grab_hold.isOn;

        jump.onClick.AddListener(btnJump);
        walk_left.onClick.AddListener(btnWalkLeft);
        walk_right.onClick.AddListener(btnWalkRight);
        push_pull.onClick.AddListener(btnPushPull);
        reset.onClick.AddListener(btnReset);
        interact.onClick.AddListener(btnInteract);
        clear.onClick.AddListener(btnClear);
        entangle.onClick.AddListener(btnEntangle);
        swap.onClick.AddListener(btnSwap);
        pause.onClick.AddListener(btnPause);

        // next/back page for screen
        next.onClick.AddListener(btnNext);
        prev.onClick.AddListener(btnPrev);

        // control
        cancel.onClick.AddListener(btnCancel);
        apply.onClick.AddListener(btnApply);
    }

    // Update is called once per frame
    void Update() {
        if (i != -1) {
            // defaults
            getSelectedButton(i).GetComponentInChildren<Text>().text = "???";
            getSelectedButton(i).GetComponent<Image>().sprite = key_short;
            getSelectedButton(i).GetComponent<RectTransform>().sizeDelta = new Vector2(50, 35);

            if (Input.GetMouseButtonDown(0)) {
                int val = findfrom(curr_keycodes, KeyCode.Mouse0);
                curr_keys[i] = "Mouse 1";
                curr_keycodes[i] = KeyCode.Mouse0;
                getSprite(curr_keycodes[i], i);
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetMouseButtonDown(1)) {
                int val = findfrom(curr_keycodes, KeyCode.Mouse1);
                curr_keys[i] = "Mouse 2";
                curr_keycodes[i] = KeyCode.Mouse1;
                getSprite(curr_keycodes[i], i);
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetMouseButtonDown(2)) {
                int val = findfrom(curr_keycodes, KeyCode.Mouse2);
                curr_keys[i] = "Mouse 3";
                curr_keycodes[i] = KeyCode.Mouse2;
                getSprite(curr_keycodes[i], i);
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.LeftControl)) {
                int val = findfrom(curr_keycodes, KeyCode.LeftControl);
                curr_keys[i] = "L-Ctrl";
                curr_keycodes[i] = KeyCode.LeftControl;
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.RightControl)) {
                int val = findfrom(curr_keycodes, KeyCode.RightControl);
                curr_keys[i] = "R-Ctrl";
                curr_keycodes[i] = KeyCode.RightControl;
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.LeftAlt)) {
                int val = findfrom(curr_keycodes, KeyCode.LeftAlt);
                curr_keys[i] = "L-Alt";
                curr_keycodes[i] = KeyCode.LeftAlt;
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.RightAlt)) {
                int val = findfrom(curr_keycodes, KeyCode.RightAlt);
                curr_keys[i] = "R-Alt";
                curr_keycodes[i] = KeyCode.RightAlt;
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.Escape)) {
                int val = findfrom(curr_keycodes, KeyCode.Escape);
                curr_keys[i] = "Esc";
                curr_keycodes[i] = KeyCode.Escape;
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                int val = findfrom(curr_keycodes, KeyCode.Return);
                curr_keys[i] = "Enter";
                curr_keycodes[i] = KeyCode.Return;
                getSprite(curr_keycodes[i], i);
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.LeftShift)) {
                int val = findfrom(curr_keycodes, KeyCode.LeftShift);
                curr_keys[i] = "L-Shift";
                curr_keycodes[i] = KeyCode.LeftShift;
                getSprite(curr_keycodes[i], i);
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.RightShift)) {
                int val = findfrom(curr_keycodes, KeyCode.RightShift);
                curr_keys[i] = "R-Shift";
                curr_keycodes[i] = KeyCode.RightShift;
                getSprite(curr_keycodes[i], i);
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.Backspace)) {
                int val = findfrom(curr_keycodes, KeyCode.Backspace);
                curr_keys[i] = "Bkspce";
                curr_keycodes[i] = KeyCode.Backspace;
                getSprite(curr_keycodes[i], i);
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                int val = findfrom(curr_keycodes, KeyCode.Space);
                curr_keys[i] = "Space";
                curr_keycodes[i] = KeyCode.Space;
                getSprite(curr_keycodes[i], i);
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            } else if (Input.GetKeyDown(KeyCode.Tab)) {
                return; // Reserved key for viewing controls
            } else if (Input.inputString.Length != 0) {
                if (Input.inputString[0] == '\n' || Input.inputString[0] == '\r' ||
                    Input.inputString[0] == '\b')
                    return;
                curr_keys[i] = Input.inputString[0].ToString().ToUpper();

                int val = findfrom(curr_keycodes, (KeyCode)System.Enum.Parse(typeof(KeyCode), curr_keys[i]));
                curr_keycodes[i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), curr_keys[i]);
                applyText(curr_keys);

                if (val == -1) {
                    for (int x = 0; x < prev_keycodes.Length; x++) {
                        getSelectedButton(x).interactable = true;
                    }
                } else {
                    getSelectedButton(i).interactable = false;
                    getSelectedButton(val).interactable = true;
                }
                i = val;
            }

            //change screen automatically if not on page of key
            if (i >= 0 && i < 6) {
                if (control2.activeSelf)
                    btnPrev();
            } else if (i >= 6) {
                if (control1.activeSelf)
                    btnNext();
            }

        } else {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) { // scroll up
                if (control2.active) {
                    control2.SetActive(false);
                    control1.SetActive(true);
                }    
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) { // scroll down
                if (control1.activeInHierarchy) {
                    control1.SetActive(false);
                    control2.SetActive(true);
                }     
            }

            if (Input.GetKeyDown(Keybinds.GetInstance().pause))
                btnCancel();
        }
    }

    void btnJump() {
        if (i != -1) {
            resetButton();
        }
        i = 0;
        for (int x = 0; x < prev_keycodes.Length; x++) {
            if (i != x)
                getSelectedButton(x).interactable = false;
        }

    }

    void btnWalkLeft() {
        if (i != -1) {
            resetButton();
        }
        i = 1;
        for (int x = 0; x < prev_keycodes.Length; x++) {
            if (i != x)
                getSelectedButton(x).interactable = false;
        }
    }

    void btnWalkRight() {
        if (i != -1) {
            resetButton();
        }
        i = 2;
        for (int x = 0; x < prev_keycodes.Length; x++) {
            if (i != x)
                getSelectedButton(x).interactable = false;
        }
    }

    void btnPushPull() {
        if (i != -1) {
            resetButton();
        }
        i = 3;
        for (int x = 0; x < prev_keycodes.Length; x++) {
            if (i != x)
                getSelectedButton(x).interactable = false;
        }
    }

    void btnReset() {
        if (i != -1) {
            resetButton();
        }
        i = 4;
        for (int x = 0; x < prev_keycodes.Length; x++) {
            if (i != x)
                getSelectedButton(x).interactable = false;
        }
    }

    void btnInteract() {
        if (i != -1) {
            resetButton();
        }
        i = 5;
        for (int x = 0; x < prev_keycodes.Length; x++) {
            if (i != x)
                getSelectedButton(x).interactable = false;
        }
    }

    void btnClear() {
        if (i != -1) {
            resetButton();
        }
        i = 6;
        for (int x = 0; x < prev_keycodes.Length; x++) {
            if (i != x)
                getSelectedButton(x).interactable = false;
        }
    }

    void btnEntangle() {
        if (i != -1) {
            resetButton();
        }
        i = 7;
        for (int x = 0; x < prev_keycodes.Length; x++) {
            if (i != x)
                getSelectedButton(x).interactable = false;
        }
    }

    void btnSwap() {
        if (i != -1) {
            resetButton();
        }
        i = 8;
        for (int x = 0; x < prev_keycodes.Length; x++) {
            if (i != x)
                getSelectedButton(x).interactable = false;
        }
    }

    void btnPause() {
        if (i != -1) {
            resetButton();
        }
        i = 9;
        for (int x = 0; x < prev_keycodes.Length; x++) {
            if (i != x)
                getSelectedButton(x).interactable = false;
        }
    }

    void applyText(string[] chosenKeys) {
        getSelectedButton(i).GetComponentInChildren<Text>().text = chosenKeys[i];
    }

    void getSprite(KeyCode key, int button) {
        switch (key) {
            case KeyCode.Mouse0:
            case KeyCode.Mouse1:
            case KeyCode.Mouse2:
            case KeyCode.Return:
            case KeyCode.RightShift:
            case KeyCode.LeftShift:
            case KeyCode.Backspace:
            case KeyCode.Space:
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
            case 0: return jump;
            case 1: return walk_left;
            case 2: return walk_right;
            case 3: return push_pull;
            case 4: return reset;
            case 5: return interact;
            case 6: return clear;
            case 7: return entangle;
            case 8: return swap;
            case 9: return pause;
            default: return null;
        }

    }

    int findfrom(KeyCode[] arr, KeyCode key) {
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

        grab_hold.isOn = prev_hold_val;

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

                Keybinds.GetInstance().keys[x] = curr_keys[x];
            }

            Keybinds.GetInstance().jump = curr_keycodes[0];
            Keybinds.GetInstance().moveLeft = curr_keycodes[1];
            Keybinds.GetInstance().moveRight = curr_keycodes[2];
            Keybinds.GetInstance().grabRelease = curr_keycodes[3];
            Keybinds.GetInstance().reset = curr_keycodes[4];
            Keybinds.GetInstance().interact = curr_keycodes[5];
            Keybinds.GetInstance().clearAllEntangled = curr_keycodes[6];
            Keybinds.GetInstance().entangle = curr_keycodes[7];
            Keybinds.GetInstance().swapEntangle = curr_keycodes[8];
            Keybinds.GetInstance().pause = curr_keycodes[9];

            Keybinds.GetInstance().hold = grab_hold.isOn;
            prev_hold_val = grab_hold.isOn;

            SaveLoadKeybinds.SaveControlScheme();

            viewControl controlView = FindObjectOfType<viewControl>();

            if (controlView != null)
                controlView.UpdateKeys();

            foreach (GrabControlIndicator grabindicator in Resources.FindObjectsOfTypeAll(typeof(GrabControlIndicator)) as GrabControlIndicator[])
                grabindicator.updatestring();

            foreach (InteractControlIndicator interactindicator in Resources.FindObjectsOfTypeAll(typeof(InteractControlIndicator)) as InteractControlIndicator[])
                interactindicator.updatestring();

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

    KeyCode KeyCodeInstance(int val) {
        switch (val) {
            case 0: return Keybinds.GetInstance().jump;
            case 1: return Keybinds.GetInstance().moveLeft;
            case 2: return Keybinds.GetInstance().moveRight;
            case 3: return Keybinds.GetInstance().grabRelease;
            case 4: return Keybinds.GetInstance().reset;
            case 5: return Keybinds.GetInstance().interact;
            case 6: return Keybinds.GetInstance().clearAllEntangled;
            case 7: return Keybinds.GetInstance().entangle;
            case 8: return Keybinds.GetInstance().swapEntangle;
            case 9: return Keybinds.GetInstance().pause;
            default: return KeyCode.None;
        }
    }

    // reset button if currently button is click
    void resetButton() {
        curr_keycodes[i] = prev_keycodes[i];
        curr_keys[i] = prev_keys[i];
        getSprite(curr_keycodes[i], i);
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

    public bool isHold() {
        return prev_hold_val;
    }
}
