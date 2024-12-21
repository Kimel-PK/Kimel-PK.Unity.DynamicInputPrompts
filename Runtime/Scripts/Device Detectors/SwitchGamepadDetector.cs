using UnityEngine;
using UnityEngine.InputSystem;
#if !UNITY_STANDALONE_LINUX
using UnityEngine.InputSystem.Switch;
#endif

namespace KimelPK.DynamicInputPrompts {
	
	[CreateAssetMenu (fileName = "SwitchGamepadDetector", menuName = "Kimel-PK/DynamicInputPrompts/DeviceDetectors/SwitchGamepadDetector")]
	public class SwitchGamepadDetector : DeviceDetector {
		
#if !UNITY_STANDALONE_LINUX
		public override bool DetectDevice (InputDevice inputDevice) => inputDevice is SwitchProControllerHID;
#else
		public override bool DetectDevice (InputDevice inputDevice) => false;
#endif
	}
}