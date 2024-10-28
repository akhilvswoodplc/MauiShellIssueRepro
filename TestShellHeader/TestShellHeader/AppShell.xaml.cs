using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace TestShellHeader;

public partial class AppShell : Shell, INotifyPropertyChanged
{
    private string _name = "Hello";
    private string _description = "Test first assignment";

	public AppShell()
	{
		InitializeComponent();
        BindingContext = this;
	}

    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            RaisePropertyChanged(() => Name);
        }
    }

    public string Description
    {
        get { return _description; }
        set
        {
            _description = value;
            RaisePropertyChanged(() => Description);
        }
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Task.Delay(5000).ContinueWith(x =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Name = "test";
                Description = "Test new change Test new change Test new change Test new change Test new change Test new change Test new change";
            });
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged()
    {
        this.OnPropertyChanged((string)null);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
        if (propertyChanged != null)
        {
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public void RaisePropertyChanged<T>(Expression<Func<T>> property)
    {
        var name = GetMemberInfo(property).Name;
        OnPropertyChanged(name);
    }

    protected MemberInfo GetMemberInfo(Expression expression)
    {
        MemberExpression operand;
        LambdaExpression lambdaExpression = (LambdaExpression)expression;
        if (lambdaExpression.Body is UnaryExpression exp)
        {
            operand = (MemberExpression)exp.Operand;
        }
        else
        {
            operand = (MemberExpression)lambdaExpression.Body;
        }
        return operand.Member;
    }
}

