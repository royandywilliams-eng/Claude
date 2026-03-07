@echo off
title NT Q&A Application
cd /d D:\nt-qa-app

echo Installing dependencies if needed...
if not exist node_modules (
  npm install
)

echo Starting NT Q&A App on http://localhost:3000...
timeout /t 2
start http://localhost:3000

npm start
