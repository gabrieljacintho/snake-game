using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ARVORE.Core
{
    public enum Panel
    {
        Home,
        Players,
        TimeRewinding,
        Gameplay,
        Pause,
        GameOver
    }

    public class UIManager : Singleton<UIManager>
    {
        public Panel Panel { get; private set; }
        public Panel LastPanel { get; private set; }

        public GameObject gameplayPanel;
        public GameObject homePanel;
        public GameObject playersPanel;
        public GameObject timeRewindingPanel;
        public GameObject pausePanel;
        public GameObject gameOverPanel;
        public GameObject scorePanel;
        public TextMeshProUGUI scoreText;


        public void SwitchPanel(Panel panel)
        {
            bool gameplayPanelActive = panel == Panel.Gameplay || panel == Panel.Pause || panel == Panel.GameOver || panel == Panel.TimeRewinding;
            if (gameplayPanel != null) gameplayPanel.SetActive(gameplayPanelActive);
            if (homePanel != null) homePanel.SetActive(panel == Panel.Home);
            if (playersPanel != null) playersPanel.SetActive(panel == Panel.Players);
            if (timeRewindingPanel != null) timeRewindingPanel.SetActive(panel == Panel.TimeRewinding);
            if (pausePanel != null) pausePanel.SetActive(panel == Panel.Pause);
            if (gameOverPanel != null) gameOverPanel.SetActive(panel == Panel.GameOver);
            if (scorePanel != null) scorePanel.SetActive(gameplayPanelActive);

            LastPanel = Panel;
            Panel = panel;
        }

        public void SwitchToLastPanel() => SwitchPanel(LastPanel);

        public void SetScoreText(string text)
        {
            if (scoreText != null) scoreText.text = text;
        }
    }
}
