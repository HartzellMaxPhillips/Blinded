using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms;

// This Script Checks what items are in the zone

public class TagsInZone : MonoBehaviour
{

    [SerializeField] BoxCollider area;

    public List<string> allowed = new List<string>();

    public Dictionary<String, int> itemCount = new Dictionary<string, int>();

    public void UpdateCount()
    {
        Dictionary<String, int> newCount = new Dictionary<string, int>();

        Vector3 center = area.transform.TransformPoint(area.center);
        Vector3 halfExtents = Vector3.Scale(area.size, area.transform.lossyScale) * .5f;
        
        Collider[] hits = Physics.OverlapBox(center, halfExtents, area.transform.rotation);

        var objects = new List<GameObject>();
        foreach (Collider c in hits)
        {
            if (!objects.Contains(c.gameObject))
            {
                objects.Add(c.gameObject);
            }
        }

        foreach (GameObject obj in objects)
        {
            if (obj.CompareTag("Untagged")) continue;
            string tag = obj.tag;
            if (!allowed.Contains(tag)) continue;
            newCount.TryGetValue(tag, out int current);
            newCount[tag] = current + 1;
        }

        itemCount = newCount;
        LogCounts(itemCount);
    }

    void LogCounts(Dictionary<string, int> counts)
    {
        if (counts.Count == 0)
        {
            Debug.Log("Zone is empty (no tagged objects)");
            return;
        }

        var sb = new StringBuilder($"Zone contents ({counts.Count} tag types):");
        foreach (var pair in counts)
        {
            sb.Append($"\n  {pair.Key}: {pair.Value}");
        }
        Debug.Log(sb.ToString());
    }

}
