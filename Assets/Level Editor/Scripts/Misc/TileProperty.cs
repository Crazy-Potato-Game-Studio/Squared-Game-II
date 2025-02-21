[System.Serializable]
public class TileProperty
{
    public int id;
    public Vector2IntSerializable position;
    public TileProperty(int id, Vector2IntSerializable position)
    {
        this.id = id;
        this.position = new(position.x, position.y);
    }
}
[System.Serializable]
public class Vector2IntSerializable
{
    public int x;
    public int y;
    public Vector2IntSerializable(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}