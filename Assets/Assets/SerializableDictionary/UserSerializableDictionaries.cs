﻿using System.Collections;
using System.Collections.Generic;
using System;
using Consts;
using UnityEngine;

[Serializable]
public class BgmAudioClipDictionary : SerializableDictionary<eBgm, AudioClip> {}

[Serializable]
public class SfxAudioClipDictionary : SerializableDictionary<eSfx, AudioClip> {}

/*
[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}*/