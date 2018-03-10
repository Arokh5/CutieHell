using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Convertible : MonoBehaviour {

    protected bool converting = false;
    protected bool unconverting = false;
    protected bool isConverted = false;

    public bool IsConverting()
    {
        return converting;
    }

    public bool IsUnconverting()
    {
        return unconverting;
    }

    public bool IsConverted()
    {
        return isConverted;
    }

    public abstract void Convert();

    public abstract void Unconvert();
}
