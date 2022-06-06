using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UsernameInputScreen : CanvasScreen
    {
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private Button submitButton;
        [SerializeField] private TMP_Text errorText;

        public event Action<string> Submitted;

        protected override void Awake()
        {
            base.Awake();
            usernameInput.onSubmit.AddListener(OnSubmit);
            submitButton.onClick.AddListener(OnSubmitButtonClicked);
            errorText.text = "";
        }

        private void OnSubmitButtonClicked()
        {
            OnSubmit(usernameInput.text);
        }

        private void OnSubmit(string username)
        {
            if (username.Length < 3)
            {
                SetError("Username must be at least 3 characters long");
                return;
            }

            Submitted?.Invoke(username);
        }

        public void SetError(string error)
        {
            errorText.text = error;
        }
    }
}