using System;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Common = 0,   //일반
    Rare,         //희귀
    Legendary     //전설
}

//2025-06-16 OJY
[CreateAssetMenu(fileName = "Ghost Data", menuName = "Scriptable Object/Ghost Data", order = int.MaxValue)]
public class GhostData : ScriptableObject
{
    [Header("목록")]
    public GhostInfo[] CommonInfos;
    public GhostInfo[] RareInfos;
    public GhostInfo[] LegendaryInfos;

    /// <summary>
    /// 모든 귀신 정보 반환
    /// </summary>
    /// <returns>귀신 정보 리스트</returns>
    public IEnumerable<GhostInfo[]> GetAllInfos()
    {
        yield return CommonInfos;
        yield return RareInfos;
        yield return LegendaryInfos;
    }

    [Header("도감 배경")]
    public Sprite Background;
    public Sprite Background_Unknown;
}

[Serializable]
public class GhostInfo
{
    [Header("이름")]
    public string name;

    [Header("몬스터 등급")]
    public MonsterType ghostType; //유령 등급

    [Header("등급 이미지")]
    public Sprite rareSprite;   

    [Header("이미지")]
    public Sprite ghostImage;

    [Header("실루엣")]
    public Sprite UnknownImage;

    [Header("설명")]
    public Sprite infoSprite;

    [Header("발견 여부")]
    public bool isFound;
}