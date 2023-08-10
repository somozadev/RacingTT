using System;
using System.Collections.Generic;
using UnityEngine;


namespace GhostRunner
{
    [Serializable]
    public struct GhostData
    {
        public List<float> timestamp;
        public List<Vector3> pos;
        public List<Quaternion> rot;

        public bool IsEmpty()
        {
            return  timestamp.Count == 0;
        }

        public void SetGhostData(GhostData ghostData)
        {
            timestamp = new List<float>(ghostData.timestamp);
            pos = new List<Vector3>(ghostData.pos);
            rot = new List<Quaternion>(ghostData.rot);
        }
    }
}