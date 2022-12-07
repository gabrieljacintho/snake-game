using UnityEngine;

namespace ARVORE.TimeTravel
{
    public class TimePoint
    {
        public readonly float time;
        public readonly Vector3 position;
        public readonly Quaternion rotation;
        public readonly bool activeSelf;


        public TimePoint(Vector3 position, Quaternion rotation, bool activeSelf)
        {
            time = Time.fixedTime;
            this.position = position;
            this.rotation = rotation;
            this.activeSelf = activeSelf;
        }
    }
}
