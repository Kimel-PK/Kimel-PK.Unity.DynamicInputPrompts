using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KimelPK.DynamicInputPrompts {
    public class InputActionTag {
        
        private static readonly Regex Regex =
            new (
                "<InputAction\\s*=\\s*\"(?<action>[^\"]+)\"(?:\\s+composite\\s*=\\s*\"(?<composite>[^\"]+)\")?\\s*>",
                RegexOptions.Compiled | RegexOptions.IgnoreCase
            );
		
        public static bool TryExtract(string text, out InputActionTagData tagData) {
            tagData = default;

            if (string.IsNullOrEmpty(text))
                return false;

            Match match = Regex.Match(text);
            if (!match.Success)
                return false;

            string actionName = match.Groups["action"].Value;
            string compositeRaw = match.Groups["composite"].Success
                ? match.Groups["composite"].Value
                : null;

            List<string> compositeParts = null;
            if (!string.IsNullOrWhiteSpace(compositeRaw)) {
                compositeParts = compositeRaw
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToList();
            }

            tagData = new InputActionTagData {
                fullTag = match.Value,
                actionName = actionName,
                compositeParts = compositeParts
            };

            return true;
        }
        
    }
}