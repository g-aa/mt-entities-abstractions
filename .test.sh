#!/usr/bin/env bash

dotnet test --test-adapter-path:. --logger:"junit;LogFilePath=../../results/{assembly}.TestResult.xml;MethodFormat=Class;FailureBodyFormat=Verbose" --settings coverlet.runsettings --results-directory results/coverage
reportgenerator -reports:results/coverage/*/coverage.opencover.xml -targetdir:results/report -reportTypes:"TextSummary;Html"
firefox results/report/index.html