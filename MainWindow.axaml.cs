using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using demkaV130126.Models;
using Microsoft.EntityFrameworkCore;

namespace demkaV130126;

public partial class MainWindow : Window
{
    List<Product> dataSourseProducts = new List<Product>();
    ObservableCollection<Product> products = new ObservableCollection<Product>();
    private int currentPage = 1;
    private int itemsPerPage = 20;
    public MainWindow()
    {
        InitializeComponent();

        using var ctx = new DatabaseContext();
        dataSourseProducts = ctx.Products.Include(p => p.Producttype).Include(p => p.ProductMaterials).ThenInclude(pm => pm.Material).ToList();
        foreach (var product in dataSourseProducts)
        {
            if (product.Image == "")
            {
                product.Image = "picture.png";
            }
        }
        var types = ctx.Producttypes.Select(x => x.Title).ToList();
        types.Insert(0, "Все типы");

        FilterComboBox.ItemsSource = types;
        ProductsList.ItemsSource = products;
        Display();
    }

    public void Display()
    {
        using var ctx = new DatabaseContext();
        var temp = dataSourseProducts;
        products.Clear();
        if (!string.IsNullOrEmpty(SearchTextBox.Text))
        {
            var search = SearchTextBox.Text;
            temp = temp.Where(it => IsContains(it.Title, it.Description, search)).ToList();
        }
        
        switch (SortComboBox.SelectedIndex)
        {
            case 1: temp = temp.OrderBy(it => it.Title).ToList(); break;
            case 0: temp = temp; break;
            case 2: temp = temp.OrderByDescending(it => it.Title).ToList(); break;
            case 3: temp = temp.OrderBy(it => it.Productionworkshopnumber).ToList(); break;
            case 4: temp = temp.OrderByDescending(it => it.Productionworkshopnumber).ToList(); break;
            case 5: temp = temp.OrderBy(it => it.Mincostforagent).ToList(); break;
            case 6: temp = temp.OrderByDescending(it => it.Mincostforagent).ToList(); break;
        }

        if (FilterComboBox.SelectedItem is string selectedTypeStr && selectedTypeStr != "Все типы")
        {
            if (FilterComboBox.SelectedIndex == 0)
            {
                temp = temp;
                return;
            }

            var selectedType = ctx.Producttypes.Where(it => it.Title == selectedTypeStr).FirstOrDefault();
            temp = temp.Where(it => it.Producttypeid == selectedType.Id).ToList();
        }
        
        int totalItems = temp.Count;
        int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

        if (totalPages == 0)
        {
            currentPage = 1;
        }
        else
        {
            currentPage = Math.Clamp(currentPage, 1, totalPages);
        }

        CurrentPageTextBlock.Text = $"{currentPage}/{totalPages}";
        
        if (currentPage > totalPages)
        {
            currentPage = totalPages;
        }
        if (currentPage < 1)
        {
            currentPage = 1;
        }

        var paginatedList = temp.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage).ToList();
        
        foreach (var item in paginatedList)
        {
            products.Add(item);
        }
    }
    
    private void NextPage(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        currentPage++;
        Display();
    }

    private void PreviousPage(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        currentPage--;
        Display();
    }
    
    public bool IsContains(string title, string? description, string search)
    {
        string desc = string.Empty;
        if (description != null) desc = description;
        string message = (title + desc).ToLower();
        search = search.ToLower();
        return message.Contains(search);
    }
    
    private void SearchTextBox_OnTextChanging(object? sender, TextChangingEventArgs e)
    {
        Display();
    }
    
    private void SortComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Display();
    }

    private void FilterComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Display();
    }
    
    private async void UpdateMinCostSelectedProducts(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var selectedProducts = ProductsList.SelectedItems.Cast<Product>().ToList();

        if (selectedProducts.Count == 0) return;
        
        UpdateMinCost updateMinCost = new UpdateMinCost(selectedProducts);
        var updatedProducts = await updateMinCost.ShowDialog<List<Product>>(this);
        foreach (var updatedProduct in updatedProducts)
        {
            var original = dataSourseProducts.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (original != null)
            {
                original.Mincostforagent = updatedProduct.Mincostforagent;
            }
        }
        Display();
    }
    
    private void ProductsList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        int count = ProductsList.SelectedItems.Count;

        UpdateMenuItem.IsEnabled = count == 1;
    }

    private async void UpdateProduct_OnClick(object? sender, RoutedEventArgs e)
    {
        using var ctx = new DatabaseContext();
        if (ProductsList.SelectedItem is not Product selectedProduct) return;
        
        AddUpdateProduct addUpdateProduct = new AddUpdateProduct(selectedProduct);
        var newProduct = await addUpdateProduct.ShowDialog<Product>(this);
        var oldProduct = dataSourseProducts.FirstOrDefault(a => a.Id == newProduct.Id);
        if (oldProduct != null)
        {
            dataSourseProducts.Remove(oldProduct);
            var newnewProduct = ctx.Products.Include(p => p.Producttype).Include(p => p.ProductMaterials).ThenInclude(pm => pm.Material).FirstOrDefault(a => a.Id == newProduct.Id);
            dataSourseProducts.Add(newnewProduct);
        }
        Display();
    }

    private async void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        using var ctx = new DatabaseContext();

        AddUpdateProduct addUpdateProduct = new AddUpdateProduct(null);
        var newProduct = await addUpdateProduct.ShowDialog<Product>(this);
        var newnewProduct = ctx.Products.Include(p => p.Producttype).Include(p => p.ProductMaterials).ThenInclude(pm => pm.Material).FirstOrDefault(a => a.Id == newProduct.Id);
        dataSourseProducts.Add(newnewProduct);
        Display();
    }
}