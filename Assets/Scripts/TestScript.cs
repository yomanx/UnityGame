﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.instance.Show<UITest>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
