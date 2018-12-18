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

using UnityEngine;

public class DefaultTrackableChangeHandler : MonoBehaviour , ITrakcingStatusChange {
   
    public void Found()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }
    }

    public void Lost()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }
    }

}
