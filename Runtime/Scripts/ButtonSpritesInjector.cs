using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KimelPK.DynamicInputPrompts {
	public class ButtonSpritesInjector : MonoBehaviour {
		
		public bool HidePrompts {
			get => hidePrompts;
			set {
				hidePrompts = value;
				InjectButtonSprites();
			}
		}
		
		[SerializeField] private bool hidePrompts;
		[SerializeField] private bool disableObjectIfNoPrompts;
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

		private void InjectButtonSprites() {
			tmpText.text = _originalText;
			if (string.IsNullOrEmpty(_originalText))
				return;

			string actionName = ExtractActionName(tmpText.text);
			while (!string.IsNullOrEmpty(actionName)) {
				InputAction inputAction = ButtonSpritesManager.GetInputAction(actionName);
				string icons = HidePrompts ? "" : GetIcons(inputAction, ButtonSpritesManager.ActiveInputDeviceNames);
				switch (disableObjectIfNoPrompts) {
					case true when icons == "" :
						gameObject.SetActive(false);
						return;
					case true when icons != "" :
						gameObject.SetActive(true);
						break;
				}
				tmpText.text = tmpText.text.Replace($"<InputAction=\"{actionName}\">", icons);
				actionName = ExtractActionName(tmpText.text);
			}
		}
		
		public void InjectButtonSprites(string text) {
			_originalText = text;
			tmpText.text = text;
			if (string.IsNullOrEmpty(text))
				return;

			string actionName = ExtractActionName(tmpText.text);
			while (!string.IsNullOrEmpty(actionName)) {
				InputAction inputAction = ButtonSpritesManager.GetInputAction(actionName);
				string icons = GetIcons(inputAction, ButtonSpritesManager.ActiveInputDeviceNames);
				tmpText.text = tmpText.text.Replace($"<InputAction=\"{actionName}\">", icons);
				actionName = ExtractActionName(tmpText.text);
			}
		}
		
		private void ButtonSpritesManager_InputDeviceChanged() {
			InjectButtonSprites(_originalText);
		}
		
		private static string ExtractActionName(string text) {
			Match match = Regex.Match(text, "<InputAction=\"([^>]+)\">");
			if (match.Success)
				return match.Groups[1].Value;

			return null;
		}

		private static string GetIcons(InputAction inputAction, List<(string, string)> activeDeviceNames) {
			string icons = string.Empty;
			foreach (InputBinding inputActionBinding in inputAction.bindings) {
				Match match = Regex.Match(inputActionBinding.ToString(), @".*?\/(.*)\[");

				if (!match.Success)
					continue;
				
				string buttonName = match.Groups[1].Value;
				
				// sprite name = Device_buttonName[_direction]
				foreach ((string, string) activeDeviceName in activeDeviceNames) {
					if ($"<{activeDeviceName.Item2}>/{buttonName}" != inputActionBinding.effectivePath)
						continue;
					
					icons += $"<sprite name=\"{activeDeviceName.Item1}_{buttonName}\">";
				}
			}
			return icons;
		}
	}
}