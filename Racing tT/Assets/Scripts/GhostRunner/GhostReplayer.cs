using UnityEngine;

namespace GhostRunner
{
    public class GhostReplayer : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Ghost ghost;
        private float timerValue;
        private int timestamp0;
        private int timestamp1;

        public void StartReplaying()
        {
            if (ghost.ghostDataSaved.IsEmpty()) return;
            ghost.isReplaying = true;
            timerValue = 0f;
            target.gameObject.SetActive(true);
        }

        public void StopReplaying()
        {
            ghost.isReplaying = false;
            timerValue = 0f;
            target.gameObject.SetActive(false);
        }

        private void Update()
        {
            timerValue += Time.deltaTime;
            if (!ghost.isReplaying) return;
            GetIndex();
            SetTransform();
        }

        private void GetIndex()
        {
            var timestamp = ghost.TimestampSaved;

            for (var i = 0; i < timestamp.Count - 2; i++)
            {
                if (timestamp[i] == timerValue)
                {
                    timestamp0 = i;
                    timestamp1 = i;
                    return;
                }

                if ((!(timestamp[i] < timerValue & timerValue < timestamp[i + 1]))) continue;
                timestamp0 = i;
                timestamp1 = i + 1;
                return;
            }

            timestamp0 = timestamp.Count - 1;
            timestamp1 = timestamp.Count - 1;
        }


        private void SetTransform()
        {
            var timestamp = ghost.TimestampSaved;
            var pos = ghost.PosSaved;
            var rot = ghost.RotSaved;
            var trfRef = target.transform;

            if (timestamp0 == timestamp1)
            {
                trfRef.position = pos[timestamp0];
                trfRef.rotation = rot[timestamp0];
            }
            else
            {
                var interpolationFactor = (timerValue - timestamp[timestamp0]) /
                                          (timestamp[timestamp1] - timestamp[timestamp0]);
                target.position = Vector3.Lerp(pos[timestamp0], pos[timestamp1], interpolationFactor);
                target.rotation = Quaternion.Lerp(rot[timestamp0], rot[timestamp1], interpolationFactor);
            }
        }

        private void OnApplicationQuit() => ghost.Clear();
    }
}