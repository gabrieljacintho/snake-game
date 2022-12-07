using ARVORE.TimeTravel;
using System.Collections.Generic;
using UnityEngine;

namespace ARVORE.Core
{
    public enum GameState
    {
        NotStarted,
        InGame,
        Paused,
        Finished
    }

    public class GameManager : Singleton<GameManager>
    {
        public GameState State { get; private set; }
        public int Score { get; private set; }

        public List<GameMatch> GameMatches { get; private set; } = new List<GameMatch>();

        public GameObject gameMatchPrefab;


        protected override void Awake()
        {
            base.Awake();
        }

        private void LateUpdate()
        {
            Cursor.visible = false;

            if (State != GameState.InGame) return;

            if (GameMatches == null || GameMatches.Find(x => x != null && x.State == GameMatchState.InGame) == null)
            {
                GameOver();
            }
        }

        public GameMatch CreateGameMatch()
        {
            if (gameMatchPrefab == null) return null;

            if (Instantiate(gameMatchPrefab).TryGetComponent(out GameMatch gameMatch) && GameMatches != null)
                GameMatches.Add(gameMatch);

            return gameMatch;
        }

        public void SetScore(int value)
        {
            Score = value;
            if (UIManager.Instance != null) UIManager.Instance.SetScoreText(Score.ToString("0000"));
        }

        public void IncreaseScore(int value = 7) => SetScore(Score + value);

        public void DecreaseScore(int value = 7) => SetScore(Score - value);

        public void StartGame()
        {
            State = GameState.InGame;
            if (UIManager.Instance != null) UIManager.Instance.SwitchPanel(Panel.Gameplay);
            if (GameRecorder.Instance != null) GameRecorder.Instance.Clear();
            if (GameMatches != null) GameMatches.ForEach(x => x.StartMatch());
            SetScore(0);
        }

        public void PauseGame()
        {
            State = GameState.Paused;
            if (UIManager.Instance != null) UIManager.Instance.SwitchPanel(Panel.Pause);
        }

        public void ContinueGame()
        {
            State = GameState.InGame;
            if (UIManager.Instance != null) UIManager.Instance.SwitchToLastPanel();
        }

        public void GameOver()
        {
            State = GameState.Finished;
            if (UIManager.Instance != null) UIManager.Instance.SwitchPanel(Panel.GameOver);
        }

        public void BackToHome()
        {
            State = GameState.NotStarted;
            if (UIManager.Instance != null) UIManager.Instance.SwitchPanel(Panel.Home);
            if (GameRecorder.Instance != null) GameRecorder.Instance.Clear();
            if (GameMatches != null)
            {
                GameMatches.ForEach(x => Destroy(x.gameObject));
                GameMatches.Clear();
            }
        }
    }
}
