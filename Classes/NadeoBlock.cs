using GBX.NET;
using GBX.NET.Engines.Game;

namespace GbxNetApi.Classes
{
    public class NadeoBlock
    {
        public NadeoBlock(CGameCtnBlock block)
        {
            cGameCtnBlock = block;

            name = block.Name;
            dir = block.Direction;
            pos = new MyVec3(
                block.Coord.X * 32 + 16,
                block.Coord.Y * 8 - 60,
                block.Coord.Z * 32 + 16
            );

            blockOffsets = new List<List<float>>()
            {
                new List<float>() { 0, 0, 0 }
            };
        }

        private CGameCtnBlock cGameCtnBlock;

        public string name { get; set; }
        public Direction dir { get; set; }
        public MyVec3 pos { get; set; }
        public List<List<float>> blockOffsets { get; set; }
    }
}
