# Opentracing example
Simple asp.net core application to log events to Jaeger using opentracing

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

git clone https://github.com/susilj/opentracing-example.git

dotnet build

dotnet run

navigate to http://localhost:portnumber/api/format/<sample_text> to generate log events.

**View logged events**

Run Jaeger agent and UI using docker. 

docker run -d -p5775:5775/udp -p6831:6831/udp -p6832:6832/udp -p5778:5778 -p16686:16686 -p14268:14268 -p9411:9411 jaegertracing/all-in-one:0.8.0

navigate to http://localhost:16686 to view logged events.

### Prerequisites

```
Dotnet core 2.0

Docker
```
## Built With

* [Dotnet core 2](https://www.microsoft.com/net/learn/get-started/windows) - The web framework used

## Authors

* **Susil Kumar J** - *Initial work* - [SUSILJ](https://github.com/susilj)

See also the list of [contributors](https://github.com/susilj/opentracing-example/contributors) who participated in this project.
