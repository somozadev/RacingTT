using System;
using UnityEngine;

[System.Serializable]
public class CarStats
{
    public CarStatsType type;

    public float acceleration;
    public float turnForce;
    public float maxSpeed;
    public float maxAngularVelocity;
    public float minAngularVelocity;
    public float grip;

    public float minVelForDrift = 0.5f;
    public float driftAcc;
    public float maxDriftSpeed;
    public float driftTurnForce;
    public float maxDriftAngularVelocity;
    public float minDriftAngularVelocity;
    public float driftGrip;


   [SerializeField]private float initialGrip;
   [SerializeField]private float initialDriftGrip;

    public void SpeedBoost(float speedValue)
    {
        maxSpeed += speedValue;
        acceleration += speedValue/2;
    } 
    public void ResetSpeed(float speedValue)
    {
        maxSpeed -= speedValue;
        acceleration -= speedValue/2;

    } 

    public void SetGrip(float gripValue, float driftGripValue)
    {
        initialGrip = grip;
        initialDriftGrip = driftGrip;
        grip = gripValue;
        driftGrip = driftGripValue;
    }

    public void ResetGrip()
    {
        grip = initialGrip;
        driftGrip = initialDriftGrip;
    }

    public void SetDefaultValues()
    {
        type = CarStatsType.Default;
        acceleration = 50;
        turnForce = 30;
        maxSpeed = 360;
        maxAngularVelocity = 60;
        minAngularVelocity = 15;
        grip = 4;
        minVelForDrift = 0.5f;
        driftAcc = 65;
        maxDriftSpeed = 400;
        driftTurnForce = 30;
        maxDriftAngularVelocity = 70;
        minDriftAngularVelocity = 17;
        driftGrip = 5;
    }

    public void SetAlbertoDefaultValues()
    {
        type = CarStatsType.AlbertoDefault;
        acceleration = 50;
        turnForce = 30;
        maxSpeed = 360;
        maxAngularVelocity = 25;
        minAngularVelocity = 6;
        grip = 4;
        minVelForDrift = 0.5f;
        driftAcc = 65;
        maxDriftSpeed = 400;
        driftTurnForce = 30;
        maxDriftAngularVelocity = 30;
        minDriftAngularVelocity = 8;
        driftGrip = 5;
    }

    public void SaveCarStatsToJson()
    {
        string jsonData = JsonUtility.ToJson(this);

        string filePath = Application.persistentDataPath + "/" + Enum.GetName(typeof(CarStatsType), type) +
                          "_carStats.json";
        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("CarStats serialized and saved to " + filePath);
    }

    public void LoadCarStats()
    {
        string filePath = Application.persistentDataPath + "/" + Enum.GetName(typeof(CarStatsType), type) +
                          "_carStats.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(jsonData, this);
            Debug.Log("CarStats loaded from " + filePath);
        }
        else
        {
            if (type == CarStatsType.Default)
            {
                SetDefaultValues();
                SaveCarStatsToJson();
            }
        }
    }
}