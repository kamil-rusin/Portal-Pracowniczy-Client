using System;

namespace PPCAndroid.Shared.Domain
{
    public interface IWorkStorage : IDisposable
    {
        void SaveEntryDate(DateTime entryDate);
        void SaveLeavingDate(DateTime leavingDate);
        void SaveAtWork(bool atWork);
        DateTime GetEnteredWorkDate();
        DateTime GetLeftWorkDate();
        bool GetIsAtWork();
        void RemoveWorkData();
    }
}