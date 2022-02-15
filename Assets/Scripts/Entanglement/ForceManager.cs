using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ForceManager
{
    public static ForceManager instance;

    private ForceManager() {}

    public static ForceManager GetInstance() {
        if (instance == null) {
            instance = new ForceManager();
        }
        return instance;
    }

    public event Action<Entanglable, Vector2> onActiveForced;
    /// <summary>
    /// Notifies EntangleComponent of forces applied
    /// </summary>
    public void ActiveForced(Entanglable e, Vector2 force) {
        if (onActiveForced != null) {
            onActiveForced(e, force);
        }
    }

}
