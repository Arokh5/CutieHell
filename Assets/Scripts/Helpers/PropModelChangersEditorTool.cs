using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropModelChangersEditorTool : MonoBehaviour
{
#if UNITY_EDITOR
    [Tooltip("This triggers the tool to refresh its registered PropModelChangers. Tick it if you have added a new prefab to the scene or added the PropModelChanger script to a GameObject")]
    public bool updateElements;
    [Tooltip("If unticked, props will be forced to their evil state")]
    public bool forceCute;

    private List<PropModelChanger> propModelChangers = null;

    private void OnValidate()
    {
        if (propModelChangers == null)
        {
            propModelChangers = new List<PropModelChanger>();
            updateElements = true;
        }

        if (updateElements)
        {
            updateElements = false;
            GetComponentsInChildren(true, propModelChangers);
        }

        if (forceCute)
        {
            foreach (PropModelChanger pmc in propModelChangers)
            {
                pmc.ForceCute();
            }
        }
        else
        {
            foreach (PropModelChanger pmc in propModelChangers)
            {
                pmc.ForceEvil();
            }
        }
    }
#endif
}
