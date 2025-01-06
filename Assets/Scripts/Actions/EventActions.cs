using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActions
{

    public static Action<Mirror> _PickMirror;
    public static Action<Mirror> _DropMirror;

    public static MirrorVariation _SelectedInvType = MirrorVariation.none;

    public static Action<MirrorVariation> _SelectInv;
}
