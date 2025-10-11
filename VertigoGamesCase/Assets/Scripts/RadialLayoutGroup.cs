using UnityEngine;

[ExecuteAlways, RequireComponent(typeof(RectTransform))]
public class RadialLayoutGroup : MonoBehaviour
{
    [Range(0,360)] public float startAngle = -90f; 
    [Range(0,1)]  public float radiusPercent = 0.38f; 
    public bool clockwise = true;
    public bool rotateChildren = false;   
    public float childAngleOffset = 0f;  

    RectTransform rt;
    void OnEnable(){ rt = GetComponent<RectTransform>(); Layout(); }
    void OnTransformChildrenChanged(){ Layout(); }
    void OnRectTransformDimensionsChange(){ Layout(); }
    void OnValidate(){ Layout(); }

    public void Layout(){
        if (!rt) rt = GetComponent<RectTransform>();
        var children = GetComponentsInChildren<RectTransform>(false);
        int count = 0;
        foreach (var c in children) if (c.parent == transform && c.gameObject.activeSelf) count++;
        if (count == 0) return;

        float size = Mathf.Min(rt.rect.width, rt.rect.height);
        float R = (size * 0.5f) * Mathf.Clamp01(radiusPercent);
        float step = 360f / count;
        float dir = clockwise ? -1f : 1f;

        int i = 0;
        foreach (var child in children){
            if (child.parent != transform || !child.gameObject.activeSelf) continue;
            float angDeg = startAngle + dir * step * i;
            float angRad = angDeg * Mathf.Deg2Rad;
            child.anchoredPosition = new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad)) * R;
            child.localRotation = Quaternion.Euler(0,0,
                rotateChildren ? angDeg + (clockwise?90f:-90f) + childAngleOffset : childAngleOffset);
            child.localScale = Vector3.one; 
            i++;
        }
    }
}