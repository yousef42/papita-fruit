using UnityEngine;
using System;
using Spine.Unity;
using Malee.List;


[CreateAssetMenu(menuName ="Menu Data", fileName ="MenuData")]
public class MenuData : ScriptableObject
{
    [Reorderable]
    public CellDataList cellDataList;
}

[Serializable]
public class CellData
{
    public SkeletonDataAsset skelData;
    public Material skelMaterial;
    public string sceneName;
    public AudioClip audio;
}

[Serializable]
public class CellDataList : ReorderableArray<CellData>
{

}