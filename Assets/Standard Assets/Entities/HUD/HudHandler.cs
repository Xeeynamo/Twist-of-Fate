using UnityEngine;
using System.Collections;

public class HudHandler : MonoBehaviour
{
    public GameObject BarHealth;
    public GameObject BarMana;
    public float MaxHealth = 100.0f;
    public float MaxMana = 100.0f;
    public float ValueHealth
    {
        get { return valueHealth; }
        set
        {
            valueHealth = value;
            if (valueHealth < 0)
                valueHealth = 0;
            else if (valueHealth > MaxHealth)
                valueHealth = MaxHealth;
        }
    }
    public float ValueMana
    {
        get { return valueMana; }
        set
        {
            valueMana = value;
            if (valueMana < 0)
                valueMana = 0;
            else if (valueMana > MaxMana)
                valueMana = MaxMana;
        }
    }

    private float valueHealth;
    private float valueMana;

    void Start()
    {
        ValueHealth = MaxHealth;
        ValueMana = MaxMana;
    }

    void UpdateBar(GameObject objBar, float value, float max, Color color)
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
            if (BarMana != null)
                BarMana.GetComponent<SpriteRenderer>().color = color;
        }
    }

    float warning = 1.0f;
    float multiplier = -1.0f;

    void Update()
    {
        UpdateBar(BarHealth, ValueHealth, MaxHealth, Color.white);
        if (ValueMana <= MaxMana / 3)
        {
            warning += multiplier * Time.deltaTime * 4;
            if (warning <= 0.0f)
            {
                warning = 0.0f;
                multiplier *= -1.0f;
            }
            else if (warning >= 1.0f)
            {
                warning = 1.0f;
                multiplier *= -1.0f;
            }
        }
        else warning = 1.0f;
        UpdateBar(BarMana, ValueMana, MaxMana, new Color(warning, warning, warning));
    }
}
