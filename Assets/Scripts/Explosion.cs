using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimatedSpriteRender _start;
    public AnimatedSpriteRender _end;
    public AnimatedSpriteRender _middle;

    public void SetActiveRender(AnimatedSpriteRender render)
    {
        _start.enabled = render == _start;
        _end.enabled = render == _end;
        _middle.enabled = render == _middle;
    }

    // Rotate bomb sprite
    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg , Vector3.forward);
    }
    
    
}
