using UnityEngine;
using System.Collections;

public class HudHandler : MonoBehaviour
{
    public GameObject BarHealth;
    public GameObject BarMana;
    public float MaxHealth = 100.0f;
    public float MaxStamina = 100.0f;

    public Color ColorHealth = Color.green;
    public Color ColorStamina = Color.cyan;

    public bool EnableHealthWarning = true;
    public bool EnableStaminaWarning = true;

    float curHealth;
    float curStamina;

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
    public float ValueStamina
    {
        get { return valueMana; }
        set
        {
            valueMana = value;
            if (valueMana < 0)
                valueMana = 0;
            else if (valueMana > MaxStamina)
                valueMana = MaxStamina;
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
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.enabled = value;
            if (BarHealth != null)
                BarHealth.gameObject.GetComponent<SpriteRenderer>().enabled = value;
            if (BarMana != null)
                BarMana.gameObject.GetComponent<SpriteRenderer>().enabled = value;
        }
    }

    void Start()
    {
        IsVisible = Visible;
        curHealth = ValueHealth = MaxHealth;
        curStamina = ValueStamina = MaxStamina;
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
        if (EnableStaminaWarning && ValueStamina <= MaxStamina / 3)
            colorStamina = new Color(0.0f, 0.0f, warning);
        else
            colorStamina = ColorStamina;

        if (curHealth > valueHealth)
        {
            curHealth -= Mathf.Sqrt(Mathf.Abs(curHealth - valueHealth)) * 0.20f;
            if (curHealth - valueHealth < 1)
                curHealth = valueHealth;
        }
        else if (curHealth < valueHealth)
        {
            curHealth += Mathf.Sqrt(Mathf.Abs(valueHealth - curHealth)) * 0.25f;
            if (curHealth - valueHealth > 1)
                curHealth = valueHealth;
        }
        UpdateBar(BarHealth, curHealth, MaxHealth, colorHealth);
        UpdateBar(BarMana, ValueStamina, MaxStamina, colorStamina);
    }
}
