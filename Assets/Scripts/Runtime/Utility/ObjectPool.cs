using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace NordicBibo.Runtime.Utility {
    public class ObjectPool {
        private readonly Transform _parent;
        private readonly GameObject _prefab;
        private readonly HashSet<GameObject> _objects;
        
        public ObjectPool(int initialSize, int capacity, Transform parent, GameObject prefab) {
            _parent = parent;
            _prefab = prefab;
            _objects = new HashSet<GameObject>(capacity);

            for (int i = 0; i < initialSize; i++) {
                GameObject defaultObj = CreateNewObject();
                defaultObj.SetActive(false);
                
                _objects.Add(defaultObj);
            }
        }

        public GameObject GetNext() {
            GameObject nextObject = _objects.FirstOrDefault(obj => !obj.activeSelf) ?? CreateNewObject();
            _objects.Add(nextObject);
            
            return nextObject;
        }

        public void Flush() {
            foreach (GameObject gameObject in _objects) {
                Object.Destroy(gameObject);
            }
            
            _objects.Clear();
        }

        private GameObject CreateNewObject() {
            GameObject newObject = Object.Instantiate(_prefab, _parent);
            return newObject;
        }
    }
}