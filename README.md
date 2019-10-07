# demo-aas-odata-api

This project contains a sample OData API that can expose data sorced from Azure Analysis Services. The solution 

# Sample Endpoint

https://demo-aas-odata-api.azurewebsites.net/

# Components

PerformanceIndicatorController - MVC controller expoding OData specification via Microsoft.AspNet.OData library.

AnalysisServicesQueryClient - Client for executing AAS queries

PerformanceIndicatorQueryService - Service that leverages AnalysisServicesQueryClient to execute performace indicator queries.

# References

https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/create-an-odata-v4-endpoint
