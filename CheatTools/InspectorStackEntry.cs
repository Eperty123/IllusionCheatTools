﻿namespace CheatTools
{
    public partial class CheatTools
    {
        class InspectorStackEntry
        {
            public InspectorStackEntry(object instance, string name)
            {
                Instance = instance;
                Name = name;
            }

            public object Instance { get; }
            public string Name { get; }
        }
    }
}
