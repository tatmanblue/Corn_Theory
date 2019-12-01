using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CornTheory.Camera
{
    /// <summary>
    /// purpose is to set the cursor the player sees based on 
    /// what the raycaster says is viewed by the player position of the mouse
    /// </summary>
    [RequireComponent(typeof(CameraRaycaster))]
    public class CursorAffordance : MonoBehaviour
    {
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D attackCursor = null;
        [SerializeField] Texture2D npcCursor = null;
        [SerializeField] Texture2D uiCursor = null;
        [SerializeField] Texture2D cantWalkCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        // TODO fix duplicate with PlayerMovementController
        // TODO fix issue between const and serializable
        [SerializeField] const int uiLayerNumber = 5;
        [SerializeField] const int enemyLayerNumber = 8;
        [SerializeField] const int npcLayerNumber = 9;
        [SerializeField] const int walkableLayerNumber = 10;
        [SerializeField] const int hackableLayerNumber = 11;
        [SerializeField] const int notWalkableLayerNumber = 12;
        [SerializeField] const int endStopRaycasterId = -1;


        CameraRaycaster cameraRaycaster;

        void Start()
        {
            cameraRaycaster = GetComponent<CameraRaycaster>();
            cameraRaycaster.NotifyLayerChangeObservers += OnNotifyLayerChangeObservers;
        }

        private void OnNotifyLayerChangeObservers(int newLayer)
        {
            Texture2D useThisCursor = cantWalkCursor;

            switch (newLayer)
            {
                case walkableLayerNumber:
                    useThisCursor = walkCursor;
                    break;
                case npcLayerNumber:
                    useThisCursor = npcCursor;
                    break;
                case hackableLayerNumber:
                    useThisCursor = attackCursor;
                    break;
                case uiLayerNumber:
                    useThisCursor = uiCursor;
                    break;
                case notWalkableLayerNumber:
                    useThisCursor = cantWalkCursor;
                    break;
                case enemyLayerNumber:
                    useThisCursor = attackCursor;
                    break;
                default:
                    break;
            }

            print("layer identified: " + newLayer.ToString() + ": " + useThisCursor.name);
            Cursor.SetCursor(useThisCursor, cursorHotspot, CursorMode.Auto);
        }

    }
}