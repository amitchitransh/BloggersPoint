namespace BloggersPoint.UI.Services
{
    public interface IMessageService
    {
        void ShowInfoMessage(string text);
        void ShowWarningMessage(string text);
        void ShowErrorMessage(string text);
    }
}
