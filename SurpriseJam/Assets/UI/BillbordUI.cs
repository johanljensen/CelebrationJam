using System;
using UnityEngine;

public class BillbordUI : MonoBehaviour
{
    private Transform mainCameraTransform;
    private Transform thisTransform;

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
        thisTransform.LookAt(mainCameraTransform.position);
    }
}
