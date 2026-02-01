using UnityEngine;
using UnityEngine.Tilemaps;

public class WallManager : MonoBehaviour
{
    [SerializeField] private Tilemap walls;
    [SerializeField] private Tilemap visibleWalls;
    const float RESET_TIMER = 0.125f;
    float timer = RESET_TIMER;
    BoundsInt curBounds;
    
    Camera cam;
    void Start()
    {
        cam = Camera.main;
    }
    void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer > 0) return;

        BoundsInt camBounds =  GetCamBounds();
        if (curBounds != null && curBounds == camBounds) return;

        curBounds = camBounds;
        timer = RESET_TIMER;

        visibleWalls.ClearAllTiles();
        for (int x = camBounds.min.x; x < camBounds.max.x; x++) { 
            for (int y = Mathf.FloorToInt(camBounds.center.y + 1); y < camBounds.max.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y);
                if (walls.GetTile(pos) == null) continue;
                
                visibleWalls.SetTile(pos, walls.GetTile(pos));
                
            }
        }

    }

    private BoundsInt GetCamBounds()
    {
        Vector3 min = cam.ScreenToWorldPoint(Vector3.zero);
        Vector3 max = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3Int minInt = new Vector3Int((int)min.x - 1, (int)min.y);
        Vector3Int maxInt = new Vector3Int((int)max.x + 1, (int)max.y + 1);

        BoundsInt camBounds = new BoundsInt();
        camBounds.min = minInt;
        camBounds.max = maxInt;

        return camBounds;
    }
}
