using UnityEngine;
using UnityEngine.UI;

namespace Nova.Library.Utilities
{
    public class UIFunctions
    {
        public enum messagePriority
        {
            noMessage,
            highPriority,
            mediumPriority,
            lowPriority
        }

        private messagePriority currentMessagePriority = messagePriority.noMessage;
        private static readonly int ShowMessage = Animator.StringToHash("ShowMessage");

        public static void ShowInteractMessage(string message, messagePriority priority)
        {
        }

        public static void ShowWarningMessage(string message, messagePriority priority)
        {
            GameObject warningMsgUI = GameObject.FindGameObjectWithTag("WarningMsg");

            if (warningMsgUI == null)
            {
                Debug.LogError("No UI Tagged as WarningMsg");
                return;
            }

            // Set UI Message
            warningMsgUI.transform.GetChild(0).gameObject.GetComponent<Text>().text = message;

            // Play UI Animation
            Animator animator = warningMsgUI.GetComponent<Animator>();
            animator.SetTrigger(ShowMessage);
        }
    }
}