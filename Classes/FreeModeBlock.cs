using GBX.NET.Engines.Game;
using System.Text.Json.Serialization;

namespace GbxNetApi.Classes
{
    public class FreeModeBlock
    {
        [JsonConstructor]
        public FreeModeBlock() { }
        public FreeModeBlock(CGameCtnBlock block)
        {

            name = block.Name;
            pos = new List<float>() {
                block.AbsolutePositionInMap.Value.X,
                block.AbsolutePositionInMap.Value.Y,
                block.AbsolutePositionInMap.Value.Z
            };
            rot = new List<float>() {
                block.PitchYawRoll.Value.X,
                block.PitchYawRoll.Value.Y,
                block.PitchYawRoll.Value.Z
            };
        }

        public string name { get; set; }
        public List<float> pos { get; set; }
        public List<float> rot { get; set; }

    }
}
