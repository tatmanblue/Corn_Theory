using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using TBD.Camera;
using UnityStandardAssets.CrossPlatformInput;

namespace TBD.Player
{

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] float walkMoveStopRadius = 0.2f;
        [SerializeField] float hackableMoveRadius = 5.0f;

        // TODO fix duplicate with CursorAffordance
        // TODO fix issue between const and serializable
        [SerializeField] const int enemyLayerNumber = 8;
        [SerializeField] const int npcLayerNumber = 9;
        [SerializeField] const int walkableLayerNumber = 10;
        [SerializeField] const int hackableLayerNumber = 11;
        [SerializeField] const int notWalkableLayerNumber = 12;
        [SerializeField] const int endStopRaycasterId = -1;


        bool isWalkingByKeyboardInput = false;
        bool isJumping = false;
        GameObject walkTarget;
        AICharacterControl aiControl;
        ThirdPersonCharacter character;
        CameraRaycaster cameraRaycaster;
        Vector3 currentDestination;
        Vector3 clickPoint;
        private Transform mainCamera;                               // A reference to the main camera in the scenes transform

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, currentDestination);
            Gizmos.DrawSphere(currentDestination, 0.2f);
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(clickPoint, 0.1f);

            Gizmos.color = new Color(255f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, hackableMoveRadius);
        }

        private void Start()
        {
            cameraRaycaster = UnityEngine.Camera.main.GetComponent<CameraRaycaster>();
            character = GetComponent<ThirdPersonCharacter>();
            currentDestination = transform.position;
            aiControl = GetComponent<AICharacterControl>();

            // get the transform of the main camera
            if (UnityEngine.Camera.main != null)
            {
                mainCamera = UnityEngine.Camera.main.transform;
            }

            cameraRaycaster.NotifyMouseClickObservers += OnNotifyMouseClickObservers;

            walkTarget = new GameObject("walkTarget");
        }

        private void OnNotifyMouseClickObservers(RaycastHit raycastHit, int layerHit)
        {
            switch (layerHit)
            {
                case walkableLayerNumber:
                    walkTarget.transform.position = raycastHit.point;
                    WalkToObject(walkTarget);
                    break;
                case enemyLayerNumber:
                case npcLayerNumber:
                case hackableLayerNumber:
                    WalkToObject(raycastHit.collider.gameObject);
                    break;
                default:
                    Debug.LogWarning("Dont know how to handle mouse movement for layer -> " + layerHit);
                    break;
            }
        }

        private void WalkToObject(GameObject walkTo)
        {
            aiControl.SetTarget(walkTo.transform);
        }

        private void Update() { }

        private void FixedUpdate() { }
        
        private Vector3 ShortenDestination(Vector3 destination, float shortening)
        {
            Vector3 reductionVector = (destination - transform.position).normalized * shortening;
            return destination - reductionVector;
        }
    }
}
