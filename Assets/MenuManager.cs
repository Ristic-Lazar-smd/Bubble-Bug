using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    public GameObject[] Menus;

    private void Awake()
    {
        Instance = this;
    }
}
