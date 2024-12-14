using System.Collections.Generic;
using UnityEngine;

namespace NordicBibo.Runtime.Utility {
    public static class CollectionExtensions {
        public static void Shuffle<T>(this IList<T> list) {
            for (int i = list.Count - 1; i >= 0; i--) {
                int randomIndex = Random.Range(0, i + 1);
                (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
            }
        }
    }
}