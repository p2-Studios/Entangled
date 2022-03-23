using System;
using System.Collections;
using System.Collections.Generic;
using Activation_System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Activator = Activation_System.Activator;

public class DestructionField : Activatable
{
    public bool destroyPlayer;
    public bool destroyObjects;

    public Animator animator;
    public Activator[] activators;
    public Light2D glowLight;

    void Start()
    {
        foreach (Activator a in activators)
        {
            AddActivator(a);
        }

        if (activateByDefault)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    public override void Activate()
    {
        base.Activate();
        if (glowLight != null) glowLight.enabled = true;
        if (animator != null) animator.SetBool("isActive", true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (glowLight != null) glowLight.enabled = false;
        if (animator != null) animator.SetBool("isActive", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated) return;
        Player p = other.GetComponent<Player>();
        Entanglable e = other.GetComponent<Entanglable>();

        if (p != null)
        {
            if (destroyPlayer)
            {
                p.Kill();
            }
            else
            {
                p.entangleComponent.ClearEntangled();
            }
        }

        if (e != null)
        {
            if (destroyObjects)
            {
                e.Kill();
            }
            else
            {
                FindObjectOfType<Player>().entangleComponent.ClearEntangled();
            }
        }
    }

}

