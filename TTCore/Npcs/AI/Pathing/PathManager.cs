﻿using System.Collections.Generic;

namespace TTCore.Npcs.AI.Pathing;

public class PathManager
{
    public static readonly List<Path> Paths = [];

    public static Path AddPath(out int index)
    {
        Path p = new();
        index = Paths.Count;
        Paths.Add(p);
        return p;
    }

    public static void RemovePath(int index) => Paths.RemoveAt(index);

    public static void RemovePath(Path p) => Paths.Remove(p);

    public static void ClearPaths() => Paths.Clear();

    public static Path GetPath(int index)
    {
        if (index >= 0 && index < Paths.Count)
            return Paths[index];
        return null;
    }

    public static bool TryGetPath(int index, out Path p)
    {
        p = GetPath(index);
        return p != null;
    }
}