using Zenject;

namespace PigeonCorp.Installers
{
    public class Containers
    {
        private static DiContainer _project;
        private static DiContainer _scene;
        
        public static DiContainer Project
        {
            get { return _project; }
            internal set
            {
                _project = value;
            }
        }

        public static DiContainer Scene
        {
            get { return _scene; }
            internal set
            {
                _scene = value;
            }
        }
    }
}