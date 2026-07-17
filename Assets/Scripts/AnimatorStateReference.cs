using System;
using UnityEngine;

[Serializable]
public class AnimatorStateReference
{
    [SerializeField]
    private string state;

    public string State => state;
}