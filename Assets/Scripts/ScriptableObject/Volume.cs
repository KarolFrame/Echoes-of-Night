using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName ="new Volume", menuName ="Tools/Audio", order = 0)]
public class Volume : PersistantScriptableObject
{
    public float volume;
}


