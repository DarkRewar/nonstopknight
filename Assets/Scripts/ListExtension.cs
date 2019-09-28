using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ListExtension
{
    public static T GetRandom<T>(this List<T> list)
    {
        int random = Random.Range(0, list.Count);
        return list[random];
    }
}
