using GBX.NET;
using GBX.NET.Engines.Game;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GbxNetApi.Classes
{
    public class NadeoBlock
    {
        [JsonConstructor]
        public NadeoBlock() { }
        public NadeoBlock(CGameCtnBlock block, List<BlockOffset> allBlockOffsets)
        {
            name = block.Name;
            dir = (int)block.Direction;
            pos = new List<float> {
                block.Coord.X * 32 + 16,
                block.Coord.Y * 8 - 60,
                block.Coord.Z * 32 + 16
            };

            BlockOffset blockOffset = allBlockOffsets.Find((offset) => offset.name == block.Name);

            if (blockOffset != null)
            {
                blockOffsets = block.IsGround ? blockOffset.blockOffsetsGround : blockOffset.blockOffsetsAir;
            }
            else
            {
                blockOffsets = new List<List<float>>()
                {
                    new List<float>() { 0, 0, 0 }
                };
            }
        }

        public string name { get; set; }
        public List<float> pos { get; set; }
        public int dir { get; set; }
        public List<List<float>> blockOffsets { get; set; }
    }
}
