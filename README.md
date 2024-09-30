# ipfin-complete-infra

To deploy both the Web API (using AWS Lambda and API Gateway) and the React Web Application (hosted in an S3 bucket) within AWS using a repeatable deployment process, we can follow a few structured steps using CloudFormation and AWS CLI. Here's how to achieve that with a single command deployment.

Deployment Overview
Web API (ASP.NET Core Minimal API) will be:
Deployed as an AWS Lambda function.
Exposed via API Gateway.
Web Application (React JS App) will be:
Deployed to an S3 bucket.
Configured to serve static web pages.
Distributed via CloudFront for better global performance.
Prerequisites
AWS CLI is installed and configured on your machine.
AWS CloudFormation is installed.
AWS SAM CLI is installed for building and packaging the Lambda function.
S3 Bucket and Lambda Function artifacts are pre-built (React app and .NET API).

### Folder Structure Example

```
project-root/
    ├── api/
    │   ├── src/
    │   ├── bin/
    │   └── lambda-publish.yml  # CloudFormation template for API
    ├── webapp/
    │   ├── build/
    │   └── s3-deploy.yml       # CloudFormation template for S3 bucket
    └── deploy.sh               # Deployment script


```


#### 1. CloudFormation Template for Web API (API Gateway + Lambda)

```
AWSTemplateFormatVersion: '2010-09-09'
Resources:
  LambdaExecutionRole:
    Type: "AWS::IAM::Role"
    Properties:
      AssumeRolePolicyDocument:
        Version: "2012-10-17"
        Statement:
          - Effect: "Allow"
            Principal:
              Service: "lambda.amazonaws.com"
            Action: "sts:AssumeRole"
      Policies:
        - PolicyName: "LambdaPolicy"
          PolicyDocument:
            Version: "2012-10-17"
            Statement:
              - Effect: "Allow"
                Action:
                  - "logs:*"
                  - "s3:*"
                  - "dynamodb:*"
                Resource: "*"

  PostcodeApiLambdaFunction:
    Type: "AWS::Lambda::Function"
    Properties:
      FunctionName: "PostcodeApiFunction"
      Handler: "api::MyLambdaHandler"
      Role: !GetAtt LambdaExecutionRole.Arn
      Runtime: "dotnetcore3.1"
      Code:
        S3Bucket: !Ref CodeS3Bucket
        S3Key: !Ref CodeS3Key

  ApiGateway:
    Type: "AWS::ApiGateway::RestApi"
    Properties:
      Name: "PostcodeApi"
  
  ApiResource:
    Type: "AWS::ApiGateway::Resource"
    Properties:
      ParentId: !GetAtt ApiGateway.RootResourceId
      PathPart: "postcode"
      RestApiId: !Ref ApiGateway

  PostcodeApiMethod:
    Type: "AWS::ApiGateway::Method"
    Properties:
      AuthorizationType: "NONE"
      HttpMethod: "ANY"
      ResourceId: !Ref ApiResource
      RestApiId: !Ref ApiGateway
      Integration:
        IntegrationHttpMethod: "POST"
        Type: "AWS_PROXY"
        Uri: !Sub
          - arn:aws:apigateway:${Region}:lambda:path/2015-03-31/functions/${LambdaArn}/invocations
          - {
              Region: !Ref "AWS::Region",
              LambdaArn: !GetAtt PostcodeApiLambdaFunction.Arn
            }

Outputs:
  ApiEndpoint:
    Value: !Sub "https://${ApiGateway}.execute-api.${AWS::Region}.amazonaws.com/"


```


#### 2. CloudFormation Template for Web Application (S3 + CloudFront)
s3-deploy.yml:

```
AWSTemplateFormatVersion: '2010-09-09'
Resources:
  WebAppS3Bucket:
    Type: AWS::S3::Bucket
    Properties:
      BucketName: !Sub "webapp-${AWS::Region}-${AWS::AccountId}"
      WebsiteConfiguration:
        IndexDocument: index.html

  CloudFrontDistribution:
    Type: AWS::CloudFront::Distribution
    Properties:
      DistributionConfig:
        Origins:
          - DomainName: !GetAtt WebAppS3Bucket.DomainName
            Id: WebAppOrigin
            S3OriginConfig: {}
        DefaultCacheBehavior:
          TargetOriginId: WebAppOrigin
          ViewerProtocolPolicy: "redirect-to-https"
        Enabled: true
        DefaultRootObject: "index.html"

Outputs:
  WebAppUrl:
    Value: !GetAtt CloudFrontDistribution.DomainName


```


#### 3. Shell Script for Deploying Web API and Web Application
deploy.sh:

```
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


```


#### 4. Steps to Execute

##### Build Artifacts:

Ensure your .NET API is built and zipped as api.zip.
The React app should be built using npm run build.
Single Command Deployment: To deploy both the Web API and Web Application, simply run the deploy.sh script:

```
chmod +x deploy.sh
./deploy.sh

```


### Process Breakdown:
Lambda Function: The .NET API is published, zipped, and uploaded to an S3 bucket. CloudFormation deploys the Lambda function and exposes it via API Gateway.
S3 for React App: The React app is built and uploaded to an S3 bucket configured as a static website. CloudFront is used for CDN distribution.

Single Command: The deploy.sh script orchestrates everything using AWS CLI and CloudFormation.
This approach ensures that both the Web API and Web Application are easily deployed with a single command.
