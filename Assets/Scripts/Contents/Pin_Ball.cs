using UnityEngine;
using Fusion;
using System.Collections;
using System;
using System.Resources;
using TMPro.Examples;
using UnityEngine.InputSystem;

public class Pin_Ball : NetworkBehaviour
{
    public Transform interpolationTarget;

    private Rigidbody _rb;
    private SphereCollider _sphereCollider;
    private PhysicsMaterial _physicMaterial;

    public override void Spawned()
    {
        _rb = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();
        _physicMaterial = _sphereCollider.material;

        //FIXME: 오디오 매니저 사용 추가 해야함.
    }

    public void MultiplyVelocity(float multiplier)
    {
        _rb.linearVelocity *= multiplier;
    }

    public void MultiplyRadius(float multiplier)
    {
        interpolationTarget.localScale *= multiplier; 
        _sphereCollider.radius *= multiplier;
    }

    public void MultiplyBouncesness(float multiplier)
    {
        _physicMaterial.bounciness *= multiplier;
    }

    //NOTE: 요거 나중에 아이템에 적용할 코루틴임.
    public IEnumerator ChangeBallSource(Action<float> action, float time, float multiplier)
    {
        action?.Invoke(multiplier);
        yield return new WaitForSeconds(time);
        action?.Invoke(1/multiplier);
    }
}
