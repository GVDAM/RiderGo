namespace RiderGo.Domain.Entities
{
    public class Rental
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public int Plan { get; set; }
        public decimal DailyRate { get; set; } = 0;

        public decimal TotalAmount { get; set; } = 0;
        public DateTime? ReturnDate { get; set; }


        public string RiderId { get; set; } = null!;
        public Rider Rider { get; set; } = null!;

        public string MotorcycleId { get; set; } = null!;
        public Motorcycle Motorcycle { get; set; } = null!;

    }
}
