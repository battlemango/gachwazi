using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    
    public class touchlocation
    {
        public int touchId;
        public GameObject circle;
        private Canvas cameraCanvas;

        private Color32 mColor32;

        public touchlocation(int newTouchId, GameObject newCircle, Vector2 pos)
        {
            cameraCanvas = GameObject.Find("CameraCanvas").GetComponent<Canvas>();
            touchId = newTouchId;
            circle = newCircle;

            showCircle(pos);
        }


        private void showCircle(Vector3 pos)
        {
            CircleController controller = circle.GetComponentInChildren<CircleController>();
            controller.initCircle();
            mColor32 = controller.getColor32();

            moveCircle(pos);
            circle.SetActive(true);
            SoundManager.sInstance.playOneShot(AudioClipManager.sinstance.AC_GEM_GET);
        }

        public void moveCircle(Vector3 pos)
        {
            pos.z = cameraCanvas.planeDistance;
            Camera renderCamera = cameraCanvas.worldCamera;
            Vector3 canvasPos = renderCamera.ScreenToWorldPoint(pos);

            circle.transform.position = canvasPos;
        }

        public void dismissCircle()
        {
            circle.SetActive(false);
        }

        public Vector3 getPos()
        {
            return circle.transform.position;
        }
        public Color32 getColor32()
        {
            return mColor32;
        }
    }
}
