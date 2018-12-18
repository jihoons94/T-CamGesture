using UnityEngine;
using Treal.Browser.Core;

namespace Treal.Browser
{
    [RequireComponent(typeof(Camera))]
    public class FakeAR : MonoBehaviour
    {
        private GameObject fakeArObj;
        private Vector3 originPos;
        private Camera arCam;
        private GyroController gyro;
        private bool instantiated;
        private DetectOrientation orientationDetect;


        private void Start()
        {
            arCam = GetComponent<Camera>();
            orientationDetect = GetComponent<DetectOrientation>();
            gyro = gameObject.AddComponent<GyroController>();
            gyro.enabled = false;
        }

        public void Play(GameObject obj, bool instantiate, Vector3 pos, Vector3 rot, Vector3 scale, bool enableAR = true)
        {
            arCam.transform.localRotation = Quaternion.identity;

            InstantiateFakeARObject(obj, instantiate, pos, rot, scale);

            gyro.enabled = true;

            if (orientationDetect != null)
            {
                orientationDetect.enabled = false;
                transform.localEulerAngles = Vector3.zero;
            }
        }

        private void InstantiateFakeARObject(GameObject obj, bool instantiate, Vector3 pos, Vector3 rot, Vector3 scale)
        {
            Vector3 objPos = pos;
            originPos = pos;

            instantiated = instantiate;

            if (instantiate)
            {
                fakeArObj = Instantiate(obj);
            }
            else
            {
                fakeArObj = obj;
            }

            fakeArObj.transform.localPosition = objPos;
            fakeArObj.transform.localRotation = Quaternion.Euler(rot);
            fakeArObj.transform.localScale = scale;
            fakeArObj.SetActive(true);
        }

        public bool SupportsFakeAR()
        {
            return SystemInfo.supportsGyroscope;
        }

        public void Pause()
        {
            gyro.enabled = false;
            arCam.transform.localRotation = Quaternion.identity;
            fakeArObj.transform.localPosition = originPos;
        }

        public void Resume()
        {
            gyro.enabled = true;
        }

        public void Stop()
        {
            if (fakeArObj != null)
            {
                if (instantiated)
                {
                    Destroy(fakeArObj);
                }
                else
                {
                    fakeArObj.SetActive(false);
                }
            }

            arCam.transform.localRotation = Quaternion.identity;

            if (orientationDetect != null)
            {
                orientationDetect.enabled = true;
            }
        }
    }
}
