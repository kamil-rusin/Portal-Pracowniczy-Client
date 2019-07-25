using Android.Content;
using PPCAndroid.Shared.Domain;

namespace PPCAndroid
{
    public static class AndroidObjectFactory
    {
        public static IWorkStorage GetWorkStorage(Context context)
        {
            return new WorkSessionManager(context);
        }

        public static IUserStorage GetUserStorage(Context context)
        {
            return new UserSessionManager(context);   
        }

        public static IEventService GetEventService()
        {
            return new MemoryEventService();
        }
    }
}