using UnityEngine;
using System.Collections;

public class HudHandler : MonoBehaviour
{
    public GameObject BarHealth;
    public GameObject BarMana;
    public int MaxHealth = 100;
    public int MaxMana = 100;
    public int ValueHealth;
    public int ValueMana;

    void Start()
    {
        ValueHealth = MaxHealth;
        ValueMana = MaxMana;
    }

    void UpdateBar(GameObject objBar, int value, int max)
    {
        if (objBar != null)
        {
            if (max < 0)
                max = 1;
            Vector3 scale = objBar.transform.localScale;
            if (value > 0)
            {
                if (value > max)
                    value = max;
                scale.x = (float)value / (float)max * 50.0f;
            }
            else
            {
                if (value < 0)
                    value = 0;
                scale.x = 0.0000001f;
            }
            objBar.transform.localScale = scale;
        }
    }

    void Update()
    {
        UpdateBar(BarHealth, ValueHealth, MaxHealth);
        UpdateBar(BarMana, ValueMana, MaxMana);
    }
}
