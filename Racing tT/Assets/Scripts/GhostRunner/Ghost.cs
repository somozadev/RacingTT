using System;
using UnityEngine;
using System.Collections.Generic;

namespace GhostRunner
{
    [CreateAssetMenu(fileName = "New Ghost")]
    public class Ghost : ScriptableObject
    {
        public bool isRecording;
        public bool isReplaying;
        public float recordFrequency = 2f;

        public GhostData ghostData;

        public List<float> Timestamp => ghostData.timestamp;
        public List<Vector3> Pos => ghostData.pos;
        public List<Quaternion> Rot => ghostData.rot;

        public void ResetCurrent()
        {
            Timestamp.Clear();
            Pos.Clear();
            Rot.Clear();
        }

        public void Clear(string path)
        {
            isRecording = false;
            isRecording = false;
            ghostData.Serialize(path + $"lastReplaySaved{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.sghost");
            ghostData = new GhostData();
        }
    }
}