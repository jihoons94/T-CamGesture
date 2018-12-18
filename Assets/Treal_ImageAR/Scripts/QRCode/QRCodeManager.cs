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

using System;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Threading;

namespace QRCode
{
	public class QRCodeManager : MonoBehaviour {

		// QRCodeManager Instance
		public static QRCodeManager Instance = null;

		private byte[] img_bytes = null;
		private bool processing = false;

		private int _width = 1280;
		private int _height = 720;

		Thread _processQRThread = null;

		public bool isQRCodeDetected = false;

        public static string qrparse_result = "";

		// Awake() Set Instance.
		void Awake(){
            Debug.Log ("UNITY: QRCodeManager: Awake() ");

			QRCodeManager.Instance = this;
			processing = false;
		}

		public void Start()
		{
            Debug.Log ("UNITY: QRCodeManager: Start() ");

		}

		public void StartQRCode( int width, int height ){
            Debug.Log ("UNITY: QRCodeManager: StartQRCode() ");

			if (_processQRThread == null) {

				_width = width;
				_height = height;

				img_bytes = new byte[ _width * _height];
				_processQRThread = new Thread (QRProcess);
			}
			
			if (!_processQRThread.IsAlive) {
				_processQRThread.Start ();
			}

			isQRCodeDetected = false;
		}

		/// <summary>
		/// Copying Luminance data to byte array.
		/// </summary>
		/// <param name="ptr">Preview IntPtr.</param>
		public void QRCodeProcess( IntPtr ptr )
		{
			if (!processing) {
				processing = true;
				Marshal.Copy (ptr, img_bytes, 0, _width * _height);
			}
		}
		/// <summary>
		/// QR Process Thread
		/// </summary>
		private void QRProcess( )
		{
			ZXing.QrCode.QRCodeReader reader = new ZXing.QrCode.QRCodeReader ();

			while (true) {
				try {
					//make source
					// var sourceNo = new ZXing.Color32LuminanceSource(c, width, height);
					var source = new ZXing.RGBLuminanceSource (img_bytes, _width, _height, true);
					//in case of RGB raw

					var binarizer = new ZXing.Common.HybridBinarizer (source);
					var binBitmap = new ZXing.BinaryBitmap (binarizer);

					binBitmap = binBitmap.crop(_width / 4, _height / 4 , _width/2, _height/2);

					//raise exception if the reader couldn't recognize it
					//                    string text = reader.decode(binBitmap).Text;

					string text = "";
					ZXing.Result zxing_result = reader.decode(binBitmap);

					if(zxing_result != null ) 
					{
						text = zxing_result.Text;
					}
					else
					{
						text = "";
					}

                    qrparse_result = text;

					//if (ScanResultUpdated != null)
					//{
					//    ScanResultUpdated.Invoke(text);
					//}
					isQRCodeDetected = true;
				} catch {
				}

				processing = false;
			}
		}

		public void pause()
		{
            Debug.Log ("UNITY: QRCodeManager: pause() ");

			isQRCodeDetected = false;
			_processQRThread.Abort ();
			_processQRThread = null;

		}
	}
}