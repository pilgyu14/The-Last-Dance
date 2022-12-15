using UnityEngine; 
using UnityEngine.Events;

/// <summary>
/// 애니메이션 클립에서 이벤트 줄 떄 
/// </summary>
public class AnimationEventComponent : MonoBehaviour
{
    public UnityEvent SetNav10Event = null;
    public UnityEvent SetNav39Event = null;

    public UnityEvent FronKickEffectEvent = null;
    public UnityEvent SideKickEffectEvent = null;
    public UnityEvent BackKickEffectEvent = null;
    public UnityEvent TackleEffectEvent = null;
    public void SetNav10()
    {
        SetNav10Event?.Invoke();
    }

    public void SetNav39()
    {
        SetNav39Event?.Invoke();
    }

    public void FrontKickEffect()
    {
        FronKickEffectEvent?.Invoke(); 
    }

    public void SideKickEffect()
    {
        SideKickEffectEvent?.Invoke(); 
    }
    public void BackKickEffect()
    {
        BackKickEffectEvent?.Invoke(); 
    }

    public void TackleEffect()
    {
        TackleEffectEvent?.Invoke(); 
    }


}
