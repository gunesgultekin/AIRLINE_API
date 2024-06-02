# SE4458 Final Project "AIRLINE_API"
## System Architecture
![architcture](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/cd2d7b95-d0b2-48f8-81bf-da8f7d8fb1af)

The system consists of 1 API Gateway and 2 different APIs (FlightService, NotificationService). All components are hosted on Azure cloud.
 
# FlightService API
* (!) All the URLs about deployments are provided in Moodle.
## FlightServiceAPI/Client
![client](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/08450c33-9e2b-4e2a-bc68-bed10a69adf3)
## Endpoints

![loginmiles](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/601a140e-5771-452a-b543-e21553fbc97c)

![registermiles](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/e0f5ff3e-345a-4afc-b6a7-24fdb47eaf06)

![search](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/25b48f0a-75f9-4777-a4f0-25d04995e7e0)

![flex](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/faddc65d-ec5b-438c-93bb-d236d0ba7c42)

![buy](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/577d444d-6bd3-450e-8d3d-1f420aaae6f3)

![buymiles](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/15dc76fe-2ed6-47af-b5c7-4dc19050fadb)

![deleteTicket](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/2cdae835-dd3a-4792-af8c-f7e587e6bc80)

![gettickets](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/5276b82c-305e-4ce0-a1cb-d84f57b79f78)

## FlightServiceAPI/FlightManager

![flightmanager](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/6e6f3ddf-4d49-420b-b2a7-1b4ddd27bc39)
## Endpoints
![getairports](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/3d867dbf-44c6-4f83-ba35-c8c3b9579613)

![getcities](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/6559307c-2551-4bcf-bb4c-9d304301ee22)

![adminlogin](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/56c3989b-7d93-48df-bc9a-b3dadfcb7648)

![getall](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/eac3db13-aa11-4957-a8ae-543a6f3332f8)

![addflight](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/8a1b175f-30eb-4e0e-bf7c-8deecbdb7a5c)

![deleteflight](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/54da3957-a6a6-46b2-a63b-fb19d4cee050)


## NotificationService API

![mail](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/a67e05c6-7356-45f2-a0bb-55fa38ba3f81)

## Gateway (Hosted App Service Links provided)
* Gateway built with .NET "Ocelot"
## Endpoints
* /loginMiles (redirects to FlightServiceAPI/api/v1/Client/loginMiles)
* /registerMiles (redirects to FlightServiceAPI/api/v1/Client/registerMiles)
* /search (redirects to FlightServiceAPI/api/v1/Client/search)
* /flexSearch (redirects to FlightServiceAPI/api/v1/Client/loginMiles)
* /buy/{code}/{mail} (redirects to FlightServiceAPI/api/v1/Client/buyTicket?flightCode={code}&clientEmail={mail})
* /buyWithMiles{code}{mail} (redirects to FlightServiceAPI/api/v1/Client/buyWithMiles?flightCode={code}&clientEmail={mail})
* /adminLogin (redirects to FlightServiceAPI/api/v1/FlightManager/adminLogin)
* /getAll (redirects to FlightServiceAPI/api/v1/FlightManager/getAll)
* /addFlight (redirects to FlightServiceAPI/api/v1/FlightManager/addFlight)

# UI
## A simple UI was developed using vue.js to test system functions. (Hosted on Azure, link provided)

* Welcome Page
![welcome](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/95768eb3-f615-4573-b8d4-16ff0268ee5a)
(sample mail address and passwords are provided - you can also test as guest by clicking "guest" button)

* Admin Login Page (sample mail address and passwords are provided)
![adminlogin](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/7cb544e4-747a-4088-b7a1-41cfeb60b917)

* Admin Page
![adminpage](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/007b7c04-6759-41a6-b459-bb5f08b33087)


* Member / Guest Page
![search](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/4ca06635-edad-4d68-8396-683ffdb6c632)

# Caching (Redis - Azure Cache for Redis)
* all the data about airports and cities are server-side cached.
![cache1](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/75d52a9f-2378-4284-9b7f-7db25588cd38)

![cache2](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/9c0bcf3b-2187-4301-a6ab-818d8081d71f)

## Security
* Some endpoints are protected with jwt
* All user passwords are hashed and saved in the database with the sha-512 algorithm.

## Additional
* "Hangfire" was used as a timer to send informative e-mails to users about their miles points.
* If member earns miles points aftera flight, an e-mail will be sent to the e-mail queue after 12 hours.
NotificationServiceAPI sends a notification e-mail to the related e-mail address as soon as a message arrives in the queue.

## Data Model
![er-diag](https://github.com/gunesgultekin/AIRLINE_API/assets/126399958/7573adde-d017-4e8d-80df-a767c10ebcf2)

