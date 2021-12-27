using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothing = 0.12f;


    // Start is called before the first frame update
    private void Awake()
    {
        targetPosition = FindObjectOfType<PlayerController>().transform;
    }

    void LateUpdate()
    {
        CameraTransforms();
    }

    //Setting Camera target to follow to prevent camera rotation during flight preventing disorientation
    private void CameraTransforms()
    {
        Vector3 desiredPosition = targetPosition.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothing);
        transform.position = smoothPosition;

        Quaternion desiredRotation = Quaternion.Euler(0, targetPosition.transform.eulerAngles.y, 0);
        transform.rotation = desiredRotation;
    }
}
