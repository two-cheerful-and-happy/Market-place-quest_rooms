namespace Domain.ViewModels.AdditionalViewModel;

public class NewFilterOfLocationPanel
{
    public string Author { get ; set; }
    public string Name { get; set; }
    public string Confirmed { get; set; }
    public int Page { get; set; }

    public NewFilterOfLocationPanel(string confirmed, string name, string author, int page)
    {
        Confirmed = confirmed;
        Name = name;
        Author = author;
        Page = page;
    }
}
