using Android.App.Job;

namespace PPCAndroid.JobServices
{
    public class WifiJobService: JobService
    {
        private bool jobCancelled = false;
         
        public override bool OnStartJob(JobParameters @params)
        {
            doBackgroundWork(@params);
            
            return true;
        }

        private void doBackgroundWork(JobParameters @params)
        {
            
        }

        public override bool OnStopJob(JobParameters @params)
        {
            throw new System.NotImplementedException();
        }
    }
}