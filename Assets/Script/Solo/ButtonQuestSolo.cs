using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonQuestSolo : MonoBehaviour
{
    public QuestGiverSolo QuestGiver;

    public void Accepter()
    {
        QuestGiver.AcceptQuest();
    }

    public void Refuser()
    {
        QuestGiver.RefusetQuest();
    }
}
