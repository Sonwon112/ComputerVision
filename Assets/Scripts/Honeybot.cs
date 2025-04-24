using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honeybot : MonoBehaviour
{
    public void HoneyBotEnterDone()
    {
        Manager.Instance.callConerbeeEnter();
    }
}
