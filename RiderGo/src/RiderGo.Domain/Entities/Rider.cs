namespace RiderGo.Domain.Entities
{
    public class Rider 
    {
        public string Id { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string CNPJ { get; set; } = String.Empty;
        public DateTime Birth { get; set; }
        public string CnhNumber { get; set; } = String.Empty;
        public string CnhType { get; set; } = String.Empty;
        public string CnhImageUrl { get; set; } = String.Empty;


        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
