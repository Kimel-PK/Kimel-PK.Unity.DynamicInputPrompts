using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KimelPK.DynamicInputPrompts {
	
	public class ButtonSpritesManager : MonoBehaviour {

		public static ButtonSpritesManager Instance { get; private set; }
        
		public static Action OnInputDeviceChanged = delegate {};

		public static List<(string, string)> ActiveInputDeviceNames { get; private set; } = new();
		
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
			foreach (InputDeviceDefinition supportedDevice in SupportedInputDevices)
				TMP_Settings.defaultSpriteAsset.fallbackSpriteAssets.AddRange(supportedDevice.ButtonSpritesAssets);
		}

		private void OnDestroy () {
			foreach (InputDeviceDefinition supportedDevice in SupportedInputDevices) {
				foreach (TMP_SpriteAsset buttonSpritesAsset in supportedDevice.ButtonSpritesAssets)
					TMP_Settings.defaultSpriteAsset.fallbackSpriteAssets.Remove (buttonSpritesAsset);
			}
		}

		public static InputAction GetInputAction(string actionName) {
			foreach (InputActionMap actionMap in Instance.PlayerInput.actions.actionMaps) {
				InputAction action = actionMap.FindAction(actionName);
				
				if (action != null)
					return action;
			}
			return null;
		}
		
		// This method is called by the PlayerInput SendMessages, you can call it manually if you want to update the prompts
		public void OnControlsChanged(PlayerInput input) {
			GetActiveInputDevices();
			OnInputDeviceChanged.Invoke();
		}

		private void GetActiveInputDevices() {
			ActiveInputDeviceNames.Clear();
			foreach (InputDeviceDefinition supportedDevice in SupportedInputDevices) {
				foreach (InputDevice inputDevice in PlayerInput.devices) {
					if (!supportedDevice.DeviceDetector.DetectDevice (inputDevice))
						continue;
					
					ActiveInputDeviceNames.Add ((supportedDevice.SpritesheetName, supportedDevice.GenericDeviceName));
				}
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