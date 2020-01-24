using UnityEngine;
using CornTheory;

namespace CornTheory.UI
{
    /// <summary>
    /// This class belongs in the UI.  It provides a public method to publish readable information on screen
    /// about the environment.  Examples: conversation, item gives, etc
    /// </summary>
    public class EnvironmentEchoHandler : MonoBehaviour, IEnvironmentEchoHandler
    {
        [SerializeField] GameObject textOnScreen;

        public void ProcessEnvironmentMessage(string message)
        {
            print(string.Format("ProcessEnvironmentMessage got {0}", message));
        }

        public void ProcessActorMessage(string actor, string message)
        {
            print(string.Format("ProcessEnvironmentMessage got {0} said {1}", actor, message));
        }
    }
}
