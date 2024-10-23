namespace ABPTestTask.BBL.Requests
{
    public interface IHallAvailabilityRequest
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Capacity { get; set; }
    }

}
