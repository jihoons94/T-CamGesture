using UnityEngine;

public class GyroController : MonoBehaviour
{

    void Start()
    {
        if (Application.isMobilePlatform)
        {
            // enable gyro mode
            Input.gyro.enabled = true;
        }
    }

    void Update()
    {
        if (!SystemInfo.supportsGyroscope || !Application.isMobilePlatform)
        {
            return;
        }

        // prevent gimbal lock
        if (transform.rotation.x <= 0.6f || transform.rotation.x >= -0.6f)
        {
            // rotate by unbiased rotation rate
            transform.Rotate(-Input.gyro.rotationRateUnbiased.x, -Input.gyro.rotationRateUnbiased.y, 0);
            //transform.Rotate(-Input.gyro.rotationRate.x, -Input.gyro.rotationRate.y, 0);
        }

        // clamping X axis in case of gimbal lock
        transform.rotation = new Quaternion(Mathf.Clamp(transform.rotation.x, -0.6f, 0.6f),
                                            transform.rotation.y,
                                            transform.rotation.z,
                                            transform.rotation.w);

        // Fix Z axis as 0 - 로테이션이 되다보면 Z축이 제 멋대로 값이 바뀌는 현상이 있어 Z값을 0으로 고정
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
    }
}
