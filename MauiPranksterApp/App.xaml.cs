﻿using Application = Microsoft.Maui.Controls.Application;

namespace MauiPranksterApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}
