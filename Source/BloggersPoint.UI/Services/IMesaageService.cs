namespace BloggersPoint.Services
{
    public interface IMesaageService
    {
        void ShowInfoMessage(string text);
        void ShowWarningMessage(string text);
        void ShowErrorMessage(string text);
    }
}
