/*
 * 
 * Copyright (c) 2017 Jan Pieter Posthuma / DataScenarios
 * 
 * All rights reserved.
 * 
 * MIT License.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 *
 */
 
/*
 * Demo 3:
 *     - Add jquery reference
 *     - Use SelectionManager to interact with other visuals
 */

module powerbi.extensibility.visual {
    // Single visual dataPoint
    export interface VisualDataPoint {
        category: string;
        value: number;
        selectionId: ISelectionId;
    }

    // Collection of visual dataPoint. Length === # categories 
    export interface VisualDataPoints {
        dataPoints: VisualDataPoint[]
    }

    export class Visual implements IVisual {
        private target: HTMLElement;
        private updateCount: number;
        private settings: VisualSettings;
        private dataView: DataView;
        private host: IVisualHost;
        private selectionManager: ISelectionManager;

        constructor(options: VisualConstructorOptions) {
            console.log('Visual constructor', options);
            this.target = options.element;
            this.updateCount = 0;
            this.host = options.host;
            this.selectionManager = options.host.createSelectionManager();
        }

        public update(options: VisualUpdateOptions) {
            console.log('Visual update', options);
            // Parse settings
            this.settings = Visual.parseSettings(options && options.dataViews && options.dataViews[0]);

            // Store dataView object
            this.dataView = options.dataViews[0];

            // Clear old information
            this.target.innerHTML = "";
            
            // Convert dataView object to usable dataPoints
            let visualDataPoints: VisualDataPoints = this.convertor(options.dataViews[0]);
            let dataPoints: VisualDataPoint[] = visualDataPoints.dataPoints;

            // Add header depending on settings
            let objects = this.dataView.metadata.objects; // Get metadata object

            if (this.settings.dataPoint.showAllDataPoints) {
                // Loop thru dataPoints and print content (category and value) 
                for (let i = 0; i < dataPoints.length; i++) {
                    // Start p element
                    let startP = document.createElement("p");
                    startP.textContent = `DataPoint (${(i.toString())}): category: `
                    // Category div element
                    let categoryDiv = $("<div>");
                    categoryDiv.attr("style", `color: ${(this.settings.dataPoint.defaultColor)}`);
                    categoryDiv.attr("style", `fontSize: ${(this.settings.dataPoint.fontSize + "px")}`);
                    categoryDiv.text(dataPoints[i].category);
                    // Define click event
                    categoryDiv.on("click", {dataPoint: dataPoints[i]}, (event) => {
                        let dataPoint = event.data.dataPoint;
                        // Pass selectionId object to the selectionManager 
                        this.selectionManager.select(dataPoint.selectionId).then((ids: ISelectionId[]) => {
                            categoryDiv.attr("style", "");
                        });
                    });
                    // Middle text (p)
                    let middleP = document.createElement("p");
                    middleP.textContent = "and value: ";
                    // Value div element
                    let valueDiv = document.createElement("div");
                    valueDiv.style.color = this.settings.dataPoint.defaultColor;
                    valueDiv.style.fontSize = this.settings.dataPoint.fontSize.toString() + "px";
                    valueDiv.textContent = dataPoints[i].value.toString();
                    // Addind newly created elements
                    this.target.appendChild(startP);
                    this.target.appendChild(categoryDiv[0]);
                    this.target.appendChild(middleP);
                    this.target.appendChild(valueDiv);
                }
            }
        }

        public destroy(): void {
            //TODO: Perform any cleanup tasks here
        }

        private convertor(dataView: DataView) : VisualDataPoints {
            // Check if given dataView is not empty
            if (!dataView ||
                !dataView.categorical ||
                !dataView.categorical.categories ||
                !dataView.categorical.categories[0].source) {
                return {
                    dataPoints: []
                }
            }

            let dataPoints: VisualDataPoint[] = [];
            let categorical = dataView.categorical.categories[0];
            let categories = categorical.values;
            let values = dataView.categorical.values[0].values;

            // Loop thru category values
            for (let i = 0; i < categories.length; i++) {
                // Get category and value
                let category = categories[i] as string;
                let value = values[i] as number;
                // Create selectionId based on the category value
                let selectionId = this.host
                    .createSelectionIdBuilder()
                    .withCategory(categorical, i)
                    .createSelectionId()

                // Create dataPoint and push it into the dataPoints array
                dataPoints.push({
                    category: category,
                    value: value,
                    selectionId: selectionId
                })
            }

            // Return dataPoints array
            return {
                dataPoints: dataPoints
            }
        }

        private static parseSettings(dataView: DataView): VisualSettings {
            return VisualSettings.parse(dataView) as VisualSettings;
        }

        /** 
         * This function gets called for each of the objects defined in the capabilities files and allows you to select which of the 
         * objects and properties you want to expose to the users in the property pane.
         * 
         */
        public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstance[] | VisualObjectInstanceEnumerationObject {
            return VisualSettings.enumerateObjectInstances(this.settings || VisualSettings.getDefault(), options);
        }

    }
}