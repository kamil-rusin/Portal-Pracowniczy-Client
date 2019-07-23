using Android.Content;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid
{
    public static class AndroidObjectFactory
    {
        public static IStorage GetStorage(Context context)
        {
            return new SessionManagerStorage(context);
        }
    }
}