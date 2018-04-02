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
                //TODO when hit switch exists.. check if hit -> remove
                break;
            case Type.MoveObjectToTrigger:
                //TODO when moveobj exists. Move into triggervol -> remove
                break;
        }
    }

    void UpdateKillAllQuest()
    {
        //If no enemies in room -> Open
        Enemy[] enemies = room.GetComponentsInChildren<Enemy>();

        if (enemies.Length == 0)
        {
            RemoveQuest();
        }
    }
}
