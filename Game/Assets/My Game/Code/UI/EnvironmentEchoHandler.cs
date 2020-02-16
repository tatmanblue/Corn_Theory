using UnityEngine;
using CornTheory;

namespace CornTheory.UI
{
    /// <summary>
    /// This class belongs in the UI. It adds readable information to a scroll view. 
    /// There is a public method to publish readable information on screen
    /// about the environment.  Examples: conversation, item gives, etc
    /// </summary>
    public class EnvironmentEchoHandler : MonoBehaviour, IEnvironmentEchoHandler
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject item;
        [SerializeField] private RectTransform content;
        private int numberOfItems = 0;

        private void Awake()
        {
        }

        public void ProcessEnvironmentMessage(string message)
        {
            print(string.Format("ProcessEnvironmentMessage got {0}", message));
        }

        public void ProcessActorMessage(string actor, string message)
        {
            if (actor.Length == 0 || message.Length == 0)
                return;

            content.sizeDelta = new Vector2(0, numberOfItems * 60);

            // TODO:  height of item should not be hard coded?
            float spawnY = numberOfItems * 60;

            // taken from https://www.codeneuron.com/creating-a-dynamic-scrollable-list-in-unity/
            // one difference is we are setting x to 0, always.  not sure why that has to be different
            Vector3 pos = new Vector3(0, -spawnY, spawnPoint.position.z);
            GameObject spawnedItem = Instantiate(item, pos, spawnPoint.rotation);
            spawnedItem.transform.SetParent(spawnPoint, false);
            ConversationItemDetail itemDetails = spawnedItem.GetComponent<ConversationItemDetail>();
            itemDetails.Text.text = string.Format("{0} said '{1}'", actor, message);

            numberOfItems += 1;

        }

    }
}
