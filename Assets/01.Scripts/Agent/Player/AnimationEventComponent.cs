using UnityEngine; 
using UnityEngine.Events;

/// <summary>
/// 애니메이션 클립에서 이벤트 줄 떄 
/// </summary>
public class AnimationEventComponent : MonoBehaviour
{
    public UnityEvent SetNav10Event = null;
    public UnityEvent SetNav39Event = null;

    public void SetNav10()
    {
        SetNav10Event?.Invoke();
    }

    public void SetNav39()
    {
        SetNav39Event?.Invoke();
    }

}
