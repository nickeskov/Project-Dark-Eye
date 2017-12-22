using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlagsController
{
    public static int NumOfFlags = 0;

    public static void PickUpFlag()
    {
        NumOfFlags++;
    }

    public static void GetOutFlag()
    {
        if (NumOfFlags > 0)
        {
            NumOfFlags--;
        }
    }
}
