using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOpenMenu : MonoBehaviour
{
    [SerializeField] private MenuType menuType;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => MenuManager.Instance.OpenMenu(menuType));
    }
}
