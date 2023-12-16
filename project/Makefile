CHALLENGEN_BINARY=challenge

## up: starts all containers in the background without forcing build
up:
	@echo "Starting Docker images..."
	docker-compose up -d
	@echo "Docker images started!"

## down: stop docker compose
down:
	@echo "Stopping docker compose..."
	docker-compose down
	@echo "Done!"

up_build: ##build_challenge
	@echo "Stopping docker images (if running...)"
	docker-compose down
	@echo "Building (when required) and starting docker images..."
	docker-compose up --build -d
	@echo "Docker images built and started!"

build_challenge:
	@echo "Building challenge..."
	cd ..
	dotnet publish -c Release
	cd project
	@echo "Challenge built!"
stop:
	@echo "Stopping front end..."
	@-pkill -SIGTERM -f "./${CHALLENGEN_BINARY}"
	@echo "Stopped front end!"

## up the sql server image
up_sql:
	@echo "Starting Docker images..."
	docker-compose up -d sqlserver
	@echo "Docker images started!"

## up the elastic search image
up_elastic:
	@echo "Starting Docker images..."
	docker-compose up -d elastic-search
	@echo "Docker images started!"


## up the kafka image
up_kafka:
	@echo "Starting Docker images..."
	docker-compose up -d kafka 
	@echo "Docker images started!"