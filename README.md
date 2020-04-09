# GoogleDriveTasker

GoogleDriveTasker is .NET Core console application which performs tasks (sets of actions) on the connected Google Drive.

## Step 1: Turn on the Drive API


# GoogleDriveTasker

GoogleDriveTasker is .NET Core console application which performs tasks (sets of actions) on the connected Google Drive.

## Prerequisites

### Turn on the Drive API

#### Create a new Cloud Platform project and enable the Drive API.
https://developers.google.com/drive/api/v3/enable-drive-api

#### Configure an OAuth client.

Create authorization credentials
Any application that uses OAuth 2.0 to access Google APIs must have authorization credentials that identify the application to Google's OAuth 2.0 server. The following steps explain how to create credentials for your project. Your applications can then use the credentials to access APIs that you have enabled for that project.

Go to the [Credentials page](https://console.developers.google.com/apis/credentials).
Click Create credentials > OAuth client ID.
Select "Other" for Application type.
Download the client configuration and save the file as credentials.json in your working directory.

## Getting Started

Implement the IGoogleDriveTask interface per operatin and register them in Program.cs
