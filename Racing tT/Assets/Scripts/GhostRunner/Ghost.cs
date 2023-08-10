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

        public GhostData ghostDataCurrent;
        public GhostData ghostDataSaved;
        public List<float> TimestampSaved => ghostDataSaved.timestamp;
        public List<Vector3> PosSaved => ghostDataSaved.pos;
        public List<Quaternion> RotSaved => ghostDataSaved.rot;

        public List<float> TimestampCurrent => ghostDataCurrent.timestamp;
        public List<Vector3> PosCurrent => ghostDataCurrent.pos;
        public List<Quaternion> RotCurrent => ghostDataCurrent.rot;

        public void ResetCurrent()
        {
            TimestampCurrent.Clear();
            PosCurrent.Clear();
            RotCurrent.Clear();
        }

        public void Clear()
        {
            isRecording = false;
            isRecording = false;
            ghostDataCurrent = new GhostData();
        }

        public void Serialize()
        {
            //todo
        }

        public void Deserialize()
        {
            //todo
        }
    }
}