[![Build Status](https://dev.azure.com/restairline/restairline/_apis/build/status/restairline?branchName=master)](https://dev.azure.com/restairline/restairline/_build/latest?definitionId=4&branchName=master)

# Overview

* A classic DDD with CQRS-ES, Hypermedia API and Ubiquitous unit test project based on EventFlow. It's targeted to ASP.NET Core 2.2 and can be deployed to docker and k8s.
* Based on [EventFlow](https://github.com/eventflow/EventFlow)
* Based .NET Core2.2, plan to migrate .NET Core3.1 after [EventFlow was fixed](https://github.com/eventflow/EventFlow/pull/686)
* Implement read model by EntityFramework, MongoDB, Elasticsearch
* Integrate RabbitMQ and used [MassTransit]() as Message bus (in progress)
* Event driven MicroService integration (in progress)
* [Wiki](https://github.com/twzhangyang/RestAirline/wiki) is in progress

# How to Run
## Clone this repo

> git clone https://github.com/PooyaAlamirpour/Airline

## Running the container
Then spin up a new container using `docker-compose`

> docker-compose up

Note: add a `-d` to run the container in background

An API service and mssql will run in docker

## Run in local
This project based on .NET Core SDK 2.2.103, please install corresponding SDK for your operating system:

 * [Window](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.103-windows-x64-installer)

 * [Mac](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.103-macos-x64-installer)

After installing, please run below command to make sure current .NET Core version is 2.2.103
`dotnet --version`

The EventStore and ReadModel are based on EF Core and connect to MSSQL, before run this service please update EventStore connect string and
ReadModel connect string in `settings.json` under `RestAirline.Api` project.
After set connect string two database will be migrated automatically by the API service.

## Run the API

### 1. Try to input home api link in Postman:

> GET http://localhost:61200/api/home/

---

### 2. Select Journeys
Journey items should come from another micro-service named flight availability, for now let's build a 
journey in the api automatically for convenience. So you need not pass any journey id to this api.

> POST api/booking/journeys

![add journey](https://user-images.githubusercontent.com/22952792/61993523-7625fb00-b09f-11e9-98a4-5fdd52774996.png)
---

### 3. Add passenger
We can get request body schema from last Api response become the whole Api is designed by Hypermedia.
The api definition is totally described by last api response under `resourceCommands\addPassengerCommand`:
```
"addPassengerCommand": 
{
    "bookingId": "booking-352cb1f3-0f68-4e04-a2f7-24036eb53ce7",
    "name": null,
    "passengerType": 0,
    "age": 0,
    "email": null,
    "postUrl": {
        "uri": "/api/booking/booking-352cb1f3-0f68-4e04-a2f7-24036eb53ce7/passenger"
    }
}
```
Obviously the endpoint is: 

> http://localhost:61100/api/booking/booking-352cb1f3-0f68-4e04-a2f7-24036eb53ce7/passenger

The payload schema is:
```
{
	"bookingId": "booking-352cb1f3-0f68-4e04-a2f7-24036eb53ce7",
    "name": null,
    "passengerType": 0,
    "age": 0,
    "email": null,
}
```
`bookingId` is filled already, please try to fill other parameters, eg:
```
{
	"bookingId": "booking-352cb1f3-0f68-4e04-a2f7-24036eb53ce7",
    "name": "test",
    "passengerType": 0,
    "age": 22,
    "email": "test@test.com",
}
```
Send request:

> POST api/{bookingId}/passenger

![add booking](https://user-images.githubusercontent.com/22952792/61993532-b8e7d300-b09f-11e9-9567-75ba0ea0a8d9.png)
---

### 4. Get booking
According to response of last api, you can either get the booking by `resourceLinks` or post data by `resourceCommand`.

> GET api/booking/{bookingId}

![get booking](https://user-images.githubusercontent.com/22952792/61993549-ffd5c880-b09f-11e9-9679-e708a7f087d3.png)
---

### 5. Update passenger Name

## Business 
The example is regarding online booking for an airline company. An airline company named 'RestAirline' is offering online booking. 
* After passenger submitted one of the available journey that means this passenger starting create an online booking.
* passenger can submit multiple available journeys, every journey including a flight.
* After passenger added journeys, he/she can add passengers.
* Once passenger have been added in booking, passenger can update passenger name for each passenger.
* passenger can submit available seats for each flight and each passenger, seat may just including seat number.
* Once passenger submitted seats, passenger still can update seat.
* After all of these steps, passenger have a chance to order insurance for all passenger some of them.
* Last step is pay for all booking, if payment is successful then create a pnr(six digit) for this booking.
* Online checkin is allowed for all the flights. Passenger can checkin at below time window:

> 2h <= timeWindow <= departure time - 30m 

* Passenger can do online checkin, after this step passenger start to his/her journey. 

## Possible Domain
There are four possible Domains for above business:
But let's focus on `Booking` for now and mock other two domains even if you can hardcode data from these two domains.

![domain](https://user-images.githubusercontent.com/22952792/59654892-bbb2f680-91ca-11e9-8465-a628a57e13b2.png)


