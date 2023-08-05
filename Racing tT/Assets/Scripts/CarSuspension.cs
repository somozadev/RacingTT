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
    [SerializeField] private List<Transform> tireTransforms;
    public bool isGrounded { get; private set; }
    [SerializeField] private bool[] individualGrounded;
    public bool[] IndividualGrounded => individualGrounded;

    private void Awake()
    {
        individualGrounded = new bool[4];
    }

    private void FixedUpdate()
    {
        List<float> list = new List<float>();
        isGrounded = false;
        int i = 0;
        foreach (Transform suspensionTarget in suspensionTargets)
        {
            if (Physics.Raycast(suspensionTarget.position, -suspensionTarget.up, out var hitInfo, suspensionLength))
            {
                individualGrounded[i] = true;
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
                individualGrounded[i] = false;
                Debug.DrawLine(suspensionTarget.position,
                    suspensionTarget.position + -suspensionTarget.up * suspensionLength, Color.red);
                list.Add(suspensionLength);
            }

            i++;
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