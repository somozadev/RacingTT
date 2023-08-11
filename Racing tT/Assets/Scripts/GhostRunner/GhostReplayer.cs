using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace GhostRunner
{
    public class GhostReplayer : MonoBehaviour
    {
        [SerializeField] private string path;
        [SerializeField] private Transform target;
        [SerializeField] private Ghost ghost;
        private float timerValue;
        private int timestamp0;
        private int timestamp1;

        [SerializeField] private bool replayAll = false;
        [SerializeField] private List<GhostData> datas;
        [SerializeField] private List<Transform> ghostCars;
        [SerializeField] private string[] files;

        private void Awake()
        {
            path = Application.persistentDataPath + "/";
            ghost.isReplaying = false;
        }


        public void StartReplaying()
        {
            ReplayAllSavedFiles();
            if (ghost.ghostData.IsEmpty()) return;
            ghost.isReplaying = true;
            timerValue = 0f;
        }

        private void ReplayAllSavedFiles()
        {
            files = Directory.GetFiles(path, "GhostSave_*");
            foreach (var file in files)
            {
                var ghostCar = Instantiate(target, transform.position, quaternion.identity, transform);
                ghostCars.Add(ghostCar);
                ghostCar.gameObject.SetActive(true);
                datas.Add(GhostData.Deserialize(file));
                replayAll = true;
                ghost.isReplaying = true;
            }
        }

        public void StopReplaying()
        {
            ghost.isReplaying = false;
            timerValue = 0f;
            StopReplayAllSavedFiles();
        }

        private void StopReplayAllSavedFiles()
        {
            replayAll = false;
            foreach (var car in ghostCars)
                Destroy(car.gameObject);
            ghostCars.Clear();
            datas.Clear();
        }

        private void Update()
        {
            timerValue += Time.deltaTime;
            if (!ghost.isReplaying) return;

            if (replayAll)
            {
                for (int i = 0; i < datas.Count; i++)
                {
                    GetIndex(datas[i]);
                    SetTransform(datas[i], ghostCars[i]);
                }
            }
        }

        private void GetIndex(GhostData ghost)
        {
            var timestamp = ghost.timestamp;
            if (timestamp.Count <= 0) return;

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

        private void SetTransform(GhostData ghost, Transform target)
        {
            var timestamp = ghost.timestamp;
            if (timestamp.Count <= 0) return;
            var pos = ghost.pos;
            var rot = ghost.rot;
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


        private void OnApplicationQuit()
        {
            ghost.Clear(path);
            foreach (var file in files)
                File.Delete(file);
        }
    }
}