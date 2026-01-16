using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using demkaV130126.Models;

namespace demkaV130126;

public partial class AddUpdateProduct : Window
{
    public string PathToImage = string.Empty;
    public Product productPresenter;
    public string productType { get; set; } = null;
    public int productTypeId { get; set; } = 0;
    ObservableCollection<MaterialWithQuantity> materialsWithQty = new();

    public AddUpdateProduct()
    {
        InitializeComponent();
    }

    public AddUpdateProduct(Product? productInput)
    {
        InitializeComponent();
        if (productInput != null)
        {
            productPresenter = productInput;
            Otrisovka(); 
        }

        using var ctx = new DatabaseContext();
        materialsWithQty = new ObservableCollection<MaterialWithQuantity>(
            ctx.Materials.ToList().Select(m => new MaterialWithQuantity { Material = m })
        );
        MaterialsList.ItemsSource = materialsWithQty;
    }
    
    private async Task<Bitmap?> SelectAndSaveImage()
    {
        var files = await StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Select an image",
                FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
            });

        if (files.Count == 0)
            return null;

        await using var stream = await files[0].OpenReadAsync();
        var bitmap = new Bitmap(stream);

        var guid = Guid.NewGuid();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products", $"{guid}.jpg");

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        bitmap.Save(path);

        PathToImage = $"{guid}.jpg";
        return bitmap;
    }
    
    private async void SelectImage(object? sender, RoutedEventArgs e)
    {
        LogoImage.Source = await SelectAndSaveImage();
    }

    private void TypeProductCombobox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (TypeProductCombobox.SelectedItem is ComboBoxItem selectedItem)
        {
            productType = selectedItem.Content.ToString();
            switch (productType)
            {
                case "Три слоя": productTypeId = 1; break;
                case "Два слоя": productTypeId = 2; break;
                case "Детская": productTypeId = 3; break;
                case "Спуре мягкая": productTypeId = 4; break;
                case "Один слой": productTypeId = 5; break;
            }
        }
    }
    
    private void Otrisovka()
    {
        ArticulBox.Text = productPresenter.Title;
        TitleBox.Text = productPresenter.Title;
        CountPersonBox.Text = productPresenter.Productionpersoncount.ToString();
        WorkShopNumberBox.Text = productPresenter.Productionworkshopnumber.ToString();
        MinCostBox.Text = productPresenter.Mincostforagent.ToString();
        DescriptionBox.Text = productPresenter.Description;
        
        if (!string.IsNullOrEmpty(productPresenter.Image))
        {
            string imagePath = AppDomain.CurrentDomain.BaseDirectory + "products/" + productPresenter.Image;
    
            if (File.Exists(imagePath))
            {
                var bitmap = new Bitmap(imagePath);
                LogoImage.Source = bitmap;
                PathToImage = productPresenter.Image;
            }
        }

        switch (productPresenter.Producttypeid)
        {
            case 1: TypeProductCombobox.SelectedIndex = 0; break; 
            case 2: TypeProductCombobox.SelectedIndex = 1; break; 
            case 3: TypeProductCombobox.SelectedIndex = 2; break; 
            case 4: TypeProductCombobox.SelectedIndex = 3; break; 
            case 5: TypeProductCombobox.SelectedIndex = 4; break; 
        }
    }

    private async void DoneButton_OnClick(object? sender, RoutedEventArgs e)
    {
        using var ctx = new DatabaseContext();
        if (productPresenter == null)
        {
            productPresenter = new Product();
        }

        if (string.IsNullOrEmpty(ArticulBox.Text))
        {
            ErrorTextBlock.Text = "Все поля должны быть заполнены!";
            ErrorTextBlock.IsVisible = true;
            return;
        }
        productPresenter.Articlenumber = ArticulBox.Text;
        productPresenter.Producttypeid = productTypeId;
        if (string.IsNullOrEmpty(TitleBox.Text))
        {
            ErrorTextBlock.Text = "Все поля должны быть заполнены!";
            ErrorTextBlock.IsVisible = true;
            return;
        }
        productPresenter.Title = TitleBox.Text;
        if (!int.TryParse(CountPersonBox.Text, out int countPerson))
        {
            ErrorTextBlock.Text = "Все поля должны быть заполнены!";
            ErrorTextBlock.IsVisible = true;
            return;
        }
        productPresenter.Productionpersoncount = countPerson;
        if (!int.TryParse(WorkShopNumberBox.Text, out int numberShop))
        {
            ErrorTextBlock.Text = "Все поля должны быть заполнены!";
            ErrorTextBlock.IsVisible = true;
            return;
        }
        productPresenter.Productionworkshopnumber = numberShop;
        if (!decimal.TryParse(MinCostBox.Text, out decimal minCost))
        {
            ErrorTextBlock.Text = "Все поля должны быть заполнены!";
            ErrorTextBlock.IsVisible = true;
            return;
        }
        productPresenter.Mincostforagent = minCost;
        if (string.IsNullOrEmpty(DescriptionBox.Text))
        {
            ErrorTextBlock.Text = "Все поля должны быть заполнены!";
            ErrorTextBlock.IsVisible = true;
            return;
        }
        productPresenter.Description = DescriptionBox.Text;
        if (!string.IsNullOrEmpty(PathToImage))
        {
            productPresenter.Image = PathToImage;
        }

        if (productPresenter.Id == 0)
        {
            ctx.Products.Add(productPresenter);
            await ctx.SaveChangesAsync();
        }
        else
        {
            ctx.Products.Update(productPresenter);

            var oldMaterials = ctx.ProductMaterials.Where(pm => pm.ProductId == productPresenter.Id);
            ctx.ProductMaterials.RemoveRange(oldMaterials);
            await ctx.SaveChangesAsync();
        }

        var selectedMaterials = materialsWithQty
            .Where(m => m.Quantity > 0)
            .ToList();

        foreach (var mat in selectedMaterials)
        {
            ctx.ProductMaterials.Add(new ProductMaterial
            {
                ProductId = productPresenter.Id,
                MaterialId = mat.Material.Id,
                Count = mat.Quantity
            });
        }

        await ctx.SaveChangesAsync();

        Close(productPresenter);
    }
    
    public class MaterialWithQuantity : INotifyPropertyChanged
    {
        public Material Material { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Quantity)));
                }
            }
        }

        public string Title => Material.Title;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
