# HotSpot

HotSpot is a web application designed for managing desk reservations and locations efficiently. This README will guide you through the setup process, including database creation and installation of required npm packages.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)

## Prerequisites

Before you begin, ensure you have the following installed:

- [Node.js](https://nodejs.org/) (v14 or later)
- [npm](https://www.npmjs.com/) (comes with Node.js)
- [Microsoft SQL Server](https://go.microsoft.com/fwlink/p/?linkid=2216019&clcid=0x409&culture=en-us&country=us)
- [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup) (optional but recommended)

## Database Setup

 **Create a Database:**
   Open SQL Server Management Studio (SSMS) and connect to your SQL Server instance. Run the following SQL command to create a new database:

   ```sql
   CREATE DATABASE HotDesk;
   ```

## Running the application
Start the backend server HotDeskApp.Server using IIS Express or by running:
```
dotnet run
```
Frontend app should run automaticly. If cannot start the frontend application
hotdeskapp.client using:

```
npm start
```
