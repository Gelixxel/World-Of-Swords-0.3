using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonQuest : MonoBehaviour
{
    public QuestGiver QuestGiver;

    public void Accepter()
    {
        QuestGiver.AcceptQuest();
    }

    public void Refuser()
    {
        QuestGiver.RefusetQuest();
    }
}
