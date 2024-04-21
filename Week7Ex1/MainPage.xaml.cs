using System.Text.RegularExpressions;

namespace Week7Ex1
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editCustomerId;

        public MainPage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            BindingContext = new CustomerViewModel();

            Task.Run(async()=> listView.ItemsSource = await _dbService.GetCustomers());
        }



        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            var isValidEmail = IsValidEmail(emailEntryField.Text);
            var isValidMobile = IsValidMobile(mobileEntryField.Text);

            if (!isValidEmail)
            {
                await DisplayAlert("Error", "Please enter a valid email address.", "OK");
                return;
            }

            if (!isValidMobile)
            {
                await DisplayAlert("Error", "Please enter a valid mobile phone number in the format 0(xxx) xxx xx xx.", "OK");
                return;
            }

            var customer = new Customer
            {
                CustomerName = nameEntryField.Text,
                Email = emailEntryField.Text,
                Mobile = mobileEntryField.Text
            };

            if (await _dbService.CreateOrUpdate(customer))
            {
                nameEntryField.Text = string.Empty;
                emailEntryField.Text = string.Empty;
                mobileEntryField.Text = string.Empty;

                listView.ItemsSource = await _dbService.GetCustomers();
            }
            else
            {
                // Show error message or handle failure
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidMobile(string mobile)
        {
            // Check if mobile number matches the format 0(xxx) xxx xx xx
            // and has 11 characters
            var regex = new Regex(@"^0\s?\d{3}\s?\d{3}\s?\d{2}\s?\d{2}$|^0\s?\d{3}\s?\d{3}\s?\d{4}$");
            return regex.IsMatch(mobile);
        }


        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var customer = (Customer)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    _editCustomerId = customer.Id;
                    nameEntryField.Text = customer.CustomerName;
                    emailEntryField.Text = customer.Email;
                    mobileEntryField.Text = customer.Mobile;
                    break;
                case "Delete":
                    if (await _dbService.Delete(customer))
                        listView.ItemsSource = await _dbService.GetCustomers();
                    else
                    {
                        // Show error message or handle failure
                    }
                    break;
            }
        }

        private List<CustomerViewModel> ConvertToViewModel(List<Customer> customers)
        {
            return customers.Select(c => new CustomerViewModel
            {
                // Diğer müşteri özelliklerini buraya ekleyin
                ProfileImageSource = ImageSource.FromFile("custom_profile.png") // Müşteriye özel profil fotoğrafı
            }).ToList();
        }


    }

}
