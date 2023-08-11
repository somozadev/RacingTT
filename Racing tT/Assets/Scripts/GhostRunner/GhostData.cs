using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            return timestamp.Count == 0;
        }

        public void SetGhostData(GhostData ghostData)
        {
            timestamp = new List<float>(ghostData.timestamp);
            pos = new List<Vector3>(ghostData.pos);
            rot = new List<Quaternion>(ghostData.rot);
        }


        public void Serialize(string filePath)
        {
            string json = JsonUtility.ToJson(this);
            byte[] encryptedData = XorEncrypt(Encoding.UTF8.GetBytes(json));
            File.WriteAllBytes(filePath, encryptedData);
        }

        public static GhostData Deserialize(string filePath)
        {
            byte[] encryptedData = File.ReadAllBytes(filePath);
            byte[] decryptedData = XorDecrypt(encryptedData);
            string json = Encoding.UTF8.GetString(decryptedData);
            return JsonUtility.FromJson<GhostData>(json);
        }

        private static byte[] XorEncrypt(byte[] data)
        {
            byte xorKey = 0x7F;
            byte[] encryptedData = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                encryptedData[i] = (byte)(data[i] ^ xorKey);
            }

            return encryptedData;
        }

        private static byte[] XorDecrypt(byte[] encryptedData)
        {
            return XorEncrypt(encryptedData);
        }
    }
}