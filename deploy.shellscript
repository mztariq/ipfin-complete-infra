#!/bin/bash

# Variables
REGION="us-east-1"
S3_BUCKET="my-webapp-bucket"
LAMBDA_S3_BUCKET="my-lambda-code-bucket"
LAMBDA_CODE_S3_KEY="api-lambda.zip"

# Build and package Lambda function
cd api
dotnet publish -c Release -o ./bin/publish
zip -r ../api.zip ./bin/publish/*
aws s3 cp ../api.zip s3://$LAMBDA_S3_BUCKET/$LAMBDA_CODE_S3_KEY

# Deploy Lambda and API Gateway using CloudFormation
aws cloudformation deploy \
  --template-file lambda-publish.yml \
  --stack-name postcode-api-stack \
  --capabilities CAPABILITY_NAMED_IAM \
  --parameter-overrides CodeS3Bucket=$LAMBDA_S3_BUCKET CodeS3Key=$LAMBDA_CODE_S3_KEY \
  --region $REGION

# Build the React Web Application
cd ../webapp
npm run build
aws s3 sync ./build s3://$S3_BUCKET

# Deploy Web Application using CloudFormation
aws cloudformation deploy \
  --template-file s3-deploy.yml \
  --stack-name webapp-stack \
  --capabilities CAPABILITY_NAMED_IAM \
  --region $REGION

echo "Deployment completed!"
