/*
 * 
 * Copyright (c) 2016 Jan Pieter Posthuma
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
 *     - Define formating options (capabilities)
 *     - Read the formatting options for use in the visual
 */

module powerbi.extensibility.visual {
    // Single visual dataPoint
    export interface VisualDataPoint {
        category: string;
        value: number;
    }

    // Collection of visual dataPoint. Length === # categories 
    export interface VisualDataPoints {
        dataPoints: VisualDataPoint[]
    }

    export class Visual implements IVisual {
        private target: HTMLElement;
        private updateCount: number;
        private dataView: DataView;

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
            let categories = dataView.categorical.categories[0].values;
            let values = dataView.categorical.values[0].values;

            // Loop thru category values
            for (let i = 0; i < categories.length; i++) {
                // Get category and value
                let category = categories[i];
                let value = values[i];

                // Create dataPoint and push it into the dataPoints array
                dataPoints.push({
                    category: category,
                    value: value
                })
            }

            // Return dataPoints array
            return {
                dataPoints: dataPoints
            }
        }

        constructor(options: VisualConstructorOptions) {
            console.log('Visual constructor', options);
            this.target = options.element;
            this.updateCount = 0;
        }

        public update(options: VisualUpdateOptions) {
            console.log('Visual update', options);
            // Store dataView object
            this.dataView = options.dataViews[0];

            // Clear old information
            this.target.innerHTML = "";
            
            // Convert dataView object to usable dataPoints
            let visualDataPoints = this.convertor(options.dataViews[0]);
            let dataPoints = visualDataPoints.dataPoints;

            // Add header depending on settings
            let objects = this.dataView.metadata.objects; // Get metadata object

            if (this.getShowSetting(objects)) {
                this.target.innerHTML += `<h1 style="color:${(this.getColorSetting(objects))}">${(this.getTitleSetting(objects))}</h1>`;
            } 

            // Loop thru dataPoints and print content (category and value) 
            for (let i = 0; i < dataPoints.length; i++) {
                this.target.innerHTML += `<p>DataPoint: category: <em>${(dataPoints[i].category)}</em> and value <em>${(dataPoints[i].value)}</em></p>`;
            }
        }

        public destroy(): void {
            //TODO: Perform any cleanup tasks here
        }

        private getShowSetting(objects: DataViewObjects): boolean {
            return this.getValue<boolean>(objects, "header", "show", true);
        }

        private getTitleSetting(objects: DataViewObjects): string {
            return this.getValue<string>(objects, "header", "title", "Default Title");
        }

        private getColorSetting(objects: DataViewObjects): string {
            return this.getColor(objects, "header", "color", "#777");
        }

        // Function for retrieving property values
        private getValue<T>(objects: DataViewObject, objectName: string, propertyName: string, defaultValue: T): T {
            if(objects) { // check if exist 
                let object = objects[objectName];
                if(object) { // check if exist
                    let property: T = object[propertyName];
                    if(property !== undefined) { // check if exist
                        return property; // return stored value
                    }
                }
            }
            return defaultValue
        }

        // Function for retrieving color values
        private getColor(objects: DataViewObject, objectName: string, propertyName: string, defaultValue: string): string {
            if(objects) { // check if exist 
                let object = objects[objectName];
                if(object) { // check if exist
                    let property = object[propertyName];
                    if(property !== undefined) { // check if exist
                        return property["solid"]["color"]; // return stored color value
                    }
                }
            }
            return defaultValue
        }

        // IVisual method for populating Formatting Pane objects
        public enumerateObjectInstances(options: EnumerateVisualObjectInstancesOptions): VisualObjectInstanceEnumeration {
            let objectName = options.objectName;
            let objectEnumeration: VisualObjectInstance[] = []; // Return object
            let objects = this.dataView.metadata.objects; // Metedata objects

            switch(objectName) {
                case "header": // Allign with capabilities definition
                    objectEnumeration.push({
                        objectName: objectName,
                        properties: {
                            title: this.getTitleSetting(objects),
                            show: this.getShowSetting(objects),
                            color: this.getColorSetting(objects)
                        },
                        selector: null
                    }); 
                    break;
            }

            return objectEnumeration;
        }
    }
}