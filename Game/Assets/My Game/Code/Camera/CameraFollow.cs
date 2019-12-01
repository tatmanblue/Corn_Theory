using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CornTheory.Camera
{
    public class CameraFollow : MonoBehaviour
    {

        private GameObject player;
        private Vector3 offset;

        // Use this for initialization
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            //Calculate and store the offset value by getting the distance between the player's position and camera's position.
            offset = transform.position - player.transform.position;
        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void LateUpdate()
        {
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = player.transform.position + offset;
        }
    }
}
