using System;
using UnityEngine;

public class BillbordUI : MonoBehaviour
{
    private Transform mainCameraTransform;
    private Transform thisTransform;
    [SerializeField] private bool FreezeHorizontalRotation = false;

    private void LateUpdate()
    {
        if (mainCameraTransform == null)
        {
            mainCameraTransform = Camera.main.transform;
        }

        if (thisTransform == null)
        {
            thisTransform = transform;
        }

        if (!FreezeHorizontalRotation)
        {
            {
                thisTransform.LookAt(mainCameraTransform.position);
            }
        }
        else
        {
            Vector3 positionToLookAT = mainCameraTransform.position + thisTransform.position;
            thisTransform.LookAt(-positionToLookAT);
        }
        
    }
}
