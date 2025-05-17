using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [System.Serializable]
    public class MenuEntry
    {
        public MenuType Type;
        public GameObject Prefab;
    }

    public static MenuManager Instance { get; private set; }

    private Dictionary<MenuType, Menu> menuRegistry = new Dictionary<MenuType, Menu>();

    [NonSerialized] public Menu CurrentMenu;
    [SerializeField] private Menu[] menusToRegister;

    private void Awake()
    {
        Instance = this;

        foreach (Menu menu in menusToRegister)
            RegisterMenu(menu.MenuType, menu);
    }

    private void Start()
    {
        foreach (Menu menu in menusToRegister)
            menu.Init();
    }

    //Treba da se preradi za Back input telefona da zatvara menije mozda cak i pauzira igricu???
    private void OnBackInput()
    {
        if (CurrentMenu?.MenuType == MenuType.Title || CurrentMenu?.MenuType == MenuType.GameOver) return;

        if (CurrentMenu != null)
            CloseCurrentMenu();
        else
        {
            OpenMenu(MenuType.Pause);
        }

    }

    public Menu GetMenu(MenuType menuType)
    {
        if (menuRegistry.TryGetValue(menuType, out Menu menu))
            return menu;
        Debug.LogError($"Tried to get unregistered menu {menuType}");
        return null;
    }

    private void RegisterMenu(MenuType menuType, Menu menu)
    {
        if (menuRegistry.ContainsKey(menuType))
        {
            Debug.LogError($"Tried to register already registed menu {menuType}");
            return;
        }

        menuRegistry.Add(menuType, menu);
    }

    public void OpenMenu(MenuType menuType)
    {
        if (!menuRegistry.TryGetValue(menuType, out Menu menu))
        {
            Debug.LogError($"Tried to open unregistered menu {menuType}");
            return;
        }

        if (CurrentMenu == menu)
        {
            Debug.LogError($"Tried to open already open menu {menuType}");
            return;
        }

        if(CurrentMenu != null)
        {
            CloseCurrentMenu();
        }

        menu.Open();
        CurrentMenu = menu;
    }

    public void CloseCurrentMenu()
    {
        if (CurrentMenu.MenuType == MenuType.GameOver) return;

        CurrentMenu.Close();
    }
}