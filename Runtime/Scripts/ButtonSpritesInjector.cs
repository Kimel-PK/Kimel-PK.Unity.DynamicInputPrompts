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
		[SerializeField] private string prefix;
		[SerializeField] private string suffix;
		[SerializeField] private TMP_Text textMeshProText;

		private string _originalText;

		private void Awake () {
			textMeshProText ??= GetComponent<TMP_Text>();
			_originalText = textMeshProText.text;
		}

		private void Start() {
			InjectButtonSprites(_originalText);
			ButtonSpritesManager.OnInputDeviceChanged += ButtonSpritesManager_InputDeviceChanged;
		}

		private void OnDestroy() {
			ButtonSpritesManager.OnInputDeviceChanged -= ButtonSpritesManager_InputDeviceChanged;
		}

		private void InjectButtonSprites() {
			InjectButtonSprites(_originalText);
		}
		
		public void InjectButtonSprites(string text) {
			_originalText = text;
			textMeshProText.text = text;
			if (string.IsNullOrEmpty(text))
				return;

			string actionName = ExtractActionName(textMeshProText.text);
			while (!string.IsNullOrEmpty(actionName)) {
				InputAction inputAction = ButtonSpritesManager.GetInputAction(actionName);
				string icons = HidePrompts ? "" : GetIcons(inputAction, ButtonSpritesManager.ActiveInputDeviceNames);
				textMeshProText.text = textMeshProText.text.Replace($"<InputAction=\"{actionName}\">", icons);
				actionName = ExtractActionName(textMeshProText.text);
			}
			
			if (disableObjectIfNoPrompts)
				gameObject.SetActive(textMeshProText.text != _originalText);
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

		private string GetIcons(InputAction inputAction, List<(string, string)> activeDeviceNames) {
			if (inputAction == null)
				return "";
			string icons = prefix;
			foreach (InputBinding inputActionBinding in inputAction.bindings) {
				Match match = Regex.Match(inputActionBinding.ToString(), @".*?\/(.*?)(?=\[|$)");

				if (!match.Success)
					continue;
				
				string buttonName = match.Groups[1].Value;
				
				// sprite name = Device_buttonName[/direction]
				foreach ((string, string) activeDeviceName in activeDeviceNames) {
					if ($"<{activeDeviceName.Item2}>/{buttonName}" != inputActionBinding.effectivePath)
						continue;
					
					icons += $"<sprite name=\"{activeDeviceName.Item1}_{buttonName}\">";
				}
			}
			return inputAction.bindings.Count == 0 ? "" : $"{icons}{suffix}";
		}
	}
}