using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBD.Player
{
    /// <summary>
    /// Purpose is to handle keyboard input in the game when not interacting with UI
    /// current used in player object
    /// </summary>
    public class KeyboardHandler : MonoBehaviour
    {
        private PopupInstanceHandler popupHandler;
        private ICameraControl cameraController;

        private void Start()
        {
            cameraController = GetComponent<ICameraControl>();
            if (null == cameraController)
                print("no ICameraControl found!");

            popupHandler = FindObjectOfType<PopupInstanceHandler>();
            if (null == popupHandler)
                print("no IPopupBehaviors found!");

        }

        private void LateUpdate()
        {
            // this state get changed in SceneSwitcher and DialogPopupManager
            // it means that a dialog is visible and the user is interacting with it
            // so player will not respond to keyboard events
            if (GameUIState.InWorldUI == PlayerState.Instance.GameState)
                return;

            if (null == cameraController)
            {
                cameraController = GetComponent<ICameraControl>() as ICameraControl;
                if (null == cameraController)
                {
                    print("Error: no ICameraControl found!");
                    return;
                }
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                cameraController.Up();
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                cameraController.Down();
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                cameraController.Right();
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                cameraController.Left();
            }

            if (Input.GetKeyDown(KeyCode.Home))
            {
                cameraController.GotoHomePosition();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                print("Keyboardhandler got escape key");
                popupHandler.ShowQuitConfirm();
            }
        }
    }
}
