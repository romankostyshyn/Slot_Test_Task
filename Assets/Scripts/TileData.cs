public struct TileData
{
    public readonly int X;
    public readonly int Y;

    public readonly int TypeId;
    public readonly float HorMultiplier;
    public readonly float VertMultiplier;
    public readonly float DiagMultiplier;

    public TileData(int x, int y, int typeId, float horMultiplier, float vertMultiplier, float diagMultiplier)
    {
        X = x;
        Y = y;

        TypeId = typeId;
        HorMultiplier = horMultiplier;
        VertMultiplier = vertMultiplier;
        DiagMultiplier = diagMultiplier;
    }
}