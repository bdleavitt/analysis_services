Programmatically retrieving meta data about models deployed in Azure Analysis Services can be important when connecting to the service from a custom reporting application. This folder contains brief examples of querying the metadata using the AMOMD.NET and AMO Tabular Object Model libraries using a service principal. To run these, you'll need:
0. An Azure Analysis Services model deployed. 
1. Create a Service Principal in Azure AD. You'll need the client id (guid) and the secret. 
2. Be sure to install and use latest ADOMD.NET and AMO nuget files. https://docs.microsoft.com/en-us/azure/analysis-services/analysis-services-data-providers


