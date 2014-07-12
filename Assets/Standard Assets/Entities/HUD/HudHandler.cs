using UnityEngine;
using System.Collections;

public class HudHandler : MonoBehaviour
{
    public GameObject BarHealth;
    public GameObject BarMana;
    public float MaxHealth = 100.0f;
    public float MaxMana = 100.0f;

    public Color ColorHealth = Color.green;
    public Color ColorStamina = Color.cyan;

    public bool EnableHealthWarning = true;
    public bool EnableStaminaWarning = true;

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

    public bool Visible = true;
    public bool IsVisible
    {
        get { return GetComponent<SpriteRenderer>().enabled; }
        set
        {
            GetComponent<SpriteRenderer>().enabled = value;
            if (BarHealth != null)
                BarHealth.gameObject.GetComponent<SpriteRenderer>().enabled = value;
            if (BarMana != null)
                BarMana.gameObject.GetComponent<SpriteRenderer>().enabled = value;
        }
    }

    void Start()
    {
        IsVisible = Visible;
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
            objBar.GetComponent<SpriteRenderer>().color = color;
        }
    }

    float warning = 1.0f;
    float multiplier = -1.0f;

    void Update()
    {
        warning += multiplier * Time.deltaTime * 3;
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

        Color colorHealth;
        if (EnableHealthWarning && ValueHealth <= MaxHealth / 3)
            colorHealth = new Color(warning, Mathf.Abs(warning - 1.0f), 0.0f);
        else
            colorHealth = ColorHealth;

        Color colorStamina;
        if (EnableStaminaWarning && ValueMana <= MaxMana / 3)
            colorStamina = new Color(0.0f, 0.0f, warning);
        else
            colorStamina = ColorStamina;

        UpdateBar(BarHealth, ValueHealth, MaxHealth, colorHealth);
        UpdateBar(BarMana, ValueMana, MaxMana, colorStamina);
    }
}
