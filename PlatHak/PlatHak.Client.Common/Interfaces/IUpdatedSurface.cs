namespace PlatHak.Client.Common.Interfaces
{
    public interface IUpdatedSurface : ISurface
    {
        void OnUpdate(GameTime time);
    }
}