using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpectrumEffect : MonoBehaviour
{
    static readonly int m_RimPowKey = Shader.PropertyToID("_RimPow");

    public void SetSpectrum(Material _Material, float _Duration, bool _AlphaDecrease)
    {
        StartCoroutine(C_SpectrumProgress(_Material, _Duration, _AlphaDecrease));
    }

    IEnumerator C_SpectrumProgress(Material _Material, float _Duration, bool _AlphaDecrease)
    {
        float rim = _Material.GetFloat(m_RimPowKey);
        float timer = _Duration;
        float descrease = rim / _Duration;
        while (timer > 0.0f)
        {
            float deltatime = Time.deltaTime;
            timer -= deltatime;
            if (_AlphaDecrease)
            {
                rim -= deltatime * descrease;
                _Material.SetFloat(m_RimPowKey, rim);
            }
            yield return null;
        }

        Destroy(gameObject);
    }
}
