namespace GGame.NetWork
{
    public abstract class PacketHeaderBase
    {
        public PacketHeaderBase(PacketType packetType, int id)
        {
            Id = id;
        }

        public int Id
        {
            get;
            set;
        }
    }
}
