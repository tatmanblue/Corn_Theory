using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBD.Camera
{
    public class CameraRotateFollow : MonoBehaviour
    {
        private GameObject player;
        private Vector3 distanceVecToPlayer;
        private float distanceToPlayer = 3f;
        private float cameraHeight = 2f;

        // Use this for initialization
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            // Calculate and store the camera arm position relative to player
            // by getting the distance between the player's position and camera arm 's position.
            distanceVecToPlayer = transform.position - player.transform.position;

            // on start, we are assuming player is facing north (rotation 0, 0, 0) (this is design flaw I can live with)
            // therefore transform.position.z is the distance from camera arm to player
            // y Height above player
            // x is left or right of player (and should always be 0)
            distanceToPlayer = distanceVecToPlayer.z;
            cameraHeight = distanceVecToPlayer.y;

        }

        private void Update() { }

        private void LateUpdate()
        {
            Follow();
        }

        private void Follow()
        {
            var forward = player.transform.forward;
            float distanceModifer = distanceToPlayer * -1.0f;

            // compute camera arm location. 
            Vector3 setCameraLocation = player.transform.position - distanceModifer * player.transform.forward;

            // camera is set above the player (to look over his shoulder). To keep a consistent height
            // set height (Vector3.y) to player position (which is techincally at the ground) + starting camera height
            setCameraLocation.y = player.transform.position.y + cameraHeight;

            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            // this keeps the camera on the same axis as the previous update            
            transform.position = setCameraLocation;

            // make sure camera arm (and hence the camara) is looking at the player
            transform.LookAt(player.transform);
        }
    }
}
