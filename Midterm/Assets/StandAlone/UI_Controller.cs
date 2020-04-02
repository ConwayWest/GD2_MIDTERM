﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    public NETWORK_ENGINE.NetworkCore myCore;

    // Start is called before the first frame update
    void Start()
    {
        myCore = GameObject.FindObjectOfType<NETWORK_ENGINE.NetworkCore>();
        StartCoroutine(Panels());
    }

    public IEnumerator Panels()
    {
        // Wait until game is connected
        yield return new WaitUntil(() => myCore.IsConnected);
        transform.GetChild(0).gameObject.SetActive(false);
        // transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitWhile(() => myCore.IsConnected);
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
