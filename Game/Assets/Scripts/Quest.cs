using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public Type type;
    Transform room;

    public void RemoveQuest()
    {
        if (type != Type.None)
            AudioManager.Instance.Play("QuestComplete", true);

        Destroy(gameObject);
    }

    public enum Type
    {
        None,
        KillAllEnemies,
        HitSwitch,
        MoveObjectToTrigger,
    }

    void Start()
    {
        room = transform.parent.parent;
    }

    void Update()
    {
        switch (type)
        {
            case Type.None:
                RemoveQuest();
                break;
            case Type.KillAllEnemies:
                UpdateKillAllQuest();
                break;
            case Type.HitSwitch:
                UpdateHitSwitchQuest();
                break;
            case Type.MoveObjectToTrigger:
                UpdateMoveObjectToTriggerQuest();
                break;
        }
    }

    void UpdateMoveObjectToTriggerQuest()
    {
        MovableTile[] mObjs = room.GetComponentsInChildren<MovableTile>();

        bool done = true;
        foreach (MovableTile o in mObjs)
        {
            if (!o.isPlaced)
                done = false;
        }

        if (done)
        {
            RemoveQuest();
        }
    }

    void UpdateHitSwitchQuest()
    {
        InteractiveObject[] iObjs = room.GetComponentsInChildren<InteractiveObject>();

        bool done = true;
        foreach (InteractiveObject o in iObjs)
        {
            if (!o.isActivated)
                done = false;
        }

        if (done)
        {
            RemoveQuest();
        }
    }

    void UpdateKillAllQuest()
    {
        Enemy[] enemies = room.GetComponentsInChildren<Enemy>();

        if (enemies.Length == 0)
        {
            RemoveQuest();
        }
    }
}
