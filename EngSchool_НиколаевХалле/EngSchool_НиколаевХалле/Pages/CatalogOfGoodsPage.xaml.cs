using EngSchool_ВячеславХалле.Models;
using EngSchool_МалышеваКоршикова.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EngSchool_ВячеславХалле.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatalogOfGoodsPage.xaml
    /// </summary>
    public partial class CatalogOfGoodsPage : Page
    {
        int itemCount = 0;
        public CatalogOfGoodsPage()
        {
            InitializeComponent();
            using (var context = new BD26_EVNikolaevHalle())
            {
                var developers = context.Manufacturer.OrderBy(x => x.Name).ToList();
                developers.Insert(0, new Manufacturer
                {
                    Name = "Все типы"
                });
                ComboDeveloper.ItemsSource = developers;
                ComboDeveloper.SelectedIndex = 0;
                LViewGoods.ItemsSource = context.Product.OrderBy(x => x.Name).ToList();
                itemCount = LViewGoods.Items.Count;
                TextBlockCount.Text = $"Результат запроcа: {itemCount} записей из {itemCount}";
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            using (var context = new BD26_EVNikolaevHalle())
            if(Visibility == Visibility.Visible)
            {
                    context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                    LViewGoods.ItemsSource = context.Product.OrderBy(x => x.Name).ToList();
            }
        }

        private void ComboDeveloper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }
        private void UpdateData()
        {
            using (var context = new BD26_EVNikolaevHalle())
            {
                var currentGoods = context.Product.OrderBy(x =>x.Name).ToList();
                if (ComboDeveloper.SelectedIndex > 0)
                {
                    currentGoods = currentGoods.Where(p => p.ManufacturerID == (ComboDeveloper.SelectedItem as Manufacturer).ID).ToList();
                }
                currentGoods = currentGoods.Where(p => p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
                if (ComboDeveloper.SelectedIndex >= 0)
                {
                    if(ComboSort.SelectedIndex == 0)
                    {
                        currentGoods = currentGoods.OrderBy(x => x.Name).ToList();
                    }
                    if (ComboSort.SelectedIndex == 1)
                    {
                        currentGoods = currentGoods.OrderByDescending(x => x.Name).ToList();
                    }
                }
                LViewGoods.ItemsSource = currentGoods;
                TextBlockCount.Text = $"Результат запроcа: {currentGoods.Count} записей из {itemCount}";
            }
        }

    }
}
