namespace PBT_205_A1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Application.Run(new StartMenu());

            var chatLogin = new ChatLogin();
            if (chatLogin.ShowDialog() == DialogResult.OK)
            {
                ChatApp chatApp = new ChatApp(chatLogin.Username, chatLogin.Password);
                chatApp.ShowDialog();
            }
        }
    }
}