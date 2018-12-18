using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCamera {
	
	public class TCamParameters : MonoBehaviour {

		/// <summary>
		/// Exposure Mode 설정
		/// 
		/// - Android
		///     . Camera2 : OFF, ON, ON_AUTO_FLASH, ON_AUTO_FLASH_REDEYE
		///     . Legacy : N/A
		/// - iOS
		///     LOCKED, AUTO_EXPOSE, CONTINUOUS_AUTO_EXPOSURE, CUSTOM
		/// </summary>
		public enum ExposureMode
		{
			// Android Camera2
			OFF = 0,
			ON = 1,
			ON_AUTO_FLASH = 2,
			ON_ALWAYS_FLASH = 3,
			ON_AUTO_FLASH_REDEYE = 4,

			// Android Camera

			// IOS
			LOCKED = 0,
			AUTO_EXPOSE = 1,
			CONTINUOUS_AUTO_EXPOSURE = 2,
			CUSTOM = 3,
		}

		/// <summary>
		/// Flash mode 설정
		/// 
		/// - Android
		///     . Camera2 : OFF, SINGLE, TORCH
		///     . Legacy : OFF, AUTO, ON, RED_EYE, TORCH 
		/// - iOS
		///     OFF, ON, AUTO
		/// </summary>
		public enum FlashMode
		{
			// Android Camera2 (OFF, SINGLE, TORCH)
			OFF = 0,
			SINGLE = 1,
			TORCH = 3,

			// Android Camera (OFF, AUTO, ON, RED_EYE, TORCH)
			//OFF = 0,
			AUTO = 1,
			ON = 2,
			//TORCH = 3,
			RED_EYE = 4,

			// IOS (OFF, ON, AUTO)
			//OFF = 0,
			//AUTO = 1,
			//ON = 2,
		}

		/// <summary>
		/// Focus Mode 설정
		/// 
		/// - Android
		///     . Camera2 : OFF, AUTO, MACRO, CONTINUOUS_VIDEO, CONTINUOUS_PICTURE, EDOF
		///     . Legacy : AUTO, MACRO, CONTINUOUS_VIDEO, CONTINUOUS_PICTURE, EDOF, FIXED, INFINITY 
		/// - iOS
		///     LOCKED, AUTO, CONTINUOUS_AUTO
		/// </summary>
		public enum FocusMode
		{
			// Android Camera2 (OFF, AUTO, MACRO, CONTINUOUS_VIDEO, CONTINUOUS_PICTURE, EDOF)
			OFF = 0,
			AUTO = 1,
			MACRO = 2,
			CONTINUOUS_VIDEO = 3,
			CONTINUOUS_PICTURE = 4,
			EDOF = 5,

			// Android Camera (AUTO, MACRO, CONTINUOUS_VIDEO, CONTINUOUS_PICTURE, EDOF, FIXED, INFINITY)
			//AUTO,
			//MACRO,
			//CONTINUOUS_VIDEO,
			//CONTINUOUS_PICTURE,
			//EDOF,
			FIXED = 6,
			INFINITY = 7,

			// IOS (LOCKED, AUTO, CONTINUOUS_AUTO)
			LOCKED = 0,
			//AUTO = 1,
			CONTINUOUS_AUTO = 2,
		}

		/// <summary>
		/// 프리뷰 화면 Scale 설정
		/// </summary>
		public enum Scale
		{
			/// <summary>
			/// scale 하지 않는다.
			/// </summary>
			NONE = 0,

			/// <summary>
			/// 프리뷰 비율에 맞게 화면의 가로 또는 세로 크기에 맞춘다, 가로 또는 세로 방향에 여백이 생길 수 있다.
			/// </summary>
			FIT,

			/// <summary>
			/// 프리뷰 비율에 맞게 화면을 가득 채운다. 가로 또는 세로 방향으로 화면이 잘릴 수 있다.
			/// </summary>
			FULL
		}

		/// <summary>
		/// 프리뷰 프레임 데이터 포맷
		/// </summary>
		public enum PreviewFormat
		{
			/// <summary>
			/// 알 수 없는 포맷, unused
			/// </summary>
			UNKNOWN = 0,

			/// <summary>
			/// YUV NV21 포맷
			/// Android Only
			/// </summary>
			NV21 = 1,

			/// <summary>
			/// YUV NV12 포맷
			/// iOS Only
			/// </summary>
			NV12 = 2,

			/// <summary>
			/// RGB24 포맷, unused
			/// </summary>
			RGB888 = 3,

			/// <summary>
			/// Gray 포맷, unused
			/// </summary>
			GRAYSCALE = 4,

			/// <summary>
			/// RGBA 포맷
			/// PC Only
			/// </summary>
			RGBA = 5
		};

	}
}
