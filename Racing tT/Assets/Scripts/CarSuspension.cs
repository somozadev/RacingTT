using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CarSuspension : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private List<Transform> suspensionTargets;
    [SerializeField] private float forceAmount;
    [SerializeField] private float suspensionLength;
    [SerializeField] private float turnImpulsePower;
    [SerializeField] private List<Transform> tireTransforms;

    public bool isGrounded { get; private set; }

    private void FixedUpdate()
    {
        List<float> list = new List<float>();
        isGrounded = false;
        foreach (Transform suspensionTarget in suspensionTargets)
        {
            if (Physics.Raycast(suspensionTarget.position, -suspensionTarget.up, out var hitInfo, suspensionLength))
            {
                isGrounded = true;
                Debug.DrawLine(suspensionTarget.position, hitInfo.point, Color.green);
                float num = Vector3.Distance(suspensionTarget.position, hitInfo.point);
                float num2 = (suspensionLength - num) / suspensionLength;
                rb.AddForceAtPosition(suspensionTarget.up * forceAmount * num2, suspensionTarget.position,
                    ForceMode.Acceleration);
                list.Add(num);
            }
            else
            {
                Debug.DrawLine(suspensionTarget.position,
                    suspensionTarget.position + -suspensionTarget.up * suspensionLength, Color.red);
                list.Add(suspensionLength);
            }
        }

        UpdateTireYOffset(list);
    }

    private void UpdateTireYOffset(List<float> offsets)
    {
        for (int i = 0; i < tireTransforms.Count; i++)
        {
            tireTransforms[i].localPosition = new Vector3(tireTransforms[i].localPosition.x, 0f - offsets[i],
                tireTransforms[i].localPosition.z);
        }
    }
}