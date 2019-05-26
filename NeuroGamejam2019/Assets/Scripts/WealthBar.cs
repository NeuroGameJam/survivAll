using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WealthBar : MonoBehaviour
{

    public Image currentbar;
    public float CurrValue;
    public float MaxValue = 100;


    // Start is called before the first frame update
    private void Start()
    {
        Update();
    }

    // Update is called once per frame
    private void Update()
    {
        float ratio = CurrValue / MaxValue;
        currentbar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    public void TakeDamage(float value)
    {
        CurrValue -= value;
        if (CurrValue < 0)
            CurrValue = 0;

        Update();
    }

    public void HealDamage(float value)
    {
        CurrValue += value;
        if (CurrValue >  MaxValue)
            CurrValue = MaxValue;

        Update();
    }
}
