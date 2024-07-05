using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Net.Maui;

namespace AwesomeUI.View;

public partial class QrCodePage : ContentPage
{

    public QrCodePage()
    {
        InitializeComponent();
        BarcodeReaderView.Options = new BarcodeReaderOptions()
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
        };
    }
    
    private bool _isScanning = false;


    private void BarcodeReaderView_OnBarcodesDetectedodesDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        if (_isScanning)
        {
            return;
        }

        _isScanning = true;
        
        Dispatcher.DispatchAsync(async () =>
        {
            await DisplayAlert("Barcode Detected", e.Results.FirstOrDefault().Value, "OK");
            _isScanning = false;
        });
    }
}