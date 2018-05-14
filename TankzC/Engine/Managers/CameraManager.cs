using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankzC
{
    struct Limits
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;
    }

    static class CameraManager
    {
        private static Camera mainCamera;
        private static GameObject target;
        private static Limits cameraLimits;

        private static Vector2 startCameraPosition;
        private static Vector2 endCameraPosition;
        private static float delay;
        private static float moveCounter;
        private static bool isUpdatingPosition;

        private static Dictionary<string, Tuple<Camera, float>> cameraList;

        public static void Init()
        {
            mainCamera = new Camera();
            ResetLimits();
            cameraList = new Dictionary<string, Tuple<Camera, float>>();
        }

        public static void Init(Vector2 cameraPosition, Vector2 cameraPivot)
        {
            mainCamera = new Camera(cameraPosition.X, cameraPosition.Y);
            mainCamera.pivot = cameraPivot;
            ResetLimits();
            cameraList = new Dictionary<string, Tuple<Camera, float>>();
        }

        public static void ResetCamera()
        {
            mainCamera.pivot = Vector2.Zero;
            mainCamera.position = Vector2.Zero;
            target = null;
            ResetLimits();
            cameraList.Clear();
        }

        private static void ResetLimits()
        {
            cameraLimits.MinY = 0;
            cameraLimits.MaxY = Game.window.Height * 0.5f;
            cameraLimits.MaxX = Game.window.Width;
            cameraLimits.MinX = 0;
        }

        private static void CheckLimits()
        {
            if (mainCamera.position.Y > cameraLimits.MaxY)
                mainCamera.position.Y = cameraLimits.MaxY;
            else if (mainCamera.position.Y < cameraLimits.MinY)
                mainCamera.position.Y = cameraLimits.MinY;

            if (mainCamera.position.X > cameraLimits.MaxX)
                mainCamera.position.X = cameraLimits.MaxX;
            else if (mainCamera.position.X < cameraLimits.MinX)
                mainCamera.position.X = cameraLimits.MinX;
        }

        public static void SetTarget(GameObject newTarget)
        {
            target = newTarget;
        }

        public static void MoveCameraTo(Vector2 cameraPosition, float duration = 1.0f)
        {
            //Move camera to the given cameraPosition with the given duration
            startCameraPosition = mainCamera.position;
            endCameraPosition = cameraPosition;
            moveCounter = 0;
            delay = duration;
            isUpdatingPosition = true;
            target = null;
        }

        public static void AddCamera(string cameraKey, float speedMul, Camera camera=null)
        {
            if (camera == null)
            {
                camera = new Camera(mainCamera.position.X, mainCamera.position.Y);
                camera.pivot = mainCamera.pivot;
            }

            cameraList.Add(cameraKey, new Tuple<Camera, float>(camera, speedMul));
        }

        public static Camera GetCamera(string cameraKey)
        {
            if (cameraList.ContainsKey(cameraKey))
            {
                return cameraList[cameraKey].Item1;
            }
            return null;
        }

        public static void Update()
        {
            //save camera current position
            Vector2 cameraDelta = mainCamera.position;

            if(isUpdatingPosition)
            {
                moveCounter += Game.DeltaTime;
                if (moveCounter >= delay)
                {
                    isUpdatingPosition = false;
                    mainCamera.position = endCameraPosition;
                }
                else
                {
                    mainCamera.position = Vector2.Lerp(startCameraPosition, endCameraPosition, moveCounter / delay);
                }
            }
            else if (target != null)
            {
                mainCamera.position = Vector2.Lerp(mainCamera.position, target.Position, Game.DeltaTime*4);
            }
            //else if (IsUpdating)
            //{
                //handle the camera movement
            //}

            CheckLimits();

            //compute camera delta (new position - old)
            cameraDelta = mainCamera.position - cameraDelta;

            foreach (var item in cameraList)
            {
                //camera position += delta * camera speed multiplier
                item.Value.Item1.position += cameraDelta * item.Value.Item2;
            }
        }
    }
}
