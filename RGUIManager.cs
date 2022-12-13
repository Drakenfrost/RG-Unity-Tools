using System;
using UnityEngine;
using UnityEngine.UI;

namespace RGUnityTools
{
    public class RGUIManager : MonoBehaviour
    {
        #region SINGLETON
        public static RGUIManager instance;

        private void Awake()
        {
            instance = this;
        }
        #endregion

        public Action<UIState> OnStateChanged;

        [SerializeField] int startingStateIndex;
        public UIState currentState { get; private set; }
        public UIState[] states;

        #region UNITY_METHODS
        void Start()
        {
            foreach (UIState s in states)
            {
                foreach (Button b in s.buttons)
                {
                    if (b != null) b.onClick.AddListener(s.Activate);
                }

                s.OnActivate += SetState;
            }

            states[startingStateIndex].Activate();
        }

        void Update()
        {
            foreach (UIState s in states)
            {
                if (Input.GetKeyDown(s.hotkey))
                {
                    if (s == currentState) continue;
                    s.Activate();
                }
            }
        }
        #endregion

        #region CORE_FUNCTIONS
        public void SetState(UIState newState)
        {
            currentState = newState;
            OnStateChanged?.Invoke(currentState);
            Debug.Log($"Set UI state to '{currentState.name}'");
        }

        public void SetState(string stateName)
        {
            SetState(GetState(stateName));
        }

        public UIState GetState(string stateName)
        {
            foreach (UIState s in states)
            {
                if (s.name == stateName) return s;
            }

            return null;
        }

        public void HideUI()
        {
            foreach (UIState g in states)
            {
                if (g == null) continue;
                g.Deactivate();
            }
        }
        #endregion
    }

    [Serializable]
    public class UIState
    {
        public Action<UIState> OnActivate;

        public string name = "New State";
        [Tooltip("Button that will trigger this state when clicked.")]
        public Button[] buttons;
        public KeyCode hotkey;
        public bool showCursor = true;
        [Min(0f)]
        public float timeScale = 1f;

        [Tooltip("The states that are allowed to be triggered by this state.")]
        [SerializeField] string[] transitions;
        [Tooltip("The objects to be enabled when this state is activated.")]
        [SerializeField] GameObject[] ShowUIObjects;
        [Tooltip("The objects to be disabled when this state is activated.")]
        [SerializeField] GameObject[] HideUIObjects;

        public void Activate()
        {
            Time.timeScale = timeScale;

            Cursor.visible = showCursor;
            Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;

            foreach (GameObject g in ShowUIObjects)
            {
                if (g == null) continue;
                if (!g.activeSelf)
                    g.SetActive(true);
            }

            foreach (GameObject g in HideUIObjects)
            {
                if (g == null) continue;
                if (g.activeSelf)
                    g.SetActive(false);
            }

            OnActivate?.Invoke(this);
        }

        public void Deactivate()
        {
            foreach (GameObject g in ShowUIObjects)
            {
                if (g == null) continue;
                if (g.activeSelf)
                    g.SetActive(false);
            }
        }
    }
}