# Xamarin.Android Service Samples

Example code from the **[Creating Services](https://developer.xamarin.com/guides/android/application_fundamentals/services/)** article. This directory contains two solutions, described below.

Each of these samples requires Android 4.4 (API level 19) or higher.

## DemoService
This solution contains two projects. Of the two, make sure that the **DemoService** app is built and installed on your emulator or device _before_ trying the **DemoMessengerClient** app.

### DemoService ###

This is a Xamarin.Android application that contains three services. `DemoService` which is an example of creating a started Service. `DemoMessengerService` is a

### DemoMessengerClient ### 

This is a sample that will perform an IPC call to the `DemoMessengerService` that is provided by the **DemoService** app.

## StockService

This solution is a sample project which demonstrates how to use an `IntentService` to query a website for stock prices.

# Authors

Mike Bluestein, Tom Opgenorth
