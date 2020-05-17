using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PigeonCorp.Dispatcher
{
    public static class MainDispatcher
    {
        public static ICollection<IDisposable> Disposables = new List<IDisposable>();
    
        private static readonly string[] DontDestroyOnLoadObjectNames = {
            "ProjectContext",
            "MainThreadDispatcher"
        };
    
        public static void Throw()
        {
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
    }
}