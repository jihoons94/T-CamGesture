//
//  TCam.h
//  TCamIOS
//
//  Created by 최지윤 on 2018. 8. 31..
//  Copyright © 2018년 최지윤. All rights reserved.
//


#pragma once

#include "TCamPlugin.h"
#include "TCamRenderer.h"

#include <string>
#include <list>
#include <pthread.h>


typedef void (*tcam_preview_start_callback)(int width, int height, int fps);
typedef void (*tcam_preview_update_callback)(int frameId, uint8_t* y, uint8_t* uv, uint8_t* u, uint8_t* v);
typedef void (*tcam_capture_update_callback)(uint8_t* data, int size, int width, int height, int rotation, bool hflip, bool vflip);
typedef void (*tcam_request_render_callback)(int event);


class TCam
{
public:
    TCam();
    ~TCam();
    
    void init(const char* gameObject, int renderMethod, bool copyFrameData);
    void dispose();
    
    void setCallback(
                     tcam_preview_start_callback previewStartCallback,
                     tcam_preview_update_callback previewUpdateCallback,
                     tcam_capture_update_callback captureUpdateCallback,
                     tcam_request_render_callback requestRenderCallback);
    
    void startPreview(bool frontFacing, int width, int height, int fps);
    void stopPreview();
    void capture();
    
    void setTexture(int texId, int yTexId, int uvTexId, int uTexId, int vTexId);
    void setTexture(void* texPtr, void* yTexPtr, void* uvTexPtr, void* uTexPtr, void* vTexPtr);
    
    void setCurrentFrame(int frameId);
    void setScreenRotation(int width, int height, int sensorOrientation, int displayRotation);
    void setScale(int scale);
    
    void getPreviewData(uint8_t* dstYUV, uint8_t* dstY, uint8_t* dstUV, uint8_t* dstU, uint8_t* dstV);
    
    void setPreviewBuffer(uint8_t* yBuffer, uint8_t* uvBuffer, uint8_t* uBuffer, uint8_t* vBuffer);
    void setPreviewBuffer(uint8_t* data);
    bool getPreviewBuffer(uint8_t **yBuffer, uint8_t **uvBuffer, uint8_t **uBuffer, uint8_t **vBuffer);
    
    void setCaptureData(uint8_t* data, int size, int width, int height, int rotation, bool hflip, bool vflip);
    
    void renderEvent(int event);
    
    int getPreviewTexture();
    int getSurfaceTexture();
    void updateSurfaceTexture(float *transform);
    void requestRender(int event);
    
#if UNITY_IOS
    void OnCaptureDevice(const char* name, int frontFacing);
    void OnCaptureInit(int width, int height, int fps);
    void OnCapturePreviewData(uint8_t* y, uint8_t* uv, uint8_t* u, uint8_t* v);
    void OnCapturePhotoData(uint8_t* data, int size, int width, int height, int rotation, bool hflip, bool vflip);
    
    const char* getDevices();
    void setDevice(const char* deviceId);
    void setPreviewResolution(int width, int height);
    void getPreviewResolution(int* width, int* height);
    void setPreviewFPS(int fps);
    int getPreviewFPS();
    void setCaptureResolution(int width, int height);
    void getCaptureResolution(int* width, int* height);
    bool setExposureMode(int mode);
    float getMinExposure();
    float getMaxExposure();
    float setExposure(float exposure);
    bool setFlashMode(int mode);
    bool setFocusMode(int mode);
    int getSensorOrientation();
    int getDisplayRotation();
    
    void sendMessage(const char* msg);
#endif
    
public:
    enum
    {
        UNITY = 1,
        NATIVE_UPDATE_TEXTURE,
        NATIVE_UPDATE_SURFACE_TEXTURE, // Android Only
        NATIVE_GL_SHADER,
        NATIVE_GL_SHADER_POST_RENDER,
    };
    
    enum
    {
        SCALE_NONE = 0,
        SCALE_FIT,
        SCALE_FULL,
    };
    
private:
    enum {
        INIT = 8226 /*tcam*/,
        DISPOSE,
        START,
        STOP,
        RENDER,
    };
    
    int     renderMethod;
    bool    copyFrameData;
    
    bool    preview;
    int     previewWidth;
    int     previewHeight;
    int     previewFps;
    
    tcam_preview_start_callback     previewStartCallback;
    tcam_preview_update_callback    previewUpdateCallback;
    tcam_capture_update_callback    captureUpdateCallback;
    tcam_request_render_callback    requestRenderCallback;
    
    TCamRenderer *                  renderer;
    
    int                             frameCount;
    int                             currentFrame;
    
    struct FrameData
    {
        FrameData(int id, uint8_t* yData, uint8_t* uvData, uint8_t* uData, uint8_t* vData)
        {
            ID = id;
            YData = yData;
            UVData = uvData;
            UData = uData;
            VData = vData;
        }
        
        int         ID;
        uint8_t *   YData;
        uint8_t *   UVData;
        uint8_t *   UData;
        uint8_t *   VData;
    };
    
    std::list<FrameData>            frameDataList;
    
    uint8_t *                       frameYData;
    uint8_t *                       frameUVData;
    uint8_t *                       frameUData;
    uint8_t *                       frameVData;
    
    //    pthread_mutex_t               mutexlock;
    
#if UNITY_IOS
    struct Device
    {
        Device(const char* name, bool frontFacing)
        {
            this->name = name;
            this->frontFacing = frontFacing;
        }
        
        std::string         name;
        bool                frontFacing;
    };
    
    std::list<Device>       devices;
    
    std::string             gameObject;
    
    void*                   captureController;
#endif
};


#if UNITY_IOS
#ifdef __cplusplus
extern "C"
{
#endif
    
    void                TCamCaptureController_GetDevices(void* udata);
    
    void*               TCamCaptureController_SetDevice(const char* deviceId, void* udata);
    void                TCamCaptureController_SetPreviewResolution(void* captureController, int width, int height);
    void                TCamCaptureController_GetPreviewResolution(void* captureController, int* width, int* height);
    void                TCamCaptureController_SetPreviewFPS(void* captureController, int fps);
    int                 TCamCaptureController_GetPreviewFPS(void* captureController);
    void                TCamCaptureController_SetCaptureResolution(void* captureController, int width, int height);
    void                TCamCaptureController_GetCaptureResolution(void* captureController, int* width, int* height);
    bool                TCamCaptureController_SetExposureMode(void* captureController, int mode);
    float               TCamCaptureController_GetMinExposure(void* captureController);
    float               TCamCaptureController_GetMaxExposure(void* captureController);
    float               TCamCaptureController_SetExposure(void* captureController, float exposure);
    bool                TCamCaptureController_SetFlashMode(void* captureController, int mode);
    bool                TCamCaptureController_SetFocusMode(void* captureController, int mode);
    
    void                TCamCaptureController_SetPreview(void* captureController, int width, int height, int fps);
    void                TCamCaptureController_Start(void* captureController);
    void                TCamCaptureController_Pause(void* captureController);
    void                TCamCaptureController_Stop(void* captureController);
    void                TCamCaptureController_Capture(void* captureController);
    
    void                TCamCaptureController_ReadToMemory(void* captureController, void* dst, int width, int height);
    int                 TCamCaptureController_VideoRotationDeg(void* captureController);
    bool                TCamCaptureController_IsFrontFacing(void* captureController);
    
#ifdef __cplusplus
} // extern "C"
#endif
#endif
/* TCam_h */
