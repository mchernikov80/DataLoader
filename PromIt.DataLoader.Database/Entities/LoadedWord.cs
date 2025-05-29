using System.ComponentModel.DataAnnotations;

namespace PromIt.DataLoader.Database.Entities
{
    public class LoadedWord
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Слово.
        /// </summary>
        [Required]
        [StringLength(4000, MinimumLength = 1)]
        public required string Word { get; set; }

        /// <summary>
        /// Кол-во упоминаний слова.
        /// </summary>
        [Required]
        public required int Amount { get; set; }
    }
}
