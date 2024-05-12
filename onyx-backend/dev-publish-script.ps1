docker build . -t onyx-budget-api:latest

docker tag onyx-budget-api:latest dbrdak/onyx-budget-api:latest

docker push dbrdak/onyx-budget-api:latest