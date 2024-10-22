using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KimelPK.DynamicInputPrompts {
	public class ButtonSpritesInjector : MonoBehaviour {
		
		[SerializeField] private TMP_Text tmpText;

		private string _originalText;

		private void Awake () {
			tmpText ??= GetComponent<TMP_Text>();
		}

		private void Start() {
			_originalText = tmpText.text;
			InjectButtonSprites(_originalText);
			ButtonSpritesManager.OnInputDeviceChanged += ButtonSpritesManager_InputDeviceChanged;
		}

		private void OnDestroy() {
			ButtonSpritesManager.OnInputDeviceChanged -= ButtonSpritesManager_InputDeviceChanged;
		}

		public void InjectButtonSprites(string text) {
			_originalText = text;
			InjectButtonSprites();
		}

		private void InjectButtonSprites() {
			tmpText.text = _originalText;
			if (string.IsNullOrEmpty(_originalText))
				return;

			string actionName = ExtractActionName(_originalText);
			while (!string.IsNullOrEmpty(actionName)) {
				InputAction inputAction = ButtonSpritesManager.GetInputAction(actionName);
				string icons = GetIcons(inputAction, "Keyboard&Mouse");
				tmpText.text = tmpText.text.Replace($"<InputAction=\"{actionName}\">", icons);
				actionName = ExtractActionName(tmpText.text);
			}
		}
		
		private void ButtonSpritesManager_InputDeviceChanged() {
			InjectButtonSprites();
		}
		
		private static string ExtractActionName(string text) {
			Match match = Regex.Match(text, "<InputAction=\"([^>]+)\">");
			if (match.Success)
				return match.Groups[1].Value;

			return null;
		}

		private static string GetIcons(InputAction inputAction, string currentDeviceName) {
			string icons = string.Empty;
			foreach (InputBinding inputActionBinding in inputAction.bindings) {
				Match match = Regex.Match(inputActionBinding.ToString(), @".*?\/(.*)\[;(.*)\]");

				if (!match.Success)
					continue;
				
				string buttonName = match.Groups[1].Value;
				
				// sprite name = Device_buttonName[_direction]
				icons += $"<sprite name=\"{currentDeviceName}_{buttonName}\">";
			}
			return icons;
		}
	}
}