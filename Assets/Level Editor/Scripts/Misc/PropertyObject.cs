using LevelBuilder;
using System.Collections.Generic;
[System.Serializable]
public class ObjectProperty
{
    public int id;
    public int state;
    public Vector2IntSerializable pos;
    public Dictionary<string, string> stringValues;
    public ObjectProperty() { }
    public ObjectProperty(int id,int state,Vector2IntSerializable pos) 
    {
        this.id = id;
        this.state = state;
        this.pos = pos;
        stringValues = new();
    }
}