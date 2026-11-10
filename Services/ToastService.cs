using BlazorApp1.Services.Interfaces;

namespace BlazorApp1.Services
{
    public class ToastService : IToastService
    {
        public event Action<string, string>? OnShow;

        public void ShowSuccess(string message)
        {
            OnShow?.Invoke(message, "success");
        }

        public void ShowError(string message)
        {
            OnShow?.Invoke(message, "error");
        }
    }
}
