using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace CornTheory.Missions
{
    /// <summary>
    /// this class belongs on the Pixel Crushers Dialog Manager prefab.  It receives conversation text and routes it to 
    /// the handlers
    /// </summary>
    public class MissionConversationEcho : MonoBehaviour
    {
        private IEnvironmentEchoHandler echoHandler;

        private void Awake()
        {
            echoHandler = GameObject.Find(Constants.Canvas).GetComponent<IEnvironmentEchoHandler>();
        }

        private void OnConversationLine(Subtitle subtitle)
        {
            // print(string.Format("{0} : {1} {2}", subtitle.speakerInfo.Name, subtitle.formattedText.text, echoHandler == null));
            if (null == echoHandler) return;

            echoHandler.ProcessActorMessage(subtitle.speakerInfo.Name, subtitle.formattedText.text);
        }
    }
}
