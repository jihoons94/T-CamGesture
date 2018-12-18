#ifndef BUGFENDER_UNITY
#define BUGFENDER_UNITY
#import <Foundation/Foundation.h>
#import <BugfenderSDK/BugfenderSDK.h>

extern "C"
{
    void BugfenderInit(const char* key)
    {
        [Bugfender activateLogger:[NSString stringWithUTF8String:key]];
    }

    void BugfenderLog(const char* msg)
    {
        BFLog(@"%@", [NSString stringWithUTF8String: msg]);
    }

    void BugfenderWarn(const char* msg)
    {
        BFLogWarn(@"%@", [NSString stringWithUTF8String: msg]);
    }

    void BugfenderErr(const char* msg)
    {
        BFLogErr(@"%@", [NSString stringWithUTF8String: msg]);
    }
}

#endif /* BUGFENDER_UNITY */