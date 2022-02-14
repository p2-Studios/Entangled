using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TypedEvent : UnityEvent<TypedEvent> { }

public class ForceManager : MonoBehaviour
{

    private Dictionary<string, UnityEvent> eventDictionary;
    private Dictionary<string, TypedEvent> typedEventDictionary;

    private static ForceManager eventManager;

    public static ForceManager instance {
        get {
            if (!eventManager) {
                eventManager = FindObjectOfType(typeof(ForceManager)) as ForceManager;

                if (!eventManager) {
                    Debug.LogError("There needs to be one active ForceManger script on a GameObject in your scene.");
                }
                else {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init() {
        if (eventDictionary == null) {
            eventDictionary = new Dictionary<string, UnityEvent>();
            typedEventDictionary = new Dictionary<string, TypedEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener) {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(listener);
        }
        else {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StartListening(string eventName, UnityAction<object> listener) {
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.AddListener(listener);
        }
        else {
            thisEvent = new TypedEvent();
            thisEvent.AddListener(listener);
            instance.typedEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener) {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.RemoveListener(listener);
        }
    }


    public static void StopListening(string eventName, UnityAction<object> listener) {
        if (eventManager == null) return;
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName) {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.Invoke();
        }
    }


    public static void TriggerEvent(string eventName, object data) {
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent)) {
            thisEvent.Invoke(data);
        }
    }
}
