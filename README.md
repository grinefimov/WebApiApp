# WebApiApp

## Endpoints

- /health/live
- /health/ready

## Running in a Docker container

    docker network create WebApiApp
    docker run -dp 27017:27017 --name MongoDb --network=WebApiApp `
        -e MONGO_INITDB_ROOT_USERNAME=user `
        -e MONGO_INITDB_ROOT_PASSWORD=secret `
        -v MongoDbData:/data/db mongo
    docker build -t webapiapp .
    docker run -dp 8080:80 --name WebApiApp --network=WebApiApp `
        -e MongoDbSettings:ConnectionString=mongodb://user:secret@MongoDb:27017 `
        webapiapp
