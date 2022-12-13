using UnityEngine;

namespace RGUnityTools.GameSettings
{
    //TODO attach save file system to here.
    [CreateAssetMenu(fileName = "New Game Settings", menuName = "Game Settings")]
    public class RGGameSettings : ScriptableObject
    {
        [Header("Video:")]
        public string windowMode = "fullscreen";

        [Header("Audio:")]
        public float masterVolume = .5f;
        public float musicVolume = .5f;
        public float effectsVolume = .5f;

        [Header("Controls:")]
        [Min(0.01f)]
        public float mouseSensitivity = .5f;
        [Min(0.01f)]
        public float gamepadSensitivity = .5f;
        [Min(0.01f)]
        public float xSensitivity = .5f;
        [Min(0.01f)]
        public float ySensitivity = .5f;

        [Header("Control Bindings:")]
        public string lookBinding = "<Mouse>/delta";
        public string lookBindingGamepad = "<Gamepad>/rightStick";

        public string useRightHandBinding = "<Mouse>/leftClick";
        public string useRightHandBindingGamepad = "<Gamepad>/rightTrigger";

        public string useLeftHandBinding = "<Mouse>/rightClick";
        public string useLeftHandBindingGamepad = "<Gamepad>/leftTrigger";

        public string walkForwardBinding = "<Keyboard>/w";
        public string walkForwardBindingGamepad = "<Gamepad>/leftStick/up";

        public string walkBackwardBinding = "<Keyboard>/s";
        public string walkBackwardBindingGamepad = "<Gamepad>/leftStick/down";

        public string strafeLeftBinding = "<Keyboard>/a";
        public string strafeLeftBindingGamepad = "<Gamepad>/leftStick/left";

        public string strafeRightBinding = "<Keyboard>/d";
        public string strafeRightBindingGamepad = "<Gamepad>/leftStick/right";

        public string runBinding = "<Keyboard>/leftShift";
        public string runBindingGamepad = "<Gamepad>/leftStickPress";

        public string sneakBinding = "<Keyboard>/c";
        public string sneakBindingGamepad = "<Gamepad>/buttonEast";

        public string jumpBinding = "<Keyboard>/space";
        public string jumpBindingGamepad = "<Gamepad>/buttonSouth";

        public string interactBinding = "<Keyboard>/e";
        public string interactBindingGamepad = "<Gamepad>/buttonWest";

        public string toggleInventoryBinding = "<Keyboard>/tab";
        public string toggleInventoryBindingGamepad = "<Gamepad>/buttonNorth";

        public string toggleMenuBinding = "<Keyboard>/escape";
        public string toggleMenuBindingGamepad = "<Gamepad>/start";
    }
}
