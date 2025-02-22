﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;

public class Library
{
    public async Task<bool> Confirm(string content, string title, string ok, string cancel)
    {
        bool result = false;
        MessageDialog dialog = new MessageDialog(content, title);
        dialog.Commands.Add(new UICommand(ok, new UICommandInvokedHandler((cmd) => result = true)));
        dialog.Commands.Add(new UICommand(cancel, new UICommandInvokedHandler((cmd) => result = false)));
        await dialog.ShowAsync();
        return result;
    }

    public async void New(TextBox display)
    {
        if (await Confirm("Create New Document?", "Codetext", "Yes", "No"))
        {
            display.Text = string.Empty;
            display.Select(display.Text.Length, 0);
        }
    }

    public async void Open(TextBox display)
    {
        FileOpenPicker picker = new FileOpenPicker
        {
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary
        };
        picker.FileTypeFilter.Add(".txt");
        StorageFile file = await picker.PickSingleFileAsync();
        if (file != null)
        {
            display.Text = await FileIO.ReadTextAsync(file);
            display.Select(display.Text.Length, 0);
        }
    }

    public async void Save(TextBox display)
    {
        try
        {
            FileSavePicker picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            picker.DefaultFileExtension = ".txt";
            picker.SuggestedFileName = "Document";
            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await FileIO.WriteTextAsync(file, display.Text);
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }
        catch
        {

        }
    }

    public void Share(TextBox display)
    {
        try
        {
            DataTransferManager.ShowShareUI();
        }
        catch
        {

        }
    }
}