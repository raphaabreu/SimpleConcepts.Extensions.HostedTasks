using System;

namespace SimpleConcepts.Hosting.BackgroundAction
{
    public class BackgroundActionOptions
    {
        public TimeSpan WaitBeforeStart { get; set; }
        public TimeSpan WaitBeforeExecution { get; set; }
        public TimeSpan WaitAfterException { get; set; }
    }
}