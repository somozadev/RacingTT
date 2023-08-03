using UnityEngine;

public class RoadEnd : CarTriggerDetection
{
    [SerializeField] private TimeController _timeController;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        _timeController.StopTimer();
    }
}