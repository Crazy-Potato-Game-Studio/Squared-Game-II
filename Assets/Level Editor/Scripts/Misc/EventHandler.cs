using System;

public static class EventHandler
{
    public static Action<int, int> OnFloorUpdate;
    public static void CallFloorUpdate(int x,int y) => OnFloorUpdate?.Invoke(x,y);
    public static Action<int, int> OnFloorDestroy;
    public static void CallFloorDestroy(int x, int y) => OnFloorDestroy?.Invoke(x, y);
}
