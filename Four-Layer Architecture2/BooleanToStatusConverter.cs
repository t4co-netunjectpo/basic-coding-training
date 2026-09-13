using Microsoft.UI.Xaml.Data;

namespace TodoApp_WinUI;

public sealed class BooleanToStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        value is true ? "完了" : "未完了";

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotSupportedException();
}
