using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

namespace KimelPK.DynamicInputPrompts {
	
	[CreateAssetMenu (fileName = "XboxGamepadDetector", menuName = "Kimel-PK/DynamicInputPrompts/DeviceDetectors/XboxGamepadDetector")]
	public class XboxGamepadDetector : DeviceDetector {

		public override bool DetectDevice (InputDevice inputDevice) => inputDevice is XInputController;
	}
}