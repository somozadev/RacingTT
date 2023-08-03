using UnityEngine;

public class RoadStart : CarTriggerDetection
{
    [SerializeField] private TimeController _timeController;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        _timeController.ResetTimer();
        _timeController.StartTimer();
    }
}