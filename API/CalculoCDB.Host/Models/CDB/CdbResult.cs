namespace CalculoCDB.API.Models.CDB
{    
    public class CdbResult
    {
        public List<string> Errors { get; set; } = [];

        public bool Success { get; set; }

        /// <summary>
        /// Valor bruto
        /// </summary>
        public decimal GrossAmount { get; set; }

        /// <summary>
        /// Valor líquido
        /// </summary>
        public decimal NetAmount { get; set; }
    }
}
