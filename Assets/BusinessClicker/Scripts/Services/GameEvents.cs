using System;
using UniRx;

namespace BusinessClicker.Services
{
    public sealed class GameEvents
    {
        public readonly Subject<double> OnTransferBusinessIncomeToUser;
        public readonly Subject<int> OnBusinessLevelPurchased;
        public readonly Subject<(int, int)> OnBusinessImprovementPurchased;
        public readonly IObservable<Unit> OnApplicationQuit;

        public GameEvents()
        {
            OnTransferBusinessIncomeToUser = new Subject<double>();
            OnBusinessLevelPurchased = new Subject<int>();
            OnBusinessImprovementPurchased = new Subject<(int, int)>();
            OnApplicationQuit = Observable.OnceApplicationQuit();
        }
    }
}