[System.Serializable]
public class TileProperty
{
    public int id;
    public Vector2IntSerializable position;
    public TileProperty(int id, int poxitionX,int positionY)
    {
        this.id = id;
        position = new(poxitionX,positionY);
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