namespace BindablePropertiesInMaui;

public partial class PersonComponent : ContentView
{
    public PersonComponent()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public static readonly BindableProperty PersonProperty = BindableProperty.Create(
        nameof(Person),
        typeof(Person),
        typeof(PersonComponent),
        default(Person));

    public Person Person
    {
        get { return (Person)GetValue(PersonProperty); }
        set { SetValue(PersonProperty, value); }
    }

    public void Refresh()
    {
        OnPropertyChanged(nameof(Person));
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        Person.DateOfBirth = e.NewDate;
    }
}