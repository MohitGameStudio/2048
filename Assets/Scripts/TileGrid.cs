using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get ; private set; }
    public TileCell[] cells { get; private set; }

    public int size => rows.Length;
    public int height => cells.Length;
    public int width => size / height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }
}
