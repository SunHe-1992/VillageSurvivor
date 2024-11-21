using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProgressBar : MonoBehaviour
{
    Transform rect;

    float fullWidth;

    float percent;
    private void Awake()
    {
        rect = this.transform;
        fullWidth = GetComponent<Transform>().localScale.x;
    }

    public void SetPercent(float _pct)
    {
        this.percent = _pct;
        rect.localScale = new Vector3(percent * fullWidth, rect.localScale.y, rect.localScale.z);
    }

}
