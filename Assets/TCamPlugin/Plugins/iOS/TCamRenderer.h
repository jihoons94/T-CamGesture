//
//  TCamRenderer.h
//  TCamIOS
//
//  Created by 최지윤 on 2018. 8. 31..
//  Copyright © 2018년 최지윤. All rights reserved.
//


#pragma once

#include "TCamPlugin.h"
#include <stdint.h>

#define UNITY_IOS 1

#if UNITY_IOS
//#    include <OpenGLES/ES2/gl.h>
#    include <OpenGLES/ES3/gl.h>
#elif UNITY_ANDROID
//#    include <GLES2/gl2.h>
//#   include <GLES2/gl2ext.h>
#   include <GLES3/gl3.h>
#   include <GLES3/gl3ext.h>
#endif

#define GL_TEXTURE_EXTERNAL_OES 0x8D65

#define BUFFER_OFFSET(i) ((char *)NULL + (i))

typedef void (*tcam_preview_update_callback)(int frameId, uint8_t* y, uint8_t* uv, uint8_t* u, uint8_t* v);


class TCamRenderer
{
public:
    static TCamRenderer * getRenderer(int renderMethod);
    
    void setUpdateCallback(tcam_preview_update_callback callback);
    void setPreview(bool frontFacing, int width, int height);
    void setTexture(int texId, int yTexId, int uvTexId, int uTexId, int vTexId);
    void setTexture(void* texPtr, void* yTexPtr, void* uvTexPtr, void* uTexPtr, void* vTexPtr);
    void setScale(int scale);
    void setScreenRotation(int width, int height, int sensorOrientation, int displayRotation);
    
    TCamRenderer();
    virtual ~TCamRenderer();
    
    virtual void init() = 0;
    virtual void dispose() = 0;
    virtual void start() = 0;
    virtual void stop() = 0;
    virtual void render(uint8_t *yBuffer, uint8_t *uvBuffer, uint8_t *uBuffer, uint8_t *vBuffer) = 0;
    
    virtual int getPreviewTexture();
    virtual int getSurfaceTexture();
    virtual void updateSurfaceTexture(float *transform);
    
protected:
    int loadProgram(const char* vshader, const char* fshader);
    GLuint loadShader(GLenum type, const char* text);
    void setDefaultGraphicsState();
    void checkGlError(const char* str);
    
    bool    gles30;
    
    enum
    {
        ATTRIB_POSITION,
        ATTRIB_TEX,
    };
    
    struct Attrib {
        float x, y;
        float u, v;
    };
    
    GLuint  attribBuffer;
    
    tcam_preview_update_callback updateCallback;
    
    bool    front;
    int     texWidth;
    int     texHeight;
    
    void *  texPtr;
    void *  yTexPtr;
    void *  uvTexPtr;
    void *  uTexPtr;
    void *  vTexPtr;
    
    int     texId;
    int     yTexId;
    int     uvTexId;
    int     uTexId;
    int     vTexId;
    
    int     scale;
    int     screenWidth;
    int     screenHeight;
    int     sensorOrientation;
    int     displayRotation;
};



class TCamRendererUpdateTex : public TCamRenderer {
public:
    TCamRendererUpdateTex();
    ~TCamRendererUpdateTex();
    
    virtual void init();
    virtual void dispose();
    virtual void start();
    virtual void stop();
    virtual void render(uint8_t *yBuffer, uint8_t *uvBuffer, uint8_t *uBuffer, uint8_t *vBuffer);
};

class TCamRendererUpdateSurfaceTex : public TCamRenderer {
public:
    TCamRendererUpdateSurfaceTex();
    ~TCamRendererUpdateSurfaceTex();
    
    virtual void init();
    virtual void dispose();
    virtual void start();
    virtual void stop();
    virtual void render(uint8_t *yBuffer, uint8_t *uvBuffer, uint8_t *uBuffer, uint8_t *vBuffer);
    
    virtual int getPreviewTexture();
    virtual int getSurfaceTexture();
    virtual void updateSurfaceTexture(float *transform);
    
private:
    void render(float* transform);
    void preserveGLContext();
    void restoreGLContext();
    void getFrameData();
    
    GLuint frameBuffer;
    GLuint renderBuffer;
    GLuint surfaceTexture;
    GLuint previewTexture;
    
    int program;
    //    int texTransformLoc;
    int surfaceLoc;
    
    int previousFBO;
    int previousRenderBuffer;
    int previousProgram;
    int previousABO;
    int previousEAB;
    
    int frameCount, index, nextIndex;
    GLuint pbo[2];
    uint8_t * data;
};

class TCamRendererGLShader : public TCamRenderer
{
public:
    TCamRendererGLShader();
    ~TCamRendererGLShader();
    
    virtual void init();
    virtual void dispose();
    virtual void start();
    virtual void stop();
    virtual void render(uint8_t *yBuffer, uint8_t *uvBuffer, uint8_t *uBuffer, uint8_t *vBuffer);
    
    virtual int getPreviewTexture();
    virtual int getSurfaceTexture();
    virtual void updateSurfaceTexture(float *transform);
    
private:
    void updateTex(uint8_t *yBuffer, uint8_t *uvBuffer, uint8_t *uBuffer, uint8_t *vBuffer);
    
    GLuint  texture[2];
    GLuint  surfaceTexture;
    
    GLuint    program;
    int     rotateMatrixLoc;
    int     yTexLoc;
    int     uvTexLoc;
    int     colorLoc;
};

/* TCamRenderer_h */
