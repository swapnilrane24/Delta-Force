using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class ObjectPoolGeneric<T>
    {
        private readonly List<T> _objectCollection; //list to hold objects
        private readonly Func<T> _factoryMethod; //method called to spawn objects
        private readonly Action<T> _turnOnCallback; //method called when object is activated
        private readonly Action<T> _turnOffCallback; //method called when object is deactivated;

        public int NoOfItemsInList => _objectCollection.Count;

        public ObjectPoolGeneric(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback,
            int initialCount = 0)
        {
            _factoryMethod = factoryMethod;
            _turnOffCallback = turnOffCallback;
            _turnOnCallback = turnOnCallback;

            _objectCollection = new List<T>();

            for (int i = 0; i < initialCount; i++)
            {
                var obj = _factoryMethod();
                _turnOffCallback(obj);
                _objectCollection.Add(obj);
            }
        }

        public T GetObject()
        {
            var result = default(T);

            if (_objectCollection.Count > 0)
            {
                result = _objectCollection[0];
                _objectCollection.RemoveAt(0);
            }
            else
            {
                result = _factoryMethod();
            }
            _turnOnCallback(result);
            return result;
        }

        public void ReturnObject(T obj)
        {
            _turnOffCallback(obj);
            if (_objectCollection.Contains(obj) == false)
                _objectCollection.Add(obj);
        }









    }
}