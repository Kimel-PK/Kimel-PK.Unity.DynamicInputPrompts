using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;

namespace KimelPK.DynamicInputPrompts {
	
	[CreateAssetMenu (fileName = "SwitchGamepadDetector", menuName = "Kimel-PK/DynamicInputPrompts/DeviceDetectors/SwitchGamepadDetector")]
	public class SwitchGamepadDetector : DeviceDetector {

		public override bool DetectDevice (InputDevice inputDevice) => inputDevice is SwitchProControllerHID;
	}
}