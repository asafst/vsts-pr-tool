using System;

namespace VstsPrTool.Utilities
{
    public static class Tracer
    {
        private static bool _verbose = false;

        public static void SetVerbosity(bool verbose)
        {
            _verbose = verbose;
        }

        public static void LogVerbose(string message)
        {
            if (_verbose)
            {
                Console.WriteLine(message);
            }
        }

        public static void LogInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}