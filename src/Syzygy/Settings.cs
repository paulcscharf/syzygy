namespace Syzygy
{
    static class Settings
    {
        public static class Network
        {
            public const int DefaultPort = 15432;
            public const string AppName = "Syzygy Prototype";

            public static class Channel
            {
                public const int ParticleUpdates = 1;
                public const int EconomyUpdates = 2;
            }
        }
    }
}
