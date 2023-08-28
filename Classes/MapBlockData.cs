using System.Text.Json.Serialization;

namespace GbxNetApi.Classes
{
    public class MapBlocksData
    {
        [JsonConstructor]
        public MapBlocksData() { }
        public MapBlocksData(IList<NadeoBlock> nadeoBlocks, IList<AnchoredObject> anchoredObjects, IList<FreeModeBlock> freeModeBlocks)
        {
            this.nadeoBlocks = nadeoBlocks;
            this.anchoredObjects = anchoredObjects;
            this.freeModeBlocks = freeModeBlocks;
        }

        public IList<NadeoBlock> nadeoBlocks { get; set; }
        public IList<FreeModeBlock> freeModeBlocks { get; set; }
        public IList<AnchoredObject> anchoredObjects { get; set; }
    }
}
