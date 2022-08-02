namespace GbxNetApi.Classes
{
    public class MapBlocksData
    {
        public MapBlocksData(IList<NadeoBlock> nadeoBlocks, IList<AnchoredObject> anchoredObjects)
        {
            this.nadeoBlocks = nadeoBlocks;
            this.anchoredObjects = anchoredObjects;
        }

        public IList<NadeoBlock> nadeoBlocks { get; set; }
        public IList<AnchoredObject> anchoredObjects { get; set; }
    }
}
