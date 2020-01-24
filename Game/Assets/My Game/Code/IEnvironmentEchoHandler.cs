
namespace CornTheory
{
    public interface IEnvironmentEchoHandler
    {
        void ProcessEnvironmentMessage(string message);
        void ProcessActorMessage(string actor, string message);
    }
}
