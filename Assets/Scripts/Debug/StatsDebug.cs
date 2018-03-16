using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsDebug : MonoBehaviour {

    public bool Show_Stats;
    public bool Show_FPS;
    public bool Show_Tris;
    public bool Show_Verts;
    public static int verts;
    public static int tris;

    public float updateInterval = 0.5F;

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
                string format = System.String.Format("{0:n} FPS", fps);
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
                GetObjectStats();
            }
        }
    }

    void OnGUI() {
        if (Show_Stats)
            ShowStatistics();
    }

    void ShowStatistics() {
        GUILayout.BeginArea(new Rect(Screen.width - 200, Screen.height - 300, 300, 300));
        if (Show_FPS) {
            string fpsdisplay = fps.ToString("0.00 fps");
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 30;
            if (fps < 50f) {
                style.normal.textColor = Color.red;

                GUILayout.Label(fpsdisplay, style);
            } else {
                style.normal.textColor = Color.green;
                GUILayout.Label(fpsdisplay, style);
            }
        }
        if (Show_Tris) {
            string trisdisplay = tris.ToString("###,###,### tris");
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 30;
            style.normal.textColor = Color.black;
            GUILayout.Label(trisdisplay, style);
        }
        if (Show_Verts) {
            string vertsdisplay = verts.ToString("###,###,### verts");
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 30;
            style.normal.textColor = Color.black;
            GUILayout.Label(vertsdisplay, style);
        }
        GUILayout.EndArea();
    }

    void GetObjectStats() {
        verts = 0;
        tris = 0;
        GameObject[] ob = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj in ob) {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null) {
                if (rend.isVisible) {
                    GetObjectStats(obj);
                }
            }
        }
    }

    void GetObjectStats(GameObject obj) {
        Component[] filters;
        filters = obj.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter f in filters) {
            tris += f.sharedMesh.triangles.Length / 3;
            verts += f.sharedMesh.vertexCount;
        }
    }
}
