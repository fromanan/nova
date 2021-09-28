using UnityEngine;
using UnityEngine.UI;

namespace Nova.Library.Interactions
{
    public class InteractMessage
    {
        public enum MessagePriority
        {
            Hidden,
            High,
            Medium,
            Low
        }

        private MessagePriority currentMessagePriority = MessagePriority.Hidden;
        private static readonly int ShowMessage = Animator.StringToHash("ShowMessage");

        public static void ShowInteractMessage(string message, MessagePriority priority)
        {
        }

        public static void ShowWarningMessage(string message, MessagePriority priority)
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