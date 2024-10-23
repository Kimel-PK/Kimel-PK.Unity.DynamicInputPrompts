using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KimelPK.DynamicInputPrompts {
	[Serializable]
	public class SupportedInputDevice {
		[field: SerializeField] public string SpritesheetName { get; private set; }
		[field: SerializeField] public string GenericDeviceName { get; private set; }
		[field: SerializeField] public List<string> MatchingUnityInputNames { get; private set; }
		[field: SerializeField] public List<TMP_SpriteAsset> ButtonSpritesAssets { get; private set; }
	}
}