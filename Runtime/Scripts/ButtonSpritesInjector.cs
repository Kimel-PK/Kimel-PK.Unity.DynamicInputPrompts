using System;
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
		
		[field: SerializeField] private string Prefix { get; set; }
		[field: SerializeField] private string Suffix { get; set; }

		[Tooltip("-1 will display every matching binding")]
		[field: SerializeField] public int LimitDisplayedBindings { get; set; } = -1; // TODO move this to <InputAction> tag

		[Tooltip("Empty list will show all bindings")]
		[field: SerializeField] public List<int> VisibleBindingIndices { get; private set; } = new(); // TODO move this to <InputAction> tag
		
		[SerializeField] private bool hidePrompts;
		[SerializeField] private bool disableObjectIfNoPrompts;
		[SerializeField] private TMP_Text textMeshProText;

		private string _originalText;

		private void Awake () {
			if (textMeshProText && string.IsNullOrEmpty (_originalText))
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
			if (!textMeshProText) {
				Debug.LogError("Cannot inject sprites into text because TextMeshProText is not assigned.", this);
				return;
			}
			textMeshProText.text = text;
			if (string.IsNullOrEmpty(text))
				return;

			string iconsCombined = "";

			while (InputActionTag.TryExtract(textMeshProText.text, out InputActionTagData tagData)) {
				InputAction inputAction = ButtonSpritesManager.GetInputAction(tagData.actionName);
				string icons = HidePrompts ? "" : GetIcons(inputAction, ButtonSpritesManager.ActiveInputDeviceNames, tagData.compositeParts);
				iconsCombined += icons;

				textMeshProText.text =
					textMeshProText.text.Replace(tagData.fullTag, icons);
			}

			if (disableObjectIfNoPrompts)
				gameObject.SetActive(!string.IsNullOrEmpty(iconsCombined));
		}
		
		private void ButtonSpritesManager_InputDeviceChanged() {
			InjectButtonSprites(_originalText);
		}
		
		private string GetIcons(InputAction inputAction, List<(string, string)> activeDeviceNames, List<string> compositeParts = null) {
			if (inputAction == null)
				return "";

			string icons = "";
			int matchedBindings = 0;

			bool filterComposite = compositeParts is { Count: > 0 };
			HashSet<string> compositeFilterSet = filterComposite ? new HashSet<string>(compositeParts, StringComparer.OrdinalIgnoreCase) : null;

			foreach (InputBinding inputActionBinding in inputAction.bindings) {
				if (LimitDisplayedBindings != -1 && matchedBindings >= LimitDisplayedBindings)
					break;

				if (inputActionBinding.isComposite)
					continue;

				if (!IsBindingShown(matchedBindings)) {
					matchedBindings++;
					continue;
				}

				if (filterComposite) {
					if (!inputActionBinding.isPartOfComposite || !compositeFilterSet.Contains(inputActionBinding.name))
						continue;
				}

				Match match = Regex.Match(inputActionBinding.ToString(), @".*?\/(.*?)(?=\[|$)");
				if (!match.Success)
					continue;

				string buttonName = match.Groups[1].Value;

				foreach ((string, string) activeDeviceName in activeDeviceNames) {
					if ($"<{activeDeviceName.Item2}>/{buttonName}" != inputActionBinding.effectivePath)
						continue;

					icons += $"<sprite name=\"{activeDeviceName.Item1}_{buttonName}\">";
					matchedBindings++;
				}
			}

			return inputAction.bindings.Count == 0 ? "" : $"{Prefix}{icons}{Suffix}";
		}
		
		private bool IsBindingShown(int index) => VisibleBindingIndices == null || VisibleBindingIndices.Count == 0 || VisibleBindingIndices.Contains(index);
	}
}