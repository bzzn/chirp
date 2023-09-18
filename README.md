# Chirp

A simple package API.

## Limitations
- Data is persisted in memory only
- Endpoint is http only (no https)
- No authentication or authorization

## Build and test
The solution is written in C# using .NET Core 7.0 and can be built and tested by running using the `dotnet` command:
```
dotnet build
dotnet test
```

## Run locally
```
dotnet run --project Src/Application/Application.csproj
```

## Build and run as a Docker container

Building the image (requires the buildx extension):
```
docker buildx -t chirp .
```

Starting a container:
```
docker run -it --rm -p 8000:80 chirp
```

## Using the API
There is a single endpoint which can be used to list all packages, get a single package or add a package. All responses are of the type `application/json`.

Location: `http://localhost:<port>/package`

### Get all packages
```
GET: /package
```
Returns a `200 OK` together with a list of all packages:
```
[
  {
    "kolliId": "999123456789000001",
    "weight": 20000,
    "length": 10,
    "height": 15,
    "width": 20,
    "isValid": true
  },
  {
    "kolliId": "999123456789000002",
    "weight": 50000,
    "length": 100,
    "height": 100,
    "width": 100,
    "isValid": false
  }
]
```

### Get a specific package (by kolli id)
```
GET: /package/{id}
```
Returns details for a specific package if found:
```
{
  "kolliId": "999123456789000001",
  "weight": 20000,
  "length": 10,
  "height": 15,
  "width": 20,
  "isValid": true
}
```
The value of `IsValid` indicates if the package has an acceptable weight and dimensions according to the ruleset:
- Weight must no exceed 20 kg (expressed in grams)
- None of Length, Height and Width must exceed 60 cm

If a package with the specified kolli id does not exist a `404 Not Found` will be returned.
```
GET: /package/999123456789000003
{
  "kolliId": "999123456789000003",
  "message": "No such package: 999123456789000003"
}
```

Providing an invalid kolli id will result in a `400 Bad Request` together with a message explaning why it's invalid.
```
GET: /package/999123456789
{
  "kolliId": "999123456789",
  "message": "Incorrect format, must be exactly 18 characters: 999123456789"
}
```
```
GET: /package/99912345678900000A
{
  "kolliId": "99912345678900000A",
  "message": "Incorrect format, only digits allowed: 99912345678900000A"
}
```
```
GET: /package/666123456789000001
{
  "kolliId": "666123456789000001",
  "message": "Incorrect format, must start with 999: 666123456789000001"
}
```

### Add a package
```
POST: /package
Content-Type: application/json

{
  "kolliId": "999123456789012340",
  "weight": 20000,
  "length": 60,
  "height": 60,
  "width": 60
}
```
On success the endpoint returns a status code of `201 Created` and the package details:
```
{
    "kolliId": "999123456789012340",
    "weight": 20000,
    "length": 60,
    "height": 60,
    "width": 60,
    "isValid": true
}
```

## Preloaded data
The API has two packages preloaded:
- 999123456789000001 which has **valid** dimensions and weight.
- 999123456789000002 which has **invalid** dimensions and weight.
