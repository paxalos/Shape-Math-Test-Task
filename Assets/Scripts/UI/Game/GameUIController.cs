using System;
using GameLogic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameUI
{
    [RequireComponent(typeof(UIDocument))]
    public class GameUIController : MonoBehaviour
    {
        private const string RESTART_BUTTON_NAME = "RestartButton";

        private UIDocument uiDocument;
        private Button restartButton;
        private VisualElement[] shapePoints;

        public event Action RestartButtonClicked;

        private void Awake()
        {
            SetComponents();
            SetButtonEvents();
        }

        private void SetComponents()
        {
            uiDocument = GetComponent<UIDocument>();
            restartButton = uiDocument.rootVisualElement.Q<Button>(RESTART_BUTTON_NAME);
        }

        private void SetButtonEvents()
        {
            restartButton.RegisterCallback<ClickEvent>(RestartButton_Click);
        }

        private void RestartButton_Click(ClickEvent clickEvent)
        {
            var handler = RestartButtonClicked;
            handler?.Invoke();
        }
    }
}