# Power BI Embedded

Simple demo to show the capabilities of Power BI Embedded via the (newly) JavaScript API and NodeJS

# Manage Power BI Embedded
Create a .powerbirc file in the `posh-scripts` file containing the following json and replace with the correct access key from the Power BI Embedded workspace: (Changed `.gitignore` file to exclude this file as it contains access keys) 
```[json]
{
    "accessKey": <accessKey>
}
```
With the posh scripts a session token can be generated to be pasted into to `config.js` file. This session token will expire in one hour.
Also the `reportId` should be inserted into the `config.js` file.

#  How to use the webapplication demos
The `www-root` folder contains the content of the website with Power BI Embedded.

- **index.html**: Starting page with references to the different demo links
- **static.html**: Webpage with a Power BI Embedded report
- **navigate.html**: Webpage with external navigation to the Power BI Embedded report 
- **filter.html**: Webpage with filter option to the Power BI Embedded report

## npm dependecies
First use 
```
npm install
```

Check if in `www-root/javascripts` the correct symlink are creates as the websites use those files.

## Running webserver
After installing all needed npm packages start the app with
```
node bin/www
```

After starting the webserver open a browser and navigate to: [http://localhost:3000](http://localhost:3000) to use the demo

## Notes
This demo uses the 'Retail Analysis Sample' Power Bi report: [https://go.microsoft.com/fwlink/?LinkID=780547](https://go.microsoft.com/fwlink/?LinkID=780547)