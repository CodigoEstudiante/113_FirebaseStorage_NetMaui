using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;

namespace MauiAppDemo
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        string authDomain = "ingresar aqui";
        string apiKey = "ingresar aqui";
        string email = "ingresar aqui";
        string passWord = "ingresar aqui";
        string token = string.Empty;
        string rutaStorage = "ingresar aqui";


        public MainPage()
        {
            InitializeComponent();

            MainThread.BeginInvokeOnMainThread(new Action(async () => await obtenerToken()));
        }

        private async Task obtenerToken()
        {
            var client = new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = apiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            });
            
            var credenciales = await client.SignInWithEmailAndPasswordAsync(email, passWord);
            token = await credenciales.User.GetIdTokenAsync();
        }


        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var foto = await MediaPicker.PickPhotoAsync();
            if (foto != null)
            {
                var task = new FirebaseStorage(
                    rutaStorage,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(token),
                        ThrowOnCancel = true
                    }
                )
                    .Child("Imagenes")
                    .Child(foto.FileName)
                    .PutAsync(await foto.OpenReadAsync());


                var urlDescarga = await task;

                lblUrl.Text = urlDescarga;
                imgUrl.Source = urlDescarga;

            }
        }
    }
}