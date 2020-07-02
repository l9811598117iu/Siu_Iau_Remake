using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //third person controller
    public float mouseSensitivity = 10f;
    public Transform target;
    public float dstFormTarget = 4f;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public float distence;

    public static CameraController instance = null;

    float yaw;
    float pitch;

    [SerializeField] LayerMask cameraLayer;

    void Awake()
    {
        instance = this;
    }

    //third person controller
    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * dstFormTarget;

        RaycastHit hit;

        if (Physics.Linecast(target.position + Vector3.up, transform.position + new Vector3(0, -0.5f, 0), out hit, cameraLayer))
        {
            if (hit.transform.tag != "Player" || hit.transform.tag != "Boss")
            {
                transform.position = hit.point + new Vector3(0, 0.5f, 0);
            }
        }
        else if (Physics.Linecast(target.position + Vector3.up, transform.position, out hit, cameraLayer))
        {
            if (hit.transform.tag != "Player" || hit.transform.tag != "Boss")
            {
                transform.position = hit.point + new Vector3(0, 0.5f, 0);
            }
        }


    }

    //CameraShake
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }


}
