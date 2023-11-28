using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPoolObj : MonoBehaviour
{
    public bool IsActive => gameObject.activeSelf;

    public void SetActive(bool active=true)
    {
        gameObject.SetActive(active);
        _SetActive(active);
    }

    protected virtual void _SetActive(bool active = true)
    {

    }
}
