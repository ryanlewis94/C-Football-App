﻿using FootballApp.Utility;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FootballApp.ErrorHandling
{
    public class ErrorHandler
    {
        public MetroWindow metroWindow = (MetroWindow)Application.Current.MainWindow;
        public async void CheckErrorMessage(Exception exception)
        {
            switch (exception.Message)
            {
                case "BadRequest":
                    //MessageBox.Show($"Error 400 {exception.Message}: Something was wrong with the request");
                    await metroWindow.ShowMessageAsync($"Error 400 { exception.Message}", "Something was wrong with the request");
                    break;
                case "Unauthorized":
                    await metroWindow.ShowMessageAsync($"Error 401 {exception.Message}", "Incorrect Api Key and Secret");
                    break;
                case "Unexpected character encountered while parsing value: B. Path '', line 0, position 0.":
                    await metroWindow.ShowMessageAsync($"Error 402", "Subscription expired");
                    break;
                case "NotFound":
                    await metroWindow.ShowMessageAsync($"Error 404 {exception.Message}", "Something can't be found");
                    break;
                case "InternalServerError":
                    await metroWindow.ShowMessageAsync($"Error 500 {exception.Message}", "Something external went wrong, not my fault I swear");
                    break;
                case "Not Implemented":
                    await metroWindow.ShowMessageAsync($"Error 501 {exception.Message}", "This Http Method is not supported");
                    break;
                case "A task was canceled.":
                    await metroWindow.ShowMessageAsync($"Error", "Connection Timed Out");
                    break;
                case "An error occurred while sending the request.":
                    await metroWindow.ShowMessageAsync($"Error", "No Internet Connection");
                    break;
                default:
                    await metroWindow.ShowMessageAsync($"Error: {exception.Message}", exception.ToString());
                    //onsole.WriteLine(exception.Message);
                    break;
            }
            Messenger.Default.Send("loaded");
        }
    }
}
