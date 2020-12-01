using Blauhaus.MVVM.Abstractions.Contracts;
using Blauhaus.MVVM.Abstractions.ViewModels;
using Xamarin.Forms;

namespace Blauhaus.MVVM.Xamarin.Views.ContentViews
{
    public abstract class BaseSelectableContentView : BaseContentView
    {
        private TapGestureRecognizer? _tgr;

        protected override void OnBindingContextChanged()
        {
            if (_tgr == null)
            {
                _tgr = new TapGestureRecognizer();
                _tgr.SetBinding(TapGestureRecognizer.CommandProperty, new Binding(nameof(ISelectableViewModel.SelectCommand)));
                _tgr.CommandParameter = BindingContext;
                Content.GestureRecognizers.Add(_tgr);
            }
            base.OnBindingContextChanged();
        }
    }
}