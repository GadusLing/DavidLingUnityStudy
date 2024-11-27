using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObject : InteractableObject
{
    protected override void Interact()
    {
        print("Interacting with npc!");
    }
}
