using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MessageUIController : MonoBehaviour
{
    private const string PLAY_AGAIN_BUTTON_NAME = "PlayAgainButton";
    private const string MESSAGE_LABEL_NAME = "MessageLabel";

    private UIDocument uiDocument;
    private Button playAgainButton;
    private Label messageLabel;

    public event Action PlayAgainButtonClicked;

    private void Awake()
    {
        SetComponents();
    }

    private void OnEnable()
    {
        SetWindowFields();
    }

    private void OnDisable()
    {
        playAgainButton.UnregisterCallback<ClickEvent>(PlayAgainButton_Click);
    }

    public void SetMessage(string message)
    {
        messageLabel.text = message;
    }

    private void SetComponents()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void SetWindowFields()
    {
        playAgainButton = uiDocument.rootVisualElement.Q<Button>(PLAY_AGAIN_BUTTON_NAME);
        playAgainButton.RegisterCallback<ClickEvent>(PlayAgainButton_Click);

        messageLabel = uiDocument.rootVisualElement.Q<Label>(MESSAGE_LABEL_NAME);
    }

    private void PlayAgainButton_Click(ClickEvent clickEvent)
    {
        var handler = PlayAgainButtonClicked;
        handler?.Invoke();
    }
}
