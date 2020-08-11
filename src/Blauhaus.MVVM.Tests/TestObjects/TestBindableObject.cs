using Blauhaus.Common.Utils.NotifyPropertyChanged;

namespace Blauhaus.MVVM.Tests.TestObjects
{
    //todo this has moved to Blauhaus.Common.Utils
    public class TestBindableObject : BaseBindableObject
    {


        public TestBindableObject(int intialIncrementValue = 0)
        {
            _count = intialIncrementValue;
            _countWithSideEffect = intialIncrementValue;
        }


        private int _count;
        public int CountMe
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        
        private int _countWithSideEffect;
        public int CountMeWithSideEffect
        {
            get => _countWithSideEffect;
            set => SetProperty(ref _countWithSideEffect, value, () => SideEffect = value);
        }

        private int _sideEffect;
        public int SideEffect
        {
            get => _sideEffect;
            set => SetProperty(ref _sideEffect, value);
        }


    }
}