﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    private void Awake()
    {
        instance = this;

    }
    public List<InvenItem> itemDB = new List<InvenItem>();
}
