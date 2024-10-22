using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KimelPK.DynamicInputPrompts {
	[RequireComponent(typeof(PlayerInput))]
	public class ButtonSpritesManager : MonoBehaviour {

		public static ButtonSpritesManager Instance { get; private set; }
        
		public static Action OnInputDeviceChanged = delegate {};
		
		[field: SerializeField] public PlayerInput PlayerInput { get; private set; }
		[field: SerializeField] public List<InputDeviceDefinition> SupportedInputDevices { get; private set; }
		[field: SerializeField] public bool DontDestroyOnSceneChange { get; set; } = true;
		
		[SerializeField] private List<ButtonPromptsGroup> activePromptGroups = new();

		private void Awake () {
			if (Instance) {
				Destroy(gameObject);
				return;
			}
			Instance = this;
			if (DontDestroyOnSceneChange)
				DontDestroyOnLoad(gameObject);
		}

		private void Start () {
			foreach (InputDeviceDefinition inputDeviceDefinition in SupportedInputDevices)
				TMP_Settings.defaultSpriteAsset.fallbackSpriteAssets.Add(inputDeviceDefinition.ButtonSpritesAsset);
		}

		private void OnDestroy () {
			foreach (InputDeviceDefinition inputDeviceDefinition in SupportedInputDevices)
				TMP_Settings.defaultSpriteAsset.fallbackSpriteAssets.Remove(inputDeviceDefinition.ButtonSpritesAsset);
		}

		public static InputAction GetInputAction(string actionName) {
			foreach (InputActionMap actionMap in Instance.PlayerInput.actions.actionMaps) {
				InputAction action = actionMap.FindAction(actionName);
				
				if (action != null)
					return action;
			}
			return null;
		}
		
		public void OnControlsChanged(PlayerInput input) {
			CacheCurrentInputDevice();
			OnInputDeviceChanged.Invoke();
		}

		private void CacheCurrentInputDevice() {
			foreach (InputDevice inputDevice in PlayerInput.devices) {
				Debug.Log(inputDevice);
			}
		}
		
		public void ActivatePromptGroup(ButtonPromptsGroup buttonPromptsGroup) {
			if (activePromptGroups.Contains(buttonPromptsGroup))
				return;
			activePromptGroups.Add(buttonPromptsGroup);
			buttonPromptsGroup.Show ();
		}
		
		public void DeactivatePromptGroup(ButtonPromptsGroup buttonPromptsGroup) {
			if (!activePromptGroups.Contains(buttonPromptsGroup))
				return;
			activePromptGroups.Remove(buttonPromptsGroup);
			buttonPromptsGroup.Hide ();
		}
		
		public void SetPromptGroup(ButtonPromptsGroup buttonPromptsGroup) {
			foreach (ButtonPromptsGroup activeGroup in activePromptGroups)
				activeGroup.Hide ();
			
			activePromptGroups.Clear ();
			activePromptGroups.Add (buttonPromptsGroup);
			buttonPromptsGroup.Show ();
		}
		
		public void SetPromptGroups(List<ButtonPromptsGroup> buttonPromptsGroup) {
			foreach (ButtonPromptsGroup activeGroup in activePromptGroups)
				activeGroup.Hide ();
			
			activePromptGroups.Clear ();
			activePromptGroups.AddRange(buttonPromptsGroup);
			foreach (ButtonPromptsGroup group in activePromptGroups)
				group.Show ();
		}
	}
}