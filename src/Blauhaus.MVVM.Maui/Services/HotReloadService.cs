
using Blauhaus.MVVM.Maui.Services;

[assembly: System.Reflection.Metadata.MetadataUpdateHandlerAttribute(typeof(HotReloadService))]
namespace Blauhaus.MVVM.Maui.Services { 
    public static class HotReloadService
    {
        public static event Action<Type[]?>? UpdateApplicationEvent;

        internal static void ClearCache(Type[]? types) { }
        internal static void UpdateApplication(Type[]? types) {
            UpdateApplicationEvent?.Invoke(types);
        }
    }
}
