using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class QuestionType
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }
        
        [StringLength(100)]
        public string Name { get; set; } // "Numeric", "Boolean", "Multichoice", "Text", etc.


        //---------------------------------------------------------------------------------------------------

    }
}
