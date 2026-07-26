using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    //main task: nandain titik nongolnya (sumbu X) player pas masuk scene lewat pintu tertentu, sumbu Y ga diurus (milik ruangan/SceneContext.floorY)
    [SerializeField] private string spawnId = "default";
    [SerializeField] private float facing = 1f;

    public string SpawnId => spawnId;
    public float Facing => facing;

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(pos.x, pos.y - 2f, pos.z), new Vector3(pos.x, pos.y + 2f, pos.z)); //garis tegak 

        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, pos + new Vector3(Mathf.Sign(facing), 0f, 0f)); //panah arah hadap
    }
}
