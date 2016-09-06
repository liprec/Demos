# Power BI Custom Visual

Simple demo to show the different steps to create a Power BI Custom Visual

#  How to use
For each demo there is defined which file needs to be included in *tsconfig.json* and *pbiviz.json*

## Demo 1
Start with the default pbiviz-cli visual. Main purpose is to show the IVisual interface and which methods are called when. 
*tsconfig.json* : "src/demo1.ts"
*pbiviz.json* : "capabilities": "capabilities.demo1.json"

## Demo 2
Adding the DataView capabilities to the visual and how to convert and use them in the visual.
*tsconfig.json* : "src/demo2.ts"
*pbiviz.json* : "capabilities": "capabilities.demo2.json"

## Demo 3
Adding DataViewObjects/ formatting options and how to use them in the visual.
*tsconfig.json* : "src/demo3.ts"
*pbiviz.json* : "capabilities": "capabilities.demo3.json"

## Demo 4
Adding d3.js reference for easy visual development
*tsconfig.json* : "src/demo4.ts"
*pbiviz.json* : "capabilities": "capabilities.demo4.json"
