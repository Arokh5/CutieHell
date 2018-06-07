using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDebug : MonoBehaviour {

    public bool Show_Stats;
    public bool Show_FPS;
    public bool Show_Tris;
    public bool Show_Verts;
    public static int verts;
    public static int tris;

    public float updateInterval = 0.5F;

    [SerializeField]
    private Text fpsText;
    [SerializeField]
    private Text trisText;
    [SerializeField]
    private Text vertsText;
    private float accum = 0;
    private int frames = 0;
    private float timeleft;
    public float fps;

    void Start() {
        timeleft = updateInterval;
    }

    void Update() {
        if (Show_Stats) {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            if (timeleft <= 0.0) {
                fps = accum / frames;
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
                GetObjectStats();
                UpdateStatisticsPanel();
            }
        }
    }

    void UpdateStatisticsPanel() {
        if (Show_FPS) {
            fpsText.text = "Fps : " + fps.ToString("00.00");
        }
        if (Show_Tris) {
            trisText.text = "Tris : " + tris.ToString("###,###,###");
        }
        if (Show_Verts) {
            vertsText.text = "Verts : " + verts.ToString("###,###,###");
        }
    }

    void GetObjectStats() {
        verts = 0;
        tris = 0;

        GameObject[] ob = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj in ob) {
            Renderer render = obj.GetComponent<Renderer>();
            if (render != null)
            {
                if (render.isVisible)
                {
                    GetObjectStats(obj);
                }
            }
        }
    }

    List<MeshFilter> children;
    void GetObjectStats(GameObject obj) {
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh)
        {
            tris += meshFilter.sharedMesh.triangles.Length / 3;
            verts += meshFilter.sharedMesh.vertexCount;
        }

        SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh)
        {
            tris += skinnedMeshRenderer.sharedMesh.triangles.Length / 3;
            verts += skinnedMeshRenderer.sharedMesh.vertexCount;
        }
    }
}
