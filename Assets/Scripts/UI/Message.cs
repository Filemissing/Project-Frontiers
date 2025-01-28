using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct Message
{
    public enum MessageType
    {
        Review,
        Dialogue,
        EndMessage
    }

    public string name;
    public string text;
    public int rating;
    public Sprite comicSprite;
    public MessageType messageType;
}
