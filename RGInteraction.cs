using System;
using UnityEngine;
using TMPro;

namespace RGUnityTools
{
    //TODO add prompt fade in and out.

    [RequireComponent(typeof(Collider))]
    public class RGInteraction : MonoBehaviour
    {
        public Action OnInteract;

        [SerializeField] KeyCode interactKey = KeyCode.Return;
        [SerializeField] KeyCode enableInteractKey = KeyCode.Backspace;
        [SerializeField] bool disableOnInteract;
        [SerializeField] string enabledMessage;
        [SerializeField] string disabledMessage;
        [SerializeField] GameObject prompt;
        [SerializeField] TextMeshProUGUI promptText;

        bool disabled;

        void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        void Update()
        {
            if (Input.GetKeyDown(interactKey))
            {
                Interact();
            }
            
            if (Input.GetKeyDown(enableInteractKey))
            {
                disabled = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
                ShowPrompt();
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 6)
                HidePrompt();
        }

        void ShowPrompt()
        {
            if (prompt == null || promptText == null) return;

            promptText.text = disabled ? disabledMessage : enabledMessage;

            prompt.SetActive(true);
        }

        void HidePrompt()
        {
            if (prompt == null || promptText == null) return;

            prompt.SetActive(false);
        }

        void Interact()
        {
            if (disabled) return;
            disabled = disableOnInteract;
            HidePrompt();
            OnInteract?.Invoke();
        }
    }
}
