using System;
using UniRx;

namespace BusinessClicker.Services
{
    public sealed class GameEvents
    {
        public readonly Subject<double> OnTransferBusinessIncomeToUser;
        public readonly Subject<int> OnBusinessLevelPurchased;
        public readonly Subject<(int, int)> OnBusinessImprovementPurchased;
        public readonly IObservable<Unit> OnGameExit;

        public GameEvents()
        {
            OnTransferBusinessIncomeToUser = new Subject<double>();
            OnBusinessLevelPurchased = new Subject<int>();
            OnBusinessImprovementPurchased = new Subject<(int, int)>();
            var pauseStream = Observable.EveryApplicationPause().Where(p => p).Select(p => Unit.Default);
            var quitStream = Observable.OnceApplicationQuit();
            OnGameExit = pauseStream.Merge(quitStream);
        }
    }
}