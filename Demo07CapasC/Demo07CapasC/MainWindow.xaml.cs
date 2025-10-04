using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Business;
using Entity;

namespace Demo07CapasC
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // GET Productos
        private void btnListProducts_Click(object sender, RoutedEventArgs e)
        {
            dgProducts.ItemsSource = new BProduct().Read();
        }

        // SAVE Producto
        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("El nombre es obligatorio.");
                    return;
                }

                // Parseo robusto para Precio y Stock
                if (!decimal.TryParse(txtPrice.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
                {
                    MessageBox.Show("Precio inválido.");
                    return;
                }

                if (!int.TryParse(txtStock.Text, out var stock))
                {
                    MessageBox.Show("Stock inválido.");
                    return;
                }

                var product = new Product
                {
                    Name = txtName.Text.Trim(),
                    Price = price,
                    Stock = stock,
                    Active = chkActive.IsChecked == true
                };

                new BProduct().Create(product);
                MessageBox.Show("Producto registrado con éxito.");

                // Refrescar grilla y limpiar formulario
                dgProducts.ItemsSource = new BProduct().Read();
                txtName.Clear();
                txtPrice.Clear();
                txtStock.Clear();
                chkActive.IsChecked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // GET Clientes (como ya tenías)
        private void btnListCustomers_Click(object sender, RoutedEventArgs e)
        {
            dgCustomers.ItemsSource = new BCustomer().Read();
        }

        private void btnSaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCName.Text))
                {
                    MessageBox.Show("El nombre es obligatorio.");
                    return;
                }

                var customer = new Customer
                {
                    Name = txtCName.Text.Trim(),
                    Address = string.IsNullOrWhiteSpace(txtCAddress.Text) ? null : txtCAddress.Text.Trim(),
                    Phone = string.IsNullOrWhiteSpace(txtCPhone.Text) ? null : txtCPhone.Text.Trim(),
                    Active = chkCActive.IsChecked == true
                };

                new BCustomer().Create(customer);
                MessageBox.Show("Cliente registrado con éxito.");

                // Refrescar y limpiar
                dgCustomers.ItemsSource = new BCustomer().Read();
                txtCName.Clear();
                txtCAddress.Clear();
                txtCPhone.Clear();
                chkCActive.IsChecked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCustomers.SelectedItem is not Customer selected)
                {
                    MessageBox.Show("Selecciona un cliente de la lista.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtCName.Text))
                {
                    MessageBox.Show("El nombre es obligatorio.");
                    return;
                }

                selected.Name = txtCName.Text.Trim();
                selected.Address = string.IsNullOrWhiteSpace(txtCAddress.Text) ? null : txtCAddress.Text.Trim();
                selected.Phone = string.IsNullOrWhiteSpace(txtCPhone.Text) ? null : txtCPhone.Text.Trim();
                selected.Active = chkCActive.IsChecked == true;

                new BCustomer().Update(selected);
                MessageBox.Show("Cliente actualizado.");

                RefrescarClientesYLimpiar();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgCustomers.SelectedItem is not Customer selected)
                {
                    MessageBox.Show("Selecciona un cliente a eliminar.");
                    return;
                }

                if (MessageBox.Show($"¿Eliminar lógicamente a \"{selected.Name}\"?",
                                    "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    new BCustomer().Delete(selected.CustomerId); // active = 0
                    MessageBox.Show("Cliente eliminado (lógico).");
                    RefrescarClientesYLimpiar();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer c)
            {
                txtCName.Text = c.Name;
                txtCAddress.Text = c.Address;
                txtCPhone.Text = c.Phone;
                chkCActive.IsChecked = c.Active;
            }
        }
        private void RefrescarClientesYLimpiar()
        {
            dgCustomers.ItemsSource = new BCustomer().Read();
            txtCName.Clear();
            txtCAddress.Clear();
            txtCPhone.Clear();
            chkCActive.IsChecked = true;
            dgCustomers.SelectedItem = null;
        }
    }
}
