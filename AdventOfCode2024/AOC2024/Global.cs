﻿namespace AOC2024
{
    public class Global
    {
        public static string InputPath = Directory.GetParent(Path.GetFullPath(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? ""))?.FullName ?? "";
    }
}
