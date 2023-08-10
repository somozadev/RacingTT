using System;
using UnityEngine;

namespace GhostRunner
{
    public class GhostManager : MonoBehaviour
    {
        [SerializeField] private VoidEvent startRecordingEvent;
        [SerializeField] private VoidEvent startReplayingEvent;
        [SerializeField] private VoidEvent stopRecordingEvent;
        [SerializeField] private VoidEvent stopReplayingEvent;


        public void StartRun()
        {
            startReplayingEvent.Raise();
            startRecordingEvent.Raise();
        }

        public void EndRun()
        {
            stopReplayingEvent.Raise();
            stopRecordingEvent.Raise();
        }
    }
}