using UnityEngine;
using UnityEngine.InputSystem;

namespace TS
{
    public enum ControllerType
    {
        Xbox,
        Playstation,
        MouseAndKeyboard,
    }

    public class WorldDynamicControllerTypeDetector : MonoBehaviour
    {
        public static WorldDynamicControllerTypeDetector Instance { get; private set; }

        public ControllerType controllerType = ControllerType.MouseAndKeyboard;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added)
            {
                if (device is Gamepad)
                {
                    if (device.name.ToLower().Contains("dualshock") || device.name.ToLower().Contains("playstation"))
                    {
                        Debug.Log("PlayStation Controller connected");
                        controllerType = ControllerType.Playstation;
                    }
                    else
                    {
                        Debug.Log("Xbox Controller connected");
                        controllerType = ControllerType.Xbox;
                    }
                }
                else if (device is Keyboard || device is Mouse)
                {
                    Debug.Log("Mouse and Keyboard connected");
                    controllerType = ControllerType.MouseAndKeyboard;
                }
            }
        }
    }
}
