#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace CellsEcosystem
{
  public static  class DoSetDirty
    {
        [MenuItem("Assets/SetDirty")]
        static void SavingAsset()
        {
            foreach (var obj in Selection.objects)
            {
                EditorUtility.SetDirty(obj);
            }
            AssetDatabase.SaveAssets();
        }
    }
}
#endif