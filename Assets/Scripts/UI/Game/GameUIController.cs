using System;
using System.Linq;
using GameModels;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameUI
{
    [RequireComponent(typeof(UIDocument))]
    public class GameUIController : MonoBehaviour
    {
        private const string RESTART_BUTTON_NAME = "RestartButton";
        private const string BAR_NAME = "Bar";
        private const int BAR_ELEMENTS_POINTS_COUNT = 7;

        private UIDocument uiDocument;
        private Button restartButton;
        private SelectedElementUIModel[] elementPointModels;

        public event Action RestartButtonClicked;

        private void Awake()
        {
            SetComponents();
            SetButtonEvents();
        }

        public void UpdateElementModel(SelectedElementUIModel selectedElementUIModel, 
                                       int elementIndex)
        {
            var elementPointModel = elementPointModels[elementIndex];
            elementPointModel.SetValues(selectedElementUIModel);
        }

        public void ClearElementModels(Range elementsRange)
        {
            var modelsForClear = elementPointModels[elementsRange];
            for (int i = 0; i < modelsForClear.Length; i++)
                modelsForClear[i].Clear();
        }

        private void SetComponents()
        {
            uiDocument = GetComponent<UIDocument>();

            restartButton = uiDocument.rootVisualElement.Q<Button>(RESTART_BUTTON_NAME);

            elementPointModels = new SelectedElementUIModel[BAR_ELEMENTS_POINTS_COUNT];
            var barElement = uiDocument.rootVisualElement.Q<VisualElement>(BAR_NAME);
            var elementPoints = barElement.Children().ToList();
            for (int i = 0; i < BAR_ELEMENTS_POINTS_COUNT; i++)
            {
                var shapePointModel = new SelectedElementUIModel();
                elementPointModels[i] = shapePointModel;
                
                var elementPoint = elementPoints[i];
                SetDataSourceInAllChildren(elementPoint, shapePointModel);
            }
        }

        private void SetDataSourceInAllChildren(VisualElement visualElement, object dataSource)
        {
            foreach(var child in visualElement.Children())
            {
                child.dataSource = dataSource;
                SetDataSourceInAllChildren(child, dataSource);
            }
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