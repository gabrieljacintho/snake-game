using ARVORE.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ARVORE.TimeTravel
{
    public class GameRecorder : Singleton<GameRecorder>
    {
        public List<TimeBody> TimeBodies { get; private set; } = new List<TimeBody>();

        public bool IsRewinding => TimeBodies.Exists(timeBody => timeBody.isRewinding);

        public float rewindSpeed = 1f;

        [Space]
        public Audio startRewindAudio;
        public Audio stopRewindAudio;

        private float _targetTime;
        private bool _isRewinding;


        private void FixedUpdate()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.InGame) return;

            if (IsRewinding) Rewind();
            else
            {
                if (_isRewinding) StopRewind();
                Record();
            }
        }

        public void AddTimeBody(TimeBody timeBody) => TimeBodies.Add(timeBody);

        public void RemoveTimeBody(TimeBody timeBody) => TimeBodies.Remove(timeBody);

        public void StartRewind(float time = 0f)
        {
            TimeBodies.ForEach(timeBody => timeBody.isRewinding = true);
            _targetTime = time;
            _isRewinding = true;
            Time.timeScale = rewindSpeed;

            startRewindAudio.Play();
        }

        public void StopRewind()
        {
            if (!IsRewinding) TimeBodies.ForEach(timeBody => timeBody.isRewinding = false);
            _isRewinding = false;
            Time.timeScale = 1f;

            stopRewindAudio.Play();
        }

        private void Rewind()
        {
            foreach (TimeBody timeBody in TimeBodies)
            {
                if (timeBody == null || !timeBody.isRewinding) continue;

                if (timeBody.TimePoints.Count > 0)
                {
                    TimePoint timePoint = timeBody.TimePoints[0];
                    if (timePoint.time >= _targetTime)
                    {
                        timeBody.Rewind(timePoint);
                        timeBody.TimePoints.RemoveAt(0);

                        if (timePoint.time == _targetTime) timeBody.isRewinding = false;
                        continue;
                    }
                }

                Destroy(timeBody.gameObject);
            }
        }

        private void Record() => TimeBodies.ForEach(timeBody => timeBody.Record());

        public void Clear()
        {
            TimeBodies.ForEach(timeBody => Destroy(timeBody.gameObject));
            TimeBodies.Clear();
        }
    }
}
