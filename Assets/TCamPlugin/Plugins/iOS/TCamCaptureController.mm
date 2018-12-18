
///
/// iOS 카메라 프리뷰 구현
///

#include "TCamCaptureController.h"
#include "TCamPlugin.h"
#include "TCam.h"

#include "AVCapture.h"
#include "CMVideoSampling.h"
#import <CoreVideo/CoreVideo.h>
#import <ImageIO/ImageIO.h>
#import <AssetsLibrary/AssetsLibrary.h>

#define PIXEL_FORMAT_BGRA 0
#define PIXEL_FORMAT_YUV 1
#define PIXEL_FORMAT PIXEL_FORMAT_YUV

@implementation TCamCaptureController
{
	AVCaptureDevice*			_captureDevice;
	AVCaptureSession*			_captureSession;
	AVCaptureDeviceInput*		_captureInput;
	AVCaptureVideoDataOutput*	_captureOutput;
    AVCaptureStillImageOutput*  _capturePhotoOutput;

	CMVideoSampling             _cmVideoSampling;
	void*                       _userData;
    
    dispatch_queue_t            _photoQueue;
}

@synthesize captureDevice	= _captureDevice;
@synthesize captureSession	= _captureSession;
@synthesize captureOutput	= _captureOutput;
@synthesize captureInput	= _captureInput;

- (bool)init:(AVCaptureDevice*)device
{
    if (UnityGetAVCapturePermission(avVideoCapture) == avCapturePermissionDenied) {
		return false;
    }
    
	self.captureDevice= device;

	self.captureInput	= [AVCaptureDeviceInput deviceInputWithDevice:device error:nil];
	self.captureOutput	= [[AVCaptureVideoDataOutput alloc] init];

    if (self.captureOutput == nil || self.captureInput == nil) {
		return false;
    }
    
	self.captureOutput.alwaysDiscardsLateVideoFrames = YES;
	[self.captureOutput setSampleBufferDelegate:self queue:dispatch_get_main_queue()];

    NSDictionary* options;
    if (PIXEL_FORMAT == PIXEL_FORMAT_YUV) {
        options = @{ (NSString*)kCVPixelBufferPixelFormatTypeKey : @(kCVPixelFormatType_420YpCbCr8BiPlanarFullRange) };
        //options = @{ (NSString*)kCVPixelBufferPixelFormatTypeKey : @(kCVPixelFormatType_420YpCbCr8BiPlanarVideoRange) };
    } else {
        options = @{ (NSString*)kCVPixelBufferPixelFormatTypeKey : @(kCVPixelFormatType_32BGRA) };
    }
    [self.captureOutput setVideoSettings:options];
    
    self.captureSession = [[AVCaptureSession alloc] init];
    [self.captureSession addInput:self.captureInput];
    [self.captureSession addOutput:self.captureOutput];
    
    _photoQueue = dispatch_queue_create("TCam Photo Queue", DISPATCH_QUEUE_SERIAL);
    
    self.capturePhotoOutput = [[AVCaptureStillImageOutput alloc] init];
    self.capturePhotoOutput.highResolutionStillImageOutputEnabled = YES;
    NSDictionary *outputSettings = @{AVVideoCodecKey : AVVideoCodecJPEG};
    //NSDictionary *outputSettings = @{(id)kCVPixelBufferPixelFormatTypeKey : [NSNumber numberWithInt:kCVPixelFormatType_32BGRA]};
    [self.capturePhotoOutput setOutputSettings:outputSettings];
    if ([self.captureSession canAddOutput:self.capturePhotoOutput]) {
        [self.captureSession addOutput:self.capturePhotoOutput];
    }
    
    CMVideoSampling_Initialize(&self->_cmVideoSampling);
   
	return true;
}

- (void)setUserData:(void*)user
{
    _userData = user;
}

- (void *)getUserData
{
    return _userData;
}

- (void)captureOutput:(AVCaptureOutput*)captureOutput didOutputSampleBuffer:(CMSampleBufferRef)sampleBuffer fromConnection:(AVCaptureConnection*)connection
{
    //NSLog(@"didOutputSampleBuffer");
    if (_cmVideoSampling.cvImageBuffer) {
        CFRelease(_cmVideoSampling.cvImageBuffer);
    }
    _cmVideoSampling.cvImageBuffer = CMSampleBufferGetImageBuffer(sampleBuffer);
    CFRetain(_cmVideoSampling.cvImageBuffer);
    
    CVPixelBufferLockBaseAddress((CVImageBufferRef)_cmVideoSampling.cvImageBuffer, kCVPixelBufferLock_ReadOnly);

//    int w = (int) CVPixelBufferGetWidth(pbuf); // 1280
//    int h = (int) CVPixelBufferGetHeight(pbuf); // 720
//
//    uint8_t* yBuf = (uint8_t *) CVPixelBufferGetBaseAddressOfPlane(pbuf, 0);
//    uint8_t* uvBuf = (uint8_t *) CVPixelBufferGetBaseAddressOfPlane(pbuf, 1);
//
//    size_t yRow = CVPixelBufferGetBytesPerRowOfPlane(pbuf, 0); // 1280
//    size_t yW = CVPixelBufferGetWidthOfPlane(pbuf, 0); // 1280
//    size_t yH = CVPixelBufferGetHeightOfPlane(pbuf, 0); // 720
//
//    size_t uvRow = CVPixelBufferGetBytesPerRowOfPlane(pbuf, 1); // 1280
//    size_t uvW = CVPixelBufferGetWidthOfPlane(pbuf, 1); // 640
//    size_t uvH = CVPixelBufferGetHeightOfPlane(pbuf, 1); // 360
    
    uint8_t* y = (uint8_t *) CVPixelBufferGetBaseAddressOfPlane((CVImageBufferRef)_cmVideoSampling.cvImageBuffer, 0);
    uint8_t* uv = (uint8_t *) CVPixelBufferGetBaseAddressOfPlane((CVImageBufferRef)_cmVideoSampling.cvImageBuffer, 1);
    uint8_t* u = uv;
    uint8_t* v = uv + 1;
    
    TCam* tcam = (TCam *) self->_userData;
    tcam->OnCapturePreviewData(y, uv, u, v);

    CVPixelBufferUnlockBaseAddress((CVImageBufferRef)_cmVideoSampling.cvImageBuffer, kCVPixelBufferLock_ReadOnly);
}

- (void)start
{
    [self.captureSession startRunning];
}

- (void)pause
{
    [self.captureSession stopRunning];
}

- (void)stop
{
	[self.captureSession stopRunning];
	[self.captureSession removeInput: self.captureInput];
	[self.captureSession removeOutput: self.captureOutput];
    [self.captureSession removeOutput: self.capturePhotoOutput];

	self.captureDevice = nil;
	self.captureInput = nil;
	self.captureOutput = nil;
    self.capturePhotoOutput = nil;
	self.captureSession = nil;

    _photoQueue = nil;
    
	CMVideoSampling_Uninitialize(&self->_cmVideoSampling);
}

- (void)capture
{
    dispatch_async(_photoQueue, ^{
        if (self.capturePhotoOutput.stillImageStabilizationSupported) {
            self.capturePhotoOutput.automaticallyEnablesStillImageStabilizationWhenAvailable = YES;
        }
        AVCaptureConnection *connection = [self.capturePhotoOutput connectionWithMediaType:AVMediaTypeVideo];
        if ([connection isVideoStabilizationSupported]) {
            [connection setPreferredVideoStabilizationMode:AVCaptureVideoStabilizationModeAuto];
        }
        if ([connection isVideoOrientationSupported]) {
            [connection setVideoOrientation:[self getCaptureOrientation]];
        }
        [self.capturePhotoOutput captureStillImageAsynchronouslyFromConnection:connection completionHandler: ^(CMSampleBufferRef sampleBuffer, NSError *error) {
            if ([connection isVideoStabilizationSupported]) {
                [connection setPreferredVideoStabilizationMode:AVCaptureVideoStabilizationModeOff];
            }

            // Exif에 orientation 정보 없음
//            CFDictionaryRef exifAttachments = (CFDictionaryRef)CMGetAttachment(sampleBuffer, kCGImagePropertyExifDictionary, NULL);
//            if (exifAttachments) {
//                NSLog(@"attachements: %@", exifAttachments);
//            } else {
//                NSLog(@"no attachments");
//            }
            
            NSData *imageData = [AVCaptureStillImageOutput jpegStillImageNSDataRepresentation:sampleBuffer];
            uint8_t *data = (uint8_t *)[imageData bytes];
            int size = [imageData length];
            
            UIImage *image = [[UIImage alloc] initWithData:imageData];
            /* video orientation을 설정한 경우
             portrait
                orientation: UIImageOrientationRight
                width: 720
                height: 1280
             landacape left
                orientation: UIImageOrientationUp
                width: 1280
                height: 720
             portrait upsidedown
                 orientation: UIImageOrientationLeft
                 width: 720
                 height: 1280
             landacape right
                 orientation: UIImageOrientationDown
                 width: 1280
                 height: 720
            */
            UIImageOrientation orientation = [image imageOrientation];
            //CGFloat width = image.size.width;
            //CGFloat height = image.size.height;
            int frontFacing = self.captureDevice.position == AVCaptureDevicePositionFront ? 1 : 0;
            int rotation = 0;
            switch (orientation) {
                case UIImageOrientationUp:
                    rotation = 0;
                    break;
                case UIImageOrientationLeft:
                    rotation = frontFacing ? 90 : 270;
                    break;
                case UIImageOrientationDown:
                    rotation = 180;
                    break;
                case UIImageOrientationRight:
                    rotation = frontFacing ? 270 : 90;
                    break;
                case UIImageOrientationUpMirrored: // h flip
                    rotation = 0;
                    break;
                case UIImageOrientationLeftMirrored: // v flip
                    rotation = frontFacing ? 90 : 270;
                    break;
                case UIImageOrientationDownMirrored: // h flip
                    rotation = 180;
                    break;
                case UIImageOrientationRightMirrored: // v flip
                    rotation = frontFacing ? 270 : 90;
                    break;
            }
            LOG("image orientation = %d rotation = %d", orientation, rotation);

//            ALAssetsLibrary *library = [[ALAssetsLibrary alloc] init];
//            [library writeImageToSavedPhotosAlbum:[image CGImage]
//                                      orientation:(ALAssetOrientation)[image imageOrientation]
//                                  completionBlock:^(NSURL *assetURL, NSError *error){
//                                  }];
//
//            [library release];
//            [image release];
            
            CMVideoDimensions dimensions = self.captureDevice.activeFormat.highResolutionStillImageDimensions; // w: 1280

            TCam* tcam = (TCam *) self->_userData;
            tcam->OnCapturePhotoData(data, size, dimensions.width, dimensions.height, rotation, frontFacing, false);
        }];
    });
}

- (AVCaptureVideoOrientation)getCaptureOrientation
{
    UIDeviceOrientation deviceOrientation = [[UIDevice currentDevice] orientation];
    
    AVCaptureVideoOrientation captureOrientation;
    
    if (deviceOrientation == UIDeviceOrientationPortrait) {
        NSLog(@"deviceOrientationDidChange - Portrait");
        captureOrientation = AVCaptureVideoOrientationPortrait;
    } else if (deviceOrientation == UIDeviceOrientationPortraitUpsideDown) {
        NSLog(@"deviceOrientationDidChange - UpsideDown");
        captureOrientation = AVCaptureVideoOrientationPortraitUpsideDown;
    // AVCapture and UIDevice have opposite meanings for landscape left and right (AVCapture orientation is the same as UIInterfaceOrientation)
    } else if (deviceOrientation == UIDeviceOrientationLandscapeLeft) {
        NSLog(@"deviceOrientationDidChange - LandscapeLeft");
        captureOrientation = AVCaptureVideoOrientationLandscapeRight;
    } else if (deviceOrientation == UIDeviceOrientationLandscapeRight) {
        NSLog(@"deviceOrientationDidChange - LandscapeRight");
        captureOrientation = AVCaptureVideoOrientationLandscapeLeft;
    } else if (deviceOrientation == UIDeviceOrientationUnknown) {
        NSLog(@"deviceOrientationDidChange - Unknown ");
        captureOrientation = AVCaptureVideoOrientationPortrait;
    } else {
        NSLog(@"deviceOrientationDidChange - Face Up or Down");
        captureOrientation = AVCaptureVideoOrientationPortrait;
    }
    
    return captureOrientation;
}

- (NSString*)pickPresetFromWidth:(int)width height:(int)height
{
	static NSString* preset[] =
	{
		AVCaptureSessionPreset352x288,
		AVCaptureSessionPreset640x480,
		AVCaptureSessionPreset1280x720,
		AVCaptureSessionPreset1920x1080,
	};
	static int presetW[] = { 352, 640, 1280, 1920 };

	#define countof(arr) sizeof(arr)/sizeof(arr[0])

	static_assert(countof(presetW) == countof(preset), "preset and preset width arrrays have different elem count");

	int ret = -1, curW = -10000;
	for (int i = 0, n = countof(presetW) ; i < n ; ++i) {
		if (::abs(width - presetW[i]) < ::abs(width - curW) && [self.captureSession canSetSessionPreset:preset[i]]) {
			ret = i;
			curW = presetW[i];
		}
	}

	NSAssert(ret != -1, @"Cannot pick capture preset");
	return ret != -1 ? preset[ret] : AVCaptureSessionPresetHigh;

	#undef countof
}

- (AVFrameRateRange*)pickFrameRateRange:(float)fps
{
	AVFrameRateRange* ret = nil;

	if ([self.captureDevice respondsToSelector:@selector(activeFormat)]) {
		float minDiff = INFINITY;

		// In some corner cases (seeing this on iPod iOS 6.1.5) activeFormat is null.
        if (!self.captureDevice.activeFormat) {
			return nil;
        }

		for (AVFrameRateRange* rate in self.captureDevice.activeFormat.videoSupportedFrameRateRanges) {
			float bestMatch = rate.minFrameRate;
            if (fps > rate.maxFrameRate) {
                bestMatch = rate.maxFrameRate;
            } else if (fps > rate.minFrameRate)	{
                bestMatch = fps;
            }

			float diff = ::fabs(fps - bestMatch);
			if (diff < minDiff) {
				minDiff = diff;
				ret = rate;
			}
		}

		NSAssert(ret != nil, @"Cannot pick frame rate range");
        if (ret == nil) {
			ret = self.captureDevice.activeFormat.videoSupportedFrameRateRanges[0];
        }
	}
	return ret;
}

- (void)setPreviewResolution:(int)width height:(int)height
{
   self.captureSession.sessionPreset = [self pickPresetFromWidth:width height:height];
}

- (void)getPreviewResolution:(int*)width height:(int*)height
{
//    CMVideoDimensions dimensions = CMVideoFormatDescriptionGetDimensions(self.captureDevice.activeFormat.formatDescription);
//    *width = dimensions.width;
//    *height = dimensions.height;
    
    NSDictionary* outputSettings = [self.captureOutput videoSettings];
    *width  = [[outputSettings objectForKey:@"Width"]  intValue];
    *height = [[outputSettings objectForKey:@"Height"] intValue];
}

- (void)setPreviewFPS:(int)fps
{
    if ([self.captureDevice lockForConfiguration:nil]) {
        AVFrameRateRange* range = [self pickFrameRateRange:fps];
        if (range) {
            if ([self.captureDevice respondsToSelector:@selector(activeVideoMinFrameDuration)]) {
                self.captureDevice.activeVideoMinFrameDuration = range.minFrameDuration;
            }
            if ([self.captureDevice respondsToSelector:@selector(activeVideoMaxFrameDuration)]) {
                self.captureDevice.activeVideoMaxFrameDuration = range.maxFrameDuration;
            }
        } else {
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
            self.captureOutput.minFrameDuration = CMTimeMake(1, fps);
#pragma clang diagnostic pop
        }
        [self.captureDevice unlockForConfiguration];
    }
}

- (int)getPreviewFPS
{
    return self.captureDevice.activeVideoMaxFrameDuration.timescale / self.captureDevice.activeVideoMaxFrameDuration.value;
}

- (void)setCaptureResolution:(int)width height:(int)height
{
}

- (void)getCaptureResolution:(int*)width height:(int*)height
{
    CMVideoDimensions dimensions = self.captureDevice.activeFormat.highResolutionStillImageDimensions;
    *width = dimensions.width;
    *height = dimensions.height;
}

- (bool)setExposureMode:(int)mode
{
    bool ret = false;
    AVCaptureDevice* device = self.captureDevice;
    if ([device lockForConfiguration:NULL] == YES ) {
        AVCaptureExposureMode exposureMode = (AVCaptureExposureMode) mode;
        if ([device isExposureModeSupported:exposureMode]) {
            [device setExposureMode:exposureMode];
            ret = true;
        }
        [device unlockForConfiguration];
    }
    return ret;
}

- (float)getMinExposure
{
    return self.captureDevice.minExposureTargetBias;
}

- (float)getMaxExposure
{
    return self.captureDevice.maxExposureTargetBias;
}

- (float)setExposure:(float)exposure
{
    AVCaptureDevice* device = self.captureDevice;
    if (exposure < device.minExposureTargetBias) {
        exposure = device.minExposureTargetBias;
    }
    if (exposure > device.maxExposureTargetBias) {
        exposure = device.maxExposureTargetBias;
    }
    if ([device lockForConfiguration:NULL] == YES) {
        [device setExposureTargetBias:exposure completionHandler:nil];
        [device unlockForConfiguration];
        return exposure;
    } else {
        return [device exposureTargetBias];
    }
}

- (bool)setFlashMode:(int)mode
{
    bool ret = false;
    AVCaptureDevice* device = self.captureDevice;
    if (device.flashAvailable) {
        if ([device lockForConfiguration:NULL] == YES ) {
            AVCaptureFlashMode flashMode = [self getAVCaptureFlashMode:mode];
            if ([device isFlashModeSupported:flashMode]) {
                [device setFlashMode:flashMode];
                ret = true;
            }
            [device unlockForConfiguration];
        }
    }
    return ret;
}

- (AVCaptureFlashMode)getAVCaptureFlashMode:(int)mode
{
    AVCaptureFlashMode ret = AVCaptureFlashModeAuto;
    switch (mode) {
        case 0: // off
            ret = AVCaptureFlashModeOff;
            break;
        case 1 : // auto
            ret = AVCaptureFlashModeAuto;
            break;
        case 2 : // on
            ret = AVCaptureFlashModeOn;
            break;
    }
    return ret;
}

- (bool)setFocusMode:(int)mode
{
    bool ret = false;
//    if (mode == 0) {
//        [self registerAutofocusObserver:device];
//    } else {
//        [self unregisterAutofocusObserver:device];
//    }
    AVCaptureDevice* device = self.captureDevice;
    if ([device lockForConfiguration:NULL] == YES ) {
        AVCaptureFocusMode focusMode = (AVCaptureFocusMode) mode;
        if ([device isFocusModeSupported:focusMode]) {
            [device setFocusMode:focusMode];
            ret = true;
        }
        [device unlockForConfiguration];
    }
    return ret;
}

//- (void)registerAutofocusObserver:(AVCaptureDevice*) device
//{
//    if (device.isSubjectAreaChangeMonitoringEnabled == YES) {
//        return;
//    }
//    if ([device lockForConfiguration:NULL] == YES) {
//        device.subjectAreaChangeMonitoringEnabled = YES;
//        [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(AutofocusDelegate:) name:AVCaptureDeviceSubjectAreaDidChangeNotification object:device];
//        [device unlockForConfiguration];
//    }
//}
//
//- (void)unregisterAutofocusObserver:(AVCaptureDevice*) device
//{
//    if (device.isSubjectAreaChangeMonitoringEnabled == NO) {
//        return;
//    }
//    if ([device lockForConfiguration:NULL] == YES ) {
//        device.subjectAreaChangeMonitoringEnabled = NO;
//        [[NSNotificationCenter defaultCenter] removeObserver:self name:AVCaptureDeviceSubjectAreaDidChangeNotification object:device];
//        [device unlockForConfiguration];
//    }
//}
//
//- (void)AutofocusDelegate:(NSNotification*) notification {
//    AVCaptureDevice* device = [notification object];
//    if (device.focusMode == AVCaptureFocusModeLocked){
//        if ([device lockForConfiguration:NULL] == YES ) {
//            if (device.isFocusPointOfInterestSupported) {
//                [device setFocusPointOfInterest:CGPointMake(0.5f, 0.5f)];
//            }
//            if (device.isExposurePointOfInterestSupported) {
//                [device setExposurePointOfInterest:CGPointMake(0.5f, 0.5f)];
//            }
//            if ([device isFocusModeSupported:AVCaptureFocusModeContinuousAutoFocus]) {
//                [device setFocusMode:AVCaptureFocusModeContinuousAutoFocus];
//            }
//            [device unlockForConfiguration];
//        }
//    }
//}

@end



extern "C"
{
AVCaptureDevice* getDevice(const char* cameraId)
{
    AVCaptureDevice* targetDevice = nil;
    for (AVCaptureDevice* device in [AVCaptureDevice devicesWithMediaType:AVMediaTypeVideo]) {
        if (!strcmp([device.localizedName UTF8String], cameraId)) {
            targetDevice = device;
            break;
        }
    }
    
    return targetDevice;
}
    
void TCamCaptureController_GetDevices(void* udata)
{
    TCam* tcam = (TCam *) udata;
    
    for (AVCaptureDevice* device in [AVCaptureDevice devicesWithMediaType:AVMediaTypeVideo]) {
		int frontFacing = device.position == AVCaptureDevicePositionFront ? 1 : 0;
        tcam->OnCaptureDevice([device.localizedName UTF8String], frontFacing);
	}
}

void* TCamCaptureController_SetDevice(const char* deviceId, void* udata)
{
    AVCaptureDevice* targetDevice = getDevice(deviceId);
    if (targetDevice != nil) {
        TCamCaptureController* controller = [TCamCaptureController alloc];
        [controller setUserData:udata];
        if ([controller init:targetDevice]) {
            return (__bridge_retained void*)controller;
        }
    }
    
    return 0;
}

void TCamCaptureController_SetPreviewResolution(void* captureController, int width, int height)
{
    [(__bridge TCamCaptureController*)captureController setPreviewResolution:width height:height];
}

void TCamCaptureController_GetPreviewResolution(void* captureController, int* width, int* height)
{
    [(__bridge TCamCaptureController*)captureController getPreviewResolution:width height:height];
}

void TCamCaptureController_SetPreviewFPS(void* captureController, int fps)
{
    [(__bridge TCamCaptureController*)captureController setPreviewFPS:fps];
}

int TCamCaptureController_GetPreviewFPS(void* captureController)
{
    return [(__bridge TCamCaptureController*)captureController getPreviewFPS];
}

void TCamCaptureController_SetCaptureResolution(void* captureController, int width, int height)
{
    [(__bridge TCamCaptureController*)captureController setCaptureResolution:width height:height];
}

void TCamCaptureController_GetCaptureResolution(void* captureController, int* width, int* height)
{
    [(__bridge TCamCaptureController*)captureController getCaptureResolution:width height:height];
}
    
float TCamCaptureController_GetMinExposure(void* captureController)
{
    return [(__bridge TCamCaptureController*)captureController getMinExposure];
}

float TCamCaptureController_GetMaxExposure(void* captureController)
{
    return [(__bridge TCamCaptureController*)captureController getMaxExposure];
}

float TCamCaptureController_SetExposure(void* captureController, float exposure)
{
    return [(__bridge TCamCaptureController*)captureController setExposure:exposure];
}

bool TCamCaptureController_SetExposureMode(void* captureController, int mode)
{
    return [(__bridge TCamCaptureController*)captureController setExposureMode:mode];
}

bool TCamCaptureController_SetFlashMode(void* captureController, int mode)
{
    return [(__bridge TCamCaptureController*)captureController setFlashMode:mode];
}

bool TCamCaptureController_SetFocusMode(void* captureController, int mode)
{
    return [(__bridge TCamCaptureController*)captureController setFocusMode:mode];
}
    
void TCamCaptureController_SetPreview(void* captureController, int width, int height, int fps)
{
    TCamCaptureController* controller = (__bridge TCamCaptureController*)captureController;
    [controller setPreviewResolution:width height:height];
    [controller setPreviewFPS:fps];
    
    NSDictionary* outputSettings = [controller.captureOutput videoSettings];
    long outputWidth  = [[outputSettings objectForKey:@"Width"]  longValue];
    long outputHeight = [[outputSettings objectForKey:@"Height"] longValue];
    int outputFps = controller.captureDevice.activeVideoMaxFrameDuration.timescale / controller.captureDevice.activeVideoMaxFrameDuration.value;
    
    TCam* tcam = (TCam *) [controller getUserData];
    tcam->OnCaptureInit((int)outputWidth, (int)outputHeight, outputFps);
}

void TCamCaptureController_Start(void* captureController)
{
	[(__bridge TCamCaptureController*)captureController start];
}

void TCamCaptureController_Pause(void* captureController)
{
	[(__bridge TCamCaptureController*)captureController pause];
}

void TCamCaptureController_Stop(void* captureController)
{
	[(__bridge TCamCaptureController*)captureController stop];
}

void TCamCaptureController_Capture(void* captureController)
{
    [(__bridge TCamCaptureController*)captureController capture];
}

int TCamCaptureController_VideoRotationDeg(void* captureController)
{
    TCamCaptureController* controller = (__bridge TCamCaptureController*)captureController;
    
	// all cams are landscape.
	switch (UnityCurrentOrientation())
	{
		case portrait:
            return 90;
		case portraitUpsideDown:
            return 270;
		case landscapeLeft:
            //return controller.captureDevice.position == AVCaptureDevicePositionFront ? 180 : 0;
            return 0;
		case landscapeRight:
            //return controller.captureDevice.position == AVCaptureDevicePositionFront ? 0 : 180;
            return 180;
		default:
            assert(false && "bad orientation returned from UnityCurrentOrientation()");	break;
	}
	return 0;
}
    
bool TCamCaptureController_IsFrontFacing(void* captureController)
{
    TCamCaptureController* controller = (__bridge TCamCaptureController*)captureController;
    return controller.captureDevice.position == AVCaptureDevicePositionFront;
}

} // extern "C"