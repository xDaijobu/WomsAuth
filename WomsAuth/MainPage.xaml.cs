﻿using System.Diagnostics;
using WomsAuth.Controls;
using WomsAuth.Models;
using WomsAuth.ViewModels;

namespace WomsAuth;

public partial class MainPage
{
    public MainPage()
    {
        InitializeComponent();

        BindingContext = new MainPageViewModel();
    }
}

