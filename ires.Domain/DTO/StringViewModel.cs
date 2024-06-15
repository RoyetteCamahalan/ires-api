namespace ires.Domain.DTO
{
    public class StringViewModel
    {
        public StringViewModel(string value)
        {
            this.value = value;
        }
        public string value { get; set; } = string.Empty;
    }
}
