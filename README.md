# Kimsufi Availability Checker
An automatic parser with alert system for OVH Kimsufi servers.

The OVH Kimsufi servers are fast and reliable dedicated servers proposed by the french hoster OVH.
Because of the low price applied to servers, they are often non available.

This software checked the status of the different kind of servers proposed in order to alert you (via MessageBox or Email) as soon as a specific server is being replenished.

# Build

The underlying browser used cannot be compiled using 'Any CPU' configuration. Please set the Active Configuration Platform to 'x86' or 'x64' depending on your architecture.
Additionally, you will need Visual Studio 2017 or higher and .NET 4.7 to compile the solution (or you can upgrade it).

# Run

In order to run this application, you will need the VC++ 2013 Runtime (https://www.microsoft.com/en-us/download/details.aspx?id=40784)
