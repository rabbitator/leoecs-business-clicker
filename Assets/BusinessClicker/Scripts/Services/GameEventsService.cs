using UniRx;

namespace BusinessClicker.Services
{
    public sealed class GameEventsService
    {
        public readonly Subject<float> OnSomeEvent;

        public GameEventsService()
        {
            OnSomeEvent = new Subject<float>();
        }
    }
}