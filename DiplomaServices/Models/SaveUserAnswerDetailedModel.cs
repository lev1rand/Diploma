using DataAccess.Entities.TestEntities;

namespace DiplomaServices.Models
{
    public class SaveUserAnswerDetailedModel
    {
        public SaveUserAnswersModel Answer { get; set; }
        public int UserId { get; set; }
        public Question Question { get; set; }
    }
}
