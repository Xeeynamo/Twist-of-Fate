using UnityEngine;
using System.Collections;

public class HudHandler : MonoBehaviour
{
    public GameObject BarHealth;
    public GameObject BarMana;
    public float MaxHealth = 100.0f;
    public float MaxMana = 100.0f;
    public float ValueHealth;
    public float ValueMana;

    void Start()
    {
        ValueHealth = MaxHealth;
        ValueMana = MaxMana;
    }

    void UpdateBar(GameObject objBar, float value, float max)
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
                scale.x = value / max * 50.0f;
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
