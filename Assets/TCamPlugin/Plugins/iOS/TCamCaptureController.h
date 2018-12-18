#pragma once

#import <AVFoundation/AVFoundation.h>

@interface TCamCaptureController : NSObject <AVCaptureVideoDataOutputSampleBufferDelegate>

@property (nonatomic, retain) AVCaptureDevice*              captureDevice;
@property (nonatomic, retain) AVCaptureSession*             captureSession;
@property (nonatomic, retain) AVCaptureDeviceInput*         captureInput;
@property (nonatomic, retain) AVCaptureVideoDataOutput*     captureOutput;
@property (nonatomic, retain) AVCaptureStillImageOutput*	capturePhotoOutput;

- (bool)init:(AVCaptureDevice*)device;
- (void)setUserData:(void*)user;
- (void *)getUserData;
- (void)captureOutput:(AVCaptureOutput*)captureOutput didOutputSampleBuffer:(CMSampleBufferRef)sampleBuffer fromConnection:(AVCaptureConnection*)connection;
- (void)start;
- (void)pause;
- (void)stop;
- (void)capture;
- (AVCaptureVideoOrientation)getCaptureOrientation;
- (NSString*)pickPresetFromWidth:(int)width height:(int)height;
- (AVFrameRateRange*)pickFrameRateRange:(float)fps;
- (void)setPreviewResolution:(int)width height:(int)height;
- (void)getPreviewResolution:(int*)width height:(int*)height;
- (void)setPreviewFPS:(int)fps;
- (int)getPreviewFPS;
- (void)setCaptureResolution:(int)width height:(int)height;
- (void)getCaptureResolution:(int*)width height:(int*)height;
- (bool)setExposureMode:(int)mode;
- (float)getMinExposure;
- (float)getMaxExposure;
- (float)setExposure:(float)exposure;
- (bool)setFlashMode:(int)mode;
- (AVCaptureFlashMode)getAVCaptureFlashMode:(int)mode;
- (bool)setFocusMode:(int)mode;
//- (void)registerAutofocusObserver:(AVCaptureDevice*) device;
//- (void)unregisterAutofocusObserver:(AVCaptureDevice*) device;
//- (void)AutofocusDelegate:(NSNotification*) notification;


@end
