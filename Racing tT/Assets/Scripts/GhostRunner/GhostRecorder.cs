using System;
using UnityEngine;

namespace GhostRunner
{
    public class GhostRecorder : MonoBehaviour
    {
        [SerializeField] private string path;
        [SerializeField] private Transform target;
        [SerializeField] private Ghost ghost;
        private float timer;
        private float timerValue;

        private void Awake()
        {
            path = Application.persistentDataPath + "/";
            ghost.isRecording = false;
        }

        public void StartRecording()
        {
            ghost.isRecording = true;
            ghost.ResetCurrent();
            timerValue = 0f;
            timer = 0f;
        }

        public void StopRecording()
        {
            ghost.ghostData.Serialize(path + $"GhostSave_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.ghost");
            ghost.isRecording = false;
            timerValue = 0f;
            timer = 0f;
        }

        private void Update()
        {
            timerValue += Time.deltaTime;
            if (!ghost.isRecording) return;
            timer += Time.deltaTime;
            if (!ghost.isRecording || !(timer >= 1 / ghost.recordFrequency)) return;
            ghost.Timestamp.Add(timerValue);
            ghost.Pos.Add(target.position);
            ghost.Rot.Add(target.rotation);
            timer = 0f;
        }
    }
}