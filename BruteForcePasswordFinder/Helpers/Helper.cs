namespace BruteForcePasswordFinder.Helpers;

public static class Helper
    {
        public static string _MachineName;

        public static string MachineName()
        {
            if (_MachineName != null) return _MachineName;

            _MachineName = Environment.MachineName.ToLower().Replace("-", "");

            return _MachineName;
        }

        public static void Write(string value, ushort level = 0)
        {
            Write(value, default, level);
        }

        public static void Write(string value, MessageType messageType = MessageType.Info, ushort level = 0)
        {
            switch (messageType)
            {
                case MessageType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default: // info
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            var log = DateTime.Now.ToString("s") + ": " + new string('\t', level) + value;
            Console.WriteLine(log);
            System.IO.File.AppendAllLines($"files/execution_{MachineName()}.log", new string[] { log });
        }
    }

