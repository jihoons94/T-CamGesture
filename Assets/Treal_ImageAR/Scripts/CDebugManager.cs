/** 
*
* Copyright 2016-2018 SK Telecom. All Rights Reserved.
*
* This file is part of T real Platform.
*
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
*/
using UnityEngine;
using UnityEngine.UI;

namespace Treal.BrowserCore
{
	public class CDebugManager : MonoBehaviour
	{

		public Transform cam;
		public Transform target;

		private Text _camPose;
		private Text _targetPose;
        private Text _qrcode_text;

		// Use this for initialization
		void Start()
		{
			_camPose = GameObject.Find("CamPosePrint/Text").GetComponent<Text>();
			_targetPose = GameObject.Find("TargetPosePrint/Text").GetComponent<Text>();
            _qrcode_text = GameObject.Find("QRCodePrint/Text").GetComponent<Text>();
        }

		// Update is called once per frame
		void Update()
		{
			_camPose.text = string.Format("[Camera Pose]  X : {0:F4},  Y : {1:F4},  Z : {2:F4}", cam.position.x, cam.position.y, cam.position.z);
			_targetPose.text = string.Format("[Target Pose] : X : {0:F4},  Y : {1:F4},  Z : {2:F4}", target.position.x, target.position.y, target.position.z);
            _qrcode_text.text = string.Format(QRCode.QRCodeManager.qrparse_result);
		}

	}

}
