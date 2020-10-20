using System;
using System.Collections;
using System.Collections.Generic;

public static class Utils
{
    public static bool AreTuplesSame(Tuple<int, int> a, Tuple<int, int> b)
    {
        return a.Item1 == b.Item1 && a.Item2 == b.Item2;
    }
}
