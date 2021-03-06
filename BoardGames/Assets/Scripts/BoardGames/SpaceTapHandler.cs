﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class SpaceTapHandler : MonoBehaviour, IPointerClickHandler {

    public delegate void SpaceEvent(Space space);
    public event SpaceEvent onClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(string.Format("clicked space {0} on ring {1}.", thisSpace.getColumn(), thisSpace.getRow()));
        if (onClick != null)
        {
            onClick.Invoke(thisSpace);
        }
    }
    public void setSpace(Space space) { thisSpace = space; }
    private Space thisSpace;
}
