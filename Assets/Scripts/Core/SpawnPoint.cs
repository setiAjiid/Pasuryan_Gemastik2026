using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    //main task: nandain titik nongolnya player pas masuk scene lewat pintu tertentu
    [SerializeField] private string spawnId = "default";

    public string SpawnId => spawnId;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
