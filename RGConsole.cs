using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/* This is a basic console for Unity applications.
 * 
 * PLEASE NOTE that Text Mesh Pro will need to be installed in your project.
 * 
 * Simply add as a component and set the input and output fields.
 */
namespace RGUnityTools
{
    public class RGConsole : MonoBehaviour
    {
        #region Singleton
        public static RGConsole instance;
        private void Awake()
        {
            instance = this;
        }
        #endregion

        private Queue<string> outputQueue;
        public int maxMessages = 10;
        public string commandSwitch;

        public TMP_InputField input;
        public TextMeshProUGUI output;
        public Button toggleConsole;
        public Button submitButton;
        public Button showLog;

        private void Start()
        {
            outputQueue = new Queue<string>(maxMessages);
            submitButton.onClick.AddListener(OnClickSubmit);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                OnClickSubmit();
        }

        //General:
        private void UpdateOutput()
        {
            if (outputQueue.Count > maxMessages)
                outputQueue.Dequeue();

            string outputText = "";

            foreach (string message in outputQueue)
            {
                outputText += message + "\n";
            }

            output.text = outputText;
        }

        //User input related:
        public void OnClickSubmit()
        {
            ParseInput();
            input.text = null;
            input.ActivateInputField();
        }

        private void ParseInput()
        {
            //Removes any unnecessary characters:
            string userInput = input.text;
            string trimmedInput = userInput.Trim();

            //Checks for the command switch:
            if (trimmedInput.StartsWith(commandSwitch))
            {//Command
                RunCommand(trimmedInput);
            }
            else if (trimmedInput.Length > 0)
            {//Message
                PostMessage(trimmedInput);
            }
        }

        private void RunCommand(string command)
        {
            //Input converted to lower case.
            command = command.Remove(0, 1);
            command = command.ToLower();

            switch (command)
            {
                case "help":
                    LogSuccess("Types help message");
                    break;
                default:
                    LogError(command + " is not a known command.");
                    break;
            }
        }

        private void PostMessage(string message)
        {
            outputQueue.Enqueue("<color=#00FFEE>You: </color><color=#FFFFFF>" + message + "</color>");
            UpdateOutput();
        }

        //Internal console related:
        public void Log(string message, Color color)
        {
            if (outputQueue == null)
                return;

            outputQueue.Enqueue(message);
            UpdateOutput();
        }

        public void LogSuccess(string message)
        {
            outputQueue.Enqueue("<color=#00FF00>" + message + "</color>");
            UpdateOutput();
        }

        public void LogWarning(string message)
        {
            outputQueue.Enqueue("<color=#FFFF00>" + message + "</color>");
            UpdateOutput();
        }

        public void LogError(string message)
        {
            outputQueue.Enqueue("<color=#FF0000>" + message + "</color>");
            UpdateOutput();
        }
    }
}
