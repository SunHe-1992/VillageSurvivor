using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SunHeTBS
{
    /// <summary>
    /// pawn state
    /// </summary>
    public enum PawnState
    {
        Default,
        Idle,
        Eat,
        Sleep,
        Work,
        Patrol,
    }

    public enum BuildingEffectType
    {
        Default,
        Sleep,
        Eat,
        Work,
    }
    public enum PawnCamp : int
    {
        Default = 0,
        Player = 1,
        Villain = 2,
        PlayerAlly = 3,
        Neutral = 4,
    }


    public enum EnumItem : int
    {
        Log = 1,
        Stick = 2,
        Stone = 3,
        IronBar = 4,
        Straw = 5,
        Leather = 6,
        LinenThread = 7,
        WoolThread = 8,
        Plank = 9,
        CopperBar = 10,
        BronzeBar = 11,
        Grapes = 12,
        Wine = 13,
        IronOre = 14,
        BronzeOre = 15,
        FishMeat = 16,
        DeerMeat = 17,
        SimpleClothes = 18,
        IronAxe = 19
    }
    public enum Gender : int
    {
        Male = 0,
        Female = 1,
    }

}
