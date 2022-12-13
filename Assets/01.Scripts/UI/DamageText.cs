using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DamageText :PoolableMono
{
    [SerializeField]
    private TextMeshProUGUI _damageText;

    private void Awake()
    {
        _damageText ??= GetComponent<TextMeshProUGUI>();
    }

    public void SetText(int damage, Vector3 pos, Color color, bool isCritical = false   )
    {
        _damageText.text = damage.ToString();
        _damageText.transform.position = pos;

        if (isCritical == true)
        {
            _damageText.color = Color.red;
        }
        else 
        {
            _damageText.color = color;
        }
        //Sequence

    }

    public override void Reset()
    {
    }
}
