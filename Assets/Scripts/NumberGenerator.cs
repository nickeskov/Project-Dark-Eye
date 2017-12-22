using System.Collections;
using UnityEngine;


public static class NumberGenerator
{
    public static int CurrentPosition = 0;
    public const string Key = "1234232132431232134231134223242331241313142314231421";

    public static int GetNextNumber()
    {
        string CurrentNum = Key.Substring(CurrentPosition++ % Key.Length, 1);
        return int.Parse(CurrentNum);

    }
}

