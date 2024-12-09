using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KimelPK.DynamicInputPrompts.Demo {
	public class ButtonPromptsDemo : MonoBehaviour {

		[SerializeField] private TMP_Text detectedDevicesText;
		
		private void OnControlsChanged (PlayerInput playerInput) {
			detectedDevicesText.text = $"Currently used devices: {string.Join (", ", playerInput.devices)}\nDisplay names: {string.Join(", ", playerInput.devices.Select(device => device.displayName))}";
		}
	}
}