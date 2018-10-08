// Copyright 2017 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using PolyToolkitInternal;
using UnityEditor;

namespace PolyToolkit { 

/// <summary>
/// A Poly Toolkit asset (custom asset type).
/// 
/// This represents an asset imported from Poly.
/// </summary>
public class PtAsset : ScriptableObject {
  /// <summary>
  /// A reference to the prefab that represents the object.
  /// </summary>
  [DisabledProperty]
  public GameObject assetPrefab;

  /// <summary>
  /// Title of the asset.
  /// </summary>
  [DisabledProperty]
  public string title = "";

  /// <summary>
  /// Author of the asset.
  /// </summary>
  [DisabledProperty]
  public string author = "";

  /// <summary>
  /// URL to the asset.
  /// </summary>
  [DisabledProperty]
  public string url = "";

  /// <summary>
  /// The license under which this asset was included in the project.
  /// </summary>
  [DisabledProperty]
  public PolyAssetLicense license;

  [Range(0,180)]
  public int SmoothingAngle = 180;
  
  [ExecuteInEditMode]
  [ContextMenu("Change Smoothing angle")]
  public void smoothie()
  {
    var path = AssetDatabase.GetAssetPath(this);
    var assets = AssetDatabase.LoadAllAssetsAtPath(path);

    for (var i = 0; i < assets.Length; i++)
    {
      Object o = assets[i];
      Mesh mesh = o as Mesh;
      if (mesh != null)
      {                
        mesh.RecalculateNormals(SmoothingAngle);
      }
      assets[i] = mesh;
    }
    
    AssetDatabase.CreateAsset(this,path);
    for (int i = 1; i < assets.Length; i++)
    {
      AssetDatabase.AddObjectToAsset(assets[i], path);
    }
  }

  /// <summary>
  /// Convenience method to return a filter string like "t:PtAsset" based on the name
  /// of this class (so that we are resilient to future renamings of this class).
  /// </summary>
  public static string FilterString {
    get {
      return "t:" + typeof(PtAsset).Name;
    }
  }
}
}