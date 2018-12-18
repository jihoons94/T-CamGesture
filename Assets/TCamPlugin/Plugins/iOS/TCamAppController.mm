#import <UIKit/UIKit.h>
#import "UnityAppController.h"

//extern "C" void UnitySetGraphicsDevice(void* device, int deviceType, int eventType);
//extern "C" void UnityRenderEvent(int marker);
extern "C" void	UnityPluginLoad(IUnityInterfaces* unityInterfaces);
extern "C" void UnityPluginUnload();

@interface TCamAppController : UnityAppController
{
}
- (void)shouldAttachRenderDelegate;
@end

@implementation TCamAppController

- (void)shouldAttachRenderDelegate;
{
    //UnityRegisterRenderingPlugin(&UnitySetGraphicsDevice, &UnityRenderEvent);
    UnityRegisterRenderingPluginV5(&UnityPluginLoad, &UnityPluginUnload);
}
@end


IMPL_APP_CONTROLLER_SUBCLASS(TCamAppController)

