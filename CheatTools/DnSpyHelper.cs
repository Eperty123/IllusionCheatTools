﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;

namespace CheatTools
{
    internal static class DnSpyHelper
    {
        private static string _dnSpyPath;

        public static string DnSpyPath
        {
            get { return _dnSpyPath; }
            set
            {
                _dnSpyPath = value?.Trim(' ', '"');

                IsAvailable = false;
                if (_dnSpyPath != null && File.Exists(_dnSpyPath))
                {
                    if (_dnSpyPath.EndsWith("dnspy.exe", StringComparison.OrdinalIgnoreCase))
                        IsAvailable = true;
                    else
                        Logger.Log(LogLevel.Error | LogLevel.Message, "Invalid dnSpy path. The path has to point to 64bit dnSpy.exe");
                }
            }
        }

        public static bool IsAvailable { get; private set; }

        public static void OpenTypeInDnSpy(ICacheEntry entry)
        {
            string GetDnspyArgs(ICacheEntry centry)
            {
                switch (centry)
                {
                    case MethodCacheEntry m:
                        if(m.MethodInfo.ToString().Contains(',') || m.MethodInfo.DeclaringType.FullName.Contains(','))
                            throw new Exception("Unsupported type or method with generic parameters");
                        return $"\"{m.MethodInfo.DeclaringType.Assembly.Location}\" --select M:{m.MethodInfo.DeclaringType.FullName}.{m.MethodInfo.ToString().Split(new[] { ' ' }, 2).Last()}";
                    case PropertyCacheEntry p:
                        if (p.PropertyInfo.DeclaringType.FullName.Contains(','))
                            throw new Exception("Unsupported type with generic parameters");
                        return $"\"{p.PropertyInfo.DeclaringType.Assembly.Location}\" --select P:{p.PropertyInfo.DeclaringType.FullName}.{p.PropertyInfo.Name}";
                    case FieldCacheEntry f:
                        if (f.FieldInfo.DeclaringType.FullName.Contains(','))
                            throw new Exception("Unsupported type with generic parameters");
                        return $"\"{f.FieldInfo.DeclaringType.Assembly.Location}\" --select F:{f.FieldInfo.DeclaringType.FullName}.{f.FieldInfo.Name}";
                    default:
                        throw new Exception("Cannot open dynamically generated items");
                }
            }

            try
            {
                var refString = GetDnspyArgs(entry);
                Logger.Log(LogLevel.Info, "Opening member " + refString);
                Process.Start(@"D:\Applications\dnSpy\dnSpy\dnSpy.exe", refString);
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Error | LogLevel.Message, "[Cheat Tools] " + e.Message);
            }
        }
    }
}