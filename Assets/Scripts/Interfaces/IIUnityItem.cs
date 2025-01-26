using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public interface IIUnityItem 
{
    public void DropItem(GameObject go,Vector3 pos,GameObject looatobj);
}

public class InventoryArgs : EventArgs
{
    //public InventoryArgs(MirrorTypes Invitem)
    //{
    //    InvItem = Invitem;
    //}
    //public MirrorTypes InvItem;
}
