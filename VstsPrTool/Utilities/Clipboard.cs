namespace VstsPrTool.Utilities
{
    public class Clipboard
    {
        public static void Copy(string val)
        {
            if (OperatingSystem.IsWindows())
            {
                $"echo {val} | clip".Bat();
            }

            if (OperatingSystem.IsMacOS())
            {
                $"echo \"{val}\" | pbcopy".Bash();
            }
        }
    }
}