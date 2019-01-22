using UnityEngine;
using System.Collections.Generic;

namespace InstaGen
{
    public static class CollectionsHelper
    {
        public static void AddUnique<T>(this List<T> uniqueList, T objectToAdd)
        {
            if (uniqueList.Contains(objectToAdd))
            {
                Debug.LogError("Cannot add the same item to Unique List!");
                return;
            }
            uniqueList.Add(objectToAdd);
        }
    }
}
