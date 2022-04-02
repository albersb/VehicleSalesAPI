## VehicleSales
This repository contain a Web API that allows the creation of a vehicle database. Vehicles can be created, deleted, updated, and search using the following criteria: Make, Model, Year, Color.

#### Prerequisites
To run this program, you will need:
- Visual Studio Code
- A web browser
- Postman

#### Running the Program
- Clone to your local repository and open in VS Community.
- Run debugger.
- Open Postman
  -	User can Get/Post/Put/Delete objects in database.
  - Get can be done by Vehicle Id or specific property
  - To search for specific property use the format action/property
	- Where action is Make, Model, Year, or Color
	- Where property is the Make, Model, Year, or Color that is being searched for
	- example: 'Make/Jeep'
  - All Post, Put, and Delete actions must be in the form of a JSON object
- Testing can be done is Postman
  - to test in Postman simple add the category plus what you want to test again
  - example: '?make=Jeep'

#### Author
- Brian Albers