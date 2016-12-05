# Xamarin.Android Service Samples

This directory holds the sample projects from the [Creating Services](https://developer.xamarin.com/guides/android/application_fundamentals/services/) guides for Xamarin.Android. There are multiple solutions in the subdirectories of this project. Each solution is meant to be a stand alone solution that focus on a specific topic.

## ServicesDemo1

This solution shows how to [create and use a bound service](https://developer.xamarin.com/guides/android/application_fundamentals/services/creating-a-service/bound-service) in Xamarin.Android. The bound service will expose a method that can be called by the activity that is bound to it.

**{TODO: SCREENSHOTS FROM SERVICESDEMO1}**

## ServicesDemo2

This solution is a example of how to [create and use a started service](https://developer.xamarin.com/guides/android/application_fundamentals/services) in Xamarin.Android. In this example, there is no direct communication between the activity and the service it starts. The activity will start and stop the services. The service, while it is running, will log messages using Android.Util.Log.

[](./Screenshots/StartedServices1.png)
[](./Screenshots/StartedServices2.png)
[](./Screenshots/StartedServices3.png)

## ServicesDemo3

This solution is a variant of **ServicesDemo2**, except that the started service will register as a _foreground service_.

**{TODO: SCREENSHOTS FROM SERVICESDEMO3}**

## Authors

Tom Opgenorth (toopge@microsoft.com)