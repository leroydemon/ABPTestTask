namespace ABPTestTask.BBL.Requests
{
    public class HallAvailabilityRequestDto
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Capacity { get; set; }
    }

}
