using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PigeonCorp.Dispatcher
{
    public static class MainDispatcher
    {
        public static ICollection<IDisposable> Disposables = new List<IDisposable>();
        public static bool Thrown => _thrown;
    
        private static bool _thrown;

        private static readonly string[] DontDestroyOnLoadObjectNames = {
            
        };
    
        // TODO:
        // Call Throw when the main screen is reset!!!
        public static void Throw()
        {
            _thrown = true;
        
            foreach (var disposable in Disposables)
            {
                disposable.Dispose();
            }
        
            Disposables = new List<IDisposable>();
        
            foreach (var name in DontDestroyOnLoadObjectNames)
            {
                var dontDestroyOnLoadObject = GameObject.Find(name);
                if (!dontDestroyOnLoadObject) continue;
            
                Object.Destroy(dontDestroyOnLoadObject);
            }
        }

        // TODO:
        // Don't know when to call Restart ¿?
        public static void Restart()
        {
            _thrown = false;
        }
    }
}