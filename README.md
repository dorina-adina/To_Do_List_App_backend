# This project represents the backend of the To-Do List App. 

Shortly, the app helps the user to keep his tasks organized and secure. It was built with ASP.Net Core Web API following SOLID principles and a 4-layer approach. Each layer has a clear responsibility and dependency direction. 
The layers:
- DB Layer
  
It is the layer that defines the data model and database configuration, represents database structure. The layer includes DB Context, Entities, Migrations.

The application uses Entity Framework Core (EF Core) as the ORM for database communication.
EF Core is responsible for:

- Mapping entities to database tables
- Executing CRUD operations
- Managing relationships between entities
- Handling database migrations

All database interactions are performed through the DbContext.
- Data Acess Layer

This layer handles all database operations and contains the Repositories and the Helpers. It uses DbContext and entities from DB Layer and encapsulates operations. Through Helpers it is managed the conversion of date and time from DateTime format to a more user friendly format. 
- Business Layer

It implements application logic and business rules. Here are present Models, where are the DTOs, and Profiles, where AutoMapper is used to convert Entities to DTOs or DTOs to Entities. 
- Presentation Layer
  
The layer that handles client interaction and HTTP requests and includes the controllers.

**Login and authentication**

This application supports two authentication methods:

- Classic one: The user can log-in into the app using email and password or, if he is new, he can create a new account. Passwords are securely hashed.
- External one: The user can authenticate using their Google account. It was implemented using OAuth 2.0 via Google provider. If the user doesn't exist, he will be created automatically.

Authorization

There are 2 roles: admin and user. An admin can access all resources.

**View, Create, Update and Delete**

A user can view his tasks, create a new one, update an existing one or delete one. An admin sees all the tasks, and can perform any operation on any user's task.

**Upload and Download**

A user can keep his information secret by hidding them into an image.The application allows users to upload images and optionally hide a secret message inside the image.
When the image is downloaded, the hidden message can be extracted.

Upload process 

1. User uploads an image file;
2. User provides a secret message;
3. The system embeds the message into the image using Least Significant Bit(LSB) encoding;
4. The modified image is stored;
5. The original message is not visible in the image and the changes in the new image are not visible by human eye.

Download process

1. User requests to download an image;
2. The system shows a list with all images;
3. The user choose one;
4. The hidden message is extracted from the image;
5. The decoded message is returned to the user.

