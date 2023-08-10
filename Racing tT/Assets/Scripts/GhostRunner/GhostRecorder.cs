using Unity.VisualScripting;
using UnityEngine;

namespace GhostRunner
{
    public class GhostRecorder : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Ghost ghost;
        private float timer;
        private float timerValue;


        public void StartRecording()
        {
            ghost.isRecording = true;
            ghost.ResetCurrent();
            timerValue = 0f;
            timer = 0f;
        }

        public void StopRecording()
        {
            ghost.ghostDataSaved.SetGhostData(ghost.ghostDataCurrent);
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
            ghost.TimestampCurrent.Add(timerValue);
            ghost.PosCurrent.Add(target.position);
            ghost.RotCurrent.Add(target.rotation);
            timer = 0f;
        }

        private void OnApplicationQuit()=> ghost.Clear();
        
    }
}