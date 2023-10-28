using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move
{
    public MoveBase Base { get; set; }

    public int Mana { get; set; }

    public Move(MoveBase pBase)
    {
        Base = pBase;
        Mana = pBase.Mana;
    }
}
