using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    //add Pause
    private Dictionary<Type, BasePage> _uiPages;
    private Dictionary<Type, BaseController> _controllers;

    private static Main _Instance;
    public static Main Instance
    {
        get
        {
            return _Instance;
        }
    }

    private void Awake()
    {
        if (_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _Instance = this;
    }

    private void Start()
    {
        CreateManagers();
        CreatePages();

        InitManagers();
        InitPages();
    }

    private void CreatePages()
    {
        _uiPages = new Dictionary<Type, BasePage>();
        foreach (var page in FindObjectsOfType<BasePage>())
        {
            _uiPages.Add(page.GetType(), page);
        }
    }

    private void CreateManagers()
    {
        _controllers = new Dictionary<Type, BaseController>();
        foreach (var controller in FindObjectsOfType<BaseController>())
        {
            _controllers.Add(controller.GetType(), controller);
        }
    }

    private void InitPages()
    {
        foreach (var page in _uiPages)
        {
            page.Value.Init();
        }
    }

    private void InitManagers()
    {
        foreach (var controller in _controllers)
        {
            controller.Value.Init();
        }
    }

    private void OnDestroy()
    {
        foreach (var page in _uiPages)
        {
            page.Value.Dispose();
        }

        foreach (var controller in _controllers)
        {
            controller.Value.Dispose();
        }
    }

    public T GetPage<T>() where T : BasePage
    {
        return (T)_uiPages[typeof(T)];
    }

    public T GetController<T>() where T : BaseController
    {
        return (T)_controllers[typeof(T)];
    }

    
}