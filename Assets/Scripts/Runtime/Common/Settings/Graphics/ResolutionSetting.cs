using System;
using UnityEngine;

namespace NordicBibo.Runtime.Common.Settings.Graphics {
    [CreateAssetMenu(menuName = "Resolution Setting")]
    public class ResolutionSetting : ScriptableObject {
        public int width;
        public int height;

        public Vector2Int Dimensions => new Vector2Int(width, height);

        public string DisplayText() {
            return $"{width} x {height}";
        }
    }
}