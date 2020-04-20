using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyWheel : CharacterBase
{
    public float m_AttackDelay;
    public float m_AttackDelayTimer;
    public float m_AttackDamage;
    public float m_SpinSpeed;

    Material m_HandMat;

    private void Start()
    {
        m_AttackDelayTimer = m_AttackDelay;
        MeshRenderer hanrederer = transform.Find("Capsule/HandMesh").GetComponent<MeshRenderer>();
        m_HandMat = new Material(hanrederer.material);
        m_HandMat.color = Color.white;
        hanrederer.material = m_HandMat;
    }

    protected override void Update()
    {
        base.Update();

        if (m_AttackDelayTimer > 0.0f)
        {
            m_AttackDelayTimer -= Time.deltaTime;
            if (m_AttackDelayTimer < 0.75f)
                m_HandMat.color = Color.red;
            if (m_AttackDelayTimer <= 0.0f)
            {
                m_AttackDelayTimer = m_AttackDelay;
                StartCoroutine(C_WheelAttack());
            }
        }
    }

    IEnumerator C_WheelAttack()
    {
        float duration = 1.5f;
        HashSet<CharacterBase> damagehash = new HashSet<CharacterBase>();
        while (duration > 0.0f)
        {
            float deltatime = Time.deltaTime;
            duration -= deltatime;

            Vector3 rot = transform.eulerAngles;
            rot.y += m_SpinSpeed * deltatime;
            transform.eulerAngles = rot;

            Vector3 extents = new Vector3(2,1.0f,0.1f);

            Collider[] colls = Physics.OverlapBox(transform.position + Vector3.up, extents, transform.rotation, this.m_EnemyLayerMask);
            for (int i = 0; i < colls.Length; ++i)
            {
                if (colls[i].gameObject == this.gameObject) continue;
                CharacterBase character = colls[i].GetComponentInParent<CharacterBase>();
                if (!character) continue;
                if (this.m_Live == CharacterBase.E_Live.DEAD) continue;
                if (this.m_Team == character.m_Team) continue;
                if (damagehash.Contains(character)) continue;
                damagehash.Add(character);
                character.GiveToDamage(this, m_AttackDamage, true);
            }

            yield return null;
        }
        m_HandMat.color = Color.white;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.up, new Vector3(4, 2.0f, 0.2f));
    }
}
