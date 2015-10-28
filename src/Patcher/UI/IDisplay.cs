using System.Threading.Tasks;
using Patcher.Logging;

namespace Patcher.UI
{
    interface IDisplay
    {
        void Shutdown();
        void SetWindowHeight(int windowHeight);
        void SetWindowWidth(int windowWidth);
        void ShowPreRunErrorMessage(string message);

        Logger GetLogger(LogLevel maxLogLevel);
        Status GetStatus();
        Prompt GetPrompt();

        void Run(Task task);
    }
}