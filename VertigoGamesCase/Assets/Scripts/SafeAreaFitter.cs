using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Compilation;
#endif

[ExecuteAlways, RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    public static readonly HashSet<SafeAreaFitter> Instances = new();

    RectTransform rt;
    Rect lastSafe, lastCanvasPixelRect;
    bool _applying;

    void OnEnable()
    {
        Instances.Add(this);
        rt = GetComponent<RectTransform>();
        Apply(true);
#if UNITY_EDITOR
        EditorApplication.delayCall += () => { if (this) Apply(true); };
#endif
    }

    void OnDisable() => Instances.Remove(this);

    void OnRectTransformDimensionsChange() => Apply();

  

    private void Apply(bool force = false)
    {
        if (_applying || !rt) return;
        _applying = true;

        var canvas = rt.GetComponentInParent<Canvas>();
        Rect canvasPx = new Rect(0, 0, Screen.width, Screen.height);
        if (canvas && canvas.isRootCanvas && canvas.renderMode != RenderMode.WorldSpace)
            canvasPx = canvas.pixelRect;

        if (canvasPx.width <= 0 || canvasPx.height <= 0)
        { _applying = false; return; }

        Rect safe = Screen.safeArea;
        if (safe.width <= 0 || safe.height <= 0) safe = canvasPx;

        bool changed = force || safe != lastSafe || canvasPx != lastCanvasPixelRect;
        lastSafe = safe; lastCanvasPixelRect = canvasPx;

        if (changed)
        {
            Vector2 min = new(
                Mathf.Clamp01((safe.x - canvasPx.x) / canvasPx.width),
                Mathf.Clamp01((safe.y - canvasPx.y) / canvasPx.height));
            Vector2 max = new(
                Mathf.Clamp01((safe.xMax - canvasPx.x) / canvasPx.width),
                Mathf.Clamp01((safe.yMax - canvasPx.y) / canvasPx.height));

            const float EPS = 0.0001f;
            if ((rt.anchorMin - min).sqrMagnitude > EPS ||
                (rt.anchorMax - max).sqrMagnitude > EPS)
            {
                rt.anchorMin = min;
                rt.anchorMax = max;
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
            }
        }

        _applying = false;
    }

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void EditorBridgeInit()
    {
        CompilationPipeline.compilationFinished += _ => ReapplyAll();
        EditorApplication.playModeStateChanged += _ => ReapplyAll();
        EditorApplication.delayCall += ReapplyAll;
    }

    static void ReapplyAll()
    {
        EditorApplication.delayCall += () =>
        {
            foreach (var inst in Instances)
                if (inst) inst.Apply(true);
        };
    }
#endif
}
