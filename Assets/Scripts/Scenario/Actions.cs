using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Debug = UnityEngine.Debug;
using UnityEngine;

public class Actions// : MonoBehaviour
{
    GameController gc;
    
    public Actions(GameController gc)
    {
        this.gc = gc;
    }

    public void Test()
    {
        Debug.Log("in action test");
    }

}
