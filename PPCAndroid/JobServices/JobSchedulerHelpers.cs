using Android.App;
using Android.App.Job;
using Android.Content;
using Java.Lang;
using System;


namespace PPCAndroid.JobServices
{/*
    [Service(Name = "PPCAndroid.JobServices.WifiJobService", Permission = "android.permission.BIND_JOB_SERVICE")]
    public static class JobSchedulerHelpers
    {
        public static JobInfo.Builder CreateJobBuilderUsingJobId<T>(this Context context, int jobId) where T : JobService
        {
            var javaClass = Java.Lang.Class.FromType(typeof(T));
            var componentName = new ComponentName(context, javaClass);
            var builder = new JobInfo.Builder(jobId, componentName).SetPeriodic(60000)
                .SetRequiredNetworkType(NetworkType.Any);

            return builder;
        }

        public static void SheduleJob<T>(Context context, int jobId)where T : JobService
        {
            var javaClass = Class.FromType(typeof(T));
            var componentName = new ComponentName(context, javaClass);
            var builder =  new JobInfo.Builder(jobId, componentName)
                .SetPeriodic(60000)
                .SetRequiredNetworkType(NetworkType.Any);
            
            var jobScheduler = (JobScheduler)context.GetSystemService(Context.JobSchedulerService);
            jobScheduler.Schedule(builder.Build());
        }
        
        public static ComponentName GetComponentNameForJob<T>(this Context context) where T : JobService
        {
            Type t = typeof(T);
            Class javaClass = Class.FromType(t);
            return new ComponentName(context, javaClass);
        }
    }*/
}