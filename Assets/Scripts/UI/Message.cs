using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct Message
{
    public enum MessageType
    {
        Review,
        Dialogue
    }

    public string name;
    public string text;
    public int rating;
    public MessageType messageType;
}
