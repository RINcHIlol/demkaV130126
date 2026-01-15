using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using demkaV130126.Models;

namespace demkaV130126;

public partial class UpdateMinCost : Window
{
    public List<Product> localProducts;
    public UpdateMinCost(List<Product> products)
    {
        InitializeComponent();

        localProducts = products;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var newCostText = CostTextBox.Text;
        if (int.TryParse(newCostText, out int newCost))
        {
            if (newCost < 0)
            {
                ErrorTextBlock.IsVisible = true;
                ErrorTextBlock.Text = "Ошибка: цена не может быть меньше нуля!";
                return;
            }
            using var ctx = new DatabaseContext();
            foreach (var p in localProducts)
            {
                p.Mincostforagent = newCost;
                ctx.Update(p);
            }
            ctx.SaveChanges();
        }
        else
        {
            ErrorTextBlock.IsVisible = true;
            ErrorTextBlock.Text = "Ошибка: введено не число!";
            return;
        }
        Close(localProducts);
    }
}