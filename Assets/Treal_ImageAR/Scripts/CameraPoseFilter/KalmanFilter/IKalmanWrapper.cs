using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalman
{
    public interface IKalmanWrapper : System.IDisposable
    {
		Matrix4x4 Update(Matrix4x4 current);
    }
}