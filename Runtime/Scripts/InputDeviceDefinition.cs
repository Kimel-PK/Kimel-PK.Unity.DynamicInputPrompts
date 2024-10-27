using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KimelPK.DynamicInputPrompts {
	
	[CreateAssetMenu(fileName = "InputDeviceDefinition", menuName = "Kimel-PK/DynamicInputPrompts/InputDeviceDefinition")]
	public class InputDeviceDefinition : ScriptableObject {
		[field: SerializeField] public string SpritesheetName { get; private set; }
		[field: SerializeField] public string GenericDeviceName { get; private set; }
		[field: SerializeField] public DeviceDetector DeviceDetector { get; private set; }
		[field: SerializeField] public List<TMP_SpriteAsset> ButtonSpritesAssets { get; private set; }
	}
}