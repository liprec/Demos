# Power BI Custom Visual

Simple demo to show the different steps to create a Power BI Custom Visual

#  How to use
For each demo there is defined which file needs to be included in *tsconfig.json* and *pbiviz.json* files.

## Demo 1
Adding the DataView capabilities to the visual and how to convert and use them in the visual.

*tsconfig.json*
- "src/settings.demo-1.ts"
- "src/demo-1.ts"

*pbiviz.json*
- "capabilities": "capabilities.demo-1.json"

## Demo 2
Adding DataViewObjects / formatting options and how to use them in the visual.

*tsconfig.json*
- "src/settings.demo-2.ts"
- "src/demo-2.ts"

*pbiviz.json*
- "capabilities": "capabilities.demo-2.json"