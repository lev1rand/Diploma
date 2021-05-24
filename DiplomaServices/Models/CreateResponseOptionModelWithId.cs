namespace DiplomaServices.Models
{
    public class CreateResponseOptionModelWithId: CreateResponseOptionModel
    {
        public int Id { get; set; }
        public decimal Grade { get; set; }
    }
}
