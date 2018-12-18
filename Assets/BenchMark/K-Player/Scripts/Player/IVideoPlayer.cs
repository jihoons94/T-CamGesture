using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVideoPlayer
{

    void OnReady();

    void OnLoad(int result);

    void OnPlay(int result);
    void OnPause(int result);

    void OnResume(int result);
    void OnStop(int result);

    void OnPlayComplete();

    void OnSeek(int result);

    void OnGoFrame(int result);
    void OnUnload(int result);

    float GetCurrentPos();

}