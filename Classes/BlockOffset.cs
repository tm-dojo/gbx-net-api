using GBX.NET.Engines.Game;

namespace GbxNetApi.Classes
{
    public class BlockOffset
    {
        public string? name { get; set; }
        public List<List<float>>? blockOffsetsAir { get; set; }
        public List<List<float>>? blockOffsetsGround { get; set; }
    }
}
