using System.Collections.Generic;
using UnityEngine;

namespace ARVORE.TimeTravel
{
    public class TimeBody : MonoBehaviour
    {
        public List<TimePoint> TimePoints { get; private set; } = new List<TimePoint>();
        
        [HideInInspector] public bool isRewinding;


        protected virtual void Awake()
        {
            if (GameRecorder.Instance != null) GameRecorder.Instance.AddTimeBody(this);
        }

        protected virtual void OnDestroy()
        {
            if (GameRecorder.Instance != null) GameRecorder.Instance.RemoveTimeBody(this);
        }

        public virtual void Rewind(TimePoint timePoint)
        {
            transform.SetPositionAndRotation(timePoint.position, timePoint.rotation);
            gameObject.SetActive(timePoint.activeSelf);
        }

        public virtual void Record() => TimePoints.Insert(0, new TimePoint(transform.position, transform.rotation, gameObject.activeSelf));
    }
}
