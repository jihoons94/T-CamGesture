//
//  TCamPlugin.h
//  TCamIOS
//
//  Created by 최지윤 on 2018. 8. 31..
//  Copyright © 2018년 최지윤. All rights reserved.
//

#pragma once

#include "TCam.h"
#include "IUnityInterface.h"
#include "IUnityGraphics.h"

#define UNITY_IOS 1
#define LOG printf

#ifdef WIN32
#ifdef TCAMIOSPLUGIN_EXPORTS
#define TCAMIOSPLUGIN_API __declspec(dllexport)
#define EXPIMP_TEMPLATE
#else
#define TCAMIOSPLUGIN_API __declspec(dllimport)
#define EXPIMP_TEMPLATE extern
#endif
#else
#ifdef TCAMIOSPLUGIN_EXPORTS
#define TCAMIOSPLUGIN_API __attribute__((__visibility__("default")))
#else
#define TCAMIOSPLUGIN_API
#endif
#endif

#define TCAMIOSPLUGIN_API

#ifdef __cplusplus
extern "C" {
#endif

    TCAMIOSPLUGIN_API void TCamPlugin_SetCallback(void * previewStartCallback, void * previewUpdateCallback, void * captureUpdateCallback, void * requestRenderCallback);
    TCAMIOSPLUGIN_API void TCamPlugin_SetScreenRotation(int width, int height, int sensorOrientation, int displayRotation);
    TCAMIOSPLUGIN_API void TCamPlugin_GetPreviewData(uint8_t * dstYUV, uint8_t * dstY, uint8_t * dstUV, uint8_t * dstU, uint8_t * dstV);
    TCAMIOSPLUGIN_API void TCamPlugin_SetCurrentFrame(int frameId);
    TCAMIOSPLUGIN_API void TCamPlugin_SetScale(int scale);
    TCAMIOSPLUGIN_API int TCamPlugin_GetPreviewTexture();
    TCAMIOSPLUGIN_API int TCamTest_GetRenderMethod();
    
    
    TCAMIOSPLUGIN_API const char* TCamPlugin_GetDevices();
    TCAMIOSPLUGIN_API void TCamPlugin_Init(const char* gameObject, int renderMethod, bool nativeCopyFrameData);
    TCAMIOSPLUGIN_API void TCamPlugin_Dispose();
    TCAMIOSPLUGIN_API void TCamPlugin_SetDevice(const char* deviceId);
    TCAMIOSPLUGIN_API void TCamPlugin_SetPreviewResolution(int width, int height);
    TCAMIOSPLUGIN_API void TCamPlugin_GetPreviewResolution(int* width, int* height);
    TCAMIOSPLUGIN_API void TCamPlugin_SetPreviewFPS(int fps);
    TCAMIOSPLUGIN_API int TCamPlugin_GetPreviewFPS();
    TCAMIOSPLUGIN_API void TCamPlugin_SetCaptureResolution(int width, int height);
    TCAMIOSPLUGIN_API void TCamPlugin_GetCaptureResolution(int* width, int* height);
    TCAMIOSPLUGIN_API bool TCamPlugin_SetExposureMode(int mode);
    TCAMIOSPLUGIN_API float TCamPlugin_GetMinExposure();
    TCAMIOSPLUGIN_API float TCamPlugin_GetMaxExposure();
    TCAMIOSPLUGIN_API float TCamPlugin_SetExposure(float exposure);
    
    TCAMIOSPLUGIN_API bool TCamPlugin_SetFlashMode(int mode);
    TCAMIOSPLUGIN_API bool TCamPlugin_SetFocusMode(int mode);
    TCAMIOSPLUGIN_API void TCamPlugin_StartPreview(int width, int height, int fps);
    TCAMIOSPLUGIN_API void TCamPlugin_StopPreview();
    TCAMIOSPLUGIN_API void TCamPlugin_Capture();
    TCAMIOSPLUGIN_API void TCamPlugin_SetTexture(void* texPtr, void* yTexPtr, void* uvTexPtr, void* uTexPtr, void* vTexPtr);
    TCAMIOSPLUGIN_API int TCamPlugin_GetSensorOrientation();
    TCAMIOSPLUGIN_API int TCamPlugin_GetDisplayRotation();
    
    
    
#ifdef __cplusplus
}
#endif

/* TCamPlugin_h */
