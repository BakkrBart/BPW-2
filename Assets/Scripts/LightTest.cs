using UnityEngine;

public class LightTest : MonoBehaviour
{
    public Light light;
    void OnPreCull()
    {
        if (light != null)
            light.enabled = false;
    }

    void OnPreRender()
    {
        if (light != null)
            light.enabled = false;
    }
    void OnPostRender()
    {
        if (light != null)
            light.enabled = true;
    }
}
