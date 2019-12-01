using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CornTheory.Camera
{
    /// <summary>
    /// currently in camera arm object
    /// </summary>
    public class CameraPositionHandler : MonoBehaviour, ICameraControl
    {
        private Queue<Quaternion> cameraMoves = new Queue<Quaternion>();
        private GameObject mainCamera;
        private Quaternion homePosition;
        private bool inCameraMove = false;

        public void GotoHomePosition()
        {
            AddToQueue(new Quaternion(0, 0, 0, 0));
        }

        public void Down(float degrees)
        {
            float down = degrees;
            Quaternion localRotation = Quaternion.Euler(down, 0.0f, 0.0f);
            AddToQueue(localRotation);
        }

        public void Up(float degrees)
        {
            // -degrees X is up
            float up = degrees * -1;
            Quaternion localRotation = Quaternion.Euler(up, 0.0f, 0.0f);
            AddToQueue(localRotation);

        }

        public void Left(float degrees)
        {
            float left = degrees * -1; 
            Quaternion localRotation = Quaternion.Euler(0.0f, left, 0.0f);
            AddToQueue(localRotation);
        }

        public void Right(float degrees)
        {
            // -degrees X is up
            float right = degrees;
            Quaternion localRotation = Quaternion.Euler(0.0f, right, 0.0f);
            AddToQueue(localRotation);

        }

        private void Start()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            homePosition = mainCamera.transform.rotation;
        }

        private void AddToQueue(Quaternion quaternion)
        {
            lock (this)
            {
                cameraMoves.Enqueue(quaternion);
            }
        }

        private Quaternion? GetNextMovment()
        {
            lock(this)
            {
                if (0 == cameraMoves.Count)
                    return null;

                return cameraMoves.Dequeue();
            }
        }

        private void LateUpdate()
        {
            lock (this)
            {
                Quaternion? movement = GetNextMovment();
                while (movement.HasValue)
                {
                    Quaternion actual = movement.Value;

                    if (actual.x == 0 && actual.y == 0)
                    {
                        // this is go to home command
                        mainCamera.transform.localRotation = actual;
                        return;
                    }

                    mainCamera.transform.localRotation = mainCamera.transform.localRotation * actual;
                    movement = GetNextMovment();
                }
            }
        } 
    }
}
