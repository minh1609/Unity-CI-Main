using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogues
{
    [TextArea(3, 10)]
    public string[] sentences;

    public Dialogues(string[] sentences)
    {
        this.sentences = sentences;
    }
}
