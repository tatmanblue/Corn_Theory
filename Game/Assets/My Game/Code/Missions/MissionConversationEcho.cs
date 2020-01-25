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
            echoHandler = GameObject.Find(Constants.Canvas).GetComponentInChildren<IEnvironmentEchoHandler>();
        }

        private void OnConversationLine(Subtitle subtitle)
        {
            if (null == echoHandler) return;

            echoHandler.ProcessActorMessage(subtitle.speakerInfo.Name, subtitle.formattedText.text);
        }
    }
}
